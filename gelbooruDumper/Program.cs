using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AzusaERP;
using libazustreamblob;
using log4net;
using Npgsql;
using NpgsqlTypes;

namespace gelbooruDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private void Run(string[] args)
        {
            FileInfo fi = new FileInfo("azusa.ini");
            if (!fi.Exists)
            {
                Console.WriteLine("azusa.ini not found!");
                return;
            }

            ini = new Ini(fi.FullName);
            log = LogManager.GetLogger(GetType());

            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.Host = ini["postgresql"]["server"];
            ncsb.Port = Convert.ToInt32(ini["postgresql"]["port"]);
            ncsb.Database = ini["postgresql"]["database"];
            ncsb.Password = ini["postgresql"]["password"];
            ncsb.Username = ini["postgresql"]["username"];
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();

            DirectoryInfo blobDir = new DirectoryInfo(ini["gb"]["blobPath"]);
            if (!blobDir.Exists)
                blobDir.Create();
            streamBlob = new AzusaStreamBlob(blobDir, true);
            gelbooru = new GelbooruClient();

            log.Info("Great. Now all the actors are assembled.");

            bool stuffLeft = true;
            while (stuffLeft)
            {
                transaction = connection.BeginTransaction();
                stuffLeft = Pass();
                transaction.Commit();
            }

            log.Info("All done!");
            connection.Dispose();
            streamBlob.Dispose();
        }

        private NpgsqlConnection connection;
        private Ini ini;
        private ILog log;
        private AzusaStreamBlob streamBlob;
        private GelbooruClient gelbooru;
        private NpgsqlTransaction transaction;

        private bool Pass()
        {
            if (CountTags() == 0)
            {
                posts posts = gelbooru.Search("");
                HandlePosts(posts);
                return true;
            }

            if (ImageScraping())
                return true;

            string unscrapedTag = TestForUnscrapedTag();
            if (!string.IsNullOrEmpty(unscrapedTag))
            {
                posts posts = gelbooru.Search(unscrapedTag);
                HandlePosts(posts);
                MarkTagAsScraped(unscrapedTag);
                return true;
            }


            return false;
        }

        private NpgsqlCommand markTagAsScrapedCommand;
        private void MarkTagAsScraped(string tagname)
        {
            if (markTagAsScrapedCommand == null)
            {
                markTagAsScrapedCommand = connection.CreateCommand();
                markTagAsScrapedCommand.CommandText = "UPDATE dump_gb_tags SET scraped = TRUE WHERE tag = @tag";
                markTagAsScrapedCommand.Parameters.Add("@tag", NpgsqlDbType.Varchar);
            }

            markTagAsScrapedCommand.Parameters["@tag"].Value = tagname;
            int executeNonQuery = markTagAsScrapedCommand.ExecuteNonQuery();
            if (executeNonQuery != 1)
                throw new AmbiguousMatchException();
        }

        private NpgsqlCommand updateImage;
        private NpgsqlCommand findUnscrapedImage;
        private bool ImageScraping()
        {
            if (findUnscrapedImage == null)
            {
                findUnscrapedImage = connection.CreateCommand();
                findUnscrapedImage.CommandText =
                    "SELECT id, fileurl FROM dump_gb_posts WHERE downloaded = FALSE AND skipdownload = FALSE";

                if (ini["gb"]["sfwonly"] == "1")
                {
                    findUnscrapedImage.CommandText =
                        "SELECT id, fileurl FROM dump_gb_posts WHERE downloaded = FALSE AND skipdownload = FALSE AND rating = 's'";
                }
            }

            NpgsqlDataReader dataReader = findUnscrapedImage.ExecuteReader();
            if (!dataReader.Read())
            {
                dataReader.Dispose();
                return false;
            }

            int id = dataReader.GetInt32(0);
            string fileurl = dataReader.GetString(1);
            dataReader.Dispose();

            if (updateImage == null)
            {
                updateImage = connection.CreateCommand();
                updateImage.CommandText =
                    "UPDATE dump_gb_posts SET downloaded = @downloaded, skipdownload = @skipped WHERE id = @id";
                updateImage.Parameters.Add("@downloaded", NpgsqlDbType.Boolean);
                updateImage.Parameters.Add("@skipped", NpgsqlDbType.Boolean);
                updateImage.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            updateImage.Parameters["@id"].Value = id;
            if (!IsBlacklisted(GetTagsForPost(id)))
            {
                byte[] downloadImage = gelbooru.DownloadImage(fileurl);
                if (!streamBlob.Obfuscation)
                    throw new InvalidOperationException();
                streamBlob.Put(1, 1, id, downloadImage);
                updateImage.Parameters["@downloaded"].Value = true;
                updateImage.Parameters["@skipped"].Value = false;
            }
            else
            {
                updateImage.Parameters["@downloaded"].Value = false;
                updateImage.Parameters["@skipped"].Value = true;
            }

            updateImage.ExecuteNonQuery();
            return true;
        }

        private NpgsqlCommand getTagsForPostCommand;
        private List<string> GetTagsForPost(int postId)
        {
            if (getTagsForPostCommand == null)
            {
                getTagsForPostCommand = connection.CreateCommand();
                getTagsForPostCommand.CommandText =
                    "SELECT tag FROM dump_gb_posttags posttag LEFT JOIN dump_gb_tags tag ON tag.id = posttag.tagid WHERE postid = @postid";
                getTagsForPostCommand.Parameters.Add("@postid", NpgsqlDbType.Integer);
            }

            getTagsForPostCommand.Parameters["@postid"].Value = postId;
            NpgsqlDataReader dataReader = getTagsForPostCommand.ExecuteReader();
            List<string> result = new List<string>();
            while (dataReader.Read())
                result.Add(dataReader.GetString(0));
            dataReader.Dispose();
            return result;
        }

        private bool IsBlacklisted(List<string> tags)
        {
            if (!ini.ContainsKey("blacklist"))
                return false;

            IniSection blacklistSection = ini["blacklist"];
            foreach (string tag in tags)
            {
                if (!blacklistSection.ContainsKey(tag))
                    continue;

                int i = Convert.ToInt32(blacklistSection[tag]);
                if (i != 0)
                    return true;
            }

            return false;
        }

        private NpgsqlCommand testForUnscrapedTagCommand;

        private string TestForUnscrapedTag()
        {
            if (testForUnscrapedTagCommand == null)
            {
                testForUnscrapedTagCommand = connection.CreateCommand();
                testForUnscrapedTagCommand.CommandText = "SELECT tag FROM dump_gb_tags WHERE scraped = FALSE AND tag != ''";
            }
            NpgsqlDataReader dataReader = testForUnscrapedTagCommand.ExecuteReader();
            string result = null;
            if (dataReader.Read())
            {
                result = dataReader.GetString(0);
            }
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand insertPostTagRelationCommand;
        private void InsertPostTagRelation(int post, int tag)
        {
            if (insertPostTagRelationCommand == null)
            {
                insertPostTagRelationCommand = connection.CreateCommand();
                insertPostTagRelationCommand.CommandText = "INSERT INTO dump_gb_posttags (postid,tagid) VALUES (@post,@tag) ON CONFLICT DO NOTHING";
                insertPostTagRelationCommand.Parameters.Add("@post", NpgsqlDbType.Integer);
                insertPostTagRelationCommand.Parameters.Add("@tag", NpgsqlDbType.Integer);
            }

            insertPostTagRelationCommand.Parameters["@post"].Value = post;
            insertPostTagRelationCommand.Parameters["@tag"].Value = tag;
            insertPostTagRelationCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand insertTagCommand;
        private NpgsqlCommand testForTagCommand;
        private int GetTagId(string tag)
        {
            if (testForTagCommand == null)
            {
                testForTagCommand = connection.CreateCommand();
                testForTagCommand.CommandText = "SELECT id FROM dump_gb_tags WHERE tag=@tag";
                testForTagCommand.Parameters.Add("@tag", NpgsqlDbType.Varchar);
            }

            testForTagCommand.Parameters["@tag"].Value = tag;
            NpgsqlDataReader dataReader = testForTagCommand.ExecuteReader();
            if (dataReader.Read())
            {
                int result = dataReader.GetInt32(0);
                dataReader.Dispose();
                return result;
            }
            dataReader.Dispose();

            log.Info("Found new tag: " + tag);
            if (insertTagCommand == null)
            {
                insertTagCommand = connection.CreateCommand();
                insertTagCommand.CommandText = "INSERT INTO dump_gb_tags (tag) VALUES (@tag) RETURNING id";
                insertTagCommand.Parameters.Add("@tag", NpgsqlDbType.Varchar);
            }

            insertTagCommand.Parameters["@tag"].Value = tag;

            dataReader = insertTagCommand.ExecuteReader();
            dataReader.Read();
            int resultB = dataReader.GetInt32(0);
            dataReader.Dispose();
            return resultB;
        }
        
        private NpgsqlCommand handlePostCommand;
        private void HandlePost(postsPost post)
        {
            log.Info("Found new post: " + post.id);
            if (handlePostCommand == null)
            {
                handlePostCommand = connection.CreateCommand();
                handlePostCommand.CommandText =
                    "INSERT INTO dump_gb_posts" +
                    "(id,height,score,fileurl,parentid,sampleurl,samplewidth,sampleheight,previewurl,rating,width,change,md5,creatorid,haschildren,createdat,status,source,hasnotes,hascomments,previewwidth,previewheight) " +
                    "VALUES " +
                    "(@id,@height,@score,@fileurl,@parentid,@sampleurl,@samplewidth,@sampleheight,@previewurl,@rating,@width,@change," +
                    " @md5,@creatorid,@haschildren,@createdat,@status,@source,@hasnotes,@hascomments,@previewwidth,@previewheight)";
                handlePostCommand.Parameters.Add("@id",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@height",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@score",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@fileurl",NpgsqlDbType.Varchar);
                handlePostCommand.Parameters.Add("@parentid",NpgsqlDbType.Varchar);
                handlePostCommand.Parameters.Add("@sampleurl",NpgsqlDbType.Varchar);
                handlePostCommand.Parameters.Add("@samplewidth",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@sampleheight",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@previewurl",NpgsqlDbType.Varchar);
                handlePostCommand.Parameters.Add("@rating",NpgsqlDbType.Varchar);
                handlePostCommand.Parameters.Add("@width",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@change",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@md5",NpgsqlDbType.Varchar);
                handlePostCommand.Parameters.Add("@creatorid",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@haschildren",NpgsqlDbType.Boolean);
                handlePostCommand.Parameters.Add("@createdat",NpgsqlDbType.Timestamp);
                handlePostCommand.Parameters.Add("@status",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@source",NpgsqlDbType.Text);
                handlePostCommand.Parameters.Add("@hasnotes",NpgsqlDbType.Boolean);
                handlePostCommand.Parameters.Add("@hascomments",NpgsqlDbType.Boolean);
                handlePostCommand.Parameters.Add("@previewwidth",NpgsqlDbType.Integer);
                handlePostCommand.Parameters.Add("@previewheight",NpgsqlDbType.Integer);
            }

            int postId = Convert.ToInt32(post.id);
            handlePostCommand.Parameters["@id"].Value = postId;
            handlePostCommand.Parameters["@height"].Value = Convert.ToInt32(post.height);
            handlePostCommand.Parameters["@score"].Value = Convert.ToInt32(post.score);
            handlePostCommand.Parameters["@fileurl"].Value = post.file_url;
            handlePostCommand.Parameters["@parentid"].Value = post.parent_id;
            handlePostCommand.Parameters["@sampleurl"].Value = post.sample_url;
            handlePostCommand.Parameters["@samplewidth"].Value = Convert.ToInt32(post.sample_width);
            handlePostCommand.Parameters["@sampleheight"].Value = Convert.ToInt32(post.sample_height);
            handlePostCommand.Parameters["@previewurl"].Value = post.preview_url;
            handlePostCommand.Parameters["@rating"].Value = post.rating;
            handlePostCommand.Parameters["@width"].Value = Convert.ToInt32(post.width);
            handlePostCommand.Parameters["@change"].Value = Convert.ToInt32(post.change);
            handlePostCommand.Parameters["@md5"].Value = post.md5;
            handlePostCommand.Parameters["@creatorid"].Value = Convert.ToInt32(post.creator_id);
            handlePostCommand.Parameters["@haschildren"].Value = Convert.ToBoolean(post.has_children);
            post.created_at = post.created_at.Replace(" -0500 ", " ");
            post.created_at = post.created_at.Replace(" -0600 ", " ");
            post.created_at = post.created_at.Substring(4);
            handlePostCommand.Parameters["@createdat"].Value = DateTime.ParseExact(post.created_at, "MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
            handlePostCommand.Parameters["@status"].Value = TranslateStatus(post.status);
            handlePostCommand.Parameters["@source"].Value = post.source;
            handlePostCommand.Parameters["@hasnotes"].Value = Convert.ToBoolean(post.has_notes);
            handlePostCommand.Parameters["@hascomments"].Value = Convert.ToBoolean(post.has_comments);
            handlePostCommand.Parameters["@previewwidth"].Value = Convert.ToInt32(post.preview_width);
            handlePostCommand.Parameters["@previewheight"].Value = Convert.ToInt32(post.preview_height);
            handlePostCommand.ExecuteNonQuery();

            foreach (string tag in post.tags.Split(' '))
            {
                int tagId = GetTagId(tag);
                InsertPostTagRelation(postId, tagId);
            }
        }

        private int TranslateStatus(string postStatus)
        {
            switch (postStatus)
            {
                case "active":
                    return 1;
                case "pending":
                    return 2;
                default:
                    throw new NotImplementedException(postStatus);
            }
        }

        private NpgsqlCommand countTagsCommand;
        private long CountTags()
        {
            if (countTagsCommand == null)
            {
                countTagsCommand = connection.CreateCommand();
                countTagsCommand.CommandText = "SELECT COUNT(*) FROM dump_gb_tags";
            }

            NpgsqlDataReader dataReader = countTagsCommand.ExecuteReader();
            dataReader.Read();
            long result = dataReader.GetInt64(0);
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand testForPostCommand;
        private bool TestForPost(int id)
        {
            if (testForPostCommand == null)
            {
                testForPostCommand = connection.CreateCommand();
                testForPostCommand.CommandText = "SELECT dateAdded FROM dump_gb_posts WHERE id=@id";
                testForPostCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            testForPostCommand.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForPostCommand.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Dispose();
            return result;
        }

        private void HandlePosts(posts posts)
        {
            foreach (postsPost post in posts.post)
            {
                int postId = Convert.ToInt32(post.id);
                if (!TestForPost(postId))
                {
                    HandlePost(post);
                }
            }
        }
    }
}

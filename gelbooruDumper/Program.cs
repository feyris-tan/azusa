using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using AzusaERP;
using log4net;
using Mono.Web;
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
            ncsb.SearchPath = "dump_gb";
            ncsb.ApplicationName = "gelbooruDumper";
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();
            
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
        }

        private NpgsqlConnection connection;
        private Ini ini;
        private ILog log;
        private GelbooruClient gelbooru;
        private NpgsqlTransaction transaction;

        private bool Pass()
        {
            if (CountTags() == 0)
            {
                posts posts = gelbooru.Search("");
                HandlePosts(posts,-1);
                return true;
            }

            int? unscrapedComments = FindUnscrapedComments();
            if (unscrapedComments != null)
            {
                comments comments = gelbooru.GetComments(unscrapedComments.Value);
                HandleComments(comments);
                MarkCommentsAsScraped(unscrapedComments.Value);
                return true;
            }
            
            Tuple<string,int> unscrapedTag = TestForUnscrapedTag();
            if (!string.IsNullOrEmpty(unscrapedTag.Item1))
            {
                string url = HttpUtility.UrlEncode(unscrapedTag.Item1);
                posts posts = gelbooru.Search(url);
                HandlePosts(posts,unscrapedTag.Item2);
                MarkTagAsScraped(unscrapedTag.Item1);
                return true;
            }


            return false;
        }

        private void HandleComments(comments comments)
        {
            foreach (commentsComment comment in comments.comment)
            {
                InsertComment(comment);
            }
        }

        private NpgsqlCommand insertCommentCommand;
        private void InsertComment(commentsComment comment)
        {
            if (insertCommentCommand == null)
            {
                insertCommentCommand = connection.CreateCommand();
                insertCommentCommand.CommandText =
                    "INSERT INTO comments (id, post_id, creator_id, created_at, body) " +
                    "VALUES " +
                    "(@id, @postid,@creatorid, @createdat, @body)";
                insertCommentCommand.Parameters.Add("@id", NpgsqlDbType.Bigint);
                insertCommentCommand.Parameters.Add("@postid", NpgsqlDbType.Integer);
                insertCommentCommand.Parameters.Add("@creatorid", NpgsqlDbType.Integer);
                insertCommentCommand.Parameters.Add("@createdat", NpgsqlDbType.Timestamp);
                insertCommentCommand.Parameters.Add("@body", NpgsqlDbType.Text);
            }

            insertCommentCommand.Parameters["@id"].Value = Convert.ToInt64(comment.id);
            insertCommentCommand.Parameters["@postid"].Value = Convert.ToInt32(comment.post_id);
            insertCommentCommand.Parameters["@creatorid"].Value = GetCreator(Convert.ToInt32(comment.creator_id), comment.creator);
            insertCommentCommand.Parameters["@createdat"].Value = DateTime.ParseExact(comment.created_at, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            insertCommentCommand.Parameters["@body"].Value = comment.body;
            insertCommentCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand getCreatorCommand;
        private int GetCreator(int creatorId, string commentCreator)
        {
            if (getCreatorCommand == null)
            {
                getCreatorCommand = connection.CreateCommand();
                getCreatorCommand.CommandText = "INSERT INTO creators (id, name) VALUES (@id, @name) ON CONFLICT DO NOTHING";
                getCreatorCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
                getCreatorCommand.Parameters.Add("@name", NpgsqlDbType.Text);
            }

            getCreatorCommand.Parameters["@id"].Value = creatorId;
            getCreatorCommand.Parameters["@name"].Value = commentCreator;
            getCreatorCommand.ExecuteNonQuery();

            return creatorId;
        }

        private NpgsqlCommand markTagAsScrapedCommand;
        private void MarkTagAsScraped(string tagname)
        {
            if (markTagAsScrapedCommand == null)
            {
                markTagAsScrapedCommand = connection.CreateCommand();
                markTagAsScrapedCommand.CommandText = "UPDATE tags SET scraped = TRUE WHERE tag = @tag";
                markTagAsScrapedCommand.Parameters.Add("@tag", NpgsqlDbType.Varchar);
            }

            markTagAsScrapedCommand.Parameters["@tag"].Value = tagname;
            int executeNonQuery = markTagAsScrapedCommand.ExecuteNonQuery();
            if (executeNonQuery != 1)
                throw new AmbiguousMatchException();
        }
        
        private NpgsqlCommand testForUnscrapedTagCommand;
        private Tuple<string,int> TestForUnscrapedTag()
        {
            if (testForUnscrapedTagCommand == null)
            {
                testForUnscrapedTagCommand = connection.CreateCommand();
                testForUnscrapedTagCommand.CommandText = "SELECT tag, id FROM tags WHERE scraped = FALSE AND tag != ''";
            }
            NpgsqlDataReader dataReader = testForUnscrapedTagCommand.ExecuteReader();
            if (dataReader.Read())
            {
                string tag = dataReader.GetString(0);
                int id = dataReader.GetInt32(1);
                dataReader.Dispose();
                return new Tuple<string, int>(tag, id);
            }
            else
            {
                dataReader.Dispose();
                return null;
            }
        }

        private NpgsqlCommand insertPostTagRelationCommand;
        private void InsertPostTagRelation(int post, int tag)
        {
            if (insertPostTagRelationCommand == null)
            {
                insertPostTagRelationCommand = connection.CreateCommand();
                insertPostTagRelationCommand.CommandText = "INSERT INTO posttags (postid,tagid) VALUES (@post,@tag) ON CONFLICT DO NOTHING";
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
                testForTagCommand.CommandText = "SELECT id FROM tags WHERE tag=@tag";
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
                insertTagCommand.CommandText = "INSERT INTO tags (tag) VALUES (@tag) RETURNING id";
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
        private void HandlePost(postsPost post, int source_tags)
        {
            log.Info("Found new post: " + post.id);
            if (handlePostCommand == null)
            {
                handlePostCommand = connection.CreateCommand();
                handlePostCommand.CommandText =
                    "INSERT INTO posts" +
                    "(id,height,score,fileurl,parentid,sampleurl,samplewidth,sampleheight,previewurl,rating,width,change,md5,creatorid,haschildren,createdat,status,source,hasnotes,hascomments,previewwidth,previewheight,source_tag) " +
                    "VALUES " +
                    "(@id,@height,@score,@fileurl,@parentid,@sampleurl,@samplewidth,@sampleheight,@previewurl,@rating,@width,@change," +
                    " @md5,@creatorid,@haschildren,@createdat,@status,@source,@hasnotes,@hascomments,@previewwidth,@previewheight,@sourcetag)";
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
                handlePostCommand.Parameters.Add("@sourcetag", NpgsqlDbType.Integer);
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
            handlePostCommand.Parameters["@sourcetag"].Value = source_tags;
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
                countTagsCommand.CommandText = "SELECT COUNT(*) FROM tags";
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
                testForPostCommand.CommandText = "SELECT dateAdded FROM posts WHERE id=@id";
                testForPostCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            testForPostCommand.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForPostCommand.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Dispose();
            return result;
        }

        private void HandlePosts(posts posts, int sourcetag)
        {
            if (posts.post != null)
            {
                foreach (postsPost post in posts.post)
                {
                    int postId = Convert.ToInt32(post.id);
                    if (!TestForPost(postId))
                    {
                        HandlePost(post, sourcetag);
                    }
                }
            }
        }

        private NpgsqlCommand findUnscrapedCommentsNpgsqlCommand;
        private int? FindUnscrapedComments()
        {
            if (findUnscrapedCommentsNpgsqlCommand == null)
            {
                findUnscrapedCommentsNpgsqlCommand = connection.CreateCommand();
                findUnscrapedCommentsNpgsqlCommand.CommandText =
                    "SELECT id FROM posts WHERE hascomments = TRUE AND scraped_comments = FALSE";
            }

            NpgsqlDataReader dataReader = findUnscrapedCommentsNpgsqlCommand.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
            {
                result = dataReader.GetInt32(0);
            }
            dataReader.Close();
            return result;
        }

        private NpgsqlCommand markCommentsAsScraped;
        private void MarkCommentsAsScraped(int id)
        {
            if (markCommentsAsScraped == null)
            {
                markCommentsAsScraped = connection.CreateCommand();
                markCommentsAsScraped.CommandText = "UPDATE posts SET scraped_comments = TRUE WHERE id=@id";
                markCommentsAsScraped.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            markCommentsAsScraped.Parameters["@id"].Value = id;
            int result = markCommentsAsScraped.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("unexpected sql result");
        }
    }
}

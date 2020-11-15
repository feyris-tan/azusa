using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using AzusaERP;
using vndbDumper.Model;
using NpgsqlTypes;
using log4net;

namespace vndbDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        void Run(string[] args)
        {
            FileInfo iniFile = new FileInfo("azusa.ini");
            if (!iniFile.Exists)
            {
                Console.WriteLine("azusa.ini not found!");
                return;
            }
            Ini ini = new Ini(iniFile.FullName);
            IniSection pgSection = ini["postgresql"];

            logger = LogManager.GetLogger(GetType());
            logger.Info("Starting up...");

            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "vndbDumper";
            ncsb.Database = pgSection["database"];
            ncsb.Host = pgSection["server"];
            ncsb.KeepAlive = 60;
            ncsb.Password = pgSection["password"];
            ncsb.Port = Int32.Parse(pgSection["port"]);
            ncsb.Username = pgSection["username"];
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Disposed += Connection_Disposed;
            connection.Notice += Connection_Notice;
            connection.Notification += Connection_Notification;
            connection.StateChange += Connection_StateChange;
            connection.Open();
            PrepareStatements();
            
            if (File.Exists("tags.json"))
            {
                importTags();
            }
            if (File.Exists("traits.json"))
            {
                importTraits();
            }
            if (File.Exists("votes2"))
            {
                importVotes();
            }

            logger.Info("Entering main loop...");
            bool workLeft = true;
            while (workLeft)
            {
                //MainLoop Standby Phase
                workLeft = false;
                VndbClient vndbClient = new VndbClient();
                vndbClient.Login("AzusaERP", "0.0.1");
                DownloadImage = vndbClient.HttpRequest;

                //MainLoop Main Phase
                int? unscrapedVnId = FindUnscrapedVisualNovel();
                if (unscrapedVnId != null)
                {
                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    logger.Info("Begin transaction.");

                    VisualNovel vn = vndbClient.GetVisualNovel(unscrapedVnId.Value);
                    ScrapeVn(vn);

                    Character[] characters = vndbClient.GetCharacters(unscrapedVnId.Value);
                    foreach (Character character in characters)
                        InsertCharacter(character);

                    Release[] releases = vndbClient.GetReleases(unscrapedVnId.Value);
                    foreach (Release release in releases)
                        InsertRelease(release);

                    vndbClient.Dispose();

                    for (int i = 0; i < 100; i++)
                    {
                        KeyValuePair<int, string> missing = FindMissingScreen();
                        if (string.IsNullOrEmpty(missing.Value))
                            break;

                        byte[] screenBuffer = vndbClient.HttpRequest(missing.Value);
                        UpdateScreenImage(missing.Key, screenBuffer);
                    }

                    transaction.Commit();
                    logger.Info("Finish transaction.");
                    workLeft = true;
                }

                //MainLoop End Phase
                vndbClient.Throttle();

            }

            logger.Info("No more to scrape, main loop exited.");
            connection.Close();
            connection.Dispose();
        }

        private void importVotes()
        {
            FileStream level1 = File.OpenRead("votes2");
            StreamReader level2 = new StreamReader(level1);
            while (!level2.EndOfStream)
            {
                string[] args = level2.ReadLine().Split(' ');
                int vnid = Convert.ToInt32(args[0]);
                if (!TestForVn(vnid))
                    InsertVn(vnid);

                int userId = Convert.ToInt32(args[1]);
                if (TestForVote(vnid, userId))
                    continue;
                short vote = Convert.ToInt16(args[2]);
                DateTime dt = DateTime.ParseExact(args[3], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                InsertVote(vnid, userId, vote, dt);
            }
            level2.Close();
            level1.Close();
        }

        private void importTags()
        {
            string inString = File.ReadAllText("tags.json");
            Tag[] tags = JsonConvert.DeserializeObject<Tag[]>(inString);
            foreach(Tag tag in tags)
            {
                if (TestForTagId(tag.id))
                    continue;

                InsertTagId(tag);
            }
        }

        private void importTraits()
        {
            string inString = File.ReadAllText("traits.json");
            Trait[] traits = JsonConvert.DeserializeObject<Trait[]>(inString);
            foreach (Trait trait in traits)
            {
                if (TestForTraitId(trait.id))
                    continue;

                InsertTraitId(trait);
            }
        }
        
        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            logger.Info(String.Format("PostgreSQL State change: {0} -> {1}", e.OriginalState, e.CurrentState));
        }

        private void Connection_Notification(object sender, NpgsqlNotificationEventArgs e)
        {
            logger.Info(String.Format("PostgreSQL Notification: {0}", e.Payload));
        }

        private void Connection_Notice(object sender, NpgsqlNoticeEventArgs e)
        {
            logger.Info(String.Format("PostgreSQL Note: {0}", e.Notice.ToString()));
        }

        private void Connection_Disposed(object sender, EventArgs e)
        {
            logger.Info("PostgreSQL Disposed");
        }

        private NpgsqlConnection connection;
        private NpgsqlCommand testForTagId;
        private NpgsqlCommand insertTag;
        private NpgsqlCommand insertTagAlias;
        private NpgsqlCommand insertTagParent;
        private NpgsqlCommand testForTrait;
        private NpgsqlCommand insertTrait;
        private NpgsqlCommand insertTraitAlias;
        private NpgsqlCommand insertTraitParent;
        private NpgsqlCommand testForVote;
        private NpgsqlCommand insertVote;
        private NpgsqlCommand testForVn;
        private NpgsqlCommand insertVn;
        private NpgsqlCommand findUnscrapedVn;
        private NpgsqlCommand scrapeVn;
        private NpgsqlCommand insertVnLanguage;
        private NpgsqlCommand insertVnPlatform;
        private NpgsqlCommand insertAnime;
        private NpgsqlCommand insertRelation;
        private NpgsqlCommand insertVnTag;
        private NpgsqlCommand insertVnScreen;
        private NpgsqlCommand insertVnStaff;
        private NpgsqlCommand insertRelease;
        private NpgsqlCommand insertReleaseLanguage;
        private NpgsqlCommand insertReleaseMedia;
        private NpgsqlCommand insertReleasePlatform;
        private NpgsqlCommand insertReleaseProducer;
        private NpgsqlCommand insertReleaseVn;
        private NpgsqlCommand insertCharacter;
        private NpgsqlCommand insertCharacterInstance;
        private NpgsqlCommand insertCharacterTrait;
        private NpgsqlCommand insertCharacterVn;
        private NpgsqlCommand insertCharacterVoiced;
        private ILog logger;
        private NpgsqlCommand testForCharacter;
        private NpgsqlCommand testForCharacterVn;
        private NpgsqlCommand testForRelease;
        private NpgsqlCommand findMissingScreen;
        private NpgsqlCommand updateScreenImage;

        
        private void PrepareStatements()
        {
            updateScreenImage = connection.CreateCommand();
            updateScreenImage.CommandText = "UPDATE dump_vndb.vn_screens SET image = @image WHERE screenid = @screenid";
            updateScreenImage.Parameters.Add(new NpgsqlParameter("@image", NpgsqlDbType.Bytea));
            updateScreenImage.Parameters.Add(new NpgsqlParameter("@screenid", DbType.Int32));

            findMissingScreen = connection.CreateCommand();
            findMissingScreen.CommandText = "SELECT screenid, imageUrl FROM dump_vndb.vn_screens WHERE image IS NULL LIMIT 1";

            testForRelease = connection.CreateCommand();
            testForRelease.CommandText = "SELECT dateadded FROM dump_vndb.release WHERE id=@id";
            testForRelease.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            testForCharacterVn = connection.CreateCommand();
            testForCharacterVn.CommandText = "SELECT dateadded FROM dump_vndb.character_vns WHERE cid=@cid AND vnid=@vnid";
            testForCharacterVn.Parameters.Add(new NpgsqlParameter("@cid", DbType.Int32));
            testForCharacterVn.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));

            testForCharacter = connection.CreateCommand();
            testForCharacter.CommandText = "SELECT dateadded FROM dump_vndb.character WHERE id=@id";
            testForCharacter.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertCharacterVoiced = connection.CreateCommand();
            insertCharacterVoiced.CommandText = "INSERT INTO dump_vndb.character_voiced" +
                "(cid, id, aid, vid, note) VALUES " +
                "(@cid, @id, @aid, @vid, @note)";
            insertCharacterVoiced.Parameters.Add(new NpgsqlParameter("@cid", DbType.Int32));
            insertCharacterVoiced.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertCharacterVoiced.Parameters.Add(new NpgsqlParameter("@aid", DbType.Int32));
            insertCharacterVoiced.Parameters.Add(new NpgsqlParameter("@vid", DbType.Int32));
            insertCharacterVoiced.Parameters.Add(new NpgsqlParameter("@note", DbType.String));

            insertCharacterVn = connection.CreateCommand();
            insertCharacterVn.CommandText = "INSERT INTO dump_vndb.character_vns " +
                "(cid, vnid, rid, spoilerlevel, role) " +
                "VALUES " +
                "(@cid, @vnid, @rid, @spoilerlevel, @role)";
            insertCharacterVn.Parameters.Add(new NpgsqlParameter("@cid", DbType.Int32));
            insertCharacterVn.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertCharacterVn.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertCharacterVn.Parameters.Add(new NpgsqlParameter("@spoilerlevel", DbType.Int32));
            insertCharacterVn.Parameters.Add(new NpgsqlParameter("@role", DbType.String));

            insertCharacterTrait = connection.CreateCommand();
            insertCharacterTrait.CommandText = "INSERT INTO dump_vndb.character_traits " +
                "(cid, tid, spoilerlevel) VALUES " +
                "(@cid, @tid, @spoilerlevel)";
            insertCharacterTrait.Parameters.Add(new NpgsqlParameter("@cid", DbType.Int32));
            insertCharacterTrait.Parameters.Add(new NpgsqlParameter("@tid", DbType.Int32));
            insertCharacterTrait.Parameters.Add(new NpgsqlParameter("@spoilerlevel", DbType.Int32));

            insertCharacterInstance = connection.CreateCommand();
            insertCharacterInstance.CommandText = "INSERT INTO dump_vndb.character_instances " +
                "(cid, id, spoiler, name, original) " +
                "VALUES " +
                "(@cid, @id, @spoiler, @name, @original)";
            insertCharacterInstance.Parameters.Add(new NpgsqlParameter("@cid", DbType.Int32));
            insertCharacterInstance.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertCharacterInstance.Parameters.Add(new NpgsqlParameter("@spoiler", DbType.Int32));
            insertCharacterInstance.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertCharacterInstance.Parameters.Add(new NpgsqlParameter("@original", DbType.String));

            insertCharacter = connection.CreateCommand();
            insertCharacter.CommandText = "INSERT INTO dump_vndb.character " +
                "(id, name, original, gender, bloodt, birthday, aliases, description, image, bust, waist, hip, height, weight) " +
                "VALUES " +
                "(@id, @name, @original, @gender, @bloodt, @birthday, @aliases, @description, @image, @bust, @waist, @hip, @height, @weight)";
            insertCharacter.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@original", DbType.String));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@gender", NpgsqlDbType.Char));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@bloodt", DbType.String));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@birthday", DbType.String));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@aliases", DbType.String));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@description", DbType.String));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@image", NpgsqlDbType.Bytea));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@bust", DbType.Int32));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@waist", DbType.Int32));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@hip", DbType.Int32));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@height", DbType.Int32));
            insertCharacter.Parameters.Add(new NpgsqlParameter("@weight", DbType.Int32));

            insertReleaseVn = connection.CreateCommand();
            insertReleaseVn.CommandText = "INSERT INTO dump_vndb.release_vns " +
                "(rid, vnid, title, original) " +
                "VALUES" +
                "(@rid, @vnid, @title, @original)";
            insertReleaseVn.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertReleaseVn.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertReleaseVn.Parameters.Add(new NpgsqlParameter("@title", DbType.String));
            insertReleaseVn.Parameters.Add(new NpgsqlParameter("@original", DbType.String));

            insertReleaseProducer = connection.CreateCommand();
            insertReleaseProducer.CommandText = "INSERT INTO dump_vndb.release_producers " +
                "(rid, pid, developer, publisher, name, original, type) " +
                "VALUES" +
                "(@rid, @pid, @developer, @publisher, @name, @original, @type)";
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@pid", DbType.Int32));
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@developer", DbType.Boolean));
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@publisher", DbType.Boolean));
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@original", DbType.String));
            insertReleaseProducer.Parameters.Add(new NpgsqlParameter("@type", DbType.String));

            insertReleasePlatform = connection.CreateCommand();
            insertReleasePlatform.CommandText = "INSERT INTO dump_vndb.release_platforms " +
                "(rid, platform) VALUES (@rid, @platform)";
            insertReleasePlatform.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertReleasePlatform.Parameters.Add(new NpgsqlParameter("@platform", DbType.String));

            insertReleaseMedia = connection.CreateCommand();
            insertReleaseMedia.CommandText = "INSERT INTO dump_vndb.release_media " +
                "(rid, medium, qty) VALUES (@rid, @medium, @qty)";
            insertReleaseMedia.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertReleaseMedia.Parameters.Add(new NpgsqlParameter("@medium", DbType.String));
            insertReleaseMedia.Parameters.Add(new NpgsqlParameter("@qty", DbType.Int32));

            insertReleaseLanguage = connection.CreateCommand();
            insertReleaseLanguage.CommandText = "INSERT INTO dump_vndb.release_languages " +
                "(rid, lang) VALUES (@rid, @lang)";
            insertReleaseLanguage.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertReleaseLanguage.Parameters.Add(new NpgsqlParameter("@lang", DbType.String));

            insertRelease = connection.CreateCommand();
            insertRelease.CommandText = "INSERT INTO dump_vndb.release " +
                "(id, title, original, released, type, patch, freeware, doujin, website, notes, minage, gtin, catalog, resolution, voiced, animation_story, animation_ero) " +
                "VALUES " +
                "(@id, @title, @original, @released, @type, @patch, @freeware, @doujin, @website, @notes, @minage, @gtin, @catalog, @resolution, @voiced, @animation_story, @animation_ero)";
            insertRelease.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertRelease.Parameters.Add(new NpgsqlParameter("@title", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@original", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@released", DbType.Date));
            insertRelease.Parameters.Add(new NpgsqlParameter("@type", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@patch", DbType.Boolean));
            insertRelease.Parameters.Add(new NpgsqlParameter("@freeware", DbType.Boolean));
            insertRelease.Parameters.Add(new NpgsqlParameter("@doujin", DbType.Boolean));
            insertRelease.Parameters.Add(new NpgsqlParameter("@website", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@notes", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@minage", DbType.Int32));
            insertRelease.Parameters.Add(new NpgsqlParameter("@gtin", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@resolution", DbType.String));
            insertRelease.Parameters.Add(new NpgsqlParameter("@voiced", DbType.Int32));
            insertRelease.Parameters.Add(new NpgsqlParameter("@animation_story", DbType.Int32));
            insertRelease.Parameters.Add(new NpgsqlParameter("@animation_ero", DbType.Int32));

            scrapeVn = connection.CreateCommand();
            scrapeVn.CommandText = "UPDATE dump_vndb.vn SET scraped = TRUE, title = @title, original = @original, released = @released, " +
                "aliases = @aliases, length = @length, description = @description, wikipedia = @wikipedia, encubed = @encubed, " +
                "renai = @renai, image = @image, image_nsfw = @image_nsfw, " +
                "popularity = @popularity, rating = @rating, votecount = @votecount " +
                "WHERE id=@id";
            scrapeVn.Parameters.Add(new NpgsqlParameter("@title", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@original", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@released", DbType.Date));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@aliases", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@length", DbType.Int32));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@description", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@wikipedia", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@encubed", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@renai", DbType.String));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@image", NpgsqlDbType.Bytea));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@image_nsfw", DbType.Boolean));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@popularity", DbType.Double));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@rating", DbType.Double));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@votecount", DbType.Int32));
            scrapeVn.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertVnStaff = connection.CreateCommand();
            insertVnStaff.CommandText = "INSERT INTO dump_vndb.vn_staff " +
                "(vnid, sid, aid, name, original, role, note) " +
                "VALUES " +
                "(@vnid, @sid, @aid, @name, @original, @role, @note)";
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@sid", DbType.Int32));
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@aid", DbType.Int32));
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@original", DbType.String));
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@role", DbType.String));
            insertVnStaff.Parameters.Add(new NpgsqlParameter("@note", DbType.String));

            insertVnScreen = connection.CreateCommand();
            insertVnScreen.CommandText = "INSERT INTO dump_vndb.vn_screens " +
                "(imageurl, rid, nsfw, height, width, vnid) " +
                "VALUES " +
                "(@imageurl, @rid, @nsfw, @height, @width, @vnid)";
            insertVnScreen.Parameters.Add(new NpgsqlParameter("@imageurl", DbType.String));
            insertVnScreen.Parameters.Add(new NpgsqlParameter("@rid", DbType.Int32));
            insertVnScreen.Parameters.Add(new NpgsqlParameter("@nsfw", DbType.Boolean));
            insertVnScreen.Parameters.Add(new NpgsqlParameter("@height", DbType.Int32));
            insertVnScreen.Parameters.Add(new NpgsqlParameter("@width", DbType.Int32));
            insertVnScreen.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            
            insertVnTag = connection.CreateCommand();
            insertVnTag.CommandText = "INSERT INTO dump_vndb.vn_tag " +
                "(srcid, tagid, score, spoilerlevel) " +
                "VALUES " +
                "(@srcid, @tagid, @score, @spoilerlevel)";
            insertVnTag.Parameters.Add(new NpgsqlParameter("@srcid", DbType.Int32));
            insertVnTag.Parameters.Add(new NpgsqlParameter("@tagid", DbType.Int32));
            insertVnTag.Parameters.Add(new NpgsqlParameter("@score", DbType.Double));
            insertVnTag.Parameters.Add(new NpgsqlParameter("@spoilerlevel", DbType.Int32));

            insertRelation = connection.CreateCommand();
            insertRelation.CommandText = "INSERT INTO dump_vndb.vn_relation " +
                "(srcid,id,relation,title,original,official) " +
                "VALUES " +
                "(@srcid,@id,@relation,@title,@original,@official)";
            insertRelation.Parameters.Add(new NpgsqlParameter("@srcid", DbType.Int32));
            insertRelation.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertRelation.Parameters.Add(new NpgsqlParameter("@relation", DbType.String));
            insertRelation.Parameters.Add(new NpgsqlParameter("@title", DbType.String));
            insertRelation.Parameters.Add(new NpgsqlParameter("@original", DbType.String));
            insertRelation.Parameters.Add(new NpgsqlParameter("@official", DbType.Boolean));

            insertAnime = connection.CreateCommand();
            insertAnime.CommandText = "INSERT INTO dump_vndb.vn_anime (vnid, id, ann_id, nfo_id, title_romaji, title_kanji, year, type) VALUES (@vnid,@id,@ann_id,@nfo_id,@title_romaji,@title_kanji,@year,@type)";
            insertAnime.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertAnime.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertAnime.Parameters.Add(new NpgsqlParameter("@ann_id", DbType.Int32));
            insertAnime.Parameters.Add(new NpgsqlParameter("@nfo_id", DbType.String));
            insertAnime.Parameters.Add(new NpgsqlParameter("@title_romaji", DbType.String));
            insertAnime.Parameters.Add(new NpgsqlParameter("@title_kanji", DbType.String));
            insertAnime.Parameters.Add(new NpgsqlParameter("@year", DbType.Int32));
            insertAnime.Parameters.Add(new NpgsqlParameter("@type", DbType.String));

            insertVnPlatform = connection.CreateCommand();
            insertVnPlatform.CommandText = "INSERT INTO dump_vndb.vn_platforms (vnid,platform) VALUES (@vnid,@platform)";
            insertVnPlatform.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertVnPlatform.Parameters.Add(new NpgsqlParameter("@platform", DbType.String));

            insertVnLanguage = connection.CreateCommand();
            insertVnLanguage.CommandText = "INSERT INTO dump_vndb.vn_languages (vnid,language,orig_lang) VALUES (@vnid,@language,@orig_lang)";
            insertVnLanguage.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertVnLanguage.Parameters.Add(new NpgsqlParameter("@language", DbType.String));
            insertVnLanguage.Parameters.Add(new NpgsqlParameter("@orig_lang", DbType.Boolean));

            findUnscrapedVn = connection.CreateCommand();
            findUnscrapedVn.CommandText = "SELECT id FROM dump_vndb.vn WHERE scraped = FALSE LIMIT 1";

            testForVn = connection.CreateCommand();
            testForVn.CommandText = "SELECT id FROM dump_vndb.vn WHERE id=@id";
            testForVn.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertVn = connection.CreateCommand();
            insertVn.CommandText = "INSERT INTO dump_vndb.vn (id) VALUES (@id)";
            insertVn.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            testForTagId = connection.CreateCommand();
            testForTagId.CommandText = "SELECT meta FROM dump_vndb.tags WHERE id=@id";
            testForTagId.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertTag = connection.CreateCommand();
            insertTag.CommandText = "INSERT INTO dump_vndb.tags (id,name,description,meta,vns,cat) VALUES (@id,@name,@desc,@meta,@vns,@cat)";
            insertTag.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertTag.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertTag.Parameters.Add(new NpgsqlParameter("@desc", DbType.String));
            insertTag.Parameters.Add(new NpgsqlParameter("@meta", DbType.Boolean));
            insertTag.Parameters.Add(new NpgsqlParameter("@vns", DbType.Int32));
            insertTag.Parameters.Add(new NpgsqlParameter("@cat", DbType.String));

            insertTagAlias = connection.CreateCommand();
            insertTagAlias.CommandText = "INSERT INTO dump_vndb.tags_aliases (tagid,name) VALUES (@tagid,@name)";
            insertTagAlias.Parameters.Add(new NpgsqlParameter("@tagid", DbType.Int32));
            insertTagAlias.Parameters.Add(new NpgsqlParameter("@name", DbType.String));

            insertTagParent = connection.CreateCommand();
            insertTagParent.CommandText = "INSERT INTO dump_vndb.tags_parents (child,parent) VALUES (@child,@parent)";
            insertTagParent.Parameters.Add(new NpgsqlParameter("@child", DbType.Int32));
            insertTagParent.Parameters.Add(new NpgsqlParameter("@parent", DbType.Int32));

            testForTrait = connection.CreateCommand();
            testForTrait.CommandText = "SELECT meta FROM dump_vndb.traits WHERE id=@id";
            testForTrait.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertTrait = connection.CreateCommand();
            insertTrait.CommandText = "INSERT INTO dump_vndb.traits (id,name,description,meta,chars) VALUES (@id,@name,@desc,@meta,@chars)";
            insertTrait.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertTrait.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertTrait.Parameters.Add(new NpgsqlParameter("@desc", DbType.String));
            insertTrait.Parameters.Add(new NpgsqlParameter("@meta", DbType.Boolean));
            insertTrait.Parameters.Add(new NpgsqlParameter("@chars", DbType.Int32));

            insertTraitAlias = connection.CreateCommand();
            insertTraitAlias.CommandText = "INSERT INTO dump_vndb.traits_aliases (id,alias) VALUES (@id,@alias)";
            insertTraitAlias.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertTraitAlias.Parameters.Add(new NpgsqlParameter("@alias", DbType.String));

            insertTraitParent = connection.CreateCommand();
            insertTraitParent.CommandText = "INSERT INTO dump_vndb.traits_parents (child,parent) VALUES (@child,@parent)";
            insertTraitParent.Parameters.Add(new NpgsqlParameter("@child", DbType.Int32));
            insertTraitParent.Parameters.Add(new NpgsqlParameter("@parent", DbType.Int32));

            testForVote = connection.CreateCommand();
            testForVote.CommandText = "SELECT vote FROM dump_vndb.votes WHERE vnid=@vnid AND userid=@userid";
            testForVote.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            testForVote.Parameters.Add(new NpgsqlParameter("@userid", DbType.Int32));

            insertVote = connection.CreateCommand();
            insertVote.CommandText = "INSERT INTO dump_vndb.votes (vnid,userid,vote,dateutime) VALUES (@vnid,@userid,@vote,@utime)";
            insertVote.Parameters.Add(new NpgsqlParameter("@vnid", DbType.Int32));
            insertVote.Parameters.Add(new NpgsqlParameter("@userid", DbType.Int32));
            insertVote.Parameters.Add(new NpgsqlParameter("@vote", DbType.Int16));
            insertVote.Parameters.Add(new NpgsqlParameter("@utime", DbType.Int32));
        }

        public Nullable<int> FindUnscrapedVisualNovel()
        {
            NpgsqlDataReader reader = findUnscrapedVn.ExecuteReader();
            bool hasRow = reader.Read();
            if (hasRow)
            {
                int result = reader.GetInt32(0);
                reader.Close();
                return result;
            }
            else
            {
                reader.Close();
                return null;
            }
        }

        private void UpdateScreenImage(int id, byte[] buffer)
        {
            updateScreenImage.Parameters["@image"].Value = buffer;
            updateScreenImage.Parameters["@screenid"].Value = id;
            updateScreenImage.ExecuteNonQuery();
        }

        private KeyValuePair<int, string> FindMissingScreen()
        {
            NpgsqlDataReader ndr = findMissingScreen.ExecuteReader();
            KeyValuePair<int, string> result = new KeyValuePair<int, string>();

            if (ndr.Read())
            {
                result = new KeyValuePair<int, string>(ndr.GetInt32(0), ndr.GetString(1));
            }
            ndr.Close();
            return result;
        }

        private bool TestForRelease(int rid)
        {
            testForRelease.Parameters["@id"].Value = rid;
            NpgsqlDataReader ndr = testForRelease.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private bool TestForCharacter(int cid)
        {
            testForCharacter.Parameters["@id"].Value = cid;
            NpgsqlDataReader ndr = testForCharacter.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private bool TestForVn(int vnid)
        {
            testForVn.Parameters["@id"].Value = vnid;
            NpgsqlDataReader ndr = testForVn.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private void InsertVn(int vnid)
        {
            logger.Info("Found new VN: " + vnid.ToString());
            insertVn.Parameters["@id"].Value = vnid;
            insertVn.ExecuteNonQuery();
        }

        private void InsertVote(int vnid, int userid, short vote, DateTime date)
        {
            Int32 unixTimestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            insertVote.Parameters["@vnid"].Value = vnid;
            insertVote.Parameters["@userid"].Value = userid;
            insertVote.Parameters["@vote"].Value = vote;
            insertVote.Parameters["@utime"].Value = unixTimestamp;
            insertVote.ExecuteNonQuery();
        }

        private bool TestForVote(int vnid, int userid)
        {
            testForVote.Parameters["@vnid"].Value = vnid;
            testForVote.Parameters["@userid"].Value = userid;
            NpgsqlDataReader ndr = testForVote.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private bool TestForTagId(int id)
        {
            testForTagId.Parameters["@id"].Value = id;
            NpgsqlDataReader ndr = testForTagId.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private bool TestForTraitId(int id)
        {
            testForTrait.Parameters["@id"].Value = id;
            NpgsqlDataReader ndr = testForTrait.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private void InsertTagId(Tag tag)
        {
            NpgsqlTransaction transaction = connection.BeginTransaction();
            insertTag.Parameters["@id"].Value = tag.id;
            insertTag.Parameters["@name"].Value = tag.name;
            insertTag.Parameters["@desc"].Value = tag.description;
            insertTag.Parameters["@meta"].Value = tag.meta;
            insertTag.Parameters["@vns"].Value = tag.vns;
            insertTag.Parameters["@cat"].Value = tag.cat;
            insertTag.ExecuteNonQuery();
            insertTagAlias.Parameters["@tagid"].Value = tag.id;
            insertTagAlias.Parameters["@name"].Value = tag.name;
            insertTagAlias.ExecuteNonQuery();
            foreach(string alias in tag.aliases)
            {
                if (alias.Equals(tag.name))
                    continue;

                insertTagAlias.Parameters["@name"].Value = alias;
                insertTagAlias.ExecuteNonQuery();
            }
            foreach(int parent in tag.parents)
            {
                insertTagParent.Parameters["@child"].Value = tag.id;
                insertTagParent.Parameters["@parent"].Value = parent;
                insertTagParent.ExecuteNonQuery();
            }
            transaction.Commit();
        }

        private void InsertTraitId(Trait trait)
        {
            NpgsqlTransaction transaction = connection.BeginTransaction();
            insertTrait.Parameters["@id"].Value = trait.id;
            insertTrait.Parameters["@name"].Value = trait.name;
            insertTrait.Parameters["@desc"].Value = trait.description;
            insertTrait.Parameters["@meta"].Value = trait.meta;
            insertTrait.Parameters["@chars"].Value = trait.chars;
            insertTrait.ExecuteNonQuery();
            insertTraitAlias.Parameters["@id"].Value = trait.id;
            insertTraitAlias.Parameters["@alias"].Value = trait.name;
            insertTraitAlias.ExecuteNonQuery();
            for (int i = 0; i < trait.aliases.Length; i++)
            {
                string iName = trait.aliases[i];
                for (int j = i+1; j < trait.aliases.Length; j++)
                {
                    string jName = trait.aliases[j];
                    if (jName.Equals(iName))
                        trait.aliases[j] = "";
                }

                if (iName.Equals(""))
                    continue;

                if (iName.Equals(trait.name))
                    continue;

                insertTraitAlias.Parameters["@alias"].Value = iName;
                insertTraitAlias.ExecuteNonQuery();
            }
            foreach (int parent in trait.parents)
            {
                insertTraitParent.Parameters["@child"].Value = trait.id;
                insertTraitParent.Parameters["@parent"].Value = parent;
                insertTraitParent.ExecuteNonQuery();
            }
            transaction.Commit();
            Console.WriteLine(trait.name);
        }

        private void InsertCharacter(Character character)
        {
            if (TestForCharacter(character.id))
                return;

            insertCharacter.Parameters["@id"].Value = character.id;
            insertCharacter.Parameters["@name"].Value = character.name;

            if (!string.IsNullOrEmpty(character.original))
                insertCharacter.Parameters["@original"].Value = character.original;
            else
                insertCharacter.Parameters["@original"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(character.gender))
                insertCharacter.Parameters["@gender"].Value = character.gender;
            else
                insertCharacter.Parameters["@gender"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(character.bloodt))
                insertCharacter.Parameters["@bloodt"].Value = character.bloodt;
            else
                insertCharacter.Parameters["@bloodt"].Value = DBNull.Value;

            if ((character.birthday[0].HasValue) && (character.birthday[1].HasValue))
            {
                StringBuilder birthdayBuilder = new StringBuilder();
                if (character.birthday[0].HasValue)
                    birthdayBuilder.Append(character.birthday[0].Value);
                else
                    birthdayBuilder.Append("??");
                birthdayBuilder.Append(".");
                if (character.birthday[1].HasValue)
                    birthdayBuilder.Append(character.birthday[1].Value);
                else
                    birthdayBuilder.Append("??");
                birthdayBuilder.Append(".");
                insertCharacter.Parameters["@birthday"].Value = birthdayBuilder.ToString();
            }
            else
            {
                insertCharacter.Parameters["@birthday"].Value = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(character.aliases))
                insertCharacter.Parameters["@aliases"].Value = character.aliases;
            else
                insertCharacter.Parameters["@aliases"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(character.description))
                insertCharacter.Parameters["@description"].Value = character.description;
            else
                insertCharacter.Parameters["@description"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(character.image))
                insertCharacter.Parameters["@image"].Value = DownloadImage(character.image);
            else
                insertCharacter.Parameters["@image"].Value = DBNull.Value;

            if (character.bust.HasValue)
                insertCharacter.Parameters["@bust"].Value = character.bust;
            else
                insertCharacter.Parameters["@bust"].Value = DBNull.Value;

            if (character.waist.HasValue)
                insertCharacter.Parameters["@waist"].Value = character.waist;
            else
                insertCharacter.Parameters["@waist"].Value = DBNull.Value;

            if (character.hip.HasValue)
                insertCharacter.Parameters["@hip"].Value = character.hip;
            else
                insertCharacter.Parameters["@hip"].Value = DBNull.Value;

            if (character.height.HasValue)
                insertCharacter.Parameters["@height"].Value = character.height;
            else
                insertCharacter.Parameters["@height"].Value = DBNull.Value;

            if (character.weight.HasValue)
                insertCharacter.Parameters["@weight"].Value = character.weight;
            else
                insertCharacter.Parameters["@weight"].Value = DBNull.Value;

            foreach (int[] trait in character.traits)
                InsertCharacterTrait(character.id, trait[0], trait[1]);

            foreach (object[] vn in character.vns)
                InsertCharacterVn(character.id, Convert.ToInt32(vn[0]), Convert.ToInt32(vn[1]), Convert.ToInt32(vn[2]), vn[3].ToString());

            foreach (CharacterVoiced voice in character.voiced)
                InsertCharacterVoice(character.id, voice);

            insertCharacter.ExecuteNonQuery();
        }

        private void InsertCharacterVoice(int cid, CharacterVoiced voice)
        {
            insertCharacterVoiced.Parameters["@cid"].Value = cid;
            insertCharacterVoiced.Parameters["@id"].Value = voice.id;
            insertCharacterVoiced.Parameters["@aid"].Value = voice.aid;
            insertCharacterVoiced.Parameters["@vid"].Value = voice.vid;

            if (!string.IsNullOrEmpty(voice.note))
                insertCharacterVoiced.Parameters["@note"].Value = voice.note;
            else
                insertCharacterVoiced.Parameters["@note"].Value = DBNull.Value;

            insertCharacterVoiced.ExecuteNonQuery();
        }

        private bool TestForCharacterVn(int cid, int vnid)
        {
            testForCharacterVn.Parameters["@cid"].Value = cid;
            testForCharacterVn.Parameters["@vnid"].Value = vnid;
            NpgsqlDataReader ndr = testForCharacterVn.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private void InsertCharacterVn(int cid, int vnid, int rid, int spoilerlevel, string role)
        {
            if (TestForCharacterVn(cid, vnid))
                return;

            insertCharacterVn.Parameters["@cid"].Value = cid;
            insertCharacterVn.Parameters["@vnid"].Value = vnid;
            insertCharacterVn.Parameters["@rid"].Value = rid;
            insertCharacterVn.Parameters["@spoilerlevel"].Value = spoilerlevel;
            insertCharacterVn.Parameters["@role"].Value = role;
            insertCharacterVn.ExecuteNonQuery();
        }

        private void InsertCharacterTrait(int cid, int tid, int spoilerlevel)
        {
            insertCharacterTrait.Parameters["@cid"].Value = cid;
            insertCharacterTrait.Parameters["@tid"].Value = tid;
            insertCharacterTrait.Parameters["@spoilerlevel"].Value = spoilerlevel;
            insertCharacterTrait.ExecuteNonQuery();
        }

        private void InsertRelease(Release release)
        {
            if (TestForRelease(release.id))
                return;

            for (int i = 0; i < release.media.Length - 1; i++)
            {
                if (release.media[i] == null)
                    continue;

                for (int j = i + 1; j < release.media.Length; j++)
                {
                    if (release.media[j] == null)
                        continue;

                    if (release.media[i].medium.Equals(release.media[j].medium))
                    {
                        release.media[i].qty += release.media[j].qty;
                        release.media[j] = null;
                    }
                }
            }

            insertRelease.Parameters["@id"].Value = release.id;
            insertRelease.Parameters["@title"].Value = release.title;

            if (!string.IsNullOrEmpty(release.original))
                insertRelease.Parameters["@original"].Value = release.original;
            else
                insertRelease.Parameters["@original"].Value = DBNull.Value;

            try
            {
                DateTime released = DateTime.Parse(release.released);
                insertRelease.Parameters["@released"].Value = released;
            }
            catch (Exception e)
            {
                insertRelease.Parameters["@released"].Value = DBNull.Value;
            }

            insertRelease.Parameters["@type"].Value = release.type;
            insertRelease.Parameters["@patch"].Value = release.patch;
            insertRelease.Parameters["@freeware"].Value = release.freeware;
            insertRelease.Parameters["@doujin"].Value = release.doujin;

            foreach (string language in release.languages)
                InsertReleaseLanguage(release.id, language);

            if (!string.IsNullOrEmpty(release.website))
                insertRelease.Parameters["@website"].Value = release.website;
            else
                insertRelease.Parameters["@website"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(release.notes))
                insertRelease.Parameters["@notes"].Value = release.notes;
            else
                insertRelease.Parameters["@notes"].Value = DBNull.Value;

            if (release.minage.HasValue)
                insertRelease.Parameters["@minage"].Value = release.minage;
            else
                insertRelease.Parameters["@minage"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(release.gtin))
                insertRelease.Parameters["@gtin"].Value = release.gtin;
            else
                insertRelease.Parameters["@gtin"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(release.catalog))
                insertRelease.Parameters["@catalog"].Value = release.catalog;
            else
                insertRelease.Parameters["@catalog"].Value = DBNull.Value;

            foreach (string platform in release.platforms)
                InsertReleasePlatform(release.id, platform);

            for (int i = 0; i < release.media.Length; i++)
            {
                if (release.media[i] == null)
                    continue;

                InsertReleaseMedia(release.id, release.media[i]);
            }

            if (!string.IsNullOrEmpty(release.resolution))
                insertRelease.Parameters["@resolution"].Value = release.resolution;
            else
                insertRelease.Parameters["@resolution"].Value = DBNull.Value;

            if (release.voiced.HasValue)
                insertRelease.Parameters["@voiced"].Value = release.voiced;
            else
                insertRelease.Parameters["@voiced"].Value = DBNull.Value;

            if (release.animation[0].HasValue)
                insertRelease.Parameters["@animation_story"].Value = release.animation[0];
            else
                insertRelease.Parameters["@animation_story"].Value = DBNull.Value;

            if (release.animation[1].HasValue)
                insertRelease.Parameters["@animation_ero"].Value = release.animation[1];
            else
                insertRelease.Parameters["@animation_ero"].Value = DBNull.Value;

            foreach (ReleaseVN vn in release.vn)
                InsertReleaseVN(release.id, vn);

            foreach (ReleaseProducer producer in release.producers)
                InsertReleaseProducer(release.id, producer);

            insertRelease.ExecuteNonQuery();
        }

        private void InsertReleaseVN(int rid, ReleaseVN vn)
        {
            insertReleaseVn.Parameters["@rid"].Value = rid;
            insertReleaseVn.Parameters["@vnid"].Value = vn.id;
            insertReleaseVn.Parameters["@title"].Value = vn.title;

            if (!string.IsNullOrEmpty(vn.original))
                insertReleaseVn.Parameters["@original"].Value = vn.original;
            else
                insertReleaseVn.Parameters["@original"].Value = DBNull.Value;

            insertReleaseVn.ExecuteNonQuery();
        }

        private void InsertReleaseProducer(int rid, ReleaseProducer producer)
        {
            insertReleaseProducer.Parameters["@rid"].Value = rid;
            insertReleaseProducer.Parameters["@pid"].Value = producer.id;
            insertReleaseProducer.Parameters["@developer"].Value = producer.developer;
            insertReleaseProducer.Parameters["@publisher"].Value = producer.publisher;
            insertReleaseProducer.Parameters["@name"].Value = producer.name;

            if (!string.IsNullOrEmpty(producer.original))
                insertReleaseProducer.Parameters["@original"].Value = producer.original;
            else
                insertReleaseProducer.Parameters["@original"].Value = DBNull.Value;

            insertReleaseProducer.Parameters["@type"].Value = producer.type;
            insertReleaseProducer.ExecuteNonQuery();
        }

        private void InsertReleasePlatform(int rid, string platform)
        {
            insertReleasePlatform.Parameters["@rid"].Value = rid;
            insertReleasePlatform.Parameters["@platform"].Value = platform;
            insertReleasePlatform.ExecuteNonQuery();
        }

        private void InsertReleaseMedia(int rid, ReleaseMedia media)
        {
            insertReleaseMedia.Parameters["@rid"].Value = rid;
            insertReleaseMedia.Parameters["@medium"].Value = media.medium;

            if (media.qty.HasValue)
                insertReleaseMedia.Parameters["@qty"].Value = media.qty;
            else
                insertReleaseMedia.Parameters["@qty"].Value = DBNull.Value;

            insertReleaseMedia.ExecuteNonQuery();
        }

        private void InsertReleaseLanguage(int rid, string lang)
        {
            insertReleaseLanguage.Parameters["@rid"].Value = rid;
            insertReleaseLanguage.Parameters["@lang"].Value = lang;
            insertReleaseLanguage.ExecuteNonQuery();
        }

        private void ScrapeVn(VisualNovel vn)
        {
            scrapeVn.Parameters["@id"].Value = vn.id;
            scrapeVn.Parameters["@title"].Value = vn.title;

            if (!string.IsNullOrEmpty(vn.original))
                scrapeVn.Parameters["@original"].Value = vn.original;
            else
                scrapeVn.Parameters["@original"].Value = DBNull.Value;

            try
            {
                DateTime released = DateTime.Parse(vn.released);
                scrapeVn.Parameters["@released"].Value = released;
            }
            catch (Exception e)
            {
                scrapeVn.Parameters["@released"].Value = DBNull.Value;
            }

            foreach (string lang in vn.languages)
                InsertVnLanguage(vn.id, lang, false);

            foreach (string lang in vn.orig_lang)
                InsertVnLanguage(vn.id, lang, true);

            foreach (string platform in vn.platforms)
                InsertVnPlatform(vn.id, platform);

            if (!string.IsNullOrEmpty(vn.aliases))
                scrapeVn.Parameters["@aliases"].Value = vn.aliases;
            else
                scrapeVn.Parameters["@aliases"].Value = DBNull.Value;

            if (vn.length.HasValue)
                scrapeVn.Parameters["@length"].Value = vn.length.Value;
            else
                scrapeVn.Parameters["@length"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(vn.description))
                scrapeVn.Parameters["@description"].Value = vn.description;
            else
                scrapeVn.Parameters["@description"].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(vn.links.wikipedia))
                scrapeVn.Parameters["@wikipedia"].Value = vn.links.wikipedia;
            else
                scrapeVn.Parameters["@wikipedia"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(vn.links.encubed))
                scrapeVn.Parameters["@encubed"].Value = vn.links.encubed;
            else
                scrapeVn.Parameters["@encubed"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(vn.links.renai))
                scrapeVn.Parameters["@renai"].Value = vn.links.renai;
            else
                scrapeVn.Parameters["@renai"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(vn.image))
            {
                byte[] imageBuffer = DownloadImage(vn.image);
                scrapeVn.Parameters["@image"].Value = imageBuffer;
            }
            else
            {
                scrapeVn.Parameters["@image"].Value = DBNull.Value;
            }

            scrapeVn.Parameters["image_nsfw"].Value = vn.image_nsfw;

            foreach (VisualNovelAnime anime in vn.anime)
                InsertAnime(vn.id, anime);

            foreach (VisualNovelRelation relation in vn.relations)
                InsertRelation(vn.id, relation);

            foreach (object[] tag in vn.tags)
            {
                InsertVnTag(vn.id, Convert.ToInt32(tag[0]), Convert.ToDouble(tag[1]), Convert.ToInt32(tag[2]));
            }

            scrapeVn.Parameters["@popularity"].Value = vn.popularity;
            scrapeVn.Parameters["@rating"].Value = vn.rating;
            scrapeVn.Parameters["@votecount"].Value = vn.votecount;

            foreach (VisualNovelScreen screen in vn.screens)
            {
                InsertVnScreen(vn.id, screen);
            }

            foreach(VisualNovelStaff staff in vn.staff)
            {
                InsertVnStaff(vn.id, staff);
            }
            scrapeVn.ExecuteNonQuery();
        }

        private void InsertVnStaff(int vnid, VisualNovelStaff staff)
        {
            insertVnStaff.Parameters["@vnid"].Value = vnid;
            insertVnStaff.Parameters["@sid"].Value = staff.sid;
            insertVnStaff.Parameters["@aid"].Value = staff.aid;
            insertVnStaff.Parameters["@name"].Value = staff.name;
            if (!string.IsNullOrEmpty(staff.original))
                insertVnStaff.Parameters["@original"].Value = staff.original;
            else
                insertVnStaff.Parameters["@original"].Value = DBNull.Value;
            insertVnStaff.Parameters["@role"].Value = staff.role;
            if (!string.IsNullOrEmpty(staff.note))
                insertVnStaff.Parameters["@note"].Value = staff.note;
            else
                insertVnStaff.Parameters["@note"].Value = DBNull.Value;
            insertVnStaff.ExecuteNonQuery();

        }

        private void InsertVnScreen(int vnid, VisualNovelScreen screen)
        {
            insertVnScreen.Parameters["@imageurl"].Value = screen.image;
            insertVnScreen.Parameters["@rid"].Value = screen.rid;
            insertVnScreen.Parameters["@nsfw"].Value = screen.nsfw;
            insertVnScreen.Parameters["@height"].Value = screen.height;
            insertVnScreen.Parameters["@width"].Value = screen.width;
            insertVnScreen.Parameters["@vnid"].Value = vnid;
            insertVnScreen.ExecuteNonQuery();
        }

        private void InsertVnTag(int srcid, int tagid, double score, int spoilerlevel)
        {
            insertVnTag.Parameters["@srcid"].Value = srcid;
            insertVnTag.Parameters["@tagid"].Value = tagid;
            insertVnTag.Parameters["@score"].Value = score;
            insertVnTag.Parameters["@spoilerlevel"].Value = spoilerlevel;
            insertVnTag.ExecuteNonQuery();
        }

        private void InsertRelation(int vnId, VisualNovelRelation relation)
        {
            if (!TestForVn(relation.id))
                InsertVn(relation.id);
            insertRelation.Parameters["@srcid"].Value = vnId;
            insertRelation.Parameters["@id"].Value = relation.id;
            insertRelation.Parameters["@relation"].Value = relation.relation;
            insertRelation.Parameters["@title"].Value = relation.title;

            if (!string.IsNullOrEmpty(relation.original))
                insertRelation.Parameters["@original"].Value = relation.original;
            else
                insertRelation.Parameters["@original"].Value = DBNull.Value;

            insertRelation.Parameters["@official"].Value = relation.official;
            insertRelation.ExecuteNonQuery();
        }

        private void InsertAnime(int vnId, VisualNovelAnime anime)
        {
            insertAnime.Parameters["@vnid"].Value = vnId;
            insertAnime.Parameters["@id"].Value = anime.id;

            if (anime.ann_id.HasValue)
                insertAnime.Parameters["@ann_id"].Value = anime.ann_id;
            else
                insertAnime.Parameters["@ann_id"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(anime.nfo_id))
                insertAnime.Parameters["@nfo_id"].Value = anime.nfo_id;
            else
                insertAnime.Parameters["@nfo_id"].Value = DBNull.Value;

            insertAnime.Parameters["@title_romaji"].Value = anime.title_romaji;

            if (!string.IsNullOrEmpty(anime.title_kanji))
                insertAnime.Parameters["@title_kanji"].Value = anime.title_kanji;
            else
                insertAnime.Parameters["@title_kanji"].Value = DBNull.Value;

            if (anime.year.HasValue)
                insertAnime.Parameters["@year"].Value = anime.year;
            else
                insertAnime.Parameters["@year"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(anime.type))
                insertAnime.Parameters["@type"].Value = anime.type;
            else
                insertAnime.Parameters["@type"].Value = DBNull.Value;

            insertAnime.ExecuteNonQuery();
        }

        private void InsertVnPlatform(int vnId, string platform)
        {
            insertVnPlatform.Parameters["@vnid"].Value = vnId;
            insertVnPlatform.Parameters["@platform"].Value = platform;
            insertVnPlatform.ExecuteNonQuery();
        }

        private void InsertVnLanguage(int vnId, string language, bool origLang)
        {
            try
            {
                insertVnLanguage.Parameters["@vnid"].Value = vnId;
                insertVnLanguage.Parameters["@language"].Value = language;
                insertVnLanguage.Parameters["@orig_lang"].Value = origLang;
                insertVnLanguage.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Could not insert vn language: {0},{1},{2}", vnId, language, origLang);
            }
        }

        private DownloadImage DownloadImage;
    }
    internal delegate byte[] DownloadImage(string url);
}

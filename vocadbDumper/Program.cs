using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzusaERP;
using IO.Swagger.Model;
using log4net;
using log4net.Repository.Hierarchy;
using Npgsql;
using NpgsqlTypes;
using vgmdbDumper;

namespace vocadbDumper
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

            Ini ini = new Ini(fi.FullName);
            IniSection pgSection = ini["postgresql"];

            pass = 0;
            rng = new Random();
            logger = LogManager.GetLogger(GetType());
            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "VocaDB Dumper";
            ncsb.Host = pgSection["server"];
            ncsb.Database = pgSection["database"];
            ncsb.Username = pgSection["username"];
            ncsb.Password = pgSection["password"];
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();
            logger.Info("Connected to PostgreSQL");

            IniSection vocadbDumperSection = ini["vocadbDumper"];
            string userAgentValue = vocadbDumperSection["userAgent"];
            vocadb = new VocadbApiWrapper(userAgentValue);
            vocadb.BeforeRequest += Vocadb_BeforeRequest;

            while (Pass())
            {
                logger.Info("Pass");
                if (transaction != null)
                {
                    transaction.Commit();
                    transaction = null;
                }
            }

            logger.Info("All done!");
            connection.Close();
        }
        
        private void Vocadb_BeforeRequest()
        {
            if (throttleOps > 0)
            {
                double multiplier = rng.NextDouble();
                multiplier += 0.5;
                int amount = (int)((double)THROTTLE * multiplier);
                Thread.Sleep(amount);
            }
            throttleOps++;
            UpdateBanEvasion(currentPassStart);
            transaction = connection.BeginTransaction();
        }

        private bool Pass()
        {
            currentPassStart = DateTime.Now;
            if (CheckBanEvasion(currentPassStart))
            {
                logger.Warn("Ban evasion in place. Waiting 1 hour...");
                Thread.Sleep(ONE_HOUR * 1000);
                return true;
            }

            DateTime hourly = new DateTime(currentPassStart.Year, currentPassStart.Month, currentPassStart.Day,
                currentPassStart.Hour, 0, 0);

            if (!TestForDumpMetadata("newAlbums", "", hourly))
            {
                HandleAlbumList(vocadb.GetNewAlbums());
                SetDumpMetadata("newAlbums", "", hourly);
                return true;
            }

            if (!TestForDumpMetadata("topAlbums", "", hourly))
            {
                HandleAlbumList(vocadb.GetTopAlbums());
                SetDumpMetadata("topAlbums", "", hourly);
                return true;
            }

            if (!TestForDumpMetadata("albumSearch", "hourly", hourly))
            {
                int page = 0;
                while (TestForDumpMetadata("albumSearch", "page", page))
                    page++;
                int offset = page * 50;
                List<AlbumForApiContract> contracts = vocadb.BrowseAlbumList(offset);
                if (contracts.Count > 0)
                {
                    HandleAlbumList(contracts);
                    SetDumpMetadata("albumSearch", "page", page);
                }
                SetDumpMetadata("albumSearch", "hourly", hourly);
                return true;
            }

            int? findUnscrapedArtist = FindUnscrapedArtist();
            if (findUnscrapedArtist.HasValue)
            {
                ScrapeArtist(vocadb.GetArtistAlbums(findUnscrapedArtist.Value));
                MarkArtistAsScraped(findUnscrapedArtist.Value);
                return true;
            }

            int? findUnscrapedTracklist = FindUnscrapedTracklist();
            if (findUnscrapedTracklist.HasValue)
            {
                List<SongInAlbumForApiContract> songInAlbumForApiContracts = vocadb.GetAlbumTracks(findUnscrapedTracklist.Value);
                HandleTrackList(songInAlbumForApiContracts, findUnscrapedTracklist.Value);
                MarkTracklistAsScraped(findUnscrapedTracklist.Value);
                return true;
            }

            return false;
        }

        private const int ONE_MINUTE = 60;
        private const int ONE_HOUR = ONE_MINUTE * 60;
        private const int ONE_DAY = ONE_HOUR * 24;
        private const int DAILY_LIMIT = 1000;
        private NpgsqlConnection connection;
        private ILog logger;
        private NpgsqlTransaction transaction;
        private long pass;
        private VocadbApiWrapper vocadb;
        private DateTime currentPassStart;
        private long throttleOps;
        private Random rng;
        private const int THROTTLE = 10000;

        private void HandleSongList(IEnumerable<SongForApiContract> songs)
        {
            foreach (SongForApiContract song in songs)
            {
                if (!TestForSongId(song.Id.Value))
                    InsertSong(song);
            }
        }

        private NpgsqlCommand setImage;
        private void ScrapeArtist(ArtistForApiContract getArtistAlbums)
        {
            if (getArtistAlbums.MainPicture != null)
            {
                if (setImage == null)
                {
                    setImage = connection.CreateCommand();
                    setImage.CommandText = "UPDATE dump_vocadb_artist SET image=@image WHERE id=@id";
                    setImage.Parameters.Add("@image", NpgsqlDbType.Bytea);
                    setImage.Parameters.Add("@id", NpgsqlDbType.Integer);
                }

                setImage.Parameters["@image"].Value = vocadb.DownloadImage(getArtistAlbums.MainPicture.UrlThumb);
                setImage.Parameters["@id"].Value = getArtistAlbums.Id.Value;
                setImage.ExecuteNonQuery();
            }

            if (getArtistAlbums.Relations != null)
            {
                HandleAlbumList(getArtistAlbums.Relations.LatestAlbums);
                HandleAlbumList(getArtistAlbums.Relations.PopularAlbums);
            }
        }

        private NpgsqlCommand markArtistAsScraped;
        private void MarkArtistAsScraped(int id)
        {
            if (markArtistAsScraped == null)
            {
                markArtistAsScraped = connection.CreateCommand();
                markArtistAsScraped.CommandText = "UPDATE dump_vocadb_artist SET scraped = TRUE WHERE id=@id";
                markArtistAsScraped.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            markArtistAsScraped.Parameters["@id"].Value = id;
            markArtistAsScraped.ExecuteNonQuery();
        }

        private NpgsqlCommand findUnscrapedArtist;
        private int? FindUnscrapedArtist()
        {
            if (findUnscrapedArtist == null)
            {
                findUnscrapedArtist = connection.CreateCommand();
                findUnscrapedArtist.CommandText = "SELECT id FROM dump_vocadb_artist WHERE scraped = FALSE";
            }

            NpgsqlDataReader dataReader = findUnscrapedArtist.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
            {
                result = dataReader.GetInt32(0);
            }
            dataReader.Close();
            return result;
        }

        private NpgsqlCommand insertArtist;
        private void InsertArtist(ArtistContract artistForSongContract)
        {
            logger.Info("Found new artist:" + artistForSongContract.Name);
            if (insertArtist == null)
            {
                insertArtist = connection.CreateCommand();
                insertArtist.CommandText = "INSERT INTO dump_vocadb_artist " +
                                           "(id,additionalnames,artisttype,deleted,name,picturemime,status,version) " +
                                           "VALUES " +
                                           "(@id,@an,@at,@deleted,@name,@pmime,@status,@version)";
                insertArtist.Parameters.Add("@id", NpgsqlDbType.Integer);
                insertArtist.Parameters.Add("@an", NpgsqlDbType.Text);
                insertArtist.Parameters.Add("@at", NpgsqlDbType.Varchar);
                insertArtist.Parameters.Add("@deleted", NpgsqlDbType.Boolean);
                insertArtist.Parameters.Add("@name", NpgsqlDbType.Varchar);
                insertArtist.Parameters.Add("@pmime", NpgsqlDbType.Varchar);
                insertArtist.Parameters.Add("@status", NpgsqlDbType.Varchar);
                insertArtist.Parameters.Add("@version", NpgsqlDbType.Integer);
            }

            insertArtist.Parameters["@id"].Value = artistForSongContract.Id;
            insertArtist.Parameters["@an"].Value = artistForSongContract.AdditionalNames;
            insertArtist.Parameters["@at"].Value = artistForSongContract.ArtistType;
            insertArtist.Parameters["@deleted"].Value = artistForSongContract.Deleted.Value;
            insertArtist.Parameters["@name"].Value = artistForSongContract.Name;
            if (artistForSongContract.PictureMime != null)
                insertArtist.Parameters["@pmime"].Value = artistForSongContract.PictureMime;
            else
                insertArtist.Parameters["@pmime"].Value = DBNull.Value;
            insertArtist.Parameters["@status"].Value = artistForSongContract.Status;
            insertArtist.Parameters["@version"].Value = artistForSongContract.Version.Value;
            insertArtist.ExecuteNonQuery();
        }

        private NpgsqlCommand testForArtist;
        private bool TestForArtist(int id)
        {
            if (testForArtist == null)
            {
                testForArtist = connection.CreateCommand();
                testForArtist.CommandText = "SELECT dateAdded FROM dump_vocadb_artist WHERE id=@id";
                testForArtist.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            testForArtist.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForArtist.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand insertSongArtist;
        private NpgsqlCommand insertSong;
        private void InsertSong(SongForApiContract id)
        {
            logger.Info("Found new Song: " + id.Name);
            foreach (ArtistForSongContract artistForSongContract in id.Artists)
            {
                if (artistForSongContract.Artist != null)
                {
                    if (!TestForArtist(artistForSongContract.Artist.Id.Value))
                        InsertArtist(artistForSongContract.Artist);
                }

                if (insertSongArtist == null)
                    {
                        insertSongArtist = connection.CreateCommand();
                        insertSongArtist.CommandText = "INSERT INTO dump_vocadb_songartists " +
                                                       "(id,trackid,artistid,categories,effectiveroles,iscustomname,issupport,name,roles) " +
                                                       "VALUES " +
                                                       "(@id,@ti,@ai,@categories,@er,@icn,@is,@name,@roles)";
                        insertSongArtist.Parameters.Add("@id", NpgsqlDbType.Integer);
                        insertSongArtist.Parameters.Add("@ti", NpgsqlDbType.Integer);
                        insertSongArtist.Parameters.Add("@ai", NpgsqlDbType.Integer);
                        insertSongArtist.Parameters.Add("@categories", NpgsqlDbType.Varchar);
                        insertSongArtist.Parameters.Add("@er", NpgsqlDbType.Varchar);
                        insertSongArtist.Parameters.Add("@icn", NpgsqlDbType.Boolean);
                        insertSongArtist.Parameters.Add("@is", NpgsqlDbType.Boolean);
                        insertSongArtist.Parameters.Add("@name", NpgsqlDbType.Varchar);
                        insertSongArtist.Parameters.Add("@roles", NpgsqlDbType.Varchar);
                    }

                    insertSongArtist.Parameters["@id"].Value = artistForSongContract.Id;
                    insertSongArtist.Parameters["@ti"].Value = id.Id;
                    insertSongArtist.Parameters["@ai"].Value = artistForSongContract.Artist != null ? artistForSongContract.Artist.Id : -1;
                    insertSongArtist.Parameters["@categories"].Value = artistForSongContract.Categories;
                    insertSongArtist.Parameters["@er"].Value = artistForSongContract.EffectiveRoles;
                    insertSongArtist.Parameters["@icn"].Value = artistForSongContract.IsCustomName.Value;
                    insertSongArtist.Parameters["@is"].Value = artistForSongContract.IsSupport.Value;
                    insertSongArtist.Parameters["@name"].Value = artistForSongContract.Name;
                    insertSongArtist.Parameters["@roles"].Value = artistForSongContract.Roles;
                    insertSongArtist.ExecuteNonQuery();
                }

            if (insertSong == null)
            {
                insertSong = connection.CreateCommand();
                insertSong.CommandText = "INSERT INTO dump_vocadb_songs " +
                                         "(id,artiststring,createdate,defaultname,defaultnamelanguage,favoritedtimes,lengthseconds," +
                                         " name,publishdate,pvservices,ratingscore,songtype,status,version) " +
                                         "VALUES " +
                                         "(@id,@as,@cd,@dn,@dnl,@ft,@ls,@name,@pd,@pvs,@rs,@st,@status,@version)";
                insertSong.Parameters.Add("@id", NpgsqlDbType.Integer);
                insertSong.Parameters.Add("@as", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@cd", NpgsqlDbType.Timestamp);
                insertSong.Parameters.Add("@dn", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@dnl", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@ft", NpgsqlDbType.Integer);
                insertSong.Parameters.Add("@ls", NpgsqlDbType.Integer);
                insertSong.Parameters.Add("@name", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@pd", NpgsqlDbType.Timestamp);
                insertSong.Parameters.Add("@pvs", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@rs", NpgsqlDbType.Integer);
                insertSong.Parameters.Add("@st", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@status", NpgsqlDbType.Varchar);
                insertSong.Parameters.Add("@version", NpgsqlDbType.Integer);
            }

            insertSong.Parameters["@id"].Value = id.Id.Value;
            insertSong.Parameters["@as"].Value = id.ArtistString;
            insertSong.Parameters["@cd"].Value = id.CreateDate;
            insertSong.Parameters["@dn"].Value = id.DefaultName;
            insertSong.Parameters["@dnl"].Value = id.DefaultNameLanguage;
            insertSong.Parameters["@ft"].Value = id.FavoritedTimes;
            insertSong.Parameters["@ls"].Value = id.LengthSeconds;
            insertSong.Parameters["@name"].Value = id.Name;

            if (id.PublishDate.HasValue)
                insertSong.Parameters["@pd"].Value = id.PublishDate;
            else
                insertSong.Parameters["@pd"].Value = DBNull.Value;

            insertSong.Parameters["@pvs"].Value = id.PvServices;
            insertSong.Parameters["@rs"].Value = id.RatingScore;
            insertSong.Parameters["@st"].Value = id.SongType;
            insertSong.Parameters["@status"].Value = id.Status;
            insertSong.Parameters["@version"].Value = id.Version;
            insertSong.ExecuteNonQuery();
        }

        private NpgsqlCommand testForSongIdCommand;
        private bool TestForSongId(int id)
        {
            if (testForSongIdCommand == null)
            {
                testForSongIdCommand = connection.CreateCommand();
                testForSongIdCommand.CommandText = "SELECT dateAdded FROM dump_vocadb_songs WHERE id=@id";
                testForSongIdCommand.Parameters.Add("@id", NpgsqlDbType.Bigint);
            }

            testForSongIdCommand.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForSongIdCommand.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand insertTrackListEntry;
        private void HandleTrackList(List<SongInAlbumForApiContract> songlist, int albumId)
        {
            foreach (SongInAlbumForApiContract songInAlbum in songlist)
            {
                if (insertTrackListEntry == null)
                {
                    insertTrackListEntry = connection.CreateCommand();
                    insertTrackListEntry.CommandText = "INSERT INTO dump_vocadb_albumtracks" +
                                                       "(id,name,discNumber,songId,tracknumber,albumid)" +
                                                       "VALUES" +
                                                       "(@id,@name,@dn,@si,@tn,@aid)";
                    insertTrackListEntry.Parameters.Add("@id", NpgsqlDbType.Integer);
                    insertTrackListEntry.Parameters.Add("@name", NpgsqlDbType.Varchar);
                    insertTrackListEntry.Parameters.Add("@dn", NpgsqlDbType.Integer);
                    insertTrackListEntry.Parameters.Add("@si", NpgsqlDbType.Bigint);
                    insertTrackListEntry.Parameters.Add("@tn", NpgsqlDbType.Integer);
                    insertTrackListEntry.Parameters.Add("@aid", NpgsqlDbType.Integer);
                }

                insertTrackListEntry.Parameters["@id"].Value = songInAlbum.Id;
                insertTrackListEntry.Parameters["@name"].Value = songInAlbum.Name;
                insertTrackListEntry.Parameters["@dn"].Value = songInAlbum.DiscNumber;
                if (songInAlbum.Song != null)
                {
                    insertTrackListEntry.Parameters["@si"].Value = songInAlbum.Song.Id;
                    if (!TestForSongId(songInAlbum.Song.Id.Value))
                        InsertSong(songInAlbum.Song);
                }
                else
                {
                    insertTrackListEntry.Parameters["@si"].Value = -1;
                }

                insertTrackListEntry.Parameters["@tn"].Value = songInAlbum.TrackNumber;
                insertTrackListEntry.Parameters["@aid"].Value = albumId;
                insertTrackListEntry.ExecuteNonQuery();
            }
        }

        private NpgsqlCommand findUnscrapedTracklistCommand;
        private int? FindUnscrapedTracklist()
        {
            if (findUnscrapedTracklistCommand == null)
            {
                findUnscrapedTracklistCommand = connection.CreateCommand();
                findUnscrapedTracklistCommand.CommandText = "SELECT id FROM dump_vocadb_albums WHERE scrapedtracks = FALSE";
            }

            NpgsqlDataReader dataReader = findUnscrapedTracklistCommand.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
                result = dataReader.GetInt32(0);
            dataReader.Close();
            return result;
        }

        private NpgsqlCommand markTracklistAsScraped;
        private void MarkTracklistAsScraped(int id)
        {
            if (markTracklistAsScraped == null)
            {
                markTracklistAsScraped = connection.CreateCommand();
                markTracklistAsScraped.CommandText = "UPDATE dump_vocadb_albums SET scrapedtracks = TRUE WHERE id = @id";
                markTracklistAsScraped.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            markTracklistAsScraped.Parameters["@id"].Value = id;
            markTracklistAsScraped.ExecuteNonQuery();
        }

        private void HandleAlbumList(IEnumerable<AlbumForApiContract> albums)
        {
            foreach (AlbumForApiContract album in albums)
            {
                if (!TestForAlbum(album.Id.Value))
                    InsertAlbum(album);
                else if (album.MainPicture != null && TestForAlbum(album.Id.Value) && !TestForAlbumCover(album.Id.Value))
                    SetAlbumCover(album.MainPicture, album.Id.Value);
            }
        }

        private NpgsqlCommand testForAlbumCoverCommand;
        private bool TestForAlbumCover(int id)
        {
            if (testForAlbumCoverCommand == null)
            {
                testForAlbumCoverCommand = connection.CreateCommand();
                testForAlbumCoverCommand.CommandText = "SELECT LENGTH(cover) FROM dump_vocadb_albums WHERE id=@id";
                testForAlbumCoverCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            testForAlbumCoverCommand.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForAlbumCoverCommand.ExecuteReader();
            dataReader.Read();
            bool result;
            if (dataReader.IsDBNull(0))
                result = false;
            else if (dataReader.GetInt32(0) > 0)
                result = true;
            else
                result = false;
            dataReader.Close();
            return result;
        }

        private NpgsqlCommand testForAlbumCommand;
        private bool TestForAlbum(int id)
        {
            if (testForAlbumCommand == null)
            {
                testForAlbumCommand = connection.CreateCommand();
                testForAlbumCommand.CommandText = "SELECT dateAdded FROM dump_vocadb_albums WHERE id=@id";
                testForAlbumCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            testForAlbumCommand.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForAlbumCommand.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Close();
            return result;
        }

        private NpgsqlCommand setAlbumCover;
        private void SetAlbumCover(EntryThumbForApiContract albumMainPicture, int id)
        {
            if (setAlbumCover == null)
            {
                setAlbumCover = connection.CreateCommand();
                setAlbumCover.CommandText = "UPDATE dump_vocadb_albums SET cover=@cover, scrapedcover = TRUE WHERE id=@id";
                setAlbumCover.Parameters.Add("@cover", NpgsqlDbType.Bytea);
                setAlbumCover.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            try
            {
                setAlbumCover.Parameters["@cover"].Value = vocadb.DownloadImage(albumMainPicture.UrlThumb);
            }
            catch (Exception)
            {
                setAlbumCover.Parameters["@cover"].Value = DBNull.Value;
                logger.Error("Download failed: " + albumMainPicture.UrlThumb);
            }
            
            setAlbumCover.Parameters["@id"].Value = id;
            setAlbumCover.ExecuteNonQuery();
        }

        private NpgsqlCommand insertAlbumCommand;
        private void InsertAlbum(AlbumForApiContract album)
        {
            logger.Info("Found new album: " + album.Name);
            if (insertAlbumCommand == null)
            {
                insertAlbumCommand = connection.CreateCommand();
                insertAlbumCommand.CommandText =
                    "INSERT INTO dump_vocadb_albums " +
                    "(id, name, artiststring, createdate, defaultnamelanguage, disctype, ratingaverage, ratingcount, releasedate, status, version, defaultname, catalognumber) " +
                    "VALUES " +
                    "(@id, @name, @as, @cd, @dnl, @dt, @ra, @rc, @rd, @status, @version,@defaultname,@cn)";
                insertAlbumCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
                insertAlbumCommand.Parameters.Add("@name", NpgsqlDbType.Varchar);
                insertAlbumCommand.Parameters.Add("@as", NpgsqlDbType.Varchar);
                insertAlbumCommand.Parameters.Add("@cd", NpgsqlDbType.Timestamp);
                insertAlbumCommand.Parameters.Add("@dnl", NpgsqlDbType.Varchar);
                insertAlbumCommand.Parameters.Add("@dt", NpgsqlDbType.Varchar);
                insertAlbumCommand.Parameters.Add("@ra", NpgsqlDbType.Double);
                insertAlbumCommand.Parameters.Add("@rc", NpgsqlDbType.Integer);
                insertAlbumCommand.Parameters.Add("@rd", NpgsqlDbType.Date);
                insertAlbumCommand.Parameters.Add("@status", NpgsqlDbType.Varchar);
                insertAlbumCommand.Parameters.Add("@version", NpgsqlDbType.Integer);
                insertAlbumCommand.Parameters.Add("@defaultname", NpgsqlDbType.Varchar);
                insertAlbumCommand.Parameters.Add("@cn", NpgsqlDbType.Varchar);
            }

            insertAlbumCommand.Parameters["@id"].Value = album.Id;
            insertAlbumCommand.Parameters["@name"].Value = album.Name;
            insertAlbumCommand.Parameters["@as"].Value = album.ArtistString;
            insertAlbumCommand.Parameters["@cd"].Value = album.CreateDate;
            insertAlbumCommand.Parameters["@dnl"].Value = album.DefaultNameLanguage;
            insertAlbumCommand.Parameters["@dt"].Value = album.DiscType;
            insertAlbumCommand.Parameters["@ra"].Value = album.RatingAverage;
            insertAlbumCommand.Parameters["@rc"].Value = album.RatingCount;
            if (album.ReleaseDate.IsEmpty.Value)
                insertAlbumCommand.Parameters["@rd"].Value = DBNull.Value;
            else
                if (album.ReleaseDate.Day.HasValue && album.ReleaseDate.Month.HasValue)
                    insertAlbumCommand.Parameters["@rd"].Value = new DateTime(album.ReleaseDate.Year.Value, album.ReleaseDate.Month.Value, album.ReleaseDate.Day.Value);
                else
                    insertAlbumCommand.Parameters["@rd"].Value = DBNull.Value;
            insertAlbumCommand.Parameters["@status"].Value = album.Status;
            insertAlbumCommand.Parameters["@version"].Value = album.Version.Value;
            if (album.DefaultName != null)
                insertAlbumCommand.Parameters["@defaultname"].Value = album.DefaultName;
            else
                insertAlbumCommand.Parameters["@defaultname"].Value = DBNull.Value;

            if (album.CatalogNumber != null)
                insertAlbumCommand.Parameters["@cn"].Value = album.CatalogNumber;
            else
                insertAlbumCommand.Parameters["@cn"].Value = DBNull.Value;

            insertAlbumCommand.ExecuteNonQuery();

            if (album.MainPicture != null)
            {
                SetAlbumCover(album.MainPicture, album.Id.Value);
            }
        }

        private NpgsqlCommand checkBanEvasionCommand;
        private NpgsqlCommand insertBanEvasionLine;
        private bool CheckBanEvasion(DateTime date)
        {
            if (checkBanEvasionCommand == null)
            {
                checkBanEvasionCommand = connection.CreateCommand();
                checkBanEvasionCommand.CommandText = "SELECT requests FROM dump_vocadb_0banevasion WHERE id = @id";
                checkBanEvasionCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            int dayId = (int)(date.ToUnixTime() / ONE_DAY);
            checkBanEvasionCommand.Parameters["@id"].Value = dayId;
            NpgsqlDataReader dataReader = checkBanEvasionCommand.ExecuteReader();
            int requestsToday = 0;
            bool containsLine = dataReader.Read();
            if (containsLine)
            {
                requestsToday = dataReader.GetInt32(0);
            }
            dataReader.Dispose();

            if (!containsLine)
            {
                if (insertBanEvasionLine == null)
                {
                    insertBanEvasionLine = connection.CreateCommand();
                    insertBanEvasionLine.CommandText = "INSERT INTO dump_vocadb_0banevasion (id,requests) VALUES (@id,0)";
                    insertBanEvasionLine.Parameters.Add("@id", NpgsqlDbType.Integer);
                }

                insertBanEvasionLine.Parameters["@id"].Value = dayId;
                insertBanEvasionLine.ExecuteNonQuery();
            }

            return requestsToday >= DAILY_LIMIT;
        }

        private NpgsqlCommand updateBanEvasionLine;
        private void UpdateBanEvasion(DateTime date)
        {
            if (updateBanEvasionLine == null)
            {
                updateBanEvasionLine = connection.CreateCommand();
                updateBanEvasionLine.CommandText =
                    "UPDATE dump_vocadb_0banevasion SET requests = requests + 1 WHERE id = @id";
                updateBanEvasionLine.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            int dayId = (int)(date.ToUnixTime() / ONE_DAY);
            updateBanEvasionLine.Parameters["@id"].Value = dayId;
            updateBanEvasionLine.ExecuteNonQuery();
        }


        private NpgsqlCommand setMetadata;
        private void SetDumpMetadata(string key1, string key2, Nullable<DateTime> keyutime)
        {
            if (setMetadata == null)
            {
                setMetadata = connection.CreateCommand();
                setMetadata.CommandText = "INSERT INTO dump_vocadb_0dumpmeta (key1,key2,keyutime) VALUES (@key1,@key2,@keyutime)";
                setMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));
            }

            if (string.IsNullOrEmpty(key2))
                key2 = "";
            if (!keyutime.HasValue)
                keyutime = DateTime.MinValue;

            setMetadata.Parameters["@key1"].Value = key1;
            setMetadata.Parameters["@key2"].Value = key2;
            setMetadata.Parameters["@keyutime"].Value = keyutime.Value.ToUnixTime();
            setMetadata.ExecuteNonQuery();
        }
        private void SetDumpMetadata(string key1, string key2, int keyutime)
        {
            if (setMetadata == null)
            {
                setMetadata = connection.CreateCommand();
                setMetadata.CommandText = "INSERT INTO dump_vocadb_0dumpmeta (key1,key2,keyutime) VALUES (@key1,@key2,@keyutime)";
                setMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));
            }

            if (string.IsNullOrEmpty(key2))
                key2 = "";

            setMetadata.Parameters["@key1"].Value = key1;
            setMetadata.Parameters["@key2"].Value = key2;
            setMetadata.Parameters["@keyutime"].Value = keyutime;
            setMetadata.ExecuteNonQuery();
        }

        private NpgsqlCommand testForDumpMetadataWithoutDate;
        private bool TestForDumpMetadata(string key1, string key2)
        {
            if (testForDumpMetadataWithoutDate == null)
            {
                testForDumpMetadataWithoutDate = connection.CreateCommand();
                testForDumpMetadataWithoutDate.CommandText =
                    "SELECT id FROM dump_vocadb_0dumpmeta WHERE key1=@key1 AND key2=@key2";
                testForDumpMetadataWithoutDate.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                testForDumpMetadataWithoutDate.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
            }
            if (string.IsNullOrEmpty(key2))
                key2 = "";

            testForDumpMetadataWithoutDate.Parameters["@key1"].Value = key1;
            testForDumpMetadataWithoutDate.Parameters["@key2"].Value = key2;

            NpgsqlDataReader reader = testForDumpMetadataWithoutDate.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }

        private NpgsqlCommand testForDumpMetadata;
        private bool TestForDumpMetadata(string key1, string key2, Nullable<DateTime> keyutime)
        {
            if (testForDumpMetadata == null)
            {
                testForDumpMetadata = connection.CreateCommand();
                testForDumpMetadata.CommandText =
                    "SELECT id FROM dump_vocadb_0dumpmeta WHERE key1=@key1 AND key2=@key2 AND keyutime=@keyutime";
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));
            }
            if (string.IsNullOrEmpty(key2))
                key2 = "";
            if (!keyutime.HasValue)
                keyutime = DateTime.MinValue;

            testForDumpMetadata.Parameters["@key1"].Value = key1;
            testForDumpMetadata.Parameters["@key2"].Value = key2;
            testForDumpMetadata.Parameters["@keyutime"].Value = keyutime.Value.ToUnixTime();

            NpgsqlDataReader reader = testForDumpMetadata.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }
        private bool TestForDumpMetadata(string key1, string key2, int keyutime)
        {
            if (testForDumpMetadata == null)
            {
                testForDumpMetadata = connection.CreateCommand();
                testForDumpMetadata.CommandText =
                    "SELECT id FROM dump_vocadb_0dumpmeta WHERE key1=@key1 AND key2=@key2 AND keyutime=@keyutime";
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));
            }
            if (string.IsNullOrEmpty(key2))
                key2 = "";

            testForDumpMetadata.Parameters["@key1"].Value = key1;
            testForDumpMetadata.Parameters["@key2"].Value = key2;
            testForDumpMetadata.Parameters["@keyutime"].Value = keyutime;

            NpgsqlDataReader reader = testForDumpMetadata.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }

    }
}

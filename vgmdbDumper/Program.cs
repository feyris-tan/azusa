using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Npgsql;
using System.Data;
using System.IO;
using System.Net;
using AzusaERP;
using vgmdbDumper.Model;
using NpgsqlTypes;


namespace vgmdbDumper
{
    class Program
    {
        private const int THROTTLE_RATE = 10000;
        private const string LETTERS = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly string[] SECTIONS = { "albums", "media", "tracklists", "scans", "artists", "products", "labels", "links", "ratings" };

        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        void Run(string[] args)
        {
            FileInfo fi = new FileInfo("azusa.ini");
            if (!fi.Exists)
            {
                Console.WriteLine("azusa.ini not found!");
                return;
            }

            Ini ini = new Ini(fi.FullName);
            IniSection pgSection = ini["postgresql"];
            
            logger = LogManager.GetLogger(GetType());
            logger.Info("Starting up...");

            vgmdbApiClient = VgmdbApiClient.GetInstance();

            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "vgmdbDumper";
            ncsb.Database = pgSection["database"];
            ncsb.Host = pgSection["server"];
            ncsb.KeepAlive = 60;
            ncsb.Password = pgSection["password"];
            ncsb.Port = Convert.ToInt32(pgSection["port"]);
            ncsb.Username = pgSection["username"];
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();
            PrepareStatements();

            if (!TestForDumpMetadata("firstboot", null))
                SetDumpMetadata("firstboot", null, DateTime.Now);

            bool workLeft = true;
            do
            {
                logger.Info("Begin Transaction");
                npgsqlTransaction = connection.BeginTransaction();

                workLeft = DumpPass();

                npgsqlTransaction.Commit();
                logger.Info("Finish Transaction");

                Throttle();
            } while (workLeft);

            logger.Info("Nothing left to scrape, goodbye!");
            connection.Close();
        }

        private bool DumpPass()
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;
            DateTime hour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            
            foreach (char c in LETTERS)
            {
                if (!TestForDumpMetadata("albumlist",c.ToString()))
                {
                    AlbumList al = vgmdbApiClient.GetAlbumList(c);
                    ScrapeAlbumList(al);
                    SetDumpMetadata("albumlist", c.ToString(), today);
                    return true;
                }
                if (!TestForDumpMetadata("artistlist",c.ToString()))
                {
                    ArtistList al = vgmdbApiClient.GetArtistList(c);
                    ScrapeArtistList(al);
                    SetDumpMetadata("artistlist", c.ToString(), today);
                    return true;
                }
                if (!TestForDumpMetadata("productlist",c.ToString()))
                {
                    ProductList pl = vgmdbApiClient.GetProductList(c);
                    ScrapeProductList(pl);
                    SetDumpMetadata("productlist", c.ToString(), today);
                    return true;
                }
            }

            if (!TestForDumpMetadata("labellist",""))
            {
                LabelList ll = vgmdbApiClient.GetLabelList();
                ScrapeLabelList(ll);
                SetDumpMetadata("labellist", "", today);
                return true;
            }

            if (!TestForDumpMetadata("eventlist",""))
            {
                EventList el = vgmdbApiClient.GetEventList();
                ScrapeEventList(el);
                SetDumpMetadata("eventlist", "", today);
                return true;
            }

            if (!TestForDumpMetadata("stats", "", hour))
            {
                InsertHourlyStatistics();
                SetDumpMetadata("stats", "", hour);
            }

            foreach(string section in SECTIONS)
            {
                if (!TestForDumpMetadata("recent",section,hour))
                {
                    UpdateList updateList = vgmdbApiClient.GetUpdateList(section);
                    ScrapeUpdate(updateList);
                    SetDumpMetadata("recent", section, hour);
                    return true;
                }
            }

            int? unscrapedAlbum = FindUnscrapedAlbum();
            if (unscrapedAlbum != null)
            {
                try
                {
                    Album album = vgmdbApiClient.GetAlbum(unscrapedAlbum.Value);
                    ScrapeAlbum(album);
                    return true;
                }
                catch (WebException e)
                {
                    HttpWebResponse hwr = e.Response as HttpWebResponse;
                    if (hwr != null)
                    {
                        if (hwr.StatusCode == HttpStatusCode.NotFound)
                        {
                            logger.Error("Got HTTP 404, marking as error.");
                            SetAlbumScraped(unscrapedAlbum.Value);
                            InsertError("album", unscrapedAlbum.Value, 404);
                            return true;
                        }
                        else if (hwr.StatusDescription.Contains("Time-out"))
                        {
                            logger.Warn("Got HTTP 522, waiting a while.");
                            Throttle();
                            return DumpPass();
                        }
                    }

                    throw e;
                }
            }

            int? unscrapedArtist = FindUnscrapedArtist();
            if (unscrapedArtist != null)
            {
                Artist artist = vgmdbApiClient.GetArtist(unscrapedArtist.Value);
                ScrapeArtist(artist);
                return true;
            }
            
            int? unscrapedEvent = FindUnscrapedEvent();
            if (unscrapedEvent != null)
            {
                Event ev = vgmdbApiClient.GetEventList(unscrapedEvent.Value);
                ScrapeEvent(ev);
                return true;
            }

            int? unscrapedLabel = FindUnscrapedLabel();
            if (unscrapedLabel != null)
            {
                Label label = vgmdbApiClient.GetLabel(unscrapedLabel.Value);
                ScrapeLabel(label);
                return true;
            }

            int? unscrapedRel = FindUnscrapedRelease();
            if (unscrapedRel != null)
            {
                Release release = vgmdbApiClient.GetRelease(unscrapedRel.Value);
                ScrapeRelease(release);
                return true;
            }

            int? unscrapedProd = FindUnscrapedProduct();
            if (unscrapedProd != null)
            {
                Product prod = vgmdbApiClient.GetProduct(unscrapedProd.Value);
                ScrapeProduct(prod);
                return true;
            }

            return false;
        }

        private void InsertHourlyStatistics()
        {
            if (insertHourlyStatistics == null)
            {
                insertHourlyStatistics = connection.CreateCommand();
                insertHourlyStatistics.CommandText =
                    "INSERT INTO dump_vgmdb.\"0statistics\" (albumsDone,albumsTotal,artistsDone,artistsTotal) VALUES (@albumsDone,@albumsTotal,@artistsDone,@artistsTotal)";
                insertHourlyStatistics.Parameters.Add(new NpgsqlParameter("@albumsDone", DbType.Int32));
                insertHourlyStatistics.Parameters.Add(new NpgsqlParameter("@albumsTotal", DbType.Int32));
                insertHourlyStatistics.Parameters.Add(new NpgsqlParameter("@artistsDone", DbType.Int32));
                insertHourlyStatistics.Parameters.Add(new NpgsqlParameter("@artistsTotal", DbType.Int32));
            }

            int albumsDone = QueryInteger("SELECT COUNT(*) FROM dump_vgmdb.albums WHERE scraped = TRUE");
            int albumsTotal = QueryInteger("SELECT COUNT(*) FROM dump_vgmdb.albums");
            int artistsDone = QueryInteger("SELECT COUNT(*) FROM dump_vgmdb.artist WHERE scraped = TRUE");
            int artistsTotal = QueryInteger("SELECT COUNT(*) FROM dump_vgmdb.artist");

            insertHourlyStatistics.Parameters["@albumsDone"].Value = albumsDone;
            insertHourlyStatistics.Parameters["@albumsTotal"].Value = albumsTotal;
            insertHourlyStatistics.Parameters["@artistsDone"].Value = artistsDone;
            insertHourlyStatistics.Parameters["@artistsTotal"].Value = artistsTotal;
            insertHourlyStatistics.ExecuteScalar();

            logger.Info(String.Format("Inserted Hourly statistics: {0} albums to do, {1} artists to do.", albumsTotal - albumsDone, artistsTotal - artistsDone));
        }

        private void ScrapeUpdate(UpdateList list)
        {
            foreach (UpdateListEntry update in list.updates)
            {
                if (update.WasDeleted())
                    continue;

                UpdateType ut = update.GetUpdateType();
                switch (ut)
                {
                    case UpdateType.Album:
                        if (!TestForAlbum(update.GetId()))
                            InsertAlbum((AlbumListAlbum)update.Convert());
                        break;
                    case UpdateType.Artist:
                        if (!TestForArtist(update.GetId()))
                            InsertArtist((ArtistListArtist)update.Convert());
                        break;
                    case UpdateType.Label:
                        if (!TestForLabel(update.GetId()))
                            InsertLabel((LabelListLabel)update.Convert(), null, null, null);
                        break;
                    case UpdateType.Product:
                        if (!TestForProduct(update.GetId()))
                            InsertProduct((ProductListProduct)update.Convert());
                        break;
                    case UpdateType.Invalid:
                        break;
                    default:
                        throw new NotImplementedException(ut.ToString());
                }
            }
        }

        private NpgsqlCommand insertErrorCommand;
        private void InsertError(string type, int itemId, int code)
        {
            if (insertErrorCommand == null)
            {
                insertErrorCommand = connection.CreateCommand();
                insertErrorCommand.CommandText = "INSERT INTO dump_vgmdb.\"0errors\" (type,item_id,code)" +
                                                 "VALUES (@type,@itemId,@code)";
                insertErrorCommand.Parameters.Add("@type", NpgsqlDbType.Varchar);
                insertErrorCommand.Parameters.Add("@itemId", NpgsqlDbType.Integer);
                insertErrorCommand.Parameters.Add("@code", NpgsqlDbType.Integer);
            }

            insertErrorCommand.Parameters["@type"].Value = type;
            insertErrorCommand.Parameters["@itemId"].Value = itemId;
            insertErrorCommand.Parameters["@code"].Value = code;
            insertErrorCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand setAlbumScrapedCommand;
        private void SetAlbumScraped(int album)
        {
            if (setAlbumScrapedCommand == null)
            {
                setAlbumScrapedCommand = connection.CreateCommand();
                setAlbumScrapedCommand.CommandText = "UPDATE dump_vgmdb.albums " +
                                                     "SET scraped=TRUE " +
                                                     "WHERE id=@id";
                setAlbumScrapedCommand.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            }
            setAlbumScrapedCommand.Parameters["@id"].Value = album;
            int result = setAlbumScrapedCommand.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("unexpected sql update result");
        }

        private void ScrapeAlbum(Album album)
        {
            if (updateAlbum == null)
            {
                updateAlbum = connection.CreateCommand();
                updateAlbum.CommandText =
                    "UPDATE dump_vgmdb.albums " +
                    "SET catalog=@catalog, classificationId=@classificationId, mediaFormatId=@mediaFormatId, " +
                    "    meta_added_date=@added_date, meta_edited_date=@edited_date, meta_fetched_date=@fetched_date, meta_ttl=@ttl, " +
                    "    meta_visitors=@visitors, name=@name, notes=@notes, picture_full=@picture_full, " +
                    "    publishFormatId=@publishFormatId, publisherId=@publisherId, rating=@rating, release_date=@release_date, " +
                    "    release_currency=@release_currency, release_price=@release_price, votes=@votes, scraped=TRUE " +
                    "WHERE id=@id";
                updateAlbum.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@classificationId", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@mediaFormatId", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@added_date", DbType.DateTime));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@edited_date", DbType.DateTime));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@fetched_date", DbType.DateTime));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@ttl", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@visitors", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@notes", DbType.String));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@picture_full", NpgsqlDbType.Bytea));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@publishFormatId", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@publisherId", DbType.Int32));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@rating", DbType.Double));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@release_date", DbType.Date));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@release_currency", DbType.String));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@release_price", DbType.Double));
                updateAlbum.Parameters.Add(new NpgsqlParameter("@votes", DbType.Int32));
            }

            updateAlbum.Parameters["@catalog"].Value = album.catalog;
            updateAlbum.Parameters["@id"].Value = album.GetId();

            int artistTypeId = GetAlbumArtistTypeId("Arranger");
            DoArtistList(artistTypeId, album.GetId(), album.arrangers);

            int classification = GetAlbumClassificationId(album.classification);
            updateAlbum.Parameters["@classificationId"].Value = classification;

            artistTypeId = GetAlbumArtistTypeId("Composer");
            DoArtistList(artistTypeId, album.GetId(), album.composers);

            if (album.covers.Length > 0)
            {
                for (int i = 0; i < album.covers.Length; i++)
                {
                    AlbumCover cover = album.covers[i];
                    if (cover.name.Equals("Front") && !string.IsNullOrEmpty(album.picture_full))
                        continue;

                    byte[] buffer = new byte[0];
                    if (!string.IsNullOrEmpty(cover.full))
                    {
                        buffer = vgmdbApiClient.DownloadGraphic(cover.full);
                        InsertAlbumCover(album.GetId(), cover.name, buffer, i);
                        Throttle();
                    }
                }
            }

            for (int i = 0; i < album.discs.Length; i++)
            {
                AlbumDisc disc = album.discs[i];
                InsertAlbumDisc(album.GetId(), i, disc.name);
                for (int j = 0; j < disc.tracks.Length; j++)
                {
                    AlbumDiscTrack track = disc.tracks[j];
                    InsertAlbumDiscTrack(album.GetId(), i, j, track.GetTrackLengthSeconds());
                    foreach(KeyValuePair<string,string> title in track.names)
                    {
                        InsertAlbumDiscTrackTranslation(album.GetId(), i, j, title.Key, title.Value);
                    }
                }
            }

            artistTypeId = GetAlbumArtistTypeId("Lyricist");
            DoArtistList(artistTypeId, album.GetId(), album.lyricists);

            int mediaFormatId = GetAlbumMediaFormatId(album.media_format);
            updateAlbum.Parameters["@mediaFormatId"].Value = mediaFormatId;

            updateAlbum.Parameters["@added_date"].Value = album.meta.added_date;
            updateAlbum.Parameters["@edited_date"].Value = album.meta.edited_date;
            updateAlbum.Parameters["@fetched_date"].Value = album.meta.fetched_date;
            updateAlbum.Parameters["@ttl"].Value = album.meta.ttl;
            updateAlbum.Parameters["@visitors"].Value = album.meta.visitors;
            updateAlbum.Parameters["@name"].Value = album.name;

            foreach(KeyValuePair<string,string> lang in album.names)
            {
                if (!TestForAlbumTitle(album.GetId(),lang.Key))
                {
                    InsertAlbumTitle(album.GetId(), lang.Key, lang.Value);
                }
            }

            if (album.notes.Equals("No notes available for this album."))
                album.notes = "";
            updateAlbum.Parameters["@notes"].Value = album.notes;

            for (int i = 0; i < album.organizations.Length; i++)
            {
                AlbumOrganization organization = album.organizations[i];
                int roleId = GetAlbumLabelRoleId(organization.role);

                if (!string.IsNullOrEmpty(organization.link))
                {
                    bool labelTwice = false;
                    if (i != 0)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (album.organizations[i].Equals(album.organizations[j]))
                            {
                                labelTwice = true;
                                logger.Warn(String.Format("Label #{0} is linked multiple times to Album #{1} as a {2}",organization.GetId(),album.GetId(),organization.role));
                                break;
                            }
                        }
                    }

                    if (labelTwice)
                        continue;

                    InsertAlbumLabel(album.GetId(), organization.GetId(), roleId);

                    if (!TestForLabel(organization.GetId()))
                    {
                        LabelListLabel label = new LabelListLabel();
                        label.names = organization.names;
                        label.link = organization.link;
                        InsertLabel(label, null, null, null);
                    }
                }
                else
                {
                    InsertAlbumArbituraryLabel(album.GetId(), i, roleId, organization.names.First().Value);
                }
            }

            artistTypeId = GetAlbumArtistTypeId("Performer");
            DoArtistList(artistTypeId, album.GetId(), album.performers);

            if (!string.IsNullOrEmpty(album.picture_full))
            {
                byte[] picture_full = vgmdbApiClient.DownloadGraphic(album.picture_full);
                updateAlbum.Parameters["@picture_full"].Value = picture_full;
                Throttle();
            }
            else
            {
                updateAlbum.Parameters["@picture_full"].Value = DBNull.Value;
            }

            int publishFormatId = GetAlbumPublishFormatId(album.publish_format);
            updateAlbum.Parameters["@publishFormatId"].Value = publishFormatId;

            if (album.publisher != null)
            {
                if (!string.IsNullOrEmpty(album.publisher.link))
                {
                    updateAlbum.Parameters["@publisherId"].Value = album.publisher.GetId();
                }
                else
                {
                    updateAlbum.Parameters["@publisherId"].Value = DBNull.Value;
                }
            }
            else
            {
                updateAlbum.Parameters["@publisherId"].Value = DBNull.Value;
            }

            updateAlbum.Parameters["@rating"].Value = album.rating;

            foreach(AlbumListAlbum relatedAlbum in album.related)
            {
                if (!TestForAlbum(relatedAlbum.GetId()))
                    InsertAlbum(relatedAlbum);

                InsertAlbumRelatedAlbum(album.GetId(), relatedAlbum.GetId());
            }

            try
            {
                DateTime release_date = DateTime.Parse(album.release_date);
                updateAlbum.Parameters["@release_date"].Value = release_date;
            }
            catch (Exception e)
            {
                updateAlbum.Parameters["@release_date"].Value = DBNull.Value;
            }

            foreach(EventListEvent ev in album.release_events)
            {
                InsertAlbumEvent(album.GetId(), ev.GetId());

                if (!TestForEvent(ev.GetId()))
                    InsertEvent(ev, 0);
            }

            double? releasePrice = album.release_price.ParsePrice();
            if (releasePrice != null)
            {
                updateAlbum.Parameters["@release_currency"].Value = album.release_price.currency;
                updateAlbum.Parameters["@release_price"].Value = releasePrice;
            }
            else
            {
                updateAlbum.Parameters["@release_currency"].Value = DBNull.Value;
                updateAlbum.Parameters["@release_price"].Value = DBNull.Value;
            }

            if (album.reprints.Length > 0)
            {
                foreach (AlbumListAlbum reprint in album.reprints)
                {
                    if (reprint.link.Equals("#"))
                        continue;

                    if (!TestForAlbum(reprint.GetId()))
                        InsertAlbum(reprint);

                    InsertAlbumReprint(album.GetId(), reprint.GetId());
                }
            }

            updateAlbum.Parameters["@votes"].Value = album.votes;

            if (album.products != null)
            {
                for (int i = 0; i < album.products.Length; i++)
                {
                    ProductListProduct product = album.products[i];
                    if (!product.IsRelease())
                    {
                        if (!string.IsNullOrEmpty(product.link) && !product.link.StartsWith("search"))
                        {
                            if (!TestForProduct(product.GetId()))
                                InsertProduct(product);
                        }
                        else
                        {
                            InsertProductArbituary(album.GetId(), i, product.names.First().Value);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            List<string> urls = new List<string>();

            if (album.stores != null)
            {
                foreach(Website store in album.stores)
                {
                    if (!urls.Contains(store.link))
                    {
                        InsertAlbumWebsite(album.GetId(), "stores", store.name, store.link);
                        urls.Add(store.link);
                    }
                }
            }

            if (album.websites != null)
            {
                foreach (KeyValuePair<string, Website[]> catalog in album.websites)
                {
                    foreach (Website website in catalog.Value)
                    {
                        if (!urls.Contains(website.link))
                        {
                            InsertAlbumWebsite(album.GetId(), catalog.Key, website.name, website.link);
                            urls.Add(website.link);
                        }
                    }
                }
            }

            updateAlbum.ExecuteNonQuery();
        }

        private void DoArtistList(int artistTypeId, int albumId, ArtistListArtist[] artists)
        {
            if (artists.Length == 0)
                return;

            List<string> arbitraries = new List<string>();

            for (int i = 0; i < artists.Length; i++)
            {
                ArtistListArtist artist = artists[i];

                if (artist.IsNatural())
                {
                    if (!TestForArtist(artist.GetId()))
                        InsertArtist(artist);

                    InsertAlbumArtist(artist.GetId(), albumId, artistTypeId);
                }
                else
                {
                    string name = artist.names.First().Value;
                    if (!arbitraries.Contains(name))
                    {
                        InsertArbitraryAlbumArtist(albumId, artistTypeId, name);
                        arbitraries.Add(name);
                    }
                }
            }
        }

        private void ScrapeArtist(Artist artist)
        {
            if (updateArtist == null)
            {
                updateArtist = connection.CreateCommand();
                updateArtist.CommandText =
                    "UPDATE dump_vgmdb.artist " +
                    "SET birthdate=@birthdate, birthplace=@birthplace, meta_added_date=@added_date, meta_edited_date=@edited_date, " +
                    "    meta_fetched_date=@fetched_date, meta_ttl=@ttl, meta_visitors=@visitors, name=@name, notes=@notes," +
                    "    picture_full=@picture_full, isFemale=@isFemale, typeId=@typeId, scraped=TRUE " +
                    "WHERE id=@id";
                updateArtist.Parameters.Add(new NpgsqlParameter("@birthdate", DbType.Date));
                updateArtist.Parameters.Add(new NpgsqlParameter("@birthplace", DbType.String));
                updateArtist.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                updateArtist.Parameters.Add(new NpgsqlParameter("@added_date", DbType.DateTime));
                updateArtist.Parameters.Add(new NpgsqlParameter("@edited_date", DbType.DateTime));
                updateArtist.Parameters.Add(new NpgsqlParameter("@fetched_date", DbType.DateTime));
                updateArtist.Parameters.Add(new NpgsqlParameter("@ttl", DbType.Int32));
                updateArtist.Parameters.Add(new NpgsqlParameter("@visitors", DbType.Int32));
                updateArtist.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                updateArtist.Parameters.Add(new NpgsqlParameter("@notes", DbType.String));
                updateArtist.Parameters.Add(new NpgsqlParameter("@picture_full", NpgsqlDbType.Bytea));
                updateArtist.Parameters.Add(new NpgsqlParameter("@isFemale", DbType.Boolean));
                updateArtist.Parameters.Add(new NpgsqlParameter("@typeId", DbType.Int32));
            }

            if (artist.aliases != null)
            {
                for (int i = 0; i < artist.aliases.Length; i++)
                {
                    foreach(KeyValuePair<string,string> lang in artist.aliases[i].names)
                    {
                        InsertArtistAlias(artist.GetId(), i, lang.Key, lang.Value);
                    }
                }
            }

            if (!string.IsNullOrEmpty(artist.birthdate))
            {
                try
                {
                    DateTime birthDate = DateTime.Parse(artist.birthdate);
                    updateArtist.Parameters["@birthdate"].Value = birthDate;
                }
                catch (Exception e)
                {
                    updateArtist.Parameters["@birthdate"].Value = DBNull.Value;
                }
            }
            else
                updateArtist.Parameters["@birthdate"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(artist.birthplace))
                updateArtist.Parameters["@birthplace"].Value = artist.birthplace;
            else
                updateArtist.Parameters["@birthplace"].Value = DBNull.Value;

            updateArtist.Parameters["@id"].Value = artist.GetId();

            foreach(AlbumListAlbum album in artist.discography)
            {
                if (!TestForAlbum(album.GetId()))
                    InsertAlbum(album);
            }
            foreach (AlbumListAlbum featureAlbum in artist.featured_on)
            {
                InsertArtistFeature(artist.GetId(), featureAlbum.GetId());

                if (!TestForAlbum(featureAlbum.GetId()))
                    InsertAlbum(featureAlbum);
            }

            updateArtist.Parameters["@added_date"].Value = artist.meta.added_date;
            updateArtist.Parameters["@edited_date"].Value = artist.meta.edited_date;
            updateArtist.Parameters["@fetched_date"].Value = artist.meta.fetched_date;
            updateArtist.Parameters["@ttl"].Value = artist.meta.ttl;
            updateArtist.Parameters["@visitors"].Value = artist.meta.visitors;
            updateArtist.Parameters["@name"].Value = artist.name;

            if (artist.notes.Equals("No notes available for this artist."))
                artist.notes = "";
            updateArtist.Parameters["@notes"].Value = artist.notes;

            if (!string.IsNullOrEmpty(artist.picture_full))
            {
                byte[] artistPic = vgmdbApiClient.DownloadGraphic(artist.picture_full);
                updateArtist.Parameters["@picture_full"].Value = artistPic;
                Throttle();
            }
            else
                updateArtist.Parameters["@picture_full"].Value = DBNull.Value;

            updateArtist.Parameters["@isFemale"].Value = "female".Equals(artist.sex);
            updateArtist.Parameters["@typeId"].Value = GetArtistTypeId(artist.type);

            List<string> knownWebsites = new List<string>();
            foreach (KeyValuePair<string, Website[]> catalog in artist.websites)
            {
                foreach (Website website in catalog.Value)
                {
                    if (!knownWebsites.Contains(website.name))
                    {
                        InsertArtistWebsite(artist.GetId(), catalog.Key, website.name, website.link);
                        knownWebsites.Add(website.name);
                    }
                    else
                    {
                        logger.Warn(String.Format("Website {0} for artist {1} exists twice.",website.name,artist.name));
                    }
                }
            }

            updateArtist.ExecuteNonQuery();
        }

        private void ScrapeEvent(Event ev)
        {
            if (updateEvent == null)
            {
                updateEvent = connection.CreateCommand();
                updateEvent.CommandText =
                    "UPDATE dump_vgmdb.events " +
                    "SET enddate=@enddate, name=@name, notes=@notes, startdate=@startdate, scraped = TRUE " +
                    "WHERE id=@id";
                updateEvent.Parameters.Add(new NpgsqlParameter("@enddate", DbType.Date));
                updateEvent.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                updateEvent.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                updateEvent.Parameters.Add(new NpgsqlParameter("@notes", DbType.String));
                updateEvent.Parameters.Add(new NpgsqlParameter("@startdate", DbType.Date));
            }

            DateTime enddate = DateTime.Parse(ev.enddate);
            updateEvent.Parameters["@enddate"].Value = enddate;

            updateEvent.Parameters["@id"].Value = ev.GetId();
            updateEvent.Parameters["@name"].Value = ev.name;
            updateEvent.Parameters["@notes"].Value = ev.notes;

            foreach(AlbumListAlbum album in ev.releases)
            {
                album.type = album.album_type;
                InsertEventRelease(ev.GetId(), album.GetId());

                if (!TestForAlbum(album.GetId()))
                {
                    InsertAlbum(album);
                }
            }

            DateTime startdate = DateTime.Parse(ev.startdate);
            updateEvent.Parameters["@startdate"].Value = startdate;

            updateEvent.ExecuteNonQuery();
        }

        private void ScrapeLabel(Label label)
        {
            if (label.description.Equals("No description available"))
                label.description = "";

            if (updateLabel == null)
            {
                updateLabel = connection.CreateCommand();
                updateLabel.CommandText =
                    "UPDATE dump_vgmdb.labels " +
                    "SET description=@description, meta_added_date=@added_date, meta_edited_date=@edited_date, meta_fetched_date=@fetched_date, " +
                    "    meta_ttl=@ttl, meta_visitors=@visitors, name=@name, regionid=@regionid, type=@type, scraped=TRUE " +
                    "WHERE id=@id";
                updateLabel.Parameters.Add(new NpgsqlParameter("@description",DbType.String));
                updateLabel.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                updateLabel.Parameters.Add(new NpgsqlParameter("@added_date", DbType.DateTime));
                updateLabel.Parameters.Add(new NpgsqlParameter("@edited_date", DbType.DateTime));
                updateLabel.Parameters.Add(new NpgsqlParameter("@fetched_date", DbType.DateTime));
                updateLabel.Parameters.Add(new NpgsqlParameter("@ttl", DbType.Int32));
                updateLabel.Parameters.Add(new NpgsqlParameter("@visitors", DbType.Int32));
                updateLabel.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                updateLabel.Parameters.Add(new NpgsqlParameter("@regionid", DbType.Int32));
                updateLabel.Parameters.Add(new NpgsqlParameter("@type", DbType.Int32));
            }
            updateLabel.Parameters["@description"].Value = label.description;
            updateLabel.Parameters["@id"].Value = label.GetId();
            updateLabel.Parameters["@added_date"].Value = label.meta.added_date;
            updateLabel.Parameters["@edited_date"].Value = label.meta.edited_date;
            updateLabel.Parameters["@fetched_date"].Value = label.meta.fetched_date;
            updateLabel.Parameters["@ttl"].Value = label.meta.ttl;
            updateLabel.Parameters["@visitors"].Value = label.meta.visitors;
            updateLabel.Parameters["@name"].Value = label.name;

            int regionId = GetLabelRegionId(label.region);
            updateLabel.Parameters["@regionid"].Value = regionId;

            List<string> releasedOnce = new List<string>();
            foreach(AlbumListAlbum album in label.releases)
            {
                if (releasedOnce.Contains(album.link))
                    continue;

                InsertLabelRelease(label.GetId(), album.GetId());

                if (!TestForAlbum(album.GetId()))
                    InsertAlbum(album);

                releasedOnce.Add(album.link);
            }

            foreach (LabelStaff staff in label.staff)
            {
                InsertLabelStaff(label.GetId(), staff.GetId(), staff.owner);

                if (!TestForArtist(staff.GetId()))
                {
                    KeyValuePair<string, string> firstName = staff.names.First();
                    InsertArtist(staff.GetId(), firstName.Key, firstName.Value, null);
                }
            }

            int type = GetLabelTypeId(label.type);
            updateLabel.Parameters["@type"].Value = type;

            foreach (KeyValuePair<string, Website[]> catalog in label.websites)
            {
                foreach (Website website in catalog.Value)
                {
                    InsertLabelWebsite(label.GetId(), catalog.Key, website.name, website.link);
                }
            }

            updateLabel.ExecuteNonQuery();
        }

        private void ScrapeRelease(Release release)
        {
            if (updateRelease == null)
            {
                updateRelease = connection.CreateCommand();
                updateRelease.CommandText =
                    "UPDATE dump_vgmdb.product_releases " +
                    "SET catalog=@catalog, meta_added_date=@added_date, meta_added_user=@added_user, meta_edited_date=@edited_date, " +
                    "    meta_edited_user=@edited_user, meta_fetched_date=@fetched_date, meta_ttl=@ttl, meta_visitors=@visitors, name=@name, " +
                    "    platformid=@platformid, regionid=@regionid, release_date=@release_date, release_type=@release_type, type=@type," +
                    "    scraped=TRUE, upc=@upc " +
                    "WHERE id=@id";
                updateRelease.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
                updateRelease.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@added_date", DbType.DateTime));
                updateRelease.Parameters.Add(new NpgsqlParameter("@added_user", DbType.String));
                updateRelease.Parameters.Add(new NpgsqlParameter("@edited_date", DbType.DateTime));
                updateRelease.Parameters.Add(new NpgsqlParameter("@edited_user", DbType.String));
                updateRelease.Parameters.Add(new NpgsqlParameter("@fetched_date", DbType.DateTime));
                updateRelease.Parameters.Add(new NpgsqlParameter("@ttl", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@visitors", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                updateRelease.Parameters.Add(new NpgsqlParameter("@platformid", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@regionid", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@release_date", DbType.Date));
                updateRelease.Parameters.Add(new NpgsqlParameter("@release_type", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@type", DbType.Int32));
                updateRelease.Parameters.Add(new NpgsqlParameter("@upc", DbType.String));
            }
            updateRelease.Parameters["@catalog"].Value = release.catalog;
            updateRelease.Parameters["@id"].Value = release.GetId();
            updateRelease.Parameters["@added_date"].Value = release.meta.added_date;
            updateRelease.Parameters["@added_user"].Value = release.meta.added_user;
            updateRelease.Parameters["@edited_date"].Value = release.meta.edited_date;
            updateRelease.Parameters["@edited_user"].Value = release.meta.edited_user;
            updateRelease.Parameters["@fetched_date"].Value = release.meta.fetched_date;
            updateRelease.Parameters["@ttl"].Value = release.meta.ttl;
            updateRelease.Parameters["@visitors"].Value = release.meta.visitors;
            updateRelease.Parameters["@name"].Value = release.name;

            int platformId = GetPlatformId(release.platform);
            updateRelease.Parameters["@platformid"].Value = platformId;

            foreach (AlbumListAlbum album in release.product_albums)
            {
                if (!TestForAlbum(album.GetId()))
                    InsertAlbum(album);
            }

            int regionId = GetRegionId(release.region);
            updateRelease.Parameters["@regionid"].Value = regionId;

            foreach(AlbumListAlbum album in release.release_albums)
            {
                InsertReleaseAlbum(release.GetId(), album.GetId());

                if (!TestForAlbum(album.GetId()))
                    InsertAlbum(album);
            }

            if (release.release_date != null)
            {
                if (release.release_date.Length > 4)
                {
                    DateTime releaseDate = DateTime.Parse(release.release_date);
                    updateRelease.Parameters["@release_date"].Value = releaseDate;
                }
                else
                {
                    updateRelease.Parameters["@release_date"].Value = DBNull.Value;
                }
            }
            else
            {
                updateRelease.Parameters["@release_date"].Value = DBNull.Value;
            }

            int releaseType = GetReleaseType(release.release_type);
            updateRelease.Parameters["@release_type"].Value = releaseType;

            int type = GetProductTypeId(release.type);
            updateRelease.Parameters["@type"].Value = type;

            updateRelease.Parameters["@upc"].Value = release.upc;
            updateRelease.ExecuteNonQuery();
        }

        private void ScrapeProduct(Product product)
        {
            if (updateProduct == null)
            {
                updateProduct = connection.CreateCommand();
                updateProduct.CommandText = "UPDATE dump_vgmdb.products " +
                    "SET description=@description, meta_added_date=@added_date, meta_added_user=@added_user, meta_edited_date=@edited_date, " +
                    "    meta_edited_user=@edited_user, meta_fetched_date=@fetched_date, meta_ttl=@ttl, meta_visitors=@visitors, " +
                    "    name_real=@name_real, release_date=@release_date, typeid=@type, scraped=TRUE, picture_full=@picture_full, " +
                    "    name=@name, parent_franchise=@parent_franchise " +
                    "WHERE id=@id";
                updateProduct.Parameters.Add(new NpgsqlParameter("@description", DbType.String));
                updateProduct.Parameters.Add(new NpgsqlParameter("@added_date", DbType.DateTime));
                updateProduct.Parameters.Add(new NpgsqlParameter("@added_user", DbType.String));
                updateProduct.Parameters.Add(new NpgsqlParameter("@edited_date", DbType.DateTime));
                updateProduct.Parameters.Add(new NpgsqlParameter("@edited_user", DbType.String));
                updateProduct.Parameters.Add(new NpgsqlParameter("@fetched_date", DbType.DateTime));
                updateProduct.Parameters.Add(new NpgsqlParameter("@ttl", DbType.Int32));
                updateProduct.Parameters.Add(new NpgsqlParameter("@visitors", DbType.Int32));
                updateProduct.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                updateProduct.Parameters.Add(new NpgsqlParameter("@name_real", DbType.String));
                updateProduct.Parameters.Add(new NpgsqlParameter("@picture_full", NpgsqlDbType.Bytea));
                updateProduct.Parameters.Add(new NpgsqlParameter("@release_date", DbType.Date));
                updateProduct.Parameters.Add(new NpgsqlParameter("@type", DbType.Int32));
                updateProduct.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                updateProduct.Parameters.Add(new NpgsqlParameter("@parent_franchise", DbType.Int32));
            }
            
            if (product.albums == null)
                product.albums = new AlbumListAlbum[0];

            foreach (AlbumListAlbum album in product.albums)
            {
                InsertProductAlbum(product.GetId(), album.GetId());

                if (TestForAlbum(album.GetId()))
                    continue;

                InsertAlbum(album);
            }
            updateProduct.Parameters["@description"].Value = product.description;

            updateProduct.Parameters["@parent_franchise"].Value = DBNull.Value;
            if (product.franchises != null)
            {
                foreach (ProductListProduct franchise in product.franchises)
                {
                    if (!TestForProduct(franchise.GetId()))
                    {
                        InsertProduct(franchise);
                    }
                    updateProduct.Parameters["@parent_franchise"].Value = franchise.GetId();
                }
            }

            updateProduct.Parameters["@id"].Value = product.GetId();
            if (product.meta.added_date != null)
                updateProduct.Parameters["@added_date"].Value = product.meta.added_date;
            else
                updateProduct.Parameters["@added_date"].Value = DBNull.Value;

            if (product.meta.added_user != null)
                updateProduct.Parameters["@added_user"].Value = product.meta.added_user;
            else
                updateProduct.Parameters["@added_user"].Value = DBNull.Value;

            updateProduct.Parameters["@edited_date"].Value = product.meta.edited_date;

            if (product.meta.edited_user != null)
                updateProduct.Parameters["@edited_user"].Value = product.meta.edited_user;
            else
                updateProduct.Parameters["@edited_user"].Value = DBNull.Value;

            updateProduct.Parameters["@fetched_date"].Value = product.meta.fetched_date;
            updateProduct.Parameters["@ttl"].Value = product.meta.ttl;
            updateProduct.Parameters["@visitors"].Value = product.meta.visitors;

            if (!string.IsNullOrEmpty(product.name_real))
                updateProduct.Parameters["@name_real"].Value = product.name_real;
            else
                updateProduct.Parameters["@name_real"].Value = DBNull.Value;

            updateProduct.Parameters["@name"].Value = product.name;

            if (product.organizations != null)
            {
                foreach (LabelListLabel organisation in product.organizations)
                {
                    string name = organisation.names.First().Value;
                    int? orgaId = FindLabelIdByName(name);
                    if (orgaId.HasValue)
                    {
                        InsertProductLabel(product.GetId(), orgaId.Value);
                    }
                    else
                    {
                        InsertArbitaryProductLabel(product.GetId(), organisation.names.First().Value);
                    }
                }
            }

            if (!string.IsNullOrEmpty(product.picture_full))
            {
                byte[] pic = vgmdbApiClient.DownloadGraphic(product.picture_full);
                updateProduct.Parameters["@picture_full"].Value = pic;
                Throttle();
            }
            else
            {
                updateProduct.Parameters["@picture_full"].Value = DBNull.Value;
            }

            try
            {
                DateTime releaseDate = DateTime.Parse(product.release_date);
                updateProduct.Parameters["@release_date"].Value = releaseDate;
            }
            catch (Exception e)
            {
                updateProduct.Parameters["@release_date"].Value = DBNull.Value;
            }

            if (product.releases != null)
            {
                for (int i = 0; i < product.releases.Length; i++)
                {
                    if (product.releases[i].link != null)
                        InsertRelease(product.releases[i]);
                    else
                        foreach (KeyValuePair<string, string> lang in product.releases[i].names)
                            InsertArbitaryRelease(product.GetId(), i, lang.Key, lang.Value);
                }
            }

            int productTypeId = GetProductTypeId(product.type);
            updateProduct.Parameters["@type"].Value = productTypeId;

            List<string> alreadyKnownWebsites = new List<string>();
            foreach (KeyValuePair<string, Website[]> catalog in product.websites)
            {
                foreach (Website website in catalog.Value)
                {
                    if (!alreadyKnownWebsites.Contains(website.link))
                    {
                        InsertProductWebsite(product.GetId(), catalog.Key, website.name, website.link);
                        alreadyKnownWebsites.Add(website.link);
                    }
                }
            }

            updateProduct.ExecuteNonQuery();
        }

        private void ScrapeProductList(ProductList pl)
        {
            foreach(ProductListProduct product in pl.products)
            {
                if (!TestForProduct(product.GetId()))
                {
                    InsertProduct(product.GetId(), product.names.First().Value, GetProductTypeId(product.type));
                }
            }
        }

        private void ScrapeEventList(EventList el)
        {
            foreach(KeyValuePair<string,EventListEvent[]> yearly in el.events)
            {
                int year = Convert.ToInt32(yearly.Key);
                foreach(EventListEvent ev in yearly.Value)
                {
                    if (!TestForEvent(ev.GetId()))
                    {
                        InsertEvent(ev, year);
                    }
                }
            }
        }

        private void ScrapeLabelList(LabelList ll)
        {
            foreach(KeyValuePair<string,LabelListLabel[]> lettering in ll.orgs)
            {
                foreach(LabelListLabel label in lettering.Value)
                {
                    if (!TestForLabel(label.GetId()))
                    {
                        InsertLabel(label, null, null, null);
                    }
                }
            }
        }
        
        private void ScrapeArtistList(ArtistList al)
        {
            foreach (ArtistListArtist artist in al.artists)
            {
                if (TestForArtist(artist.GetId()))
                    continue;

                InsertArtist(artist);
            }
        }

        private void ScrapeAlbumList(AlbumList al)
        {
            foreach (AlbumListAlbum album in al.albums)
            {
                if (TestForAlbum(album.GetId()))
                    continue;
                
                InsertAlbum(album);
            }
        }

        private void Throttle()
        {
            if (rng == null)
            {
                rng = new Random();
            }

            double quotient = rng.NextDouble() + 0.5;
            int waitTime = (int)((double)THROTTLE_RATE * quotient);
            System.Threading.Thread.Sleep(waitTime);
        }

        private ILog logger;
        private NpgsqlConnection connection;
        private NpgsqlCommand testForDumpMetadataWithoutDate;
        private NpgsqlCommand testForDumpMetadata;
        private NpgsqlCommand setMetadata;
        private Random rng;
        private NpgsqlTransaction npgsqlTransaction;
        private NpgsqlCommand testForAlbum;
        private NpgsqlCommand insertAlbumTitle;
        private NpgsqlCommand testForAlbumType;
        private NpgsqlCommand insertAlbumType;
        private NpgsqlCommand insertAlbum;
        private VgmdbApiClient vgmdbApiClient;
        private NpgsqlCommand testForArtist;
        private NpgsqlCommand insertArtist;
        private NpgsqlCommand testForLabel;
        private NpgsqlCommand insertLabel;
        private NpgsqlCommand insertEvent;
        private NpgsqlCommand insertEventTranslation;
        private NpgsqlCommand testForEvent;
        private NpgsqlCommand getProductTypeId;
        private NpgsqlCommand insertProductType;
        private NpgsqlCommand insertProduct;
        private NpgsqlCommand testForProduct;
        private NpgsqlCommand updateProduct;
        private NpgsqlCommand findLabelIdByName;
        private NpgsqlCommand insertProductLabel;
        private NpgsqlCommand insertRelease;
        private NpgsqlCommand getPlatformId;
        private NpgsqlCommand insertPlatform;
        private NpgsqlCommand getRegionId;
        private NpgsqlCommand insertRegion;
        private NpgsqlCommand insertReleaseTranslation;
        private NpgsqlCommand insertProductWebsite;
        private NpgsqlCommand findUnscrapedProduct;
        private NpgsqlCommand insertArbitaryProductLabel;
        private int arbitaryLabelId = 0;
        private NpgsqlCommand testForReleaseTranslation;
        private NpgsqlCommand findUnscrapedRelease;
        private NpgsqlCommand markReleaseAsScraped;
        private NpgsqlCommand insertProductAlbum;
        private NpgsqlCommand updateRelease;
        private NpgsqlCommand insertArbitaryProductRelease;
        private NpgsqlCommand testForArbitaryProductLabel;
        private NpgsqlCommand insertReleaseAlbum;
        private NpgsqlCommand getReleaseTypeId;
        private NpgsqlCommand insertReleaseType;
        private NpgsqlCommand updateLabel;
        private NpgsqlCommand getLabelRegionId;
        private NpgsqlCommand insertLabelRegion;
        private NpgsqlCommand insertLabelRelease;
        private NpgsqlCommand insertLabelStaff;
        private NpgsqlCommand getLabelTypeId;
        private NpgsqlCommand insertLabelType;
        private NpgsqlCommand insertLabelWebsite;
        private NpgsqlCommand findUnscrapedLabel;
        private NpgsqlCommand updateEvent;
        private NpgsqlCommand insertEventRelease;
        private NpgsqlCommand findUnscrapedEvent;
        private NpgsqlCommand updateArtist;
        private NpgsqlCommand insertArtistFeature;
        private NpgsqlCommand getArtistTypeId;
        private NpgsqlCommand insertArtistType;
        private NpgsqlCommand insertArtistWebsite;
        private NpgsqlCommand findUnscrapedArtist;
        private NpgsqlCommand updateAlbum;
        private NpgsqlCommand findAlbumArtistTypeId;
        private NpgsqlCommand insertAlbumArtistType;
        private NpgsqlCommand insertAlbumArtist;
        private NpgsqlCommand insertArbitraryAlbumArtist;
        private NpgsqlCommand findAlbumClassificationId;
        private NpgsqlCommand insertAlbumClassificationId;
        private NpgsqlCommand insertAlbumDisc;
        private NpgsqlCommand insertAlbumDiscTrack;
        private NpgsqlCommand insertAlbumDiscTrackTranslation;
        private NpgsqlCommand getAlbumMediaFormatId;
        private NpgsqlCommand insertAlbumMediaFormat;
        private NpgsqlCommand testForAlbumTitle;
        private NpgsqlCommand getAlbumLabelRoleId;
        private NpgsqlCommand insertAlbumLabelRole;
        private NpgsqlCommand insertAlbumLabel;
        private NpgsqlCommand getAlbumPublishFormatId;
        private NpgsqlCommand insertAlbumPublishFormat;
        private NpgsqlCommand insertAlbumEvent;
        private NpgsqlCommand findUnscrapedAlbum;
        private NpgsqlCommand insertAlbumCover;
        private NpgsqlCommand insertAlbumRelatedAlbum;
        private NpgsqlCommand insertAlbumReprint;
        private NpgsqlCommand insertAlbumWebsite;
        private NpgsqlCommand insertAlbumArbituraryProduct;
        private NpgsqlCommand insertAlbumArbituraryLabel;
        private NpgsqlCommand testForAlbumArtist;
        private NpgsqlCommand insertArtistAlias;
        private NpgsqlCommand insertHourlyStatistics;

        private void PrepareStatements()
        {
            testForProduct = connection.CreateCommand();
            testForProduct.CommandText = "SELECT dateAdded FROM dump_vgmdb.products WHERE id=@id";
            testForProduct.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertProduct = connection.CreateCommand();
            insertProduct.CommandText = "INSERT INTO dump_vgmdb.products (id,name,typeId) VALUES (@id,@name,@typeId)";
            insertProduct.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertProduct.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertProduct.Parameters.Add(new NpgsqlParameter("@typeId", DbType.Int32));

            getProductTypeId = connection.CreateCommand();
            getProductTypeId.CommandText = "SELECT id FROM dump_vgmdb.product_types WHERE name=@name";
            getProductTypeId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));

            insertProductType = connection.CreateCommand();
            insertProductType.CommandText = "INSERT INTO dump_vgmdb.product_types (name) VALUES (@name)";
            insertProductType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));

            testForEvent = connection.CreateCommand();
            testForEvent.CommandText = "SELECT dateAdded FROM dump_vgmdb.events WHERE id=@id";
            testForEvent.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertEventTranslation = connection.CreateCommand();
            insertEventTranslation.CommandText = "INSERT INTO dump_vgmdb.events_translation (id,lang,name) VALUES (@id,@lang,@name)";
            insertEventTranslation.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertEventTranslation.Parameters.Add(new NpgsqlParameter("@lang", DbType.String));
            insertEventTranslation.Parameters.Add(new NpgsqlParameter("@name", DbType.String));

            insertEvent = connection.CreateCommand();
            insertEvent.CommandText = "INSERT INTO dump_vgmdb.events (id, year,enddate,shortname,startdate) VALUES (@id,@year,@enddate,@shortname,@startdate)";
            insertEvent.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertEvent.Parameters.Add(new NpgsqlParameter("@year", DbType.Int32));
            insertEvent.Parameters.Add(new NpgsqlParameter("@enddate", DbType.Date));
            insertEvent.Parameters.Add(new NpgsqlParameter("@shortname", DbType.String));
            insertEvent.Parameters.Add(new NpgsqlParameter("@startdate", DbType.Date));

            insertLabel = connection.CreateCommand();
            insertLabel.CommandText = "INSERT INTO dump_vgmdb.labels (id,name,imprintid,formerlyid,subsidid) VALUES (@id,@name,@imprintid,@formerlyid,@subsidid)";
            insertLabel.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertLabel.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertLabel.Parameters.Add(new NpgsqlParameter("@imprintid", DbType.Int32));
            insertLabel.Parameters.Add(new NpgsqlParameter("@formerlyid", DbType.Int32));
            insertLabel.Parameters.Add(new NpgsqlParameter("@subsidid", DbType.Int32));

            testForLabel = connection.CreateCommand();
            testForLabel.CommandText = "SELECT dateAdded FROM dump_vgmdb.labels WHERE id=@id";
            testForLabel.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertArtist = connection.CreateCommand();
            insertArtist.CommandText = "INSERT INTO dump_vgmdb.artist (id,namelang,name,namereal) VALUES (@id,@namelang,@name,@namereal)";
            insertArtist.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertArtist.Parameters.Add(new NpgsqlParameter("@namelang", DbType.String));
            insertArtist.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            insertArtist.Parameters.Add(new NpgsqlParameter("@namereal", DbType.String));

            testForArtist = connection.CreateCommand();
            testForArtist.CommandText = "SELECT dateAdded FROM dump_vgmdb.artist WHERE id=@id";
            testForArtist.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            insertAlbum = connection.CreateCommand();
            insertAlbum.CommandText = "INSERT INTO dump_vgmdb.albums (id,catalog,release_date,typeid) VALUES (@id,@catalog,@release_date,@typeid)";
            insertAlbum.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            insertAlbum.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
            insertAlbum.Parameters.Add(new NpgsqlParameter("@release_date", DbType.Date));
            insertAlbum.Parameters.Add(new NpgsqlParameter("@typeid", DbType.Int16));

            testForAlbumType = connection.CreateCommand();
            testForAlbumType.CommandText = "SELECT id FROM dump_vgmdb.album_types WHERE name=@name";
            testForAlbumType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));

            insertAlbumType = connection.CreateCommand();
            insertAlbumType.CommandText = "INSERT INTO dump_vgmdb.album_types (name) VALUES (@name)";
            insertAlbumType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));

            insertAlbumTitle = connection.CreateCommand();
            insertAlbumTitle.CommandText = "INSERT INTO dump_vgmdb.album_titles (albumid,langname,title) VALUES (@albumid,@langname,@title)";
            insertAlbumTitle.Parameters.Add(new NpgsqlParameter("@albumid", DbType.Int32));
            insertAlbumTitle.Parameters.Add(new NpgsqlParameter("@langname", DbType.String));
            insertAlbumTitle.Parameters.Add(new NpgsqlParameter("@title", DbType.String));

            testForAlbum = connection.CreateCommand();
            testForAlbum.CommandText = "SELECT id FROM dump_vgmdb.albums WHERE id=@id";
            testForAlbum.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));

            testForDumpMetadata = connection.CreateCommand();
            testForDumpMetadata.CommandText =
                "SELECT id FROM dump_vgmdb.\"0dumpmeta\" WHERE key1=@key1 AND key2=@key2 AND keyutime=@keyutime";
            testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
            testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
            testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));

            testForDumpMetadataWithoutDate = connection.CreateCommand();
            testForDumpMetadataWithoutDate.CommandText =
                "SELECT id FROM dump_vgmdb.\"0dumpmeta\" WHERE key1=@key1 AND key2=@key2";
            testForDumpMetadataWithoutDate.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
            testForDumpMetadataWithoutDate.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));

            setMetadata = connection.CreateCommand();
            setMetadata.CommandText = "INSERT INTO dump_vgmdb.\"0dumpmeta\" (key1,key2,keyutime) VALUES (@key1,@key2,@keyutime)";
            setMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
            setMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
            setMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));

        }

        private void InsertArtistAlias(int v, int i, string key, string value)
        {
            if (insertArtistAlias == null)
            {
                insertArtistAlias = connection.CreateCommand();
                insertArtistAlias.CommandText = "INSERT INTO dump_vgmdb.artist_alias (artistId,ordinal,lang,name) VALUES (@artistId,@ordinal,@lang,@name)";
                insertArtistAlias.Parameters.Add(new NpgsqlParameter("@artistId", DbType.Int32));
                insertArtistAlias.Parameters.Add(new NpgsqlParameter("@ordinal", DbType.Int32));
                insertArtistAlias.Parameters.Add(new NpgsqlParameter("@lang", DbType.String));
                insertArtistAlias.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            insertArtistAlias.Parameters["@artistId"].Value = v;
            insertArtistAlias.Parameters["@ordinal"].Value = i;
            insertArtistAlias.Parameters["@lang"].Value = key;
            insertArtistAlias.Parameters["@name"].Value = value;
            insertArtistAlias.ExecuteNonQuery();
        }

        private void InsertAlbumArbituraryLabel(int v, int i, int roleId, string value)
        {
            if (insertAlbumArbituraryLabel == null)
            {
                insertAlbumArbituraryLabel = connection.CreateCommand();
                insertAlbumArbituraryLabel.CommandText = "INSERT INTO dump_vgmdb.album_label_arbiturary (albumId,ordinal,roleId,name) VALUES (@albumId,@ordinal,@roleId,@name)";
                insertAlbumArbituraryLabel.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumArbituraryLabel.Parameters.Add(new NpgsqlParameter("@ordinal", DbType.Int32));
                insertAlbumArbituraryLabel.Parameters.Add(new NpgsqlParameter("@roleId", DbType.Int32));
                insertAlbumArbituraryLabel.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            insertAlbumArbituraryLabel.Parameters["@albumId"].Value = v;
            insertAlbumArbituraryLabel.Parameters["@ordinal"].Value = i;
            insertAlbumArbituraryLabel.Parameters["@roleId"].Value = roleId;
            insertAlbumArbituraryLabel.Parameters["@name"].Value = value;
            insertAlbumArbituraryLabel.ExecuteNonQuery();

        }

        private void InsertProductArbituary(int albumId, int index, string name)
        {
            if (insertAlbumArbituraryProduct == null)
            {
                insertAlbumArbituraryProduct = connection.CreateCommand();
                insertAlbumArbituraryProduct.CommandText = "INSERT INTO dump_vgmdb.album_arbituaryproducts (albumId,ordinal,name) VALUES (@albumId,@ordinal,@name)";
                insertAlbumArbituraryProduct.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumArbituraryProduct.Parameters.Add(new NpgsqlParameter("@ordinal", DbType.Int32));
                insertAlbumArbituraryProduct.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            insertAlbumArbituraryProduct.Parameters["@albumId"].Value = albumId;
            insertAlbumArbituraryProduct.Parameters["@ordinal"].Value = index;
            insertAlbumArbituraryProduct.Parameters["@name"].Value = name;
            insertAlbumArbituraryProduct.ExecuteNonQuery();
        }

        private void InsertAlbumReprint(int albumId, int reprintId)
        {
            if (insertAlbumReprint == null)
            {
                insertAlbumReprint = connection.CreateCommand();
                insertAlbumReprint.CommandText = "INSERT INTO dump_vgmdb.album_reprints (albumId,reprintid) VALUES (@albumId,@reprintid)";
                insertAlbumReprint.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumReprint.Parameters.Add(new NpgsqlParameter("@reprintid", DbType.Int32));
            }
            insertAlbumReprint.Parameters["@albumId"].Value = albumId;
            insertAlbumReprint.Parameters["@reprintid"].Value = reprintId;
            insertAlbumReprint.ExecuteNonQuery();
        }

        private void InsertAlbumRelatedAlbum(int albumId, int relatedAlbumId)
        {
            if (insertAlbumRelatedAlbum == null)
            {
                insertAlbumRelatedAlbum = connection.CreateCommand();
                insertAlbumRelatedAlbum.CommandText = "INSERT INTO dump_vgmdb.album_relatedalbum (albumId,relatedAlbumId) VALUES (@albumId,@relatedAlbumId)";
                insertAlbumRelatedAlbum.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumRelatedAlbum.Parameters.Add(new NpgsqlParameter("@relatedAlbumId", DbType.Int32));
            }
            insertAlbumRelatedAlbum.Parameters["@albumId"].Value = albumId;
            insertAlbumRelatedAlbum.Parameters["@relatedAlbumId"].Value = relatedAlbumId;
            insertAlbumRelatedAlbum.ExecuteNonQuery();
        }

        private void InsertAlbumCover(int albumId, string covername, byte[] buffer, int index)
        {
            if (insertAlbumCover == null)
            {
                insertAlbumCover = connection.CreateCommand();
                insertAlbumCover.CommandText = "INSERT INTO dump_vgmdb.album_cover (albumId,covername,buffer,ordinal) VALUES (@albumId,@covername,@buffer,@ordinal)";
                insertAlbumCover.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumCover.Parameters.Add(new NpgsqlParameter("@covername", DbType.String));
                insertAlbumCover.Parameters.Add(new NpgsqlParameter("@buffer", NpgsqlDbType.Bytea));
                insertAlbumCover.Parameters.Add(new NpgsqlParameter("@ordinal", DbType.Int32));
            }
            insertAlbumCover.Parameters["@albumId"].Value = albumId;
            insertAlbumCover.Parameters["@covername"].Value = covername;
            insertAlbumCover.Parameters["@buffer"].Value = buffer;
            insertAlbumCover.Parameters["@ordinal"].Value = index;
            insertAlbumCover.ExecuteNonQuery();
        }

        private int? FindUnscrapedAlbum()
        {
            if (findUnscrapedAlbum == null)
            {
                findUnscrapedAlbum = connection.CreateCommand();
                findUnscrapedAlbum.CommandText = "SELECT id FROM dump_vgmdb.albums WHERE scraped = FALSE";
            }
            NpgsqlDataReader ndr = findUnscrapedAlbum.ExecuteReader();
            int? result = null;
            if (ndr.Read())
                result = ndr.GetInt32(0);
            ndr.Close();
            return result;
        }

        private void InsertAlbumEvent(int albumId, int eventId)
        {
            if (insertAlbumEvent == null)
            {
                insertAlbumEvent = connection.CreateCommand();
                insertAlbumEvent.CommandText = "INSERT INTO dump_vgmdb.album_releaseEvent (albumId,eventId) VALUES (@albumId,@eventId)";
                insertAlbumEvent.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumEvent.Parameters.Add(new NpgsqlParameter("@eventId", DbType.Int32));
            }
            insertAlbumEvent.Parameters["@albumId"].Value = albumId;
            insertAlbumEvent.Parameters["@eventId"].Value = eventId;
            insertAlbumEvent.ExecuteNonQuery();
        }

        private int GetAlbumPublishFormatId(string name)
        {
            if (getAlbumPublishFormatId == null)
            {
                getAlbumPublishFormatId = connection.CreateCommand();
                getAlbumPublishFormatId.CommandText = "SELECT id FROM dump_vgmdb.album_mediaformat WHERE name=@name";
                getAlbumPublishFormatId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getAlbumPublishFormatId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getAlbumPublishFormatId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertAlbumPublishFormat == null)
                {
                    insertAlbumPublishFormat = connection.CreateCommand();
                    insertAlbumPublishFormat.CommandText = "INSERT INTO dump_vgmdb.album_mediaformat (name) VALUES (@name)";
                    insertAlbumPublishFormat.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertAlbumPublishFormat.Parameters["@name"].Value = name;
                insertAlbumPublishFormat.ExecuteNonQuery();
                return GetAlbumPublishFormatId(name);
            }
        }
        
        private void InsertAlbumLabel(int albumId, int labelId, int roleId)
        {
            if (insertAlbumLabel == null)
            {
                insertAlbumLabel = connection.CreateCommand();
                insertAlbumLabel.CommandText = "INSERT INTO dump_vgmdb.album_labels (albumId,labelId,roleId) VALUES (@albumId,@labelId,@roleId)";
                insertAlbumLabel.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumLabel.Parameters.Add(new NpgsqlParameter("@labelId", DbType.Int32));
                insertAlbumLabel.Parameters.Add(new NpgsqlParameter("@roleId", DbType.Int32));
            }
            insertAlbumLabel.Parameters["@albumId"].Value = albumId;
            insertAlbumLabel.Parameters["@labelId"].Value = labelId;
            insertAlbumLabel.Parameters["@roleId"].Value = roleId;
            insertAlbumLabel.ExecuteNonQuery();
        }

        private int GetAlbumLabelRoleId(string name)
        {
            if (getAlbumLabelRoleId == null)
            {
                getAlbumLabelRoleId = connection.CreateCommand();
                getAlbumLabelRoleId.CommandText = "SELECT id FROM dump_vgmdb.album_label_roles WHERE name=@name";
                getAlbumLabelRoleId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getAlbumLabelRoleId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getAlbumLabelRoleId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertAlbumLabelRole == null)
                {
                    insertAlbumLabelRole = connection.CreateCommand();
                    insertAlbumLabelRole.CommandText = "INSERT INTO dump_vgmdb.album_label_roles (name) VALUES (@name)";
                    insertAlbumLabelRole.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertAlbumLabelRole.Parameters["@name"].Value = name;
                insertAlbumLabelRole.ExecuteNonQuery();
                return GetAlbumLabelRoleId(name);
            }
        }

        private bool TestForAlbumTitle(int albumId, string language)
        {
            if (testForAlbumTitle == null)
            {
                testForAlbumTitle = connection.CreateCommand();
                testForAlbumTitle.CommandText = "SELECT dateAdded FROM dump_vgmdb.album_titles WHERE albumid=@albumid AND langname=@langname";
                testForAlbumTitle.Parameters.Add(new NpgsqlParameter("@albumid", DbType.Int32));
                testForAlbumTitle.Parameters.Add(new NpgsqlParameter("@langname", DbType.String));
            }
            testForAlbumTitle.Parameters["@albumid"].Value = albumId;
            testForAlbumTitle.Parameters["@langname"].Value = language;
            NpgsqlDataReader ndr = testForAlbumTitle.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private int GetAlbumMediaFormatId(string name)
        {
            if (getAlbumMediaFormatId == null)
            {
                getAlbumMediaFormatId = connection.CreateCommand();
                getAlbumMediaFormatId.CommandText = "SELECT id FROM dump_vgmdb.album_mediaformat WHERE name=@name";
                getAlbumMediaFormatId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getAlbumMediaFormatId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getAlbumMediaFormatId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertAlbumMediaFormat == null)
                {
                    insertAlbumMediaFormat = connection.CreateCommand();
                    insertAlbumMediaFormat.CommandText = "INSERT INTO dump_vgmdb.album_mediaformat (name) VALUES (@name)";
                    insertAlbumMediaFormat.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertAlbumMediaFormat.Parameters["@name"].Value = name;
                insertAlbumMediaFormat.ExecuteNonQuery();
                return GetAlbumMediaFormatId(name);
            }
        }

        private void InsertAlbumDiscTrackTranslation(int albumid, int discIndex, int trackIndex, string language, string name)
        {
            if (insertAlbumDiscTrackTranslation == null)
            {
                insertAlbumDiscTrackTranslation = connection.CreateCommand();
                insertAlbumDiscTrackTranslation.CommandText = 
                    "INSERT INTO dump_vgmdb.album_disc_track_translation (albumId, discIndex, trackIndex, lang, name) " +
                    "     VALUES (@albumId, @discIndex, @trackIndex, @lang, @name)";
                insertAlbumDiscTrackTranslation.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumDiscTrackTranslation.Parameters.Add(new NpgsqlParameter("@discIndex", DbType.Int32));
                insertAlbumDiscTrackTranslation.Parameters.Add(new NpgsqlParameter("@trackIndex", DbType.Int32));
                insertAlbumDiscTrackTranslation.Parameters.Add(new NpgsqlParameter("@lang", DbType.String));
                insertAlbumDiscTrackTranslation.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            insertAlbumDiscTrackTranslation.Parameters["@albumId"].Value = albumid;
            insertAlbumDiscTrackTranslation.Parameters["@discIndex"].Value = discIndex;
            insertAlbumDiscTrackTranslation.Parameters["@trackIndex"].Value = trackIndex;
            insertAlbumDiscTrackTranslation.Parameters["@lang"].Value = language;
            insertAlbumDiscTrackTranslation.Parameters["@name"].Value = name;
            insertAlbumDiscTrackTranslation.ExecuteNonQuery();
        }

        private void InsertAlbumDiscTrack(int albumid, int discIndex, int trackIndex, int trackLength)
        {
            if (insertAlbumDiscTrack == null)
            {
                insertAlbumDiscTrack = connection.CreateCommand();
                insertAlbumDiscTrack.CommandText = "INSERT INTO dump_vgmdb.album_disc_tracks (albumId, discIndex, trackIndex, trackLength) VALUES (@albumId,@discIndex,@trackIndex,@trackLength)";
                insertAlbumDiscTrack.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumDiscTrack.Parameters.Add(new NpgsqlParameter("@discIndex", DbType.Int32));
                insertAlbumDiscTrack.Parameters.Add(new NpgsqlParameter("@trackIndex", DbType.Int32));
                insertAlbumDiscTrack.Parameters.Add(new NpgsqlParameter("@trackLength", DbType.Int32));
            }
            insertAlbumDiscTrack.Parameters["@albumId"].Value = albumid;
            insertAlbumDiscTrack.Parameters["@discIndex"].Value = discIndex;
            insertAlbumDiscTrack.Parameters["@trackIndex"].Value = trackIndex;
            insertAlbumDiscTrack.Parameters["@trackLength"].Value = trackLength;
            insertAlbumDiscTrack.ExecuteNonQuery();
        }

        private void InsertAlbumDisc(int albumId, int discindex, string name)
        {
            if (insertAlbumDisc == null)
            {
                insertAlbumDisc = connection.CreateCommand();
                insertAlbumDisc.CommandText = "INSERT INTO dump_vgmdb.album_discs (albumId,discindex,name) VALUES (@albumId,@discindex,@name)";
                insertAlbumDisc.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumDisc.Parameters.Add(new NpgsqlParameter("@discindex", DbType.Int32));
                insertAlbumDisc.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            insertAlbumDisc.Parameters["@albumId"].Value = albumId;
            insertAlbumDisc.Parameters["@discindex"].Value = discindex;
            insertAlbumDisc.Parameters["@name"].Value = name;
            insertAlbumDisc.ExecuteNonQuery();
        }

        private int GetAlbumClassificationId(string name)
        {
            if (findAlbumClassificationId == null)
            {
                findAlbumClassificationId = connection.CreateCommand();
                findAlbumClassificationId.CommandText = "SELECT id FROM dump_vgmdb.album_classification WHERE name=@name";
                findAlbumClassificationId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            findAlbumClassificationId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = findAlbumClassificationId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertAlbumClassificationId == null)
                {
                    insertAlbumClassificationId = connection.CreateCommand();
                    insertAlbumClassificationId.CommandText = "INSERT INTO dump_vgmdb.album_classification (name) VALUES (@name)";
                    insertAlbumClassificationId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertAlbumClassificationId.Parameters["@name"].Value = name;
                insertAlbumClassificationId.ExecuteNonQuery();
                return GetAlbumClassificationId(name);
            }
        }

        private void InsertArbitraryAlbumArtist(int albumId, int artistTypeId, string name)
        {
            if (insertArbitraryAlbumArtist == null)
            {
                insertArbitraryAlbumArtist = connection.CreateCommand();
                insertArbitraryAlbumArtist.CommandText = "INSERT INTO dump_vgmdb.album_artist_arbitrary (albumid,artisttypeid,name) VALUES (@albumid,@artisttypeid,@name)";
                insertArbitraryAlbumArtist.Parameters.Add(new NpgsqlParameter("@albumid", DbType.Int32));
                insertArbitraryAlbumArtist.Parameters.Add(new NpgsqlParameter("@artisttypeid", DbType.Int32));
                insertArbitraryAlbumArtist.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            insertArbitraryAlbumArtist.Parameters["@albumid"].Value = albumId;
            insertArbitraryAlbumArtist.Parameters["@artisttypeid"].Value = artistTypeId;
            insertArbitraryAlbumArtist.Parameters["@name"].Value = name;
            insertArbitraryAlbumArtist.ExecuteNonQuery();
        }

        private void InsertAlbumArtist(int artistId, int albumId, int artistTypeId)
        {
            if (testForAlbumArtist == null)
            {
                testForAlbumArtist = connection.CreateCommand();
                testForAlbumArtist.CommandText = "SELECT dateAdded FROM dump_vgmdb.album_artists WHERE artistId=@artistId AND albumId=@albumId AND artistTypeId=@artistTypeId";
                testForAlbumArtist.Parameters.Add(new NpgsqlParameter("@artistId", DbType.Int32));
                testForAlbumArtist.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                testForAlbumArtist.Parameters.Add(new NpgsqlParameter("@artistTypeId", DbType.Int32));
            }
            testForAlbumArtist.Parameters["@artistId"].Value = artistId;
            testForAlbumArtist.Parameters["@albumId"].Value = albumId;
            testForAlbumArtist.Parameters["@artistTypeId"].Value = artistTypeId;
            NpgsqlDataReader ndr = testForAlbumArtist.ExecuteReader();
            bool alreadyKnown = ndr.Read();
            ndr.Close();
            if (alreadyKnown)
                return;

            if (insertAlbumArtist == null)
            {
                insertAlbumArtist = connection.CreateCommand();
                insertAlbumArtist.CommandText = "INSERT INTO dump_vgmdb.album_artists (artistId,albumId,artistTypeId) VALUES (@artistId,@albumId,@artistTypeId)";
                insertAlbumArtist.Parameters.Add(new NpgsqlParameter("@artistId", DbType.Int32));
                insertAlbumArtist.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumArtist.Parameters.Add(new NpgsqlParameter("@artistTypeId", DbType.Int32));
            }
            insertAlbumArtist.Parameters["@artistId"].Value = artistId;
            insertAlbumArtist.Parameters["@albumId"].Value = albumId;
            insertAlbumArtist.Parameters["@artistTypeId"].Value = artistTypeId;
            insertAlbumArtist.ExecuteNonQuery();
        }

        private int GetAlbumArtistTypeId(string type)
        {
            if (findAlbumArtistTypeId == null)
            {
                findAlbumArtistTypeId = connection.CreateCommand();
                findAlbumArtistTypeId.CommandText = "SELECT id FROM dump_vgmdb.album_artist_type WHERE name=@name";
                findAlbumArtistTypeId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            findAlbumArtistTypeId.Parameters["@name"].Value = type;
            NpgsqlDataReader ndr = findAlbumArtistTypeId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertAlbumArtistType == null)
                {
                    insertAlbumArtistType = connection.CreateCommand();
                    insertAlbumArtistType.CommandText = "INSERT INTO dump_vgmdb.album_artist_type (name) VALUES (@name)";
                    insertAlbumArtistType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertAlbumArtistType.Parameters["@name"].Value = type;
                insertAlbumArtistType.ExecuteNonQuery();
                return GetAlbumArtistTypeId(type);
            }
        }

        private Nullable<int> FindUnscrapedArtist()
        {
            if (findUnscrapedArtist == null)
            {
                findUnscrapedArtist = connection.CreateCommand();
                findUnscrapedArtist.CommandText = "SELECT id FROM dump_vgmdb.artist WHERE scraped=FALSE";
            }
            NpgsqlDataReader ndr = findUnscrapedArtist.ExecuteReader();
            Nullable<int> result = null;
            if (ndr.Read())
            {
                result = ndr.GetInt32(0);
            }
            ndr.Close();
            return result;
        }

        private int GetArtistTypeId(string artistType)
        {
            if (getArtistTypeId == null)
            {
                getArtistTypeId = connection.CreateCommand();
                getArtistTypeId.CommandText = "SELECT id FROM dump_vgmdb.artist_type WHERE name=@name";
                getArtistTypeId.Parameters.Add(new NpgsqlParameter("@name",DbType.String));
            }
            getArtistTypeId.Parameters["@name"].Value = artistType;
            NpgsqlDataReader ndr = getArtistTypeId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                if (insertArtistType == null)
                {
                    insertArtistType = connection.CreateCommand();
                    insertArtistType.CommandText = "INSERT INTO dump_vgmdb.artist_type (name) VALUES (@name)";
                    insertArtistType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                insertArtistType.Parameters["@name"].Value = artistType;
                insertArtistType.ExecuteNonQuery();
                return GetArtistTypeId(artistType);
            }
        }

        private void InsertArtistFeature(int artistId, int albumId)
        {
            if (insertArtistFeature == null)
            {
                insertArtistFeature = connection.CreateCommand();
                insertArtistFeature.CommandText = "INSERT INTO dump_vgmdb.artist_featured (artistId,albumId) VALUES (@artistId,@albumId)";
                insertArtistFeature.Parameters.Add(new NpgsqlParameter("@artistId", DbType.Int32));
                insertArtistFeature.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
            }
            insertArtistFeature.Parameters["@artistId"].Value = artistId;
            insertArtistFeature.Parameters["@albumId"].Value = albumId;
            insertArtistFeature.ExecuteNonQuery();
        }

        private Nullable<int> FindUnscrapedEvent()
        {
            if (findUnscrapedEvent == null)
            {
                findUnscrapedEvent = connection.CreateCommand();
                findUnscrapedEvent.CommandText = "SELECT id FROM dump_vgmdb.events WHERE scraped=FALSE";
            }
            NpgsqlDataReader ndr = findUnscrapedEvent.ExecuteReader();
            Nullable<int> result = null;
            if (ndr.Read())
            {
                result = ndr.GetInt32(0);
            }
            ndr.Close();
            return result;
        }

        private void InsertEventRelease(int eventId, int albumId)
        {
            if (insertEventRelease == null)
            {
                insertEventRelease = connection.CreateCommand();
                insertEventRelease.CommandText = "INSERT INTO dump_vgmdb.event_releases (eventId,albumId) VALUES (@eventId,@albumId)";
                insertEventRelease.Parameters.Add(new NpgsqlParameter("@eventId", DbType.Int32));
                insertEventRelease.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
            }
            insertEventRelease.Parameters["@eventId"].Value = eventId;
            insertEventRelease.Parameters["@albumId"].Value = albumId;
            insertEventRelease.ExecuteNonQuery();
        }

        private int? FindUnscrapedLabel()
        {
            if (findUnscrapedLabel == null)
            {
                findUnscrapedLabel = connection.CreateCommand();
                findUnscrapedLabel.CommandText = "SELECT id FROM dump_vgmdb.labels WHERE scraped=FALSE";
            }
            NpgsqlDataReader ndr = findUnscrapedLabel.ExecuteReader();
            int? result = null;
            if (ndr.Read())
            {
                result = ndr.GetInt32(0);
            }
            ndr.Close();
            return result;
        }

        private int GetLabelTypeId(string name)
        {
            if (getLabelTypeId == null)
            {
                getLabelTypeId = connection.CreateCommand();
                getLabelTypeId.CommandText = "SELECT id FROM dump_vgmdb.label_types WHERE name=@name";
                getLabelTypeId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getLabelTypeId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getLabelTypeId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                if (insertLabelType == null)
                {
                    insertLabelType = connection.CreateCommand();
                    insertLabelType.CommandText = "INSERT INTO dump_vgmdb.label_types (name) VALUES (@name)";
                    insertLabelType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                insertLabelType.Parameters["@name"].Value = name;
                insertLabelType.ExecuteNonQuery();
                logger.Info("Found new label type: " + name);
                return GetLabelTypeId(name);
            }
        }

        private void InsertLabelStaff(int labelId, int artistId, bool owner)
        {
            if (insertLabelStaff == null)
            {
                insertLabelStaff = connection.CreateCommand();
                insertLabelStaff.CommandText = "INSERT INTO dump_vgmdb.label_staff (labelId,artistId,owner) VALUES (@labelId,@artistId,@owner)";
                insertLabelStaff.Parameters.Add(new NpgsqlParameter("@labelId", DbType.Int32));
                insertLabelStaff.Parameters.Add(new NpgsqlParameter("@artistId", DbType.Int32));
                insertLabelStaff.Parameters.Add(new NpgsqlParameter("@owner", DbType.Boolean));
            }
            insertLabelStaff.Parameters["@labelId"].Value = labelId;
            insertLabelStaff.Parameters["@artistId"].Value = artistId;
            insertLabelStaff.Parameters["@owner"].Value = owner;
            insertLabelStaff.ExecuteNonQuery();
        }

        private void InsertLabelRelease(int labelId, int albumId)
        {
            if (insertLabelRelease == null)
            {
                insertLabelRelease = connection.CreateCommand();
                insertLabelRelease.CommandText = "INSERT INTO dump_vgmdb.label_releases (labelId,albumId) VALUES (@labelId,@albumId)";
                insertLabelRelease.Parameters.Add(new NpgsqlParameter("@labelId", DbType.Int32));
                insertLabelRelease.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
            }
            insertLabelRelease.Parameters["@labelId"].Value = labelId;
            insertLabelRelease.Parameters["@albumId"].Value = albumId;
            insertLabelRelease.ExecuteNonQuery();
        }

        private int GetLabelRegionId(string name)
        {
            if (getLabelRegionId == null)
            {
                getLabelRegionId = connection.CreateCommand();
                getLabelRegionId.CommandText = "SELECT id FROM dump_vgmdb.label_regions WHERE name=@name";
                getLabelRegionId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getLabelRegionId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getLabelRegionId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertLabelRegion == null)
                {
                    insertLabelRegion = connection.CreateCommand();
                    insertLabelRegion.CommandText = "INSERT INTO dump_vgmdb.label_regions (name) VALUES (@name)";
                    insertLabelRegion.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertLabelRegion.Parameters["@name"].Value = name;
                insertLabelRegion.ExecuteNonQuery();
                logger.Info("Found new label region: " + name);
                return GetLabelRegionId(name);
            }
        }

        private int GetReleaseType(string name)
        {
            if (getReleaseTypeId == null)
            {
                getReleaseTypeId = connection.CreateCommand();
                getReleaseTypeId.CommandText = "SELECT id FROM dump_vgmdb.product_release_types WHERE name=@name";
                getReleaseTypeId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getReleaseTypeId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getReleaseTypeId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                if (insertReleaseType == null)
                {
                    insertReleaseType = connection.CreateCommand();
                    insertReleaseType.CommandText = "INSERT INTO dump_vgmdb.product_release_types (name) VALUES (@name)";
                    insertReleaseType.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                insertReleaseType.Parameters["@name"].Value = name;
                insertReleaseType.ExecuteNonQuery();
                logger.Info("Found new release type: " + name);
                return GetReleaseType(name);
            }
        }

        private void InsertReleaseAlbum(int releaseId, int albumId)
        {
            if (insertReleaseAlbum == null)
            {
                insertReleaseAlbum = connection.CreateCommand();
                insertReleaseAlbum.CommandText = "INSERT INTO dump_vgmdb.product_release_albums (releaseid,albumid) VALUES (@releaseid,@albumid)";
                insertReleaseAlbum.Parameters.Add(new NpgsqlParameter("@releaseid", DbType.Int32));
                insertReleaseAlbum.Parameters.Add(new NpgsqlParameter("@albumid", DbType.Int32));
            }

            insertReleaseAlbum.Parameters["@releaseid"].Value = releaseId;
            insertReleaseAlbum.Parameters["@albumid"].Value = albumId;
            insertReleaseAlbum.ExecuteNonQuery();
        }

        private void InsertArbitaryRelease(int prodId, int arrIdx, string key, string value)
        {
            if (insertArbitaryProductRelease == null)
            {
                insertArbitaryProductRelease = connection.CreateCommand();
                insertArbitaryProductRelease.CommandText = "INSERT INTO dump_vgmdb.product_release_arbitaries (productid,arrayindex,key,value) VALUES (@productid,@arrayindex,@key,@value)";
                insertArbitaryProductRelease.Parameters.Add(new NpgsqlParameter("@productid", DbType.Int32));
                insertArbitaryProductRelease.Parameters.Add(new NpgsqlParameter("@arrayindex", DbType.Int32));
                insertArbitaryProductRelease.Parameters.Add(new NpgsqlParameter("@key", DbType.String));
                insertArbitaryProductRelease.Parameters.Add(new NpgsqlParameter("@value", DbType.String));
            }
            insertArbitaryProductRelease.Parameters["@productid"].Value = prodId;
            insertArbitaryProductRelease.Parameters["@arrayindex"].Value = arrIdx;
            insertArbitaryProductRelease.Parameters["@key"].Value = key;
            insertArbitaryProductRelease.Parameters["@value"].Value = value;
            insertArbitaryProductRelease.ExecuteNonQuery();
        }

        private void InsertProductAlbum(int productId, int albumId)
        {
            if (insertProductAlbum == null)
            {
                insertProductAlbum = connection.CreateCommand();
                insertProductAlbum.CommandText = "INSERT INTO dump_vgmdb.product_albums (productid,albumid) VALUES (@productid,@albumid)";
                insertProductAlbum.Parameters.Add(new NpgsqlParameter("@productid", DbType.Int32));
                insertProductAlbum.Parameters.Add(new NpgsqlParameter("@albumid", DbType.Int32));
            }

            insertProductAlbum.Parameters["@productid"].Value = productId;
            insertProductAlbum.Parameters["@albumid"].Value = albumId;
            insertProductAlbum.ExecuteNonQuery();
        }

        private void MarkReleaseAsScraped(string id)
        {
            if (markReleaseAsScraped == null)
            {
                markReleaseAsScraped = connection.CreateCommand();
                markReleaseAsScraped.CommandText = "UPDATE dump_vgmdb.product_releases SET scraped=TRUE WHERE id=@id";
                markReleaseAsScraped.Parameters.Add(new NpgsqlParameter("@id", DbType.String));
            }
            markReleaseAsScraped.Parameters["@id"].Value = id;
            markReleaseAsScraped.ExecuteNonQuery();
            logger.Info("No scraping needed for release: " + id);
        }

        private int? FindUnscrapedRelease()
        {
            if (findUnscrapedRelease == null)
            {
                findUnscrapedRelease = connection.CreateCommand();
                findUnscrapedRelease.CommandText = "SELECT id FROM dump_vgmdb.product_releases WHERE scraped = FALSE";
            }
            NpgsqlDataReader ndr = findUnscrapedRelease.ExecuteReader();
            int? result;
            if (ndr.Read())
                result = ndr.GetInt32(0);
            else
                result = null;
            ndr.Close();
            return result;
        }

        private void InsertArbitaryProductLabel(int productId, string arbitaryName)
        {
            if (testForArbitaryProductLabel == null)
            {
                testForArbitaryProductLabel = connection.CreateCommand();
                testForArbitaryProductLabel.CommandText = "SELECT dateAdded FROM dump_vgmdb.product_labels WHERE productId=@productId AND arbitaryName=@name";
                testForArbitaryProductLabel.Parameters.Add(new NpgsqlParameter("@productId", DbType.Int32));
                testForArbitaryProductLabel.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            testForArbitaryProductLabel.Parameters["@productId"].Value = productId;
            testForArbitaryProductLabel.Parameters["@name"].Value = arbitaryName;
            NpgsqlDataReader reader = testForArbitaryProductLabel.ExecuteReader();
            bool alreadyExist = reader.Read();
            reader.Close();
            if (alreadyExist)
                return;

            if (insertArbitaryProductLabel == null)
            {
                insertArbitaryProductLabel = connection.CreateCommand();
                insertArbitaryProductLabel.CommandText = "INSERT INTO dump_vgmdb.product_labels (productId,labelId,arbitary,arbitaryName) VALUES (@productId,@labelId,TRUE,@arbitaryName)";
                insertArbitaryProductLabel.Parameters.Add(new NpgsqlParameter("@productId", DbType.Int32));
                insertArbitaryProductLabel.Parameters.Add(new NpgsqlParameter("@labelId", DbType.Int32));
                insertArbitaryProductLabel.Parameters.Add(new NpgsqlParameter("@arbitaryName", DbType.String));
            }

            insertArbitaryProductLabel.Parameters["@productId"].Value = productId;
            insertArbitaryProductLabel.Parameters["@labelId"].Value = --arbitaryLabelId;
            insertArbitaryProductLabel.Parameters["@arbitaryName"].Value = arbitaryName;
            insertArbitaryProductLabel.ExecuteNonQuery();


        }

        private Nullable<int> FindUnscrapedProduct()
        {
            if (findUnscrapedProduct == null)
            {
                findUnscrapedProduct = connection.CreateCommand();
                findUnscrapedProduct.CommandText = "SELECT id FROM dump_vgmdb.products WHERE scraped=FALSE";
            }
            NpgsqlDataReader ndr = findUnscrapedProduct.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                return null;
            }
        }
        
        private void InsertAlbumWebsite(int albumId, string catalog, string name, string link)
        {
            if (insertAlbumWebsite == null)
            {
                insertAlbumWebsite = connection.CreateCommand();
                insertAlbumWebsite.CommandText = "INSERT INTO dump_vgmdb.album_websites (albumId,catalog,name,link) VALUES (@albumId,@catalog,@name,@link)";
                insertAlbumWebsite.Parameters.Add(new NpgsqlParameter("@albumId", DbType.Int32));
                insertAlbumWebsite.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
                insertAlbumWebsite.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                insertAlbumWebsite.Parameters.Add(new NpgsqlParameter("@link", DbType.String));
            }

            insertAlbumWebsite.Parameters["@albumId"].Value = albumId;
            insertAlbumWebsite.Parameters["@catalog"].Value = catalog;
            insertAlbumWebsite.Parameters["@name"].Value = name;
            insertAlbumWebsite.Parameters["@link"].Value = link;
            insertAlbumWebsite.ExecuteNonQuery();
        }

        private void InsertArtistWebsite(int artistId, string catalog, string name, string link)
        {
            if (insertArtistWebsite == null)
            {
                insertArtistWebsite = connection.CreateCommand();
                insertArtistWebsite.CommandText = "INSERT INTO dump_vgmdb.artist_websites (artistId,catalog,name,link) VALUES (@artistId,@catalog,@name,@link)";
                insertArtistWebsite.Parameters.Add(new NpgsqlParameter("@artistId", DbType.Int32));
                insertArtistWebsite.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
                insertArtistWebsite.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                insertArtistWebsite.Parameters.Add(new NpgsqlParameter("@link", DbType.String));
            }

            insertArtistWebsite.Parameters["@artistId"].Value = artistId;
            insertArtistWebsite.Parameters["@catalog"].Value = catalog;
            insertArtistWebsite.Parameters["@name"].Value = name;
            insertArtistWebsite.Parameters["@link"].Value = link;
            insertArtistWebsite.ExecuteNonQuery();
        }

        private void InsertProductWebsite(int prodId, string catalog, string name, string link)
        {
            if (insertProductWebsite == null)
            {
                insertProductWebsite = connection.CreateCommand();
                insertProductWebsite.CommandText = "INSERT INTO dump_vgmdb.product_websites (productId,catalog,name,link) VALUES (@productId,@catalog,@name,@link)";
                insertProductWebsite.Parameters.Add(new NpgsqlParameter("@productId", DbType.Int32));
                insertProductWebsite.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
                insertProductWebsite.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                insertProductWebsite.Parameters.Add(new NpgsqlParameter("@link", DbType.String));
            }

            insertProductWebsite.Parameters["@productId"].Value = prodId;
            insertProductWebsite.Parameters["@catalog"].Value = catalog;
            insertProductWebsite.Parameters["@name"].Value = name;
            insertProductWebsite.Parameters["@link"].Value = link;
            insertProductWebsite.ExecuteNonQuery();
        }

        private void InsertLabelWebsite(int labelId, string catalog, string name, string link)
        {
            if (insertLabelWebsite == null)
            {
                insertLabelWebsite = connection.CreateCommand();
                insertLabelWebsite.CommandText = "INSERT INTO dump_vgmdb.label_websites (labelId,catalog,name,link) VALUES (@labelId,@catalog,@name,@link)";
                insertLabelWebsite.Parameters.Add(new NpgsqlParameter("@labelId", DbType.Int32));
                insertLabelWebsite.Parameters.Add(new NpgsqlParameter("@catalog", DbType.String));
                insertLabelWebsite.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                insertLabelWebsite.Parameters.Add(new NpgsqlParameter("@link", DbType.String));
            }

            insertLabelWebsite.Parameters["@labelId"].Value = labelId;
            insertLabelWebsite.Parameters["@catalog"].Value = catalog;
            insertLabelWebsite.Parameters["@name"].Value = name;
            insertLabelWebsite.Parameters["@link"].Value = link;
            insertLabelWebsite.ExecuteNonQuery();
        }

        private int GetRegionId(string name)
        {
            if (getRegionId == null)
            {
                getRegionId = connection.CreateCommand();
                getRegionId.CommandText = "SELECT id FROM dump_vgmdb.product_release_regions WHERE name=@name";
                getRegionId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getRegionId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getRegionId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertRegion == null)
                {
                    insertRegion = connection.CreateCommand();
                    insertRegion.CommandText = "INSERT INTO dump_vgmdb.product_release_regions (name) VALUES (@name)";
                    insertRegion.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertRegion.Parameters["@name"].Value = name;
                insertRegion.ExecuteNonQuery();
                logger.Info("Found new region: " + name);
                return GetRegionId(name);
            }
        }

        private int GetPlatformId(string name)
        {
            if (getPlatformId == null)
            {
                getPlatformId = connection.CreateCommand();
                getPlatformId.CommandText = "SELECT id FROM dump_vgmdb.product_release_platforms WHERE name=@name";
                getPlatformId.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }
            getPlatformId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getPlatformId.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                if (insertPlatform == null)
                {
                    insertPlatform = connection.CreateCommand();
                    insertPlatform.CommandText = "INSERT INTO dump_vgmdb.product_release_platforms (name) VALUES (@name)";
                    insertPlatform.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
                }
                ndr.Close();
                insertPlatform.Parameters["@name"].Value = name;
                insertPlatform.ExecuteNonQuery();
                logger.Info("Found new Platform: " + name);
                return GetPlatformId(name);
            }
        }

        private void InsertReleaseTranslation(int id, string lang, string name)
        {
            if (testForReleaseTranslation == null)
            {
                testForReleaseTranslation = connection.CreateCommand();
                testForReleaseTranslation.CommandText = "SELECT dateAdded FROM dump_vgmdb.product_release_translations WHERE id=@id AND lang=@lang";
                testForReleaseTranslation.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                testForReleaseTranslation.Parameters.Add(new NpgsqlParameter("@lang", DbType.String));
            }

            testForReleaseTranslation.Parameters["@id"].Value = id;
            testForReleaseTranslation.Parameters["@lang"].Value = lang;
            NpgsqlDataReader ndr = testForReleaseTranslation.ExecuteReader();
            bool testResult = ndr.Read();
            ndr.Close();
            if (testResult)
                return;

            if (insertReleaseTranslation == null)
            {
                insertReleaseTranslation = connection.CreateCommand();
                insertReleaseTranslation.CommandText = "INSERT INTO dump_vgmdb.product_release_translations (id,lang,name) VALUES (@id,@lang,@name)";
                insertReleaseTranslation.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                insertReleaseTranslation.Parameters.Add(new NpgsqlParameter("@lang", DbType.String));
                insertReleaseTranslation.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }

            insertReleaseTranslation.Parameters["@id"].Value = id;
            insertReleaseTranslation.Parameters["@lang"].Value = lang;
            insertReleaseTranslation.Parameters["@name"].Value = name;
            insertReleaseTranslation.ExecuteNonQuery();
        }

        private void InsertRelease(ProductRelease release)
        {
            if (insertRelease == null)
            {
                insertRelease = connection.CreateCommand();
                insertRelease.CommandText = "INSERT INTO dump_vgmdb.product_releases (id, platformId, regionId) VALUES (@id,@platformId,@regionId)";
                insertRelease.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
                insertRelease.Parameters.Add(new NpgsqlParameter("@platformId", DbType.Int32));
                insertRelease.Parameters.Add(new NpgsqlParameter("@regionId", DbType.Int32));
            }

            insertRelease.Parameters["@id"].Value = release.GetId();
            insertRelease.Parameters["@platformId"].Value = GetPlatformId(release.platform);
            insertRelease.Parameters["@regionId"].Value = GetRegionId(release.region);
            insertRelease.ExecuteNonQuery();
            logger.Info("Found new Release: " + release.names.First().Value);

            foreach(KeyValuePair<string, string> language in release.names)
            {
                InsertReleaseTranslation(release.GetId(), language.Key, language.Value);
            }
        }

        private void InsertProductLabel(int productId, int labelId)
        {
            if (insertProductLabel == null)
            {
                insertProductLabel = connection.CreateCommand();
                insertProductLabel.CommandText = "INSERT INTO dump_vgmdb.product_labels (productId,labelId) VALUES (@productId,@labelId)";
                insertProductLabel.Parameters.Add(new NpgsqlParameter("@productId", DbType.Int32));
                insertProductLabel.Parameters.Add(new NpgsqlParameter("@labelId", DbType.Int32));
            }

            insertProductLabel.Parameters["@productId"].Value = productId;
            insertProductLabel.Parameters["@labelId"].Value = labelId;
            insertProductLabel.ExecuteNonQuery();
        }

        private int? FindLabelIdByName(string name)
        {
            if (findLabelIdByName == null)
            {
                findLabelIdByName = connection.CreateCommand();
                findLabelIdByName.CommandText = "SELECT id FROM dump_vgmdb.labels WHERE name=@name";
                findLabelIdByName.Parameters.Add(new NpgsqlParameter("@name", DbType.String));
            }

            findLabelIdByName.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = findLabelIdByName.ExecuteReader();
            if (ndr.Read())
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                return null;
            }
        }

        private void InsertProduct(ProductListProduct product)
        {
            insertProduct.Parameters["@id"].Value = product.GetId();
            if (product.names.Count == 0)
            {
                logger.Warn(String.Format("Product {0} was removed from the Database before I grabbed it.",product.GetId()));
                return;
            }
            insertProduct.Parameters["@name"].Value = product.names.First().Value;
            insertProduct.Parameters["@typeId"].Value = DBNull.Value;
            insertProduct.ExecuteNonQuery();
            logger.Info("Found new product: #" + product.GetId());
        }

        private void InsertProduct(int id, string name, int typeId)
        {
            insertProduct.Parameters["@id"].Value = id;
            insertProduct.Parameters["@name"].Value = name;
            insertProduct.Parameters["@typeId"].Value = typeId;
            insertProduct.ExecuteNonQuery();
            logger.Info("Found new product: " + name);
        }

        private bool TestForProduct(int pid)
        {
            testForProduct.Parameters["@id"].Value = pid;
            NpgsqlDataReader ndr = testForProduct.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private int GetProductTypeId(string name)
        {
            getProductTypeId.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = getProductTypeId.ExecuteReader();
            bool exists = ndr.Read();
            if (exists)
            {
                int result = ndr.GetInt32(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                insertProductType.Parameters["@name"].Value = name;
                insertProductType.ExecuteNonQuery();
                logger.Info("Found new product type: " + name);
                return GetProductTypeId(name);
            }
        }

        private void InsertEvent(EventListEvent ev, int year)
        {
            insertEvent.Parameters["@id"].Value = ev.GetId();
            insertEvent.Parameters["@year"].Value = year;

            if (ev.enddate != null)
            {
                DateTime endDate = DateTime.Parse(ev.enddate);
                insertEvent.Parameters["@enddate"].Value = endDate;
            }
            else
            {
                insertEvent.Parameters["@enddate"].Value = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(ev.shortname))
                insertEvent.Parameters["@shortname"].Value = ev.shortname;
            else
                insertEvent.Parameters["@shortname"].Value = DBNull.Value;

            if (ev.startdate != null)
            {
                DateTime startDate = DateTime.Parse(ev.startdate);
                insertEvent.Parameters["@startdate"].Value = startDate;
            }
            else
            {
                insertEvent.Parameters["@startdate"].Value = DBNull.Value;
            }

            logger.Info("Found new event #" + ev.GetId());
            insertEvent.ExecuteNonQuery();

            if (ev.names != null)
            {
                foreach (KeyValuePair<string, string> translation in ev.names)
                {
                    InsertEventTranslation(ev.GetId(), translation.Key, translation.Value);
                }
            }
        }

        private void InsertEventTranslation(int id, string lang, string name)
        {
            insertEventTranslation.Parameters["@id"].Value = id;
            insertEventTranslation.Parameters["@lang"].Value = lang;
            insertEventTranslation.Parameters["@name"].Value = name;
            insertEventTranslation.ExecuteNonQuery();
        }

        private bool TestForEvent(int id)
        {
            testForEvent.Parameters["@id"].Value = id;
            NpgsqlDataReader ndr = testForEvent.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private void InsertLabel(LabelListLabel label, LabelListLabel imprintParent, LabelListLabel formerlyParent, LabelListLabel subsidParent)
        {
            if (label.names == null)
            {
                label.names = new Dictionary<string, string>();
                label.names.Add("", "");
                logger.WarnFormat("{0} has no names", label.link);
            }

            insertLabel.Parameters["@id"].Value = label.GetId();
            insertLabel.Parameters["@name"].Value = label.names.First().Value;

            if (formerlyParent != null)
                insertLabel.Parameters["@formerlyid"].Value = formerlyParent.GetId();
            else
                insertLabel.Parameters["@formerlyid"].Value = DBNull.Value;

            if (imprintParent != null)
                insertLabel.Parameters["@imprintid"].Value = imprintParent.GetId();
            else
                insertLabel.Parameters["@imprintid"].Value = DBNull.Value;

            if (subsidParent != null)
                insertLabel.Parameters["@subsidid"].Value = subsidParent.GetId();
            else
                insertLabel.Parameters["@subsidid"].Value = DBNull.Value;

            logger.Info("Discovered new label: " + label.names.First().Value);
            insertLabel.ExecuteNonQuery();

            if (label.formerly != null)
                foreach (LabelListLabel pastForm in label.formerly)
                    if (!TestForLabel(pastForm.GetId()))
                        InsertLabel(pastForm, null, label, null);

            if (label.imprints != null)
                foreach (LabelListLabel imprint in label.imprints)
                    if (!TestForLabel(imprint.GetId()))
                        InsertLabel(imprint, label, null, null);

            if (label.subsidiaries != null)
                foreach (LabelListLabel subsidiary in label.subsidiaries)
                    if (!TestForLabel(subsidiary.GetId()))
                        InsertLabel(subsidiary, null, null, subsidiary);

        }
        
        private bool TestForLabel(int id)
        {
            testForLabel.Parameters["@id"].Value = id;
            NpgsqlDataReader ndr = testForLabel.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private void InsertArtist(ArtistListArtist artist)
        {
            KeyValuePair<string,string> firstlang = artist.names.First();
            InsertArtist(artist.GetId(), firstlang.Key, firstlang.Value, artist.name_real);
        }

        private void InsertArtist(int id, string namelang, string name, string namereal)
        {
            insertArtist.Parameters["@id"].Value = id;
            insertArtist.Parameters["@namelang"].Value = namelang;
            insertArtist.Parameters["@name"].Value = name;

            if (!string.IsNullOrEmpty(namereal))
                insertArtist.Parameters["@namereal"].Value = namereal;
            else
                insertArtist.Parameters["@namereal"].Value = DBNull.Value;

            logger.Info("Found new artist: " + name);
            insertArtist.ExecuteNonQuery();
        }

        private bool TestForArtist(int id)
        {
            testForArtist.Parameters["@id"].Value = id;
            NpgsqlDataReader ndr = testForArtist.ExecuteReader();
            bool result = ndr.Read();
            ndr.Close();
            return result;
        }

        private void InsertAlbum(AlbumListAlbum album)
        {

            Nullable<DateTime> releaseDate = null;
            if (!string.IsNullOrEmpty(album.release_date))
            {
                try
                {
                    releaseDate = DateTime.Parse(album.release_date);
                }
                catch (Exception e)
                {
                    releaseDate = null;
                }
            }

            int id = album.GetId();

            if (!string.IsNullOrEmpty(album.type))
            {
                short typeId = GetAlbumTypeId(album.type);
                InsertAlbum(id, album.catalog, releaseDate, typeId);
            }
            else
            {
                InsertAlbum(id, album.catalog, releaseDate);
            }
            

            

            if (album.titles != null)
            {
                foreach (KeyValuePair<string, string> language in album.titles)
                {
                    InsertAlbumTitle(album.GetId(), language.Key, language.Value);
                }
            }
        }

        private void InsertAlbum(int id, string catalog, Nullable<DateTime> releaseDate)
        {
            insertAlbum.Parameters["@id"].Value = id;
            insertAlbum.Parameters["@catalog"].Value = catalog;

            if (releaseDate.HasValue)
                insertAlbum.Parameters["@release_date"].Value = releaseDate;
            else
                insertAlbum.Parameters["@release_date"].Value = DBNull.Value;

            insertAlbum.Parameters["@typeId"].Value = DBNull.Value;
            logger.Info("Found new album: " + catalog);
            insertAlbum.ExecuteNonQuery();
        }

        private void InsertAlbum(int id, string catalog, Nullable<DateTime> releaseDate, short typeId)
        {
            insertAlbum.Parameters["@id"].Value = id;
            insertAlbum.Parameters["@catalog"].Value = catalog;

            if (releaseDate.HasValue)
                insertAlbum.Parameters["@release_date"].Value = releaseDate;
            else
                insertAlbum.Parameters["@release_date"].Value = DBNull.Value;

            insertAlbum.Parameters["@typeId"].Value = typeId;
            logger.Info("Found new album: " + catalog);
            insertAlbum.ExecuteNonQuery();
        }

        private short GetAlbumTypeId(string name)
        {
            testForAlbumType.Parameters["@name"].Value = name;
            NpgsqlDataReader ndr = testForAlbumType.ExecuteReader();
            if (ndr.Read())
            {
                short result = ndr.GetInt16(0);
                ndr.Close();
                return result;
            }
            else
            {
                ndr.Close();
                logger.Info("Found new album type: " + name);
                insertAlbumType.Parameters["@name"].Value = name;
                insertAlbumType.ExecuteNonQuery();
                return GetAlbumTypeId(name);
            }
        }

        private void InsertAlbumTitle(int albumid, string langname, string title)
        {
            insertAlbumTitle.Parameters["@albumid"].Value = albumid;
            insertAlbumTitle.Parameters["@langname"].Value = langname;
            insertAlbumTitle.Parameters["@title"].Value = title;
            insertAlbumTitle.ExecuteNonQuery();
        }

        private bool TestForAlbum(int id)
        {
            testForAlbum.Parameters["@id"].Value = id;
            NpgsqlDataReader reader = testForAlbum.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }

        private void SetDumpMetadata(string key1, string key2, Nullable<DateTime> keyutime)
        {
            if (string.IsNullOrEmpty(key2))
                key2 = "";
            if (!keyutime.HasValue)
                keyutime = DateTime.MinValue;

            setMetadata.Parameters["@key1"].Value = key1;
            setMetadata.Parameters["@key2"].Value = key2;
            setMetadata.Parameters["@keyutime"].Value = keyutime.Value.ToUnixTime();
            setMetadata.ExecuteNonQuery();
        }

        private bool TestForDumpMetadata(string key1, string key2)
        {
            if (string.IsNullOrEmpty(key2))
                key2 = "";

            testForDumpMetadataWithoutDate.Parameters["@key1"].Value = key1;
            testForDumpMetadataWithoutDate.Parameters["@key2"].Value = key2;

            NpgsqlDataReader reader = testForDumpMetadataWithoutDate.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }

        private bool TestForDumpMetadata(string key1, string key2, Nullable<DateTime> keyutime)
        {
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

        private int QueryInteger(string sql)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            object tmp = cmd.ExecuteScalar();
            if (tmp == null)
                throw new Exception("Integer Query returned null: " + sql);
            int result = Convert.ToInt32((long) tmp);
            cmd.Dispose();
            return result;
        }
    }
}
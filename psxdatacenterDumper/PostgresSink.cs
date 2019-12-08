using System;
using AzusaERP;
using Npgsql;
using NpgsqlTypes;

namespace psxdatacenterDumper
{
    class PostgresSink : IPdcSink
    {
        public PostgresSink()
        {
            Ini ini = new Ini("azusa.ini");

            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "Playstation Datacenter Parser";
            ncsb.Host = ini["postgresql"]["server"];
            ncsb.Port = Int32.Parse(ini["postgresql"]["port"]);
            ncsb.Database = ini["postgresql"]["database"];
            ncsb.Password = ini["postgresql"]["password"];
            ncsb.Username = ini["postgresql"]["username"];
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();
        }

        private NpgsqlConnection connection;

        public void Dispose()
        {
            connection.Dispose();
        }

        private NpgsqlCommand insertGame;
        public void HandleGame(Game game)
        {
            NpgsqlTransaction transaction = connection.BeginTransaction();

            if (insertGame == null)
            {
                insertGame = connection.CreateCommand();
                insertGame.CommandText = "INSERT INTO dump_psxdatacenter_games " +
                                         "(platform,sku,title,languages,additionals,commontitle,region,genreid,developerid,publisherid,daterelease,cover,description,barcode) " +
                                         "VALUES" +
                                         "(@platform,@sku,@title,@languages,@additionals,@commontitle,@region,@genreid,@developerid,@publisherid,@daterelease,@cover,@description,@barcode) " +
                                         "RETURNING id";
                insertGame.Parameters.Add("@platform", NpgsqlDbType.Varchar);
                insertGame.Parameters.Add("@sku", NpgsqlDbType.Varchar);
                insertGame.Parameters.Add("@title", NpgsqlDbType.Varchar);
                insertGame.Parameters.Add("@languages", NpgsqlDbType.Varchar);
                insertGame.Parameters.Add("@additionals", NpgsqlDbType.Boolean);
                insertGame.Parameters.Add("@commontitle", NpgsqlDbType.Varchar);
                insertGame.Parameters.Add("@region", NpgsqlDbType.Varchar);
                insertGame.Parameters.Add("@genreid", NpgsqlDbType.Integer);
                insertGame.Parameters.Add("@developerid", NpgsqlDbType.Integer);
                insertGame.Parameters.Add("@publisherid", NpgsqlDbType.Integer);
                insertGame.Parameters.Add("@daterelease", NpgsqlDbType.Date);
                insertGame.Parameters.Add("@cover", NpgsqlDbType.Bytea);
                insertGame.Parameters.Add("@description", NpgsqlDbType.Text);
                insertGame.Parameters.Add("@barcode", NpgsqlDbType.Varchar);
            }

            insertGame.Parameters["@platform"].Value = game.Platform;
            insertGame.Parameters["@sku"].Value = game.SKU;
            insertGame.Parameters["@title"].Value = game.Title;

            if (!string.IsNullOrEmpty(game.Language))
                insertGame.Parameters["@languages"].Value = game.Language;
            else
                insertGame.Parameters["@languages"].Value = DBNull.Value;

            insertGame.Parameters["@additionals"].Value = game.AdditionalData;

            if (!string.IsNullOrEmpty(game.CommonTitle))
                insertGame.Parameters["@commontitle"].Value = game.CommonTitle;
            else
                insertGame.Parameters["@commontitle"].Value = DBNull.Value;

            insertGame.Parameters["@region"].Value = game.Region;

            if (!string.IsNullOrEmpty(game.Genre))
                insertGame.Parameters["@genreid"].Value = GetGenreId(game.Genre);
            else
                insertGame.Parameters["@genreid"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(game.Developer))
                insertGame.Parameters["@developerid"].Value = GetCompanyId(game.Developer);
            else
                insertGame.Parameters["@developerid"].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(game.Publisher))
                insertGame.Parameters["@publisherid"].Value = GetCompanyId(game.Publisher);
            else
                insertGame.Parameters["@publisherid"].Value = DBNull.Value;

            if (game.DateReleased != DateTime.MinValue)
                insertGame.Parameters["@daterelease"].Value = new NpgsqlDate(game.DateReleased);
            else
                insertGame.Parameters["@daterelease"].Value = DBNull.Value;

            if (game.Cover != null)
                insertGame.Parameters["@cover"].Value = game.Cover;
            else
                insertGame.Parameters["@cover"].Value = DBNull.Value;

            if (game.Description != null)
                insertGame.Parameters["@description"].Value = game.Description;
            else
                insertGame.Parameters["@description"].Value = DBNull.Value;

            if (game.Barcode != null)
                insertGame.Parameters["@barcode"].Value = game.Barcode;
            else
                insertGame.Parameters["@barcode"].Value = DBNull.Value;

            int gameId = (int) insertGame.ExecuteScalar();
            
            foreach (Screenshot screenshot in game.Screenshots)
            {
                int screenshotId = GetScreenshotId(screenshot);
                InsertGameScreenshot(gameId, screenshotId);
            }

            transaction.Commit();
            transaction.Dispose();
        }

        private NpgsqlCommand insertGameScreenshot;
        private void InsertGameScreenshot(int gameid, int screenshotid)
        {
            if (insertGameScreenshot == null)
            {
                insertGameScreenshot = connection.CreateCommand();
                insertGameScreenshot.CommandText =
                    "INSERT INTO dump_psxdatacenter_game_screenshots (gameId,screenshotId) VALUES (@gameid,@screenshotid)";
                insertGameScreenshot.Parameters.Add("@gameId", NpgsqlDbType.Integer);
                insertGameScreenshot.Parameters.Add("@screenshotId", NpgsqlDbType.Integer);
            }

            insertGameScreenshot.Parameters["@gameId"].Value = gameid;
            insertGameScreenshot.Parameters["@screenshotId"].Value = screenshotid;
            if (insertGameScreenshot.ExecuteNonQuery() != 1)
                throw new Exception("insert failed");
        }

        private NpgsqlCommand getGenreId, createGenre;
        private int GetGenreId(string genre)
        {
            if (getGenreId == null)
            {
                getGenreId = connection.CreateCommand();
                getGenreId.CommandText = "SELECT id FROM dump_psxdatacenter_genres WHERE name=@name";
                getGenreId.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            getGenreId.Parameters["@name"].Value = genre;
            NpgsqlDataReader dataReader = getGenreId.ExecuteReader();
            bool hasdata = dataReader.Read();
            if (hasdata)
            {
                int result = dataReader.GetInt32(0);
                dataReader.Dispose();
                return result;
            }
            dataReader.Dispose();

            if (createGenre == null)
            {
                createGenre = connection.CreateCommand();
                createGenre.CommandText = "INSERT INTO dump_psxdatacenter_genres (name) VALUES (@name) RETURNING id";
                createGenre.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            createGenre.Parameters["@name"].Value = genre;
            return (int)createGenre.ExecuteScalar();
        }

        private NpgsqlCommand getCompanyId, createCompany;
        private int GetCompanyId(string genre)
        {
            if (getCompanyId == null)
            {
                getCompanyId = connection.CreateCommand();
                getCompanyId.CommandText = "SELECT id FROM dump_psxdatacenter_companies WHERE name=@name";
                getCompanyId.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            getCompanyId.Parameters["@name"].Value = genre;
            NpgsqlDataReader dataReader = getCompanyId.ExecuteReader();
            bool hasdata = dataReader.Read();
            if (hasdata)
            {
                int result = dataReader.GetInt32(0);
                dataReader.Dispose();
                return result;
            }
            dataReader.Dispose();

            if (createCompany == null)
            {
                createCompany = connection.CreateCommand();
                createCompany.CommandText = "INSERT INTO dump_psxdatacenter_companies (name) VALUES (@name) RETURNING id";
                createCompany.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            createCompany.Parameters["@name"].Value = genre;
            return (int)createCompany.ExecuteScalar();
        }

        private NpgsqlCommand getScreenshotId, createScreenshot;
        private int GetScreenshotId(Screenshot screenshot)
        {
            if (getScreenshotId == null)
            {
                getScreenshotId = connection.CreateCommand();
                getScreenshotId.CommandText = "SELECT id FROM dump_psxdatacenter_screenshots WHERE name=@name";
                getScreenshotId.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            getScreenshotId.Parameters["@name"].Value = screenshot.Name;
            NpgsqlDataReader dataReader = getScreenshotId.ExecuteReader();
            bool hasData = dataReader.Read();
            if (hasData)
            {
                int result = dataReader.GetInt32(0);
                dataReader.Dispose();
                return result;
            }
            dataReader.Dispose();

            if (createScreenshot == null)
            {
                createScreenshot = connection.CreateCommand();
                createScreenshot.CommandText = "INSERT INTO dump_psxdatacenter_screenshots (name,buffer) VALUES (@name,@buffer) RETURNING id";
                createScreenshot.Parameters.Add("@name", NpgsqlDbType.Varchar);
                createScreenshot.Parameters.Add("@buffer", NpgsqlDbType.Bytea);
            }

            createScreenshot.Parameters["@name"].Value = screenshot.Name;
            createScreenshot.Parameters["@buffer"].Value = screenshot.Data;
            return (int) createScreenshot.ExecuteScalar();
        }

        private NpgsqlCommand isGameKnown;
        public bool GameKnown(string sku)
        {
            if (isGameKnown == null)
            {
                isGameKnown = connection.CreateCommand();
                isGameKnown.CommandText = "SELECT id FROM dump_psxdatacenter_games WHERE sku=@sku";
                isGameKnown.Parameters.Add("@sku", NpgsqlDbType.Varchar);
            }

            isGameKnown.Parameters["@sku"].Value = sku;
            NpgsqlDataReader datareader = isGameKnown.ExecuteReader();
            bool result = datareader.Read();
            datareader.Dispose();
            return result;
        }
    }
}

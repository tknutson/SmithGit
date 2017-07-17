using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace WebsiteManagement
{
    public class DatabaseManager
    {
        LoggerInfo loggerInfo;

        public DatabaseManager()
        {
            loggerInfo = new LoggerInfo()
            {
                ErrorFlag = true,
                FileName = "DatabaseManager.cs",
                RootPath = ConfigurationManager.AppSettings["LogRootPath"].ToString(),
                FolderName = ConfigurationManager.AppSettings["LogFolderName"].ToString()
            };
        }

        public List<PhotoInformation> getPhotoInfo(string categoryGuid)
        {
            var photoInfoList = new List<PhotoInformation>();
            MySqlConnection connection = null;

            connection = getMySqlConnection();

            var query = "SELECT ci.category_name, ci.category_id, pi.photo_id, pi.file_name, pi.original_name, " +
                "pi.original_description, cd.order_number, cd.photo_name, cd.photo_description, fi.file_root_location " +
                //",si.width " +
                "FROM photoCategoryDetails cd " +
                "JOIN photoCategoryInfo ci ON ci.category_id = cd.category_id AND ci.inactive_switch != 1 " +
                "JOIN photoInfo pi ON pi.photo_id = cd.photo_id AND pi.inactive_switch != 1 " +
                "JOIN photoFileInfo fi " +
                //"JOIN photoSizeInfo si ON si.size_id = pi.photo_id " +
                "WHERE RTRIM(ci.category_id) = ?";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("ci.category_id", categoryGuid);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var photoInfo = new PhotoInformation();
                photoInfo.categoryName = reader.GetString(0);
                photoInfo.categoryId = reader.GetString(1);
                photoInfo.photoId = reader.GetString(2);
                photoInfo.photoFileName = reader.GetString(3);
                photoInfo.photoName = reader.IsDBNull(7) ? reader.GetString(4) : reader.GetString(7);
                photoInfo.photoDescription = reader.IsDBNull(8) ? reader.GetString(5) : reader.GetString(8);
                photoInfo.photoOrderNumber = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                photoInfo.photoFileLocation = reader.GetString(9);
                //photoInfo.photoWidth = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                photoInfoList.Add(photoInfo);
            }
            reader = null;

            closeConnection(connection);

            return photoInfoList;
        }

        public List<PhotoInformation> getFrontPagePhotoInfo_MySQL()
        {
            var photoInfoList = new List<PhotoInformation>();
            MySqlConnection connection = null;

            connection = getMySqlConnection();

            var query = "SELECT ci.category_name, ci.category_id, pi.photo_id, pi.file_name, pi.original_name, " +
                "pi.original_description, fp.order_number, fp.photo_name, fp.photo_description, fi.file_root_location " +
                "FROM photoFrontPage fp " +
                "JOIN photoCategoryInfo ci ON ci.category_id = fp.category_id AND ci.inactive_switch != 1 " +
                "JOIN photoInfo pi ON pi.photo_id = fp.photo_id AND pi.inactive_switch != 1 " +
                "JOIN photoFileInfo fi " +
                "WHERE fp.inactive_switch != 1";
            var command = connection.CreateCommand();
            command.CommandText = query;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var photoInfo = new PhotoInformation();
                photoInfo.categoryName = reader.GetString(0);
                photoInfo.categoryId = reader.GetString(1);
                photoInfo.photoId = reader.GetString(2);
                photoInfo.photoFileName = reader.GetString(3);
                photoInfo.photoName = reader.IsDBNull(7) ? reader.GetString(4) : reader.GetString(7);
                photoInfo.photoDescription = reader.IsDBNull(8) ? reader.GetString(5) : reader.GetString(8);
                photoInfo.photoOrderNumber = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                photoInfo.photoFileLocation = reader.GetString(9);
                photoInfoList.Add(photoInfo);
            }
            reader = null;

            closeConnection(connection);

            return photoInfoList;
        }

        public List<PhotoInformation> getFrontPagePhotoInfo()
        {
            var photoInfoList = new List<PhotoInformation>();

            try
            {

                var connection = getSQLConnection();

                var command = new SqlCommand("select * from view_FrontPagePhotos", connection);

                connection.Open();

                var reader = command.ExecuteReader();

                PhotoInformation photoInfo;
                while (reader.Read())
                {
                    photoInfo = new PhotoInformation();

                    photoInfo.categoryName = reader["category_name"].ToString();
                    photoInfo.categoryId = reader["category_id"].ToString();
                    photoInfo.photoId = reader["photo_id"].ToString();
                    photoInfo.photoFileName = reader["file_name"].ToString();
                    photoInfo.photoName = reader["photo_name"] == null ? reader["original_name"].ToString() : reader["photo_name"].ToString();
                    photoInfo.photoDescription = reader["photo_description"] == null ? reader["original_name"].ToString() : reader["photo_description"].ToString();
                    photoInfo.photoOrderNumber = DBNull.Value.Equals(reader["order_number"]) ? 0 : Convert.ToInt32(reader["order_number"].ToString());
                    photoInfo.photoFileLocation = reader["file_root_location"].ToString();

                    photoInfoList.Add(photoInfo);
                }

                //photoInfo.categoryName = items.GetValue(0).ToString();
                //photoInfo.categoryId = items.GetValue(1).ToString();
                //photoInfo.photoId = items.GetValue(2).ToString();
                //photoInfo.photoFileName = items.GetValue(4).ToString();
                //photoInfo.photoName = items.GetValue(7) == null ? items.GetValue(4).ToString() : items.GetValue(7).ToString();
                //photoInfo.photoDescription = items.GetValue(8) == null ? items.GetValue(5).ToString() : items.GetValue(8).ToString();
                //photoInfo.photoOrderNumber = items.GetValue(6) == null ? 0 : Convert.ToInt32(items.GetValue(6));
                //photoInfo.photoFileLocation = items.GetValue(9).ToString();
            }
            catch (Exception Ex)
            {
                Logger.Write(loggerInfo, "getFrontPagePhotoInfo", Ex);
            }

            return photoInfoList;
        }

        private MySqlConnection getMySqlConnection()
        {
            //var connectionString = "Server=50.62.209.51;Database=photosSmithPix;Uid=pixAdmin;Pwd=VikesR0k;";
            var connectionString = "Server=.;Database=photosSmithPix;Uid=pixAdmin;Pwd=VikesR0k;";
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        private SqlConnection getSQLConnection()
        {
            var mgr = ConfigurationManager.ConnectionStrings;

            return new SqlConnection(ConfigurationManager.ConnectionStrings["photosSmithPixConnection"].ConnectionString);
        }

        private MySqlConnection getBiehlConnection()
        {
            var connectionString = "Server=184.168.194.51;Database=Biehl_Wedding_Bingo;Uid=pixAdmin;Pwd=ARodgPac1;";
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public List<BingoInformation> getBingoInfo()
        {
            var bingoList = new List<BingoInformation>();
            MySqlConnection connection = null;

            connection = getBiehlConnection();

            var query = "SELECT id, short_name, description FROM pixAdmin.Bingo";
            var command = connection.CreateCommand();
            command.CommandText = query;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var bingoInfo = new BingoInformation();
                bingoInfo.bingoId = reader.GetString(0);
                bingoInfo.shortName = reader.GetString(1);
                bingoInfo.description = reader.GetString(2);
                bingoList.Add(bingoInfo);
            }
            reader = null;

            closeConnection(connection);

            return bingoList;
        }

        private void closeConnection(MySqlConnection connection)
        {
             connection.Close();
        }
    }
}

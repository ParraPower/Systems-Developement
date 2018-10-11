using ProjectCSV;
using System.Collections.Generic;
using MySql;
using System.Linq;
using System.Text;
using SimulatorCore;

namespace ImportTrackSimulatorDataUtil
{
    public class ImportTrackSimulatorData
    {
        public bool TrackPointsWithStationImport()
        {
            #region Retrieve tracks and stations from file
            string filePath = "G:\\Users\\Ahmed\\Documents\\My Documents\\Stations.csv";

            Dictionary<string, string>[] dataFromFile = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile(filePath, 5, true);
            #endregion

            #region Process tracks and stations from file into objects
            List<TrackPoint> trackPointsFromFile = new List<TrackPoint>();
            List<Station> stationsFromFile = new List<Station>();
            for (int x = 0; x < dataFromFile.Length; ++x)
            {
                Dictionary<string, object> dctOfObjs = new Dictionary<string, object>();

                foreach (var item in dataFromFile[x])
                {
                    dctOfObjs.Add(item.Key, (object)item.Value);   
                }
                TrackPoint trackPointObj = new TrackPoint(dctOfObjs);
                trackPointsFromFile.Add(new TrackPoint(dctOfObjs));

                string error = "";
                if (Station.IsValidName(dataFromFile[x]["Name"], out error))
                {
                    Station stationObj = new Station(dataFromFile[x]["Name"], trackPointObj);
                    stationsFromFile.Add(stationObj);
                }
            }
            #endregion

            MySql.Data.MySqlClient.MySqlConnection objMySqlConnection_Transport = new MySql.Data.MySqlClient.MySqlConnection(
                "Server=127.0.0.1; Port=3306; Database=transport; Uid=root; Pwd=ihateprogramming;"
            );

            objMySqlConnection_Transport.Open();

            MySql.Data.MySqlClient.MySqlCommand objMysqlCmd = objMySqlConnection_Transport.CreateCommand();
   
            #region Retrieve tracks from database
            objMysqlCmd.CommandType = System.Data.CommandType.Text;

            objMysqlCmd.CommandText = @"
                SELECT ID, ASWKT(Coordinate) AS Coordinate, MaxSpeed, TrackCount FROM TrackPoint
            ";

            List<TrackPoint> trackPointsFromDB = new List<TrackPoint>();
            int count = 0;
            using (System.Data.IDataReader reader = objMysqlCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    trackPointsFromDB.Add(new TrackPoint(reader));
                    ++count;
                }
            }
            #endregion

            #region Retrieve stations from database
            objMysqlCmd.CommandType = System.Data.CommandType.Text;

            objMysqlCmd.CommandText = @"
                SELECT Station.*
                    , ASWKT(TrackPoint.Coordinate) AS Coordinate
                    , TrackPoint.MaxSpeed
                    , TrackPoint.TrackCount 
                FROM Station
                INNER JOIN TrackPoint ON TrackPoint.ID = Station.TrackPointID
            ";

            List<Station> stationsFromDB = new List<Station>();

            using (System.Data.IDataReader reader = objMysqlCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    stationsFromDB.Add(new Station(reader));
                    //stationsFromDB
                    //    .Add(
                    //        new Station(
                    //            (string)reader["Name"]
                    //            , trackPointsFromDB
                    //                .First(x => x.DatabaseUniqueID == (int)reader["TrackPointID"]))
                    //    );
                   
                }
            }
            #endregion

            #region Compare and then insert track records
            List<TrackPoint> trackPointsToInsert = new List<TrackPoint>();
            //List<Dictionary<string, string>> dataToInsert = new List<Dictionary<string, string>>();

            if (trackPointsFromFile.Count > 0)
            {
                int x = 0;

                while (x < trackPointsFromFile.Count)
                {
                    if ((!trackPointsFromDB.Any(y => y == trackPointsFromFile[x])) 
                        && !(trackPointsToInsert.Any(z => z == trackPointsFromFile[x]))) 
                    {
                        trackPointsToInsert.Add(trackPointsFromFile[x]);
                    }
                    ++x;
                }
            }
            else
            {
                trackPointsToInsert = trackPointsFromFile;
                //int i = 0;
                //while (i < trackPointsFromFile.Count)
                //{
                    //Dictionary<string, string> rowToInsert = new Dictionary<string, string>();

                    //rowToInsert.Add("Coordinate", "POINT(" + dataFromFile[i][(i + 1).ToString() + "A"].Replace(",", "") + ")");
                    //rowToInsert.Add("MaxSpeed", dataFromFile[i][(i + 1).ToString() + "B"]);
                    //rowToInsert.Add("TrackCount", dataFromFile[i][(i + 1).ToString() + "C"]);

                    //dataToInsert.Add(rowToInsert);

                    //++i;
                //}
            }

            if (trackPointsToInsert.Count > 0)
            {
                objMysqlCmd.CommandType = System.Data.CommandType.Text;

                //objMysqlCmd.CommandText = @"
                //    CREATE TEMPORARY TABLE tmp_TrackPoint (Coordinate POINT, MaxSpeed DECIMAL, TrackCount INT); 

                objMysqlCmd.CommandText = @"
                    INSERT INTO TrackPoint 
                        (Coordinate, MaxSpeed, TrackCount) 
                    VALUES
                ";

                StringBuilder sb = new StringBuilder();

                int i = 0;

                while (i < trackPointsToInsert.Count)
                {
                    sb.Append(
                        string.Format(
                            "(st_pointfromtext('{0}'), {1}, {2}){3}"
                                , trackPointsToInsert[i].CoordinateToDatabaseString()
                                , trackPointsToInsert[i].MaxSpeed
                                , trackPointsToInsert[i].TrackCount
                                , (i != (trackPointsToInsert.Count - 1) ? "," : ";")));
                    ++i;
                }

                objMysqlCmd.CommandText += sb.ToString();

                objMysqlCmd.ExecuteNonQuery();
            }
            #endregion

            #region Compare and then insert or update station records 
            List<Station> stationsToInsert = new List<Station>();
            List<Station> stationsToUpdate = new List<Station>();
            if (stationsFromFile.Count > 0)
            {
                int x = 0;

                while (x < stationsFromFile.Count)
                {
                    if ((!stationsFromDB.Any(y => y == stationsFromFile[x]))
                        && !(stationsToInsert.Any(z => z == stationsFromFile[x])))
                    {
                        stationsToInsert.Add(stationsFromFile[x]);
                    }
                    else if (stationsFromDB.Any(y => y == stationsFromFile[x])
                        /*&& !(stationsToUpdate.Any(z => z == stationsFromFile[x]))*/)
                    {
                        stationsToUpdate.Add(stationsFromFile[x]);
                    }
                    // add logic here to update station records
                    ++x;
                }
            }
            else
            {
                stationsToInsert = stationsFromFile;
            }

            if (stationsToInsert.Count > 0)
            {
                objMysqlCmd.CommandType = System.Data.CommandType.Text;

                objMysqlCmd.CommandText = @"
                    INSERT INTO Station 
                        (Name, TrackPointID) 
                    VALUES
                        
                ";

                StringBuilder sb = new StringBuilder();

                int i = 0;

                while (i < stationsToInsert.Count)
                {
                    sb.Append(
                        string.Format(
                            "('{0}', {1}){2}"
                                , stationsToInsert[i].Name
                                , trackPointsFromDB.First(x => x == stationsToInsert[i].Location).DatabaseUniqueID
                                , (i != (stationsToInsert.Count - 1) ? "," : ";")));
                    ++i;
                }

                objMysqlCmd.CommandText += sb.ToString();

                objMysqlCmd.ExecuteNonQuery();
            }

            if (stationsToUpdate.Count > 0)
            {
                objMysqlCmd.CommandType = System.Data.CommandType.Text;

                StringBuilder sb = new StringBuilder();

                int i = 0;

                while (i < stationsToUpdate.Count)
                {
                    sb.Append(
                        string.Format(
                            "UPDATE Station SET Name = '{0}', TrackPointID = {1} WHERE ID = {2};"
                                , stationsToUpdate[i].Name
                                , trackPointsFromDB.First(x => x == stationsToUpdate[i].Location).DatabaseUniqueID
                                , stationsToUpdate[i].DatabaseUniqueID));
                    ++i;
                }

                objMysqlCmd.CommandText = sb.ToString();

                objMysqlCmd.ExecuteNonQuery();
            }
            #endregion

            objMySqlConnection_Transport.Close();

            return false;
        }
    }
}

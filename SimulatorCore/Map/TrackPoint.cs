using Extensions;
using System;
using System.Collections.Generic;
using System.Device.Location;

namespace SimulatorCore
{
    public class TrackPoint : ITrackPoint
    {
        private int? _databaseUniqueID;
        private Guid _uniqueID;
        private GeoCoordinate _coordinate;
        private Double _maxSpeed;
        //private Double _gauge;
        private SByte _trackCount;

        private void _instializeObjectWithDefaults()
        {
            _uniqueID = new Guid();
            //_gauge = 1.435;
            _trackCount = 1;
            _maxSpeed = 10;
        }

        public TrackPoint(GeoCoordinate Coordinate)
        {
            _instializeObjectWithDefaults();
            _coordinate = Coordinate;
        }

        public TrackPoint(GeoCoordinate Coordinate, Double MaxSpeed, /*Double Gauge,*/ SByte TrackCount)
        {
            _instializeObjectWithDefaults();
            _coordinate = Coordinate;
            _maxSpeed = MaxSpeed;
            //_gauge = Gauge;
            _trackCount = TrackCount; 
        }

        public TrackPoint(Dictionary<string, object> DictionaryObject)
        {
            _instializeObjectWithDefaults();

            try
            {
                string coord = (string)DictionaryObject["Coordinate"];

                if (coord == null)
                {
                    throw new FormatException("Coordinate data cannot be empty.");
                }

                string[] coordSplits = coord.Split(',');

                double lat = 0, lng = 0;

                try
                {
                    lat = Convert.ToDouble(coordSplits[0]);
                    lng = Convert.ToDouble(coordSplits[1]);
                }
                catch
                {
                    new FormatException("Coordinate data must be in the following format: {double:Latitude}, {double:Longitude}");
                }

                _coordinate = new GeoCoordinate(lat, lng);
            }
            catch (FormatException forExp)
            {
               new FormatException("Data supplied for coordinates is not in the correct format. " + forExp.Message);
            }
            catch (InvalidCastException invCstExp)
            {
                new InvalidCastException("Data supplied for coordinates is not in the correct format. " + invCstExp.Message);
            }
            catch (Exception exp)
            {
                new Exception("Unable to create object from data supplied. " + exp.Message);
            }

            try
            {
                _trackCount = (sbyte)( ((sbyte?)(DictionaryObject["TrackCount"]) == null) ? 1 : DictionaryObject["TrackCount"]);
            }
            catch (InvalidCastException invCstExp)
            {
                new InvalidCastException("Data supplied for track count is not in the correct format. " + invCstExp.Message);
            }
            catch (Exception exp)
            {
                new Exception("Unable to create object from data supplied. " + exp.Message);
            }

            try
            {
                _maxSpeed = (double)( ((double?)(DictionaryObject["MaxSpeed"]) == null) ? 10 : DictionaryObject["MaxSpeed"]);
            }
            catch (InvalidCastException invCstExp)
            {
                new InvalidCastException("Data supplied for max speed is not in the correct format TrackPoint. " + invCstExp.Message);
            }
            catch (Exception exp)
            {
                new Exception("Unable to create object from data supplied. " + exp.Message);
            }
        }

        public TrackPoint(System.Data.IDataReader reader)
        {
            if (reader != null)
            {
                try
                {
                    _databaseUniqueID = (int)(reader.GetOrdinalOf("TrackPointID") > -1 ? reader["TrackPointID"] : reader["ID"]);

                    string coord = (string)reader["Coordinate"];
                    
                    if (!coord.ToUpper().StartsWith("POINT"))
                    {
                        throw new Exception();
                    }

                    string[] latAndLng = coord.Replace("POINT(", "").Replace(")", "").Split(' ');

                    _coordinate 
                        = new GeoCoordinate(
                            Convert.ToDouble(latAndLng[0])
                            , Convert.ToDouble(latAndLng[1])
                        );

                    _trackCount = (sbyte)(reader.IsDBNull(reader.GetOrdinal("TrackCount")) ? 1 : Convert.ToInt16(reader["TrackCount"].ToString()));
                    _maxSpeed = (double)(reader.IsDBNull(reader.GetOrdinal("MaxSpeed")) ? 10 : Convert.ToInt16(reader["MaxSpeed"].ToString()));
                }
                catch
                {
                    throw new FormatException("Row of data cannot be converted to object of TrackPoint");
                }
            }
        }

        public string CoordinateToDatabaseString()
        {
            return "POINT(" + _coordinate.Latitude + " " + _coordinate.Longitude + ")";
        }

        public int? DatabaseUniqueID
        {
            get
            {
                return _databaseUniqueID;
            }
        }

        public Guid UniqueID
        {
            get
            {
                return _uniqueID;
            }
        }

        public GeoCoordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
        }
        public Double MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }
        }
        //public Double Gauge
        //{
        //    get
        //    {
        //        return _gauge;
        //    }
        //}
        public SByte TrackCount
        {
            get
            {
                return _trackCount;
            }
        }

        public static bool operator !=(TrackPoint LHS, TrackPoint RHS)
        {
            return Comparison(LHS, RHS) != 0;
        }

        public static bool operator ==(TrackPoint LHS, TrackPoint RHS)
        {
            return Comparison(LHS, RHS) == 0;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return (this == ((TrackPoint)obj)); //return base.Equals(obj);
            }
            catch
            {
                return false;
            }
        }

        private static int Comparison(TrackPoint LHS, TrackPoint RHS)
        {
            if (LHS._coordinate == RHS._coordinate)
                return 0;

            return 1;
        }
    }
}
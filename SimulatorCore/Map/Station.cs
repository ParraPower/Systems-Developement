using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorCore
{
    public class Station 
    {
        private int _databaseUniqueID;
        private String _name;
        private TrackPoint _location;

        public Station(string Name, TrackPoint Location)
        {
            try
            {
                string error = "";
                if (!IsValidName(Name, out error))
                {
                    throw new Exception(error);
                }

                _name = Name;
            }
            catch (Exception exp)
            {
                throw new Exception("Unable to create object from data supplied. " + exp.Message);
            }

            _location = Location;
        }

        public Station(System.Data.IDataReader reader)
        {
            if (reader != null)
            {
                try
                {
                    _databaseUniqueID = (int)reader["ID"];

                    _name = (string)reader["name"];

                    _location = new TrackPoint(reader);
                }
                catch
                {
                    throw new FormatException("Row of data cannot be converted to object of TrackPoint");
                }
            }
        }

        public int? DatabaseUniqueID
        {
            get
            {
                return _databaseUniqueID;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public TrackPoint Location
        {
            get
            {
                return _location;
            }
        }

        /*public Station(Dictionary<string, object> DictionaryObject)
        {
            try
            {
                string name = (string)DictionaryObject["name"];
                if (String.IsNullOrWhiteSpace(name))
                {
                    throw new Exception("Name of station cannot be empty.");
                }

                _name = name;
            }
            catch (Exception exp)
            {
                new Exception("Unable to create object from data supplied. " + exp.Message);
            }
        }*/

        public static bool IsValidName(string name, out string error)
        {
            error = null;
            if (String.IsNullOrWhiteSpace(name))
            {
                error = "Name of station cannot be empty.";
                return false;
            }

            return true;
        }

        public static bool operator != (Station LHS, Station RHS)
        {
            return Comparison(LHS, RHS) != 0;
        }

        public static bool operator ==(Station LHS, Station RHS)
        {
            return Comparison(LHS, RHS) == 0;
        }

        private static int Comparison(Station LHS, Station RHS)
        {
            if (LHS._name == RHS._name && LHS._location == RHS._location)
                return 0;

            return 1;
        }
    }
}

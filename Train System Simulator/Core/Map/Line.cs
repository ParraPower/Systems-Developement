using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Line
    {
        private Guid _uniqueID;
        private String _name;
        private List<RailVehicle> _railVehicles;
        private List<TrackPoint> _tracksPoints;

        public Line (String Name, List<RailVehicle> RailVehicles, List<TrackPoint> TrackPoints)
        {
            _uniqueID = new Guid();
            _name = Name;
            _railVehicles = RailVehicles;
            _tracksPoints = TrackPoints;
        }

        public Guid UniqueID
        {
            get
            {
                return _uniqueID;
            }
        }

        public List<TrackPoint> TrackPoints
        {
            get
            {
                return _tracksPoints;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public List<RailVehicle> RailVehciles
        {
            get
            {
                return _railVehicles;
            }
        }

        public ConsoleColor Colour
        {
            get
            {
                return ConsoleColor.Black;
            }
        }
    }
}

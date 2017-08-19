using System;

namespace Core
{
    public class RailVehicle : IRailVehicle
    {
        private Guid _uniqueID;
        private String _name;
        private Decimal _speed;
        private TrackPoint _currentTrack;

        public RailVehicle(String Name)
        {
            _uniqueID = new Guid();
            _name = Name;
            _speed = 0;
        }

        public Guid UniqueID
        {
            get
            {
                return _uniqueID;
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

        public TrackPoint CurrentTrack
        {
            get
            {
                return _currentTrack;
            }
        }

        public Decimal Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
    }
}
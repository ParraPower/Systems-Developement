using System;
using System.Device.Location;

namespace Core
{
    public class TrackPoint : ITrackPoint
    {
        private Guid _uniqueID;
        private GeoCoordinate _coordinate;
        private Double _maxSpeed;
        private Double _gauge;
        private SByte _trackCount;

        public TrackPoint(GeoCoordinate Coordinate)
        {
            _uniqueID = new Guid();
            _coordinate = Coordinate;
            _gauge = 1.435;
            _trackCount = 1;
            _maxSpeed = 10;

        }

        public TrackPoint(GeoCoordinate Coordinate, Double MaxSpeed, Double Gauge, SByte TrackCount)
        {
            _uniqueID = new Guid();
            _coordinate = Coordinate;
            _maxSpeed = MaxSpeed;
            _gauge = Gauge;
            _trackCount = TrackCount;
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
        public Double Gauge
        {
            get
            {
                return _gauge;
            }
        }
        public SByte TrackCount
        {
            get
            {
                return _trackCount;
            }
        }
    }
}
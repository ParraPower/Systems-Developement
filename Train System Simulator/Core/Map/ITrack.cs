using System;
using System.Device.Location;

namespace Core
{
    interface ITrackPoint
    {
        Guid UniqueID
        {
            get;
        }
        GeoCoordinate Coordinate
        {
            get;
        }
        //GeoCoordinate PointB
        //{
        //    get;
        //}
        Double MaxSpeed
        {
            get;
        }
        Double Gauge
        {
            get;
        }
        SByte TrackCount
        {
            get;
        }
    }
}
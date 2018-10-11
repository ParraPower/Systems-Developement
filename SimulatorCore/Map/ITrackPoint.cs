using System;
using System.Device.Location;

namespace SimulatorCore
{
    interface ITrackPoint
    {
        int? DatabaseUniqueID
        {
            get;
        }
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
        //Double Gauge
        //{
        //    get;
        //}
        SByte TrackCount
        {
            get;
        }
    }
}
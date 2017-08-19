using System;

namespace Core
{

    interface IRailVehicle
    {
        Guid UniqueID
        {
            get;
        }

        String Name
        {
            get; set;
        }

        TrackPoint CurrentTrack
        {
            get;
        }

        Decimal Speed
        {
            get; set;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;

namespace ITS.Domain.Models.Bus.Admin
{
    public class BusStationListViewModel
    {
        public IList<BusStationModel> StationList { get; set; }
    }

    public class BusStationModel
    {
        public Guid ID;
        public Guid RoadSessionID;
        public String StationName;
        public float Position_X;
        public float Position_Y;

        public string StreetName { get; set; }
    }
}

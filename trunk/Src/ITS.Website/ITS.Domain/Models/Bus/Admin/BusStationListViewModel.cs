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
        public BusStation BusStation { get; set; }
        public string StreetName { get; set; }
    }
}

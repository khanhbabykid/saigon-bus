using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;

namespace ITS.Domain.Models.Bus.Website
{
    public class BusStationDetailViewModel
    {
        public BusStation BusStation { get; set; }
        public IList<BusRoute> BusRoutes { get; set; }
        public string RoadName { get; set; }
    }
}

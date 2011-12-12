using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;

namespace ITS.Domain.Models.Bus.Admin
{
    public class BusMovementsViewModel
    {
        public BusRoute BusRoute { get; set; }
        public IList<BusMovementModel> Movements { get; set; }
    }

    public class BusMovementModel
    {
        public Guid ID { get; set; }
        public Guid BusStationFrom { get; set; }
        public Guid BusStationTo { get; set; }
        public bool Direction { get; set; }
        public int OrderNumber { get; set; }

        //Addition
        public string BusStationNameFrom { get; set; }
        public string BusStationNameTo { get; set; }
    }
}

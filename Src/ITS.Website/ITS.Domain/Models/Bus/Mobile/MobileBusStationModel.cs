using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITS.Domain.Models.Bus.Mobile
{
    public class MobileBusStationModel
    {
        public Guid ID;
        public String StationName;
        public float Position_X;
        public float Position_Y;

        public string StreetName { get; set; }
        public string Routes { get; set; }
    }
}

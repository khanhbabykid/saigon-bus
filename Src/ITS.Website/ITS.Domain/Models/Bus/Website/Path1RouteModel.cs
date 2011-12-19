using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;

namespace ITS.Domain.Models.Bus.Website
{
    public class Path1RoutesModel
    {

        public BusStation Station_Src { get; set; }
        public BusRoute BusRoute { get; set; }
        public BusStation Station_Dst { get; set; }
    }
}

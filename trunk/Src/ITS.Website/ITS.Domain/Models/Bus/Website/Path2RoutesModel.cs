using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;

namespace ITS.Domain.Models.Bus.Website
{
    public class Path2RoutesModel
    {

        public BusStation Station_Src { get; set; }
        public BusRoute BusRoute1 { get; set; }
        public BusStation IntermediateStation { get; set; }
        public BusRoute BusRoute2 { get; set; }
        public BusStation Station_Dst { get; set; }
    }
}

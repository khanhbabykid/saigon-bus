using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities.Extensions;
using ITS.Domain.Entities;

namespace ITS.Domain.Models.Bus
{
    public class AddIntermediatePointViewModel
    {
        public Guid MovementID { get; set; }
        public IList<IntermediatePoint> PointList { get; set; }
        public double? NewPointLat { get; set; }
        public double? NewPointLng { get; set; }
        public int NewPointOrder { get; set; }
    }
}

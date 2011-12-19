using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ITS.Domain.Models.Bus
{
    public class RoadSessionViewModel
    {
        public Guid ID { get; set; }
        public Guid RoadID { get; set; }
        public int AddressLower { get; set; }
        public int AddressUpper { get; set; }
        [Required(AllowEmptyStrings = false)]
        public String Description { get; set; }
        public float PositionLower_X { get; set; }
        public float PositionLower_Y { get; set; }
        public float PositionUpper_X { get; set; }
        public float PositionUpper_Y { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ITS.Domain.Models.Bus.Admin
{
    public class AddBusStationViewModel
    {

        public Guid SelectedRoadID { get; set; }
        public Guid SelectedRoadSessionID { get; set; }
        [Required(AllowEmptyStrings=false)]
        public string StationName { get; set; }
        public float Position_X { get; set; }
        public float Position_Y { get; set; }
    }
}

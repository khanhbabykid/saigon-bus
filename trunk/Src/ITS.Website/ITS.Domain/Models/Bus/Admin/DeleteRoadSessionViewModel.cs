using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITS.Domain.Models.Bus.Admin
{
    public class DeleteRoadSessionViewModel
    {
        public Guid RoadSessionID { get; set; }
        public Guid RoadID { get; set; }
    }
}

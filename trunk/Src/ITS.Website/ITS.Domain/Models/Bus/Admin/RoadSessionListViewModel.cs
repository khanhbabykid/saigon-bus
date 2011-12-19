using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ITS.Domain.Models.Bus.Admin
{
    public class RoadSessionListViewModel
    {
        public Guid RoadID { get; set; }
        public IList<RoadSessionViewModel> RoadSessionList { get; set; }
    }
}

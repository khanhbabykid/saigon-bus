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
        public IList<RoadSessionViewModel> RoadSessionList { get; set; }
    }
}

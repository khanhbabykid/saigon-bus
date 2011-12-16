using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ITS.Domain.Models.Bus.Admin
{
    public class RoadListViewModel
    {
        public IList<RoadModel> RoadList { get; set; }
        public string NewRoad { get; set; }
    }

    public class RoadModel
    {
        public Guid RoadID { get; set; }
        [Required(ErrorMessage="Tên đường không được trống")]
        public string RoadName { get; set; }
    }
}

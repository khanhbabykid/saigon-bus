using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ITS.Domain.Models.Bus.Admin
{
    public class BusRouteListViewModel
    {
        public IList<BusRouteModel> BusRouteList { get; set; }
        public string NewBusRoute { get; set; }
    }

    public class BusRouteModel
    {
        public Guid RouteID { get; set; }
        [Required(ErrorMessage="Tên tuyến không được trống")]
        public string RouteName { get; set; }
    }
}

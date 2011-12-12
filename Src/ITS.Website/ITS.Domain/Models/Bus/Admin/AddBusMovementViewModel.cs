using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using System.Web.Mvc;

namespace ITS.Domain.Models.Bus.Admin
{
    public class AddBusMovementViewModel
    {
        public Guid RouteID { get; set; }
        public Boolean Direction { get; set; }
        public IList<BusStation> StationFrom { get; set; }
        public IList<BusStation> StationTo { get; set; }
        public IEnumerable<SelectListItem> StationFromSelectList { get; set; }
        public IEnumerable<SelectListItem> StationToSelectList { get; set; }
        public int OrderNumber { get; set; }
        public Guid SelectedStationFrom { get; set; }
        public Guid SelectedStationTo { get; set; }
    }
}

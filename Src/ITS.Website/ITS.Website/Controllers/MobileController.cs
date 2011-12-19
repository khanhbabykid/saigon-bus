using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Domain.Models.Bus.Website;
using ITS.Business.Abstract;

namespace ITS.Website.Controllers
{
    public class MobileController : Controller
    {
        private readonly IBusService busService;
        public MobileController(IBusService busService)
        {
            this.busService = busService;
        }

        public JsonResult StationDetail(Guid ID)
        {
            BusStationDetailViewModel model = new BusStationDetailViewModel()
            {
                BusStation = busService.GetBusStation(ID),
                RoadName = busService.GetRoadNameFromBusStationID(ID),
                BusRoutes = busService.BusRoutesThroughAStation(ID)
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}

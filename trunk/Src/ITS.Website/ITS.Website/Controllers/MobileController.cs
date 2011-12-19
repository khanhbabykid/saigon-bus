using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Domain.Models.Bus.Website;
using ITS.Business.Abstract;
using ITS.Domain.Models.Bus.Admin;
using ITS.Domain.Entities;
using ITS.Domain.Models.Bus.Mobile;
using ITS.Domain.Entities.Extensions;

namespace ITS.Website.Controllers
{
    public class MobileController : Controller
    {
        private readonly IBusService busService;
        public MobileController(IBusService busService)
        {
            this.busService = busService;
        }
        #region Bus Route
        public JsonResult BusRouteList()
        {
            IList<BusRoute> routes = busService.GetAllBusRoutes();
            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BusRouteList_2()
        {
            IList<RouteInfoModel> list = new List<RouteInfoModel>();
            IList<BusRoute> routes = busService.GetAllBusRoutes();
            IList<Point> points;
            foreach (BusRoute r in routes)
            {
                points = busService.GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(r.RouteID, true);
                list.Add(new RouteInfoModel()
                {
                    ID = r.RouteID,
                    Name = r.RouteName,
                    Movements1 = BuildMovementString(busService.GetMovementsOfARouteInOrder(r.RouteID, true)),
                    Movements2 = BuildMovementString(busService.GetMovementsOfARouteInOrder(r.RouteID, false)),
                    AllStationPostions = points
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RouteInfo(Guid ID)
        {
            IList<Point> points = busService.GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(ID, true);
            Point center = new Point() { lng = points.First().lng, lat = points.First().lat };
            RouteInfoModel model = new RouteInfoModel()
            {
                Movements1 = BuildMovementString(busService.GetMovementsOfARouteInOrder(ID, true)),
                Movements2 = BuildMovementString(busService.GetMovementsOfARouteInOrder(ID, false)),
                AllStationPostions = points
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        private string BuildMovementString(IList<string> movements)
        {
            string result = string.Empty;
            foreach (var m in movements)
            {
                result = result + " - " + m;
            }
            return result;
        }
        #endregion
        #region Bus Station
        public JsonResult StationList()
        {
            IList<BusStationModel> stations = BuildStationModel(busService.GetAllBusStation());
            return Json(stations, JsonRequestBehavior.AllowGet);
        }
        private IList<BusStationModel> BuildStationModel(IList<BusStation> stations)
        {
            IList<BusStationModel> list = new List<BusStationModel>();
            foreach (BusStation s in stations)
            {
                list.Add(new BusStationModel()
                {
                    ID = s.ID,
                    StationName = s.StationName,
                    Position_X = s.Position_X,
                    Position_Y = s.Position_Y,
                    RoadSessionID = s.RoadSessionID,
                    StreetName = busService.GetRoadNameFromBusStationID(s.ID)
                });
            }
            return list.OrderBy(x => x.StreetName).ToList();
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
        #endregion
    }
}

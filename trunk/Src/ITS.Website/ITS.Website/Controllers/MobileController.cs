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
            IList<MobileBusStationModel> stations = BuildStationModel(busService.GetAllBusStation());
            return Json(stations, JsonRequestBehavior.AllowGet);
        }
        private IList<MobileBusStationModel> BuildStationModel(IList<BusStation> stations)
        {
            IList<MobileBusStationModel> list = new List<MobileBusStationModel>();
            foreach (BusStation s in stations)
            {
                list.Add(new MobileBusStationModel()
                {
                    ID = s.ID,
                    StationName = s.StationName,
                    Position_X = s.Position_X,
                    Position_Y = s.Position_Y,
                    StreetName = busService.GetRoadNameFromBusStationID(s.ID),
                    Routes = BusRoutesThroughAStation(s.ID)
                });
            }
            return list.OrderBy(x => x.StreetName).ToList();
        }
        private string BusRoutesThroughAStation(Guid ID)
        {
            string result = string.Empty;
            IList<BusRoute> routes = busService.BusRoutesThroughAStation(ID);
            foreach (BusRoute r in routes)
            {
                result = result + " " + r.RouteName;
            }
            return result;
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
        #region Your Position
        public JsonResult UserPosition(float lng, float lat)
        {
            IList<MobileBusStationModel> stations = BuildStationModel(busService.GetAllBusStation());
            Point point = new Point() { lng = lng, lat = lat };
            if (point != null)
            {
                IList<BusStation> nearestStations = busService.GetNearestStations(point, 4);

                stations = BuildStationModel(nearestStations);
            }

            return Json(stations, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Find Route
        public JsonResult FindBusRoute(int AddNo1, string Street1, int AddNo2, string Street2)
        {
            MobileFindBusRouteModel model = new MobileFindBusRouteModel();
            //model.debug = Street1 + Street2;
            Point Position_Src = busService.GetPositionFromAddress(AddNo1, Street1);
            Point Position_Dst = busService.GetPositionFromAddress(AddNo2, Street2);
            if (Position_Src != null && Position_Dst != null)
            {
                IList<BusStation> nearestStations_Src = busService.GetNearestStations(Position_Src, 2);
                IList<BusStation> nearestStations_Dst = busService.GetNearestStations(Position_Dst, 2);
                IList<BusRoute> tempRouteList;
                Path1RoutesModel tempPath1;
                List<Path1RoutesModel> path1 = new List<Path1RoutesModel>();
                List<Path2RoutesModel> path2 = new List<Path2RoutesModel>();
                foreach (BusStation s1 in nearestStations_Src)
                {
                    foreach (BusStation s2 in nearestStations_Dst)
                    {
                        tempRouteList = busService.FindPath_OneRoute(s1.ID, s2.ID);
                        foreach (BusRoute r in tempRouteList)
                        {
                            tempPath1 = new Path1RoutesModel()
                            {
                                Station_Src = s1,
                                BusRoute = r,
                                Station_Dst = s2
                            };
                            if (!path1.Contains(tempPath1))
                                path1.Add(tempPath1);
                        }
                        path2.AddRange(busService.FindPath_2Routes(s1.ID, s2.ID));
                    }
                }

                foreach (Path1RoutesModel p in path1)
                {
                    model.Path_OneRoute.Add(new SimplePath1RoutesModel() { BusRoute = p.BusRoute.RouteName, Station_Dst= p.Station_Dst.StationName, Station_Src = p.Station_Src.StationName });
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

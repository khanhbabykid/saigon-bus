using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Business.Abstract;
using ITS.Domain.Entities;
using ITS.Domain.Models.Bus;
using ITS.Domain.Models.Bus.Admin;
using ITS.Domain.Models.Bus.Website;
using ITS.Domain.Entities.Extensions;

namespace ITS.Website.Controllers
{
    public class BusController : Controller
    {
        //
        // GET: /Bus/
        private readonly IBusService busService;
        public BusController(IBusService busService)
        {
            this.busService = busService;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region Route Info
        public ActionResult RouteInfo()
        {
            BusRouteInfoViewModel model = new BusRouteInfoViewModel();
            //model.Routes = busService.GetAllBusRoutes();
            model.RouteSelectList = new SelectList(BuildRouteSelectList(busService.GetAllBusRoutes()), "Value", "Text");
            return View(model);
        }
        [HttpPost]
        public ActionResult RouteInfo(FormCollection collection)
        {
            BusRouteInfoViewModel model = new BusRouteInfoViewModel();
            if (TryUpdateModel<BusRouteInfoViewModel>(model))
            {
                model.Movements1 = BuildMovementString(busService.GetMovementsOfARouteInOrder(model.SelectedRoute, true));
                model.Movements2 = BuildMovementString(busService.GetMovementsOfARouteInOrder(model.SelectedRoute, false));
                model.AllStationPostions = busService.GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(model.SelectedRoute, true);
                if (model.AllStationPostions.Count > 0)
                {
                    model.MapCenter.lat = model.AllStationPostions.First().lat;
                    model.MapCenter.lng = model.AllStationPostions.First().lng;
                }
            }
            model.RouteSelectList = new SelectList(BuildRouteSelectList(busService.GetAllBusRoutes()), "Value", "Text");
            return View(model);
        }

        private string BuildMovementString(IList<string> movements)
        {
            string result = string.Empty;
            foreach (var m in movements)
            {
                result = result + " -> " + m;
            }
            return result;
        }

        private IList<SelectListItem> BuildRouteSelectList(IList<BusRoute> routes)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var r in routes)
            {
                items.Add(new SelectListItem
                {
                    Text = r.RouteName,
                    Value = r.RouteID.ToString()
                });
            }

            return items;
        }
        #endregion

        #region Bus Station
        public ActionResult BusStationList()
        {
            BusStationListViewModel model = new BusStationListViewModel()
            {
                StationList = BuildStationModel(busService.GetAllBusStation())
            };
            return View(model);
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

        public ActionResult BusStationDetail(Guid ID)
        {
            BusStationDetailViewModel model = new BusStationDetailViewModel()
            {
                BusStation = busService.GetBusStation(ID),
                RoadName = busService.GetRoadNameFromBusStationID(ID),
                BusRoutes = busService.BusRoutesThroughAStation(ID)
            };
            return View(model);
        }
        #endregion

        #region User Position
        public ActionResult UserPositionInfo()
        {
            UserPositionInfoViewModel model = new UserPositionInfoViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult UserPositionInfo(FormCollection collection)
        {
            UserPositionInfoViewModel model = new UserPositionInfoViewModel();
            if (TryUpdateModel<UserPositionInfoViewModel>(model))
            {
                //model.DisplayInfo.RoadSession = busService.GetFirstMatchRoadSessionFromAddress(model.AddressNo, model.StreetName);
                model.DisplayInfo.Position = busService.GetPositionFromAddress(model.AddressNo, model.StreetName);
                if (model.DisplayInfo.Position != null)
                {
                    IList<BusStation> nearestStations = busService.GetNearestStations(model.DisplayInfo.Position, 4);

                    foreach (BusStation s in nearestStations)
                    {
                        model.DisplayInfo.BusStations.Add(new BusStationDetailViewModel()
                            {
                                BusStation = s,
                                RoadName = busService.GetRoadNameFromBusStationID(s.ID),
                                BusRoutes = busService.BusRoutesThroughAStation(s.ID)
                            });
                    }
                }
            }
            return View(model);
        }
        #endregion

        #region Find Bus
        public ActionResult FindBusRoute()
        {
            FindBusRouteViewModel model = new FindBusRouteViewModel();

            return View(model);
        }
        [HttpPost]
        public ActionResult FindBusRoute(FormCollection collection)
        {
            FindBusRouteViewModel model = new FindBusRouteViewModel();
            if (TryUpdateModel<FindBusRouteViewModel>(model))
            {
                Point Position_Src = busService.GetPositionFromAddress(model.AddressNo_Src, model.StreetName_Src);
                Point Position_Dst = busService.GetPositionFromAddress(model.AddressNo_Dst, model.StreetName_Dst);
                if (Position_Src != null && Position_Dst != null)
                {
                    IList<BusStation> nearestStations_Src = busService.GetNearestStations(Position_Src, 1);
                    IList<BusStation> nearestStations_Dst = busService.GetNearestStations(Position_Dst, 1);
                    IList<BusRoute> tempRouteList;
                    IList<Path2RoutesModel> tempRouteList2;
                    Path1RoutesModel tempPath1;
                    Path2RoutesModel tempPath2;
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
                                if(!model.Path_OneRoute.Contains(tempPath1))
                                    model.Path_OneRoute.Add(tempPath1);
                            }
                            model.Path_2Routes.AddRange(busService.FindPath_2Routes(s1.ID, s2.ID));
                        }
                    }
                }
            }
            return View(model);
        }
        #endregion
    }
}

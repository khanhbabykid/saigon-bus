using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Business.Abstract;
using ITS.Domain.Models.Bus;
using ITS.Domain.Entities;
using ITS.Domain.Entities.Extensions;
using ITS.Domain.Models.Bus.Admin;

namespace ITS.Admin.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusService busService;
        public BusController(IBusService busService)
        {
            this.busService = busService;
        }
        //
        // GET: /Bus/

        public ActionResult Index()
        {
            BusRouteInfoViewModel model = new BusRouteInfoViewModel();
            model.RouteSelectList = new SelectList(BuildRouteSelectList(busService.GetAllBusRoutes()), "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            BusRouteInfoViewModel model = new BusRouteInfoViewModel();
            if (TryUpdateModel<BusRouteInfoViewModel>(model))
            {
                return RedirectToAction("EditBus", new { ID = model.SelectedRoute });
            }
            model.RouteSelectList = new SelectList(BuildRouteSelectList(busService.GetAllBusRoutes()), "Value", "Text");
            return View(model);
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

        public ActionResult EditBus(Guid ID)
        {
            EditBusViewModel model = new EditBusViewModel();
            model.RouteID = ID;
            return View(model);
        }

        public ActionResult BusRouteList()
        {
            BusRouteListViewModel model = new BusRouteListViewModel();
            model.BusRouteList = BuildBusRouteListModel(busService.GetAllBusRoutes());
            return View(model);
        }

        [HttpPost]
        public ActionResult BusRouteList(FormCollection collection)
        {
            BusRouteListViewModel model = new BusRouteListViewModel();
            if (TryUpdateModel<BusRouteListViewModel>(model))
            {
                foreach (BusRouteModel r in model.BusRouteList)
                {
                    busService.SaveBusRoute(new BusRoute() { RouteID = r.RouteID, RouteName = r.RouteName });
                }
                if (!string.IsNullOrEmpty(model.NewBusRoute))
                {
                    busService.InsertBusRoute(new BusRoute() { RouteName = model.NewBusRoute });
                }
                return RedirectToAction("BusRouteList");
            }
            return View(model);
        }
        private IList<BusRouteModel> BuildBusRouteListModel(IList<BusRoute> iList)
        {
            IList<BusRouteModel> list = new List<BusRouteModel>();
            foreach (BusRoute r in iList)
            {
                list.Add(new BusRouteModel() { RouteID = r.RouteID, RouteName = r.RouteName });
            }
            return list;
        }
        #region Road
        public ActionResult RoadList()
        {
            RoadListViewModel model = new RoadListViewModel();
            model.RoadList = BuildRoadListModel(busService.GetAllRoads());
            return View(model);
        }

        [HttpPost]
        public ActionResult RoadList(FormCollection collection)
        {
            RoadListViewModel model = new RoadListViewModel();
            if (TryUpdateModel<RoadListViewModel>(model))
            {
                foreach (RoadModel r in model.RoadList)
                {
                    busService.SaveRoad(new Road() { RoadID = r.RoadID, RoadName = r.RoadName });
                }
                if (!string.IsNullOrEmpty(model.NewRoad))
                {
                    busService.InsertRoad(new Road() { RoadName = model.NewRoad });
                }
                return RedirectToAction("RoadList");
            }
            return View(model);
        }
        private IList<RoadModel> BuildRoadListModel(IList<Road> iList)
        {
            IList<RoadModel> list = new List<RoadModel>();
            foreach (Road r in iList)
            {
                list.Add(new RoadModel() { RoadID = r.RoadID, RoadName = r.RoadName });
            }
            return list;
        }
        public ActionResult DeleteRoad(Guid ID)
        {
            DeleteRoadViewModel model = new DeleteRoadViewModel() { RoadID = ID };
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteRoad(FormCollection collection)
        {
            DeleteRoadViewModel model = new DeleteRoadViewModel();
            if (TryUpdateModel<DeleteRoadViewModel>(model))
            {
                busService.DeleteRoad(model.RoadID);
                return RedirectToAction("RoadList");
            }
            return View(model);
        }
        #endregion

        #region RoadSession
        public ActionResult RoadSession(Guid ID)
        {
            RoadSessionViewModel model = BuildRoadSessionViewModel(busService.GetRoadSession(ID));
            return View(model);
        }
        [HttpPost]
        public ActionResult RoadSession(FormCollection collection)
        {
            RoadSessionViewModel model = new RoadSessionViewModel();
            if (TryUpdateModel<RoadSessionViewModel>(model))
            {
                RoadSession rs = new RoadSession()
                {
                    ID = model.ID,
                    RoadID = model.RoadID,
                    AddressLower = model.AddressLower,
                    AddressUpper = model.AddressUpper,
                    Description = model.Description,
                    PositionLower = new Point() { lat = model.PositionLower_X, lng = model.PositionLower_Y },
                    PositionUpper = new Point() { lat = model.PositionUpper_X, lng = model.PositionUpper_Y }
                };
                busService.SaveRoadSession(rs);
            }
            return View(model);
        }
        private RoadSessionViewModel BuildRoadSessionViewModel(RoadSession roadSession)
        {
            RoadSessionViewModel model = new RoadSessionViewModel()
            {
                ID = roadSession.ID,
                RoadID = roadSession.RoadID,
                AddressLower = roadSession.AddressLower,
                AddressUpper = roadSession.AddressUpper,
                Description = roadSession.Description,
                PositionLower_X = roadSession.PositionLower.lat,
                PositionLower_Y = roadSession.PositionLower.lng,
                PositionUpper_X = roadSession.PositionUpper.lat,
                PositionUpper_Y = roadSession.PositionUpper.lng
            };
            return model;
        }
        private IList<RoadSessionViewModel> BuildRoadSessionListViewModel(IList<RoadSession> iList)
        {
            IList<RoadSessionViewModel> list = new List<RoadSessionViewModel>();
            foreach (RoadSession r in iList)
            {
                list.Add(BuildRoadSessionViewModel(r));
            }
            return list;
        }
        public ActionResult RoadSessionList(Guid ID)
        {
            RoadSessionListViewModel model = new RoadSessionListViewModel();
            model.RoadSessionList = BuildRoadSessionListViewModel(busService.GetAllRoadSessionOfARoad(ID));
            return View(model);
        }

        [HttpPost]
        public ActionResult RoadSessionList(FormCollection collection)
        {
            RoadSessionListViewModel model = new RoadSessionListViewModel();
            if (TryUpdateModel<RoadSessionListViewModel>(model))
            {
                foreach (RoadSessionViewModel r in model.RoadSessionList)
                {
                    RoadSession rs = new RoadSession()
                    {
                        ID = r.ID,
                        RoadID = r.RoadID,
                        AddressLower = r.AddressLower,
                        AddressUpper = r.AddressUpper,
                        Description = r.Description,
                        PositionLower = new Point() { lng = r.PositionLower_X, lat = r.PositionLower_Y },
                        PositionUpper = new Point() { lng = r.PositionUpper_X, lat = r.PositionUpper_Y }
                    };
                    busService.SaveRoadSession(rs);
                }
                return RedirectToAction("RoadSessionList");
            }
            return View(model);
        }
        #endregion
        public ActionResult IntermediatePoints(Guid ID)
        {
            AddIntermediatePointViewModel model = new AddIntermediatePointViewModel()
            {
                MovementID = ID,
                PointList = busService.GetIntermediatePoints_2(ID)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult IntermediatePoints(FormCollection collection)
        {
            AddIntermediatePointViewModel model = new AddIntermediatePointViewModel();
            if (TryUpdateModel<AddIntermediatePointViewModel>(model))
            {
                if (model.NewPointLat != null && model.NewPointLng != null)
                {
                    busService.InsertIntermediatePoint(model.MovementID, (double)model.NewPointLat, (double)model.NewPointLng, model.NewPointOrder);
                }
                foreach (IntermediatePoint p in model.PointList)
                {
                    busService.SaveIntermediatePoint(p);
                }
            }
            return RedirectToAction("IntermediatePoints");
        }

        

        public ActionResult BusMovements(Guid ID)
        {
            BusMovementsViewModel model = new BusMovementsViewModel();
            model.BusRoute = busService.GetBusRoute(ID);
            model.Movements = BuildBusMovementsModel(busService.GetAllBusMovements(ID, true));
            return View(model);
        }
        [HttpPost]
        public ActionResult BusMovements(FormCollection collection)
        {
            BusMovementsViewModel model = new BusMovementsViewModel();
            if (TryUpdateModel<BusMovementsViewModel>(model))
            {
                BusMovement movement;
                foreach (BusMovementModel m in model.Movements)
                {
                    movement = busService.GetBusMovement(m.ID);
                    movement.OrderNumber = m.OrderNumber;
                    busService.SaveBusMovement(movement);
                }
                return RedirectToAction("BusMovements");
            }
            return View(model);
        }
        private IList<BusMovementModel> BuildBusMovementsModel(IList<BusMovement> iList)
        {
            IList<BusMovementModel> list = new List<BusMovementModel>();
            foreach (BusMovement m in iList)
            {
                list.Add(new BusMovementModel()
                {
                    ID = m.ID,
                    BusStationFrom = m.BusStationFrom,
                    BusStationTo = m.BusStationTo,
                    Direction = m.Direction,
                    OrderNumber = m.OrderNumber,
                    BusStationNameFrom = busService.GetBusStation(m.BusStationFrom).StationName,
                    BusStationNameTo = busService.GetBusStation(m.BusStationTo).StationName
                });
            }

            return list;
        }

        public ActionResult AddBusMovement(Guid ID)
        {
            AddBusMovementViewModel model = new AddBusMovementViewModel();
            model.RouteID = ID;
            model.StationFromSelectList = new SelectList(BuildStationSelectList(), "Value", "Text");
            model.StationToSelectList = new SelectList(BuildStationSelectList(), "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddBusMovement(FormCollection collection)
        {
            AddBusMovementViewModel model = new AddBusMovementViewModel();
            if (TryUpdateModel<AddBusMovementViewModel>(model))
            {
                BusMovement movement = new BusMovement()
                {
                    ID = Guid.NewGuid(),
                    RouteID = model.RouteID,
                    BusStationFrom = model.SelectedStationFrom,
                    BusStationTo = model.SelectedStationTo,
                    OrderNumber = model.OrderNumber,
                    Direction = true
                };
                busService.InsertBusMovement(movement);
                return RedirectToAction("BusMovements", new { ID = model.RouteID });
            }
            return View(model);
        }

        private List<SelectListItem> BuildStationSelectList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            IList<BusStation> stations = busService.GetAllBusStation();
            foreach (var stt in stations)
            {
                items.Add(new SelectListItem
                {
                    Text = stt.StationName,
                    Value = stt.ID.ToString()
                });
            }

            return items;
        }

        public ActionResult DeleteBusMovement(Guid ID)
        {
            BusMovement movement = busService.GetBusMovement(ID);
            DeleteBusMovementViewModel model = new DeleteBusMovementViewModel()
            {
                BusMovementID = ID,
                RouteID = movement.RouteID
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteBusMovement(FormCollection collection)
        {
            DeleteBusMovementViewModel model = new DeleteBusMovementViewModel();
            if (TryUpdateModel<DeleteBusMovementViewModel>(model))
            {
                busService.DeleteBusMovement(model.BusMovementID);
                return RedirectToAction("BusMovements", new { ID = model.RouteID });
            }
            return View(model);

        }


        public ActionResult BusStationList()
        {
            BusStationListViewModel model = new BusStationListViewModel();
            model.StationList = BuildStationModel(busService.GetAllBusStation());
            return View(model);
        }
        private IList<BusStationModel> BuildStationModel(IList<BusStation> stations)
        {
            IList<BusStationModel> list = new List<BusStationModel>();
            foreach (BusStation s in stations)
            {
                list.Add(new BusStationModel()
                {
                    BusStation = s,
                    StreetName = busService.GetRoadNameFromBusStationID(s.ID)
                });
            }
            return list;
        }

        public ActionResult DeleteBusRoute(Guid ID)
        {
            DeleteBusRouteViewModel model = new DeleteBusRouteViewModel() { RouteID = ID };
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteBusRoute(FormCollection collection)
        {
            DeleteBusRouteViewModel model = new DeleteBusRouteViewModel();
            if (TryUpdateModel<DeleteBusRouteViewModel>(model))
            {
                busService.DeleteBusRoute(model.RouteID);
                return RedirectToAction("BusRouteList");
            }
            return View(model);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Business.Abstract;
using ITS.Domain.Entities;
using ITS.Domain.Models.Bus;

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
    }
}

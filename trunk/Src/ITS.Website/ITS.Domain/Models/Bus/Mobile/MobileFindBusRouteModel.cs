using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Models.Bus.Website;

namespace ITS.Domain.Models.Bus.Mobile
{
    public class MobileFindBusRouteModel
    {
        //public string debug;
        public List<SimplePath1RoutesModel> Path_OneRoute
        {
            get
            {
                return _path_OneRoute ?? (_path_OneRoute = new List<SimplePath1RoutesModel>());
            }
            set { _path_OneRoute = value; }
        }
        public List<SimplePath2RoutesModel> Path_2Routes
        {
            get
            {
                return _path_2Routes ?? (_path_2Routes = new List<SimplePath2RoutesModel>());
            }
            set { _path_2Routes = value; }
        }

        //private
        private List<SimplePath1RoutesModel> _path_OneRoute;
        private List<SimplePath2RoutesModel> _path_2Routes;

    }

    public class SimplePath1RoutesModel
    {

        public string Station_Src { get; set; }
        public string BusRoute { get; set; }
        public string Station_Dst { get; set; }
    }
    public class SimplePath2RoutesModel
    {

        public string Station_Src { get; set; }
        public string BusRoute1 { get; set; }
        public string IntermediateStation { get; set; }
        public string BusRoute2 { get; set; }
        public string Station_Dst { get; set; }
    }
}

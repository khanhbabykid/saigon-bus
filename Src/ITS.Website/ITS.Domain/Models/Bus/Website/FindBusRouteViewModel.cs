using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ITS.Domain.Models.Bus.Website
{
    public class FindBusRouteViewModel
    {
        [Required(AllowEmptyStrings=false, ErrorMessage="Nhập số nhà nơi đi")]
        public int AddressNo_Src { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhập tên đường nơi đi")]
        public string StreetName_Src { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhập số nhà nơi đến")]
        public int AddressNo_Dst { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhập tên đường nơi đến")]
        public string StreetName_Dst { get; set; }

        //result
        public List<Path1RoutesModel> Path_OneRoute
        {
            get
            {
                return _path_OneRoute ?? (_path_OneRoute = new List<Path1RoutesModel>());
            }
            set { _path_OneRoute = value; }
        }
        public List<Path2RoutesModel> Path_2Routes
        {
            get
            {
                return _path_2Routes ?? (_path_2Routes = new List<Path2RoutesModel>());
            }
            set { _path_2Routes = value; }
        }

        //private
        private List<Path1RoutesModel> _path_OneRoute;
        private List<Path2RoutesModel> _path_2Routes;
    }
}

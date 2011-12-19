using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using ITS.Domain.Entities.Extensions;

namespace ITS.Domain.Models.Bus.Website
{
    public class UserPositionInfoViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bạn phải nhập số nhà")]
        public int AddressNo { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bạn phải nhập tên đường")]
        public string StreetName { get; set; }

        public UserPositionInfoDisplayModel DisplayInfo
        {
            get
            {
                return _displayInfo ?? (_displayInfo = new UserPositionInfoDisplayModel());
            }
            set { _displayInfo = value; }
        }
        //private

        private UserPositionInfoDisplayModel _displayInfo;
    }

    public class UserPositionInfoDisplayModel
    {
        public RoadSession RoadSession { get; set; }
        public Point Position { get; set; }
        public IList<BusStationDetailViewModel> BusStations
        {
            get
            {
                return _busStationDetail ?? (_busStationDetail = new List<BusStationDetailViewModel>());
            }
            set { _busStationDetail = value; }
        }

        //private
        private IList<BusStationDetailViewModel> _busStationDetail;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities.Extensions;

namespace ITS.Domain.Models.Bus.Mobile
{
    public class RouteInfoModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Movements1
        {
            get
            {
                return _movements1 ?? (_movements1 = string.Empty);
            }
            set { _movements1 = value; }
        }
        public string Movements2
        {
            get
            {
                return _movements2 ?? (_movements2 = string.Empty);
            }
            set { _movements2 = value; }
        }

        public IList<Point> AllStationPostions
        {
            get
            {
                return _allStationPostions ?? (_allStationPostions = new List<Point>());
            }
            set { _allStationPostions = value; }
        }
        //public Point MapCenter
        //{
        //    get
        //    {
        //        return _mapCenter ?? (_mapCenter = new Point() { lat = 106.657875f, lng = 10.771481f });
        //    }
        //    set { _mapCenter = value; }
        //}
        //private
        private string _movements1;
        private string _movements2;
        private IList<Point> _allStationPostions;
        //private Point _mapCenter;
    }
}

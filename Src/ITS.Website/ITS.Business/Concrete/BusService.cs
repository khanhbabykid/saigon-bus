using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Business.Abstract;
using ITS.DAL.Repository;
using ITS.Domain.Entities;
using ITS.Domain.Entities.Extensions;

namespace ITS.Business.Concrete
{
    public class BusService: IBusService
    {
        private readonly IBusRepository busRepository;
        public BusService(IBusRepository busRepository)
        {
            this.busRepository = busRepository;
        }

        public void TestService()
        {
            busRepository.TestRepository();
        }


        public IList<BusRoute> GetAllBusRoutes()
        {
            return busRepository.GetAllBusRoutes();
        }

        public IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction)
        {
            return busRepository.GetMovementsOfARouteInOrder(RouteID, Direction);
        }

        public IList<Point> GetAllStationPositionsOfARouteInOrder(Guid RouteID, Boolean Direction)
        {
            return busRepository.GetAllStationPositionsOfARouteInOrder(RouteID, Direction);
        }
    }
}

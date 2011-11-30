using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using ITS.Domain.Entities.Extensions;

namespace ITS.Business.Abstract
{
    public interface IBusService
    {
        void TestService();

        IList<BusRoute> GetAllBusRoutes();
        IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<Point> GetAllStationPositionsOfARouteInOrder(Guid RouteID, Boolean Direction);
    }
}

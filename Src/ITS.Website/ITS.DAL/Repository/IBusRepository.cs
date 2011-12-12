using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using ITS.Domain.Entities.Extensions;

namespace ITS.DAL.Repository
{
    public interface IBusRepository
    {
        void TestRepository();
        RoadSession GetRoadSession(Guid ID);
        IList<BusRoute> GetAllBusRoutes();
        IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<Point> GetAllStationPositionsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<Point> GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(Guid RouteID, Boolean Direction);
        IList<BusMovement> GetAllBusMovements(Guid RouteID, bool Direction);

        IList<Point> GetIntermediatePoints(Guid MovementID);
        IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID);
        void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order);

        void SaveIntermediatePoint(IntermediatePoint p);
        void SaveRoadSession(RoadSession r);
    }
}

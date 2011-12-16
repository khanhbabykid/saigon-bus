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
        RoadSession GetRoadSession(Guid ID);
        BusStation GetBusStation(Guid ID);
        BusRoute GetBusRoute(Guid RouteID);
        IList<BusRoute> GetAllBusRoutes();
        IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<Point> GetAllStationPositionsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<Point> GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(Guid RouteID, Boolean Direction);
        string GetRoadNameFromBusStationID(Guid BusStationID);

        IList<Point> GetIntermediatePoints(Guid MovementID);
        IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID);
        void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order);

        void SaveIntermediatePoint(IntermediatePoint p);
        void SaveRoadSession(RoadSession r);

        #region Bus Station
        IList<BusStation> GetAllBusStation();
        #endregion

        #region Bus Movements
        IList<BusMovement> GetAllBusMovements(Guid RouteID, bool Direction);
        void SaveBusMovement(BusMovement movement);
        BusMovement GetBusMovement(Guid movementID);
        void InsertBusMovement(BusMovement movement);
        void DeleteBusMovement(Guid MovementId);
        #endregion

        void SaveBusRoute(BusRoute busRoute);
        void InsertBusRoute(BusRoute busRoute);
        void DeleteBusRoute(Guid routeID);

        IList<Road> GetAllRoads();
        void SaveRoad(Road road);
        void InsertRoad(Road road);
        void DeleteRoad(Guid roadID);

        IList<RoadSession> GetAllRoadSessionOfARoad(Guid roadID);
    }
}

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

        #region Intermediate Point
        IList<Point> GetIntermediatePoints(Guid MovementID);
        IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID);
        IntermediatePoint IntermediatePoint(Guid pointID);
        void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order);
        void SaveIntermediatePoint(IntermediatePoint p);
        void DeleteIntermediatePoint(Guid pointID);
        #endregion
        void SaveRoadSession(RoadSession r);

        #region Bus Movement
        void SaveBusMovement(BusMovement movement);
        BusMovement GetBusMovement(Guid movementID);
        void InsertBusMovement(BusMovement movement);
        void DeleteBusMovement(Guid MovementId);
        IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<BusMovement> GetAllBusMovements(Guid RouteID, bool Direction);
        #endregion

        #region Route
        void SaveBusRoute(BusRoute busRoute);
        void InsertBusRoute(BusRoute busRoute);
        void DeleteBusRoute(Guid routeID);
        BusRoute GetBusRoute(Guid ID);
        IList<BusRoute> GetAllBusRoutes();
        IList<BusRoute> BusRoutesThroughAStation(Guid stationID);
        #endregion

        #region Road
        IList<Road> GetAllRoads();
        void SaveRoad(Road road);
        void InsertRoad(Road road);
        void DeleteRoad(Guid roadID);
        string GetRoadNameFromBusStationID(Guid BusStationID);
        #endregion
        #region Road Session
        IList<RoadSession> GetAllRoadSessionOfARoad(Guid roadID);
        void InsertRoadSession(RoadSession roadSession);
        void DeleteRoadSession(Guid roadSessionID);
        RoadSession GetRoadSession(Guid ID);
        #endregion
        #region Bus Station
        IList<BusStation> GetAllBusStation();
        void SaveBusStation(BusStation busStation);
        BusStation GetBusStation(Guid ID);
        void InsertBusStation(BusStation busStation);
        void DeleteBusStation(Guid stationID);
        IList<Point> GetAllStationPositionsOfARouteInOrder(Guid RouteID, Boolean Direction);
        IList<Point> GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(Guid RouteID, Boolean Direction);
        #endregion
    }
}

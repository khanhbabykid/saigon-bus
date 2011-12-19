using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.Domain.Entities;
using ITS.Domain.Entities.Extensions;
using ITS.Domain.Models.Bus.Website;

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

        #region Intermediate Point
        IList<Point> GetIntermediatePoints(Guid MovementID);
        IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID);
        IntermediatePoint IntermediatePoint(Guid pointID);
        void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order);
        void SaveIntermediatePoint(IntermediatePoint p);
        void DeleteIntermediatePoint(Guid pointID);
        #endregion
        void SaveRoadSession(RoadSession r);

        #region Bus Station
        IList<BusStation> GetAllBusStation();
        void SaveBusStation(BusStation busStation);
        #endregion

        #region Bus Movements
        IList<BusMovement> GetAllBusMovements(Guid RouteID, bool Direction);
        void SaveBusMovement(BusMovement movement);
        BusMovement GetBusMovement(Guid movementID);
        void InsertBusMovement(BusMovement movement);
        void DeleteBusMovement(Guid MovementId);
        #endregion

        #region Route
        IList<BusRoute> BusRoutesThroughAStation(Guid stationID);
        void SaveBusRoute(BusRoute busRoute);
        void InsertBusRoute(BusRoute busRoute);
        void DeleteBusRoute(Guid routeID);
        #endregion
        IList<Road> GetAllRoads();
        void SaveRoad(Road road);
        void InsertRoad(Road road);
        void DeleteRoad(Guid roadID);

        #region Road Session
        IList<RoadSession> GetAllRoadSessionOfARoad(Guid roadID);
        RoadSession GetFirstMatchRoadSessionFromAddress(int AddressNumber, string StreetName);
        void InsertRoadSession(RoadSession roadSession);
        void DeleteRoadSession(Guid roadSessionID);
        #endregion

        #region Bus Station
        void InsertBusStation(BusStation busStation);
        void DeleteBusStation(Guid stationID);
        IList<BusStation> GetNearestStations(Point p, int count);
        #endregion

        #region Helper
        Point GetPositionFromAddress(int AddressNumber, string StreetName);
        #endregion
        #region Find Bus
        IList<BusRoute> FindPath_OneRoute(Guid StationID_Src, Guid StationID_Dst);
        IList<Path2RoutesModel> FindPath_2Routes(Guid StationID_Src, Guid StationID_Dst);
        #endregion
    }
}

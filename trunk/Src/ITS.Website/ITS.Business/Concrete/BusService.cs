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
    public class BusService : IBusService
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

        public RoadSession GetRoadSession(Guid ID)
        {
            return busRepository.GetRoadSession(ID);
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
        public IList<Point> GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(Guid RouteID, Boolean Direction)
        {
            return busRepository.GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(RouteID, Direction);
        }

        #region Intermediate Point
        public IList<Point> GetIntermediatePoints(Guid MovementID)
        {
            return busRepository.GetIntermediatePoints(MovementID);
        }
        public IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID)
        {
            return busRepository.GetIntermediatePoints_2(MovementID);
        }
        public IntermediatePoint IntermediatePoint(Guid pointID)
        {
            return busRepository.IntermediatePoint(pointID);
        }
        public void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order)
        {
            busRepository.InsertIntermediatePoint(MovementID, lat, lng, order);
        }
        public void SaveIntermediatePoint(IntermediatePoint p)
        {
            busRepository.SaveIntermediatePoint(p);
        }
        public void DeleteIntermediatePoint(Guid pointID)
        {
            busRepository.DeleteIntermediatePoint(pointID);
        }
        #endregion
        public void SaveRoadSession(RoadSession r)
        {
            busRepository.SaveRoadSession(r);
        }


        public IList<BusMovement> GetAllBusMovements(Guid RouteID, bool Direction)
        {
            return busRepository.GetAllBusMovements(RouteID, Direction);
        }


        public BusStation GetBusStation(Guid StationID)
        {
            return busRepository.GetBusStation(StationID);
        }

        #region Route
        public BusRoute GetBusRoute(Guid RouteID)
        {
            return busRepository.GetBusRoute(RouteID);
        }
        public IList<BusRoute> BusRoutesThroughAStation(Guid stationID)
        {
            return busRepository.BusRoutesThroughAStation(stationID);
        }
        #endregion

        public void SaveBusMovement(BusMovement movement)
        {
            busRepository.SaveBusMovement(movement);
        }


        public BusMovement GetBusMovement(Guid movementID)
        {
            return busRepository.GetBusMovement(movementID);
        }


        public IList<BusStation> GetAllBusStation()
        {
            return busRepository.GetAllBusStation();
        }

        public void InsertBusMovement(BusMovement movement)
        {
            busRepository.InsertBusMovement(movement);
        }
        public void DeleteBusMovement(Guid MovementId)
        {
            busRepository.DeleteBusMovement(MovementId);
        }

        public string GetRoadNameFromBusStationID(Guid BusStationID)
        {
            return busRepository.GetRoadNameFromBusStationID(BusStationID);
        }


        public void SaveBusRoute(BusRoute busRoute)
        {
            busRepository.SaveBusRoute(busRoute);
        }

        public void InsertBusRoute(BusRoute busRoute)
        {
            busRepository.InsertBusRoute(busRoute);
        }
        public void DeleteBusRoute(Guid routeID)
        {
            busRepository.DeleteBusRoute(routeID);
        }


        public IList<Road> GetAllRoads()
        {
            return busRepository.GetAllRoads();
        }


        public void SaveRoad(Road road)
        {
            busRepository.SaveRoad(road);
        }

        public void InsertRoad(Road road)
        {
            busRepository.InsertRoad(road);
        }

        public void DeleteRoad(Guid roadID)
        {
            busRepository.DeleteRoad(roadID);
        }

        #region Road Session
        public IList<RoadSession> GetAllRoadSessionOfARoad(Guid roadID)
        {
            return busRepository.GetAllRoadSessionOfARoad(roadID);
        }

        public void InsertRoadSession(RoadSession roadSession)
        {
            busRepository.InsertRoadSession(roadSession);
        }

        public void DeleteRoadSession(Guid roadSessionID)
        {
            busRepository.DeleteRoadSession(roadSessionID);
        }
        #endregion
        #region Bus Station
        public void InsertBusStation(BusStation busStation)
        {
            busRepository.InsertBusStation(busStation);
        }
        public void SaveBusStation(BusStation busStation)
        {
            busRepository.SaveBusStation(busStation);
        }
        public void DeleteBusStation(Guid stationID)
        {
            busRepository.DeleteBusStation(stationID);
        }
        #endregion
    }
}

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

        public IList<Point> GetIntermediatePoints(Guid MovementID)
        {
            return busRepository.GetIntermediatePoints(MovementID);
        }
        public IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID)
        {
            return busRepository.GetIntermediatePoints_2(MovementID);
        }
        public void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order)
        {
            busRepository.InsertIntermediatePoint(MovementID, lat, lng, order);
        }


        public void SaveIntermediatePoint(IntermediatePoint p)
        {
            busRepository.SaveIntermediatePoint(p);
        }


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


        public BusRoute GetBusRoute(Guid RouteID)
        {
            return busRepository.GetBusRoute(RouteID);
        }


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
    }
}

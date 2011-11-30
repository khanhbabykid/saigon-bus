using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITS.DAL.Repository;
using ITS.Domain.Entities;
using Npgsql;
using ITS.Domain.Entities.Extensions;

namespace ITS.DAL.Implementation.Repository
{
    public class BusRepository : IBusRepository
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=vanducsonha;Database=SaigonBus;");

        private void OpenConnection()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        public void TestRepository()
        {
            throw new NotImplementedException();
        }


        public IList<BusRoute> GetAllBusRoutes()
        {
            IList<BusRoute> list = new List<BusRoute>();
            BusRoute route = new BusRoute();
            OpenConnection();
            string sqlcmd = string.Format("select * from BusRoute");
            NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn);


            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                route = new BusRoute()
                {
                    RouteID = Guid.Parse(dr["RouteID"].ToString()),
                    RouteName = dr["RouteName"].ToString()
                };
                list.Add(route);
            }
            return list;

        }

        public IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction)
        {
            IList<string> list = new List<string>();
            IList<Guid> Stations = new List<Guid>();
            OpenConnection();
            string sqlcmd = string.Format("select BusStationFrom from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RouteID", RouteID);
                command.Parameters.Add("Direction", Direction);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Stations.Add(Guid.Parse(dr["BusStationFrom"].ToString()));
                    }
                }
            }
            foreach (var station in Stations)
            {
                list.Add(GetRoadNameFromBusStationID(station));
            }
            return list;
        }

        public string GetRoadNameFromRoadSession(RoadSession RoadSession)
        {
            OpenConnection();
            string sqlcmd = string.Format("select RoadName from Road where RoadID = :RoadID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RoadID", RoadSession.RoadID);
                return command.ExecuteScalar().ToString();
            }
        }

        public string GetRoadNameFromBusStationID(Guid BusStationID)
        {
            OpenConnection();
            string sqlcmd = string.Format("select R.RoadName from BusStation S, RoadSession RS, Road R where S.RoadSessionID = RS.ID and RS.RoadId =R.RoadId and S.ID = :BusStationID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("BusStationID", BusStationID);
                return command.ExecuteScalar().ToString();
            }
        }

        public IList<Point> GetAllStationPositionsOfARouteInOrder(Guid RouteID, Boolean Direction)
        {
            IList<Point> list = new List<Point>();
            IList<Guid> Stations = new List<Guid>();
            OpenConnection();
            string sqlcmd = string.Format("select BusStationFrom from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RouteID", RouteID);
                command.Parameters.Add("Direction", Direction);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Stations.Add(Guid.Parse(dr["BusStationFrom"].ToString()));
                    }
                }
            }
            foreach (var station in Stations)
            {
                list.Add(GetPositionFromBusStationID(station));
            }
            return list;
        }

        public Point GetPositionFromBusStationID(Guid BusStationID)
        {
            OpenConnection();
            Point point = new Point();
            string sqlcmd = "select ST_X(S.Position::geometry) as X, ST_Y(S.Position::geometry) as Y from BusStation S where S.ID = :BusStationID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("BusStationID", BusStationID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        point.lat = float.Parse(dr["X"].ToString());
                        point.lng = float.Parse(dr["Y"].ToString());
                    }
                }
            }
            return point;
        }
    }
}

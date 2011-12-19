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
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=vanducsonha;Database=SaigonBus; Preload Reader = true");

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


        #region Helpers
        public Point GetPositionFromAddress(int AddressNumber, string StreetName)
        {
            Point p = new Point();
            IList<RoadSession> ss = GetRoadSessionFromAddress(AddressNumber, StreetName);
            if (ss.Count > 0)
            {
                RoadSession selected = ss.First<RoadSession>();
                float fraction = (float)(AddressNumber - selected.AddressLower) / (selected.AddressUpper - selected.AddressLower);
                p = InterpolatePoint(selected.PositionLower, selected.PositionUpper, fraction);
                return p;
            }
            else return null;
        }
        private Point InterpolatePoint(Point point, Point point_2, float fraction)
        {
            OpenConnection();
            Point p = new Point();
            string sqlcmd = String.Format("SELECT ST_X(ST_Line_Interpolate_Point(the_line, :fraction)::geometry) as X, ST_Y(ST_Line_Interpolate_Point(the_line, :fraction)::geometry) as Y FROM (SELECT ST_GeomFromEWKT('LINESTRING({0} {1}, {2} {3})') as the_line) As foo", point.lat, point.lng, point_2.lat, point_2.lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("fraction", fraction);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        p.lat = float.Parse(dr["X"].ToString());
                        p.lng = float.Parse(dr["Y"].ToString());
                    }
                }
            }
            return p;
        }
        #endregion


        #region Intermediate Point
        public IList<Point> GetIntermediatePoints(Guid MovementID)
        {
            IList<Point> list = new List<Point>();
            OpenConnection();
            string sqlcmd = string.Format("select ST_X(P.Position::geometry) as X, ST_Y(P.Position::geometry) as Y from IntermediatePoint P where BusMovementID = :MovementID order by OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("MovementID", MovementID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Point()
                        {
                            lat = float.Parse(dr["X"].ToString()),
                            lng = float.Parse(dr["Y"].ToString())
                        });
                    }
                }
            }
            return list;
        }
        public IList<IntermediatePoint> GetIntermediatePoints_2(Guid MovementID)
        {
            IList<IntermediatePoint> list = new List<IntermediatePoint>();
            OpenConnection();
            string sqlcmd = string.Format("select ID, ST_X(P.Position::geometry) as X, ST_Y(P.Position::geometry) as Y, OrderNumber from IntermediatePoint P where BusMovementID = :MovementID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("MovementID", MovementID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new IntermediatePoint()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            lat = float.Parse(dr["Y"].ToString()),
                            lng = float.Parse(dr["X"].ToString()),
                            OrderNumber = int.Parse(dr["OrderNumber"].ToString())
                        });
                    }
                }
            }
            return list;
        }
        public IntermediatePoint IntermediatePoint(Guid pointID)
        {
            OpenConnection();
            IntermediatePoint r = new IntermediatePoint();
            string sqlcmd = "select ID, BusMovementID, ST_X(P.Position::geometry) as X, ST_Y(P.Position::geometry) as Y, OrderNumber from IntermediatePoint P where ID = :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", pointID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r.ID = Guid.Parse(dr["ID"].ToString());
                        r.lat = float.Parse(dr["Y"].ToString());
                        r.lng = float.Parse(dr["X"].ToString());
                        r.BusMovementID = Guid.Parse(dr["BusMovementID"].ToString());
                        r.OrderNumber = int.Parse(dr["OrderNumber"].ToString());
                    }
                }
            }
            return r;
        }
        public void SaveIntermediatePoint(IntermediatePoint p)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update IntermediatePoint set Position = ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), OrderNumber = :OrderNumber where ID= :ID", p.lng, p.lat);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", p.ID);
                //command.Parameters.AddWithValue("lat", lat);
                //command.Parameters.AddWithValue("lng", lng);
                command.Parameters.AddWithValue("OrderNumber", p.OrderNumber);
                command.ExecuteNonQuery();
            }
        }
        public void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order)
        {
            OpenConnection();
            string sqlcmd = string.Format("insert into IntermediatePoint values(:newID, :MovementID, ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), :OrderNumber)", lng, lat);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("newID", Guid.NewGuid());
                command.Parameters.AddWithValue("MovementID", MovementID);
                //command.Parameters.AddWithValue("lat", lat);
                //command.Parameters.AddWithValue("lng", lng);
                command.Parameters.AddWithValue("OrderNumber", order);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteIntermediatePoint(Guid pointID)
        {
            OpenConnection();
            string sqlcmd = "delete from IntermediatePoint where ID= :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", pointID);
                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region Bus Station

        public IList<BusStation> GetAllBusStation()
        {
            IList<BusStation> list = new List<BusStation>();
            BusStation station = new BusStation();
            OpenConnection();
            string sqlcmd = string.Format("select ID, RoadSessionID, StationName, ST_X(Position::geometry) as Position_X, ST_Y(Position::geometry) as Position_Y from BusStation");
            NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn);


            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                station = new BusStation()
                {
                    ID = Guid.Parse(dr["ID"].ToString()),
                    RoadSessionID = Guid.Parse(dr["RoadSessionID"].ToString()),
                    StationName = dr["StationName"].ToString(),
                    Position_X = float.Parse(dr["Position_X"].ToString()),
                    Position_Y = float.Parse(dr["Position_Y"].ToString())
                };
                list.Add(station);
            }
            return list;

        }
        public BusStation GetBusStation(Guid ID)
        {
            OpenConnection();
            BusStation r = new BusStation();
            string sqlcmd = "Select ID, RoadSessionID, StationName,  ST_X(S.Position::geometry) as Position_X, ST_Y(S.Position::geometry) as Position_Y from BusStation S where ID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", ID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r.ID = Guid.Parse(dr["ID"].ToString());
                        r.RoadSessionID = Guid.Parse(dr["RoadSessionID"].ToString());
                        r.Position_X = float.Parse(dr["Position_X"].ToString());
                        r.Position_Y = float.Parse(dr["Position_Y"].ToString());
                        r.StationName = dr["StationName"].ToString();
                    }
                }
            }
            return r;
        }
        public void SaveBusStation(BusStation busStation)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update BusStation set roadsessionid = :roadsessionid, stationname=:stationname, position = ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\') where id = :id", busStation.Position_X, busStation.Position_Y);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("id", busStation.ID);
                command.Parameters.AddWithValue("roadsessionid", busStation.RoadSessionID);
                command.Parameters.AddWithValue("stationname", busStation.StationName);
                command.ExecuteNonQuery();
            }
        }
        public void InsertBusStation(BusStation busStation)
        {
            OpenConnection();
            string sqlcmd = string.Format("insert into BusStation(id, roadsessionid, stationname, position) values(:id, :roadsessionid, :stationname, ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'))", busStation.Position_X, busStation.Position_Y);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("id", Guid.NewGuid());
                command.Parameters.AddWithValue("roadsessionid", busStation.RoadSessionID);
                command.Parameters.AddWithValue("stationname", busStation.StationName);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteBusStation(Guid stationID)
        {
            OpenConnection();
            string sqlcmd = "delete from BusStation where ID= :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", stationID);
                command.ExecuteNonQuery();
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
                command.Parameters.AddWithValue("RouteID", RouteID);
                command.Parameters.AddWithValue("Direction", Direction);
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
        public IList<Point> GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(Guid RouteID, Boolean Direction)
        {
            IList<Point> list = new List<Point>();
            IList<Guid> Stations = new List<Guid>();
            OpenConnection();
            string sqlcmd = string.Format("select BusStationFrom, BusStationTo, ID from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RouteID", RouteID);
                command.Parameters.AddWithValue("Direction", Direction);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        list.Add(GetPositionFromBusStationID(Guid.Parse(dr["BusStationFrom"].ToString())));
                        //Add diem giua
                        foreach (Point p in GetIntermediatePoints(Guid.Parse(dr["ID"].ToString())))
                        {
                            list.Add(p);
                        }
                        list.Add(GetPositionFromBusStationID(Guid.Parse(dr["BusStationTo"].ToString())));
                    }

                    while (dr.Read())
                    {
                        //Add diem giua
                        foreach (Point p in GetIntermediatePoints(Guid.Parse(dr["ID"].ToString())))
                        {
                            list.Add(p);
                        }
                        list.Add(GetPositionFromBusStationID(Guid.Parse(dr["BusStationTo"].ToString())));
                    }
                }
            }
            foreach (var station in Stations)
            {
                list.Add(GetPositionFromBusStationID(station));
            }
            return list;
        }
        public IList<BusStation> GetNeighborStations(Point p, int distance)
        {
            IList<BusStation> list = new List<BusStation>();
            OpenConnection();
            string sqlcmd = string.Format("SELECT ID,RoadSessionID,StationName,ST_X(Position::geometry) as X, ST_Y(Position::geometry) as Y  FROM BusStation WHERE ST_DWithin(geocolumn, 'POINT({0} {1})', :distance);", p.lat, p.lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("distance", distance);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new BusStation()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            RoadSessionID = Guid.Parse(dr["RoadSessionID"].ToString()),
                            StationName = dr["StationName"].ToString(),
                            Position_X = float.Parse(dr["X"].ToString()),
                            Position_Y = float.Parse(dr["Y"].ToString())
                        });
                    }
                }
            }
            return list;
        }
        public IList<BusStation> GetNearestStations(Point p, int count)
        {
            IList<BusStation> list = new List<BusStation>();
            OpenConnection();
            string sqlcmd = string.Format("SELECT ID,RoadSessionID,StationName,ST_X(Position::geometry) as X, ST_Y(Position::geometry) as Y  FROM BusStation order by ST_Distance(Position, 'POINT({0} {1})') limit :count", p.lng, p.lat);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("count", count);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new BusStation()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            RoadSessionID = Guid.Parse(dr["RoadSessionID"].ToString()),
                            StationName = dr["StationName"].ToString(),
                            Position_X = float.Parse(dr["X"].ToString()),
                            Position_Y = float.Parse(dr["Y"].ToString())
                        });
                    }
                }
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
                command.Parameters.AddWithValue("BusStationID", BusStationID);
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
        public IList<BusStation> BusStationsOfARoute(Guid routeID)
        {
            IList<BusStation> list = new List<BusStation>();
            OpenConnection();
            string sqlcmd = "select distinct S.ID, S.RoadSessionID, S.StationName,  ST_X(S.Position::geometry) as X, ST_Y(S.Position::geometry) as Y from BusStation S, BusMovement M where (S.ID = M.BusStationFrom or S.ID = M.BusStationTo) and M.RouteID = :RouteID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RouteID", routeID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new BusStation()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            RoadSessionID = Guid.Parse(dr["ID"].ToString()),
                            StationName = dr["StationName"].ToString(),
                            Position_X = float.Parse(dr["X"].ToString()),
                            Position_Y = float.Parse(dr["Y"].ToString())
                        });
                    }
                }
            }
            return list;
        }
        #endregion

        #region Bus Movement
        public IList<BusMovement> GetAllBusMovements(Guid RouteID, bool bDirection)
        {
            IList<BusMovement> list = new List<BusMovement>();
            OpenConnection();
            string sqlcmd = string.Format("select * from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RouteID", RouteID);
                command.Parameters.AddWithValue("Direction", bDirection);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new BusMovement()
                        {
                            BusStationFrom = Guid.Parse(dr["BusStationFrom"].ToString()),
                            BusStationTo = Guid.Parse(dr["BusStationTo"].ToString()),
                            Direction = bDirection,
                            ID = Guid.Parse(dr["ID"].ToString()),
                            OrderNumber = int.Parse(dr["OrderNumber"].ToString())
                        });
                    }
                }
            }
            return list;
        }
        public void SaveBusMovement(BusMovement movement)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update BusMovement set OrderNumber=:OrderNumber where ID= :ID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", movement.ID);
                command.Parameters.AddWithValue("OrderNumber", movement.OrderNumber);
                command.ExecuteNonQuery();
            }
        }
        public BusMovement GetBusMovement(Guid movementID)
        {
            OpenConnection();
            BusMovement r = new BusMovement();
            string sqlcmd = "Select ID, RouteID,BusStationFrom, BusStationTo,Direction, OrderNumber from BusMovement where ID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", movementID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r.ID = Guid.Parse(dr["ID"].ToString());
                        r.RouteID = Guid.Parse(dr["RouteID"].ToString());
                        r.BusStationFrom = Guid.Parse(dr["BusStationFrom"].ToString());
                        r.BusStationTo = Guid.Parse(dr["BusStationTo"].ToString());
                        r.Direction = bool.Parse(dr["Direction"].ToString());
                        r.OrderNumber = int.Parse(dr["OrderNumber"].ToString());
                    }
                }
            }
            return r;
        }
        public IList<string> GetMovementsOfARouteInOrder(Guid RouteID, Boolean Direction)
        {
            IList<string> list = new List<string>();
            IList<Guid> Stations = new List<Guid>();
            OpenConnection();
            string sqlcmd = string.Format("select BusStationFrom, BusStationTo from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RouteID", RouteID);
                command.Parameters.AddWithValue("Direction", Direction);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        Stations.Add(Guid.Parse(dr["BusStationFrom"].ToString()));
                        Stations.Add(Guid.Parse(dr["BusStationTo"].ToString()));
                    }
                    while (dr.Read())
                    {
                        Stations.Add(Guid.Parse(dr["BusStationTo"].ToString()));
                    }
                }
            }
            foreach (var station in Stations)
            {
                list.Add(GetRoadNameFromBusStationID(station));
            }
            return list;
        }
        public void InsertBusMovement(BusMovement movement)
        {
            OpenConnection();
            string sqlcmd = "insert into BusMovement(ID,RouteID,BusStationFrom,BusStationTo,Direction,OrderNumber) values(:ID, :RouteID,:BusStationFrom, :BusStationTo,:Direction ,:OrderNumber)";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", Guid.NewGuid());
                command.Parameters.AddWithValue("RouteID", movement.RouteID);
                command.Parameters.AddWithValue("BusStationFrom", movement.BusStationFrom);
                command.Parameters.AddWithValue("BusStationTo", movement.BusStationTo);
                command.Parameters.AddWithValue("Direction", movement.Direction);
                command.Parameters.AddWithValue("OrderNumber", movement.OrderNumber);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteBusMovement(Guid MovementId)
        {
            OpenConnection();
            string sqlcmd = "Delete from BusMovement where ID = :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", MovementId);
                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region Route
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
        public BusRoute GetBusRoute(Guid ID)
        {
            OpenConnection();
            BusRoute r = new BusRoute();
            string sqlcmd = "Select RouteID, RouteName from BusRoute S where RouteID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", ID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r.RouteID = Guid.Parse(dr["RouteID"].ToString());
                        r.RouteName = dr["RouteName"].ToString();
                    }
                }
            }
            return r;
        }
        public IList<BusRoute> BusRoutesThroughAStation(Guid stationID)
        {
            IList<BusRoute> list = new List<BusRoute>();
            OpenConnection();
            string sqlcmd = "select distinct R.RouteID, R.RouteName from BusRoute R, BusMovement M where R.RouteID = M.RouteID and (BusStationFrom = :StationID or BusStationTo = :StationID)";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("StationID", stationID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new BusRoute()
                        {
                            RouteID = Guid.Parse(dr["RouteID"].ToString()),
                            RouteName = dr["RouteName"].ToString()
                        });
                    }
                }
            }
            return list;
        }
        public IList<BusRoute> BusRoutesBetween2Stations(Guid s1, Guid s2)
        {
            IList<BusRoute> result = new List<BusRoute>();
            List<BusRoute> list1 = BusRoutesThroughAStation(s1).ToList();
            List<BusRoute> list2 = BusRoutesThroughAStation(s2).ToList();
            //result =  list1.Intersect<BusRoute>(list2).ToList<BusRoute>();
            foreach (BusRoute r in list1)
            {
                if (list2.Contains(r))
                {
                    result.Add(r);
                }
            }
            return result;
        }
        public void SaveBusRoute(BusRoute busRoute)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update BusRoute set RouteName=:RouteName where RouteID= :ID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", busRoute.RouteID);
                command.Parameters.AddWithValue("RouteName", busRoute.RouteName);
                command.ExecuteNonQuery();
            }
        }
        public void InsertBusRoute(BusRoute busRoute)
        {
            OpenConnection();
            string sqlcmd = string.Format("Insert into BusRoute(RouteID, RouteName) values(:ID, :RouteName)");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", Guid.NewGuid());
                command.Parameters.AddWithValue("RouteName", busRoute.RouteName);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteBusRoute(Guid routeID)
        {
            OpenConnection();
            const string sqlcmd = "delete from BusRoute where RouteID= :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", routeID);
                command.ExecuteNonQuery();
            }
        }
        #endregion
        #region Road
        public IList<Road> GetAllRoads()
        {
            IList<Road> list = new List<Road>();
            Road station = new Road();
            OpenConnection();
            string sqlcmd = string.Format("select * from Road");
            NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn);


            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                station = new Road()
                {
                    RoadID = Guid.Parse(dr["RoadID"].ToString()),
                    RoadName = dr["RoadName"].ToString()
                };
                list.Add(station);
            }
            return list;
        }
        public string GetRoadNameFromRoadSession(RoadSession RoadSession)
        {
            OpenConnection();
            string sqlcmd = string.Format("select RoadName from Road where RoadID = :RoadID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RoadID", RoadSession.RoadID);
                return command.ExecuteScalar().ToString();
            }
        }
        public string GetRoadNameFromBusStationID(Guid BusStationID)
        {
            OpenConnection();
            string sqlcmd = string.Format("select R.RoadName from BusStation S, RoadSession RS, Road R where S.RoadSessionID = RS.ID and RS.RoadId =R.RoadId and S.ID = :BusStationID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("BusStationID", BusStationID);
                return command.ExecuteScalar().ToString();
            }
        }
        public Road GetRoad(string StreetName)
        {
            OpenConnection();
            Road r = new Road();
            string sqlcmd = "Select * from Road where RoadName=:StreetName";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("StreetName", StreetName);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r.RoadID = Guid.Parse(dr["RoadId"].ToString());
                        r.RoadName = StreetName;
                    }
                }
            }
            return r;
        }
        public void SaveRoad(Road road)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update Road set RoadName=:RoadName where RoadID= :ID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", road.RoadID);
                command.Parameters.AddWithValue("RoadName", road.RoadName);
                command.ExecuteNonQuery();
            }
        }

        public void InsertRoad(Road road)
        {
            OpenConnection();
            string sqlcmd = string.Format("Insert into Road(RoadID, RoadName) values(:ID, :RoadName)");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", Guid.NewGuid());
                command.Parameters.AddWithValue("RoadName", road.RoadName);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteRoad(Guid roadID)
        {
            OpenConnection();
            string sqlcmd = string.Format("delete from Road where RoadID= :ID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", roadID);
                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region Road Session
        public RoadSession GetRoadSession(Guid ID)
        {
            OpenConnection();
            RoadSession r = new RoadSession();
            string sqlcmd = "Select ID, RoadID, AddressLower, AddressUpper, Description, ST_X(S.PositionLower::geometry) as PositionLower_X, ST_Y(S.PositionLower::geometry) as PositionLower_Y, ST_X(S.PositionUpper::geometry) as PositionUpper_X, ST_Y(S.PositionUpper::geometry) as PositionUpper_Y from RoadSession S where ID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", ID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        r.ID = Guid.Parse(dr["ID"].ToString());
                        r.RoadID = Guid.Parse(dr["RoadId"].ToString());
                        r.AddressLower = int.Parse(dr["AddressLower"].ToString());
                        r.AddressUpper = int.Parse(dr["AddressUpper"].ToString());
                        r.Description = dr["Description"].ToString();
                        r.PositionLower = new Point();
                        if (!String.IsNullOrEmpty(dr["PositionLower_X"].ToString()))
                            r.PositionLower.lat = float.Parse(dr["PositionLower_X"].ToString());
                        if (!String.IsNullOrEmpty(dr["PositionLower_Y"].ToString()))
                            r.PositionLower.lng = float.Parse(dr["PositionLower_Y"].ToString());

                        r.PositionUpper = new Point();
                        if (!String.IsNullOrEmpty(dr["PositionUpper_X"].ToString()))
                            r.PositionUpper.lat = float.Parse(dr["PositionUpper_X"].ToString());
                        if (!String.IsNullOrEmpty(dr["PositionUpper_Y"].ToString()))
                            r.PositionUpper.lng = float.Parse(dr["PositionUpper_Y"].ToString());
                    }
                }
            }
            return r;
        }
        public IList<RoadSession> GetAllRoadSessionOfARoad(Guid roadID)
        {
            IList<RoadSession> rs = new List<RoadSession>();
            OpenConnection();
            string sqlcmd = "Select ID,AddressLower,AddressUpper,RoadID,Description,ST_X(PositionLower::geometry) as PositionLower_X,ST_Y(PositionLower::geometry) as PositionLower_Y,ST_X(PositionUpper::geometry) as PositionUpper_X, ST_Y(PositionUpper::geometry) as PositionUpper_Y from RoadSession where RoadId=:RoadID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RoadId", roadID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        rs.Add(new RoadSession()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            AddressLower = int.Parse(dr["AddressLower"].ToString()),
                            AddressUpper = int.Parse(dr["AddressUpper"].ToString()),
                            RoadID = Guid.Parse(dr["RoadID"].ToString()),
                            Description = dr["Description"].ToString(),
                            PositionLower = new Point() { lng = float.Parse(dr["PositionLower_X"].ToString()), lat = float.Parse(dr["PositionLower_Y"].ToString()) },
                            PositionUpper = new Point() { lng = float.Parse(dr["PositionUpper_X"].ToString()), lat = float.Parse(dr["PositionUpper_Y"].ToString()) }
                        });
                    }
                }
            }
            return rs;
        }
        public IList<RoadSession> GetRoadSessionFromAddressNumberAndRoad(int AddressNumber, Guid roadID)
        {
            IList<RoadSession> rs = new List<RoadSession>();
            OpenConnection();
            string sqlcmd = "Select ID, RoadID, AddressLower, AddressUpper, Description, ST_X(S.PositionLower::geometry) as PositionLower_X, ST_Y(S.PositionLower::geometry) as PositionLower_Y, ST_X(S.PositionUpper::geometry) as PositionUpper_X, ST_Y(S.PositionUpper::geometry) as PositionUpper_Y from RoadSession S where AddressLower<= :Address AND AddressUpper>= :Address and RoadId=:RoadID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("Address", AddressNumber);
                command.Parameters.AddWithValue("RoadId", roadID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    RoadSession r = new RoadSession();
                    while (dr.Read())
                    {
                        r.ID = Guid.Parse(dr["ID"].ToString());
                        r.RoadID = Guid.Parse(dr["RoadId"].ToString());
                        r.AddressLower = int.Parse(dr["AddressLower"].ToString());
                        r.AddressUpper = int.Parse(dr["AddressUpper"].ToString());
                        r.Description = dr["Description"].ToString();
                        r.PositionLower = new Point();
                        if (!String.IsNullOrEmpty(dr["PositionLower_X"].ToString()))
                            r.PositionLower.lng = float.Parse(dr["PositionLower_X"].ToString());
                        if (!String.IsNullOrEmpty(dr["PositionLower_Y"].ToString()))
                            r.PositionLower.lat = float.Parse(dr["PositionLower_Y"].ToString());

                        r.PositionUpper = new Point();
                        if (!String.IsNullOrEmpty(dr["PositionUpper_X"].ToString()))
                            r.PositionUpper.lng = float.Parse(dr["PositionUpper_X"].ToString());
                        if (!String.IsNullOrEmpty(dr["PositionUpper_Y"].ToString()))
                            r.PositionUpper.lat = float.Parse(dr["PositionUpper_Y"].ToString());

                        rs.Add(r);
                    }
                }
            }
            return rs;
        }
        public IList<RoadSession> GetAllRoadSessionsOfARoad(Guid roadID)
        {
            IList<RoadSession> rs = new List<RoadSession>();
            OpenConnection();
            string sqlcmd = "Select * from RoadSession where RoadId=:RoadID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("RoadId", roadID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        rs.Add(new RoadSession()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            AddressLower = int.Parse(dr["AddressLower"].ToString()),
                            AddressUpper = int.Parse(dr["AddressUpper"].ToString()),
                            RoadID = Guid.Parse(dr["RoadID"].ToString()),
                            Description = dr["Description"].ToString()
                        });
                    }
                }
            }
            return rs;
        }
        public IList<RoadSession> GetRoadSessionFromAddress(int AddressNumber, string StreetName)
        {
            IList<RoadSession> rs = new List<RoadSession>();
            Road road = GetRoad(StreetName);
            rs = GetRoadSessionFromAddressNumberAndRoad(AddressNumber, road.RoadID);
            return rs;
        }
        public RoadSession GetFirstMatchRoadSessionFromAddress(int AddressNumber, string StreetName)
        {
            Road road = GetRoad(StreetName);
            IList<RoadSession> sessions = GetRoadSessionFromAddressNumberAndRoad(AddressNumber, road.RoadID);
            if (sessions.Count > 0)
                return GetRoadSessionFromAddressNumberAndRoad(AddressNumber, road.RoadID).First();
            else
                return null;
        }
        public void SaveRoadSession(RoadSession r)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update RoadSession set RoadID=:RoadID, AddressLower = :AddressLower, AddressUpper= :AddressUpper, Description=:Description, PositionLower = ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), PositionUpper = ST_GeographyFromText(\'SRID=4326;POINT({2} {3})\') where ID= :ID", r.PositionLower.lng, r.PositionLower.lat, r.PositionUpper.lng, r.PositionUpper.lat);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", r.ID);
                command.Parameters.AddWithValue("RoadID", r.RoadID);
                command.Parameters.AddWithValue("AddressLower", r.AddressLower);
                command.Parameters.AddWithValue("AddressUpper", r.AddressUpper);
                command.Parameters.AddWithValue("Description", r.Description);
                command.ExecuteNonQuery();
            }
        }
        public void InsertRoadSession(RoadSession roadSession)
        {
            OpenConnection();
            string sqlcmd = string.Format("insert into RoadSession(id, roadid, addresslower, addressupper, description, positionlower, positionupper) values(:ID, :RoadId, :addresslower, :addressupper, :description, ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), ST_GeographyFromText(\'SRID=4326;POINT({2} {3})\'))", roadSession.PositionLower.lng, roadSession.PositionLower.lat, roadSession.PositionUpper.lng, roadSession.PositionUpper.lat);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", Guid.NewGuid());
                command.Parameters.AddWithValue("RoadId", roadSession.RoadID);
                command.Parameters.AddWithValue("addresslower", roadSession.AddressLower);
                command.Parameters.AddWithValue("addressupper", roadSession.AddressUpper);
                command.Parameters.AddWithValue("description", roadSession.Description);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteRoadSession(Guid roadSessionID)
        {
            OpenConnection();
            const string sqlcmd = "delete from RoadSession where id= :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.AddWithValue("ID", roadSessionID);
                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}

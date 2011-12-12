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

        #region Get Entity
        public RoadSession GetRoadSession(Guid ID)
        {
            OpenConnection();
            RoadSession r = new RoadSession();
            string sqlcmd = "Select ID, RoadID, AddressLower, AddressUpper, Description, ST_X(S.PositionLower::geometry) as PositionLower_X, ST_Y(S.PositionLower::geometry) as PositionLower_Y, ST_X(S.PositionUpper::geometry) as PositionUpper_X, ST_Y(S.PositionUpper::geometry) as PositionUpper_Y from RoadSession S where ID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", ID);
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
        public BusStation GetBusStation(Guid ID)
        {
            OpenConnection();
            BusStation r = new BusStation();
            string sqlcmd = "Select ID, RoadSessionID, StationName,  ST_X(S.Position::geometry) as Position_X, ST_Y(S.Position::geometry) as Position_Y from BusStation S where ID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", ID);
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
        public BusRoute GetBusRoute(Guid ID)
        {
            OpenConnection();
            BusRoute r = new BusRoute();
            string sqlcmd = "Select RouteID, RouteName from BusRoute S where RouteID=:ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", ID);
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
        #endregion
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
            string sqlcmd = string.Format("select BusStationFrom, BusStationTo from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RouteID", RouteID);
                command.Parameters.Add("Direction", Direction);
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
        public Road GetRoad(string StreetName)
        {
            OpenConnection();
            Road r = new Road();
            string sqlcmd = "Select * from Road where RoadName=:StreetName";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("StreetName", StreetName);
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
        public IList<RoadSession> GetRoadSessionFromAddressNumberAndRoad(int AddressNumber, Road road)
        {
            IList<RoadSession> rs = new List<RoadSession>();
            OpenConnection();
            string sqlcmd = "Select * from RoadSession where AddressLower<= :Address AND AddressUpper>= :Address and RoadId=:RoadID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("Address", AddressNumber);
                command.Parameters.Add("RoadId", road.RoadID);
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
            rs = GetRoadSessionFromAddressNumberAndRoad(AddressNumber, road);
            return rs;
        }
        public Point GetPositionFromAddress(int AddressNumber, string StreetName)
        {
            Point p = new Point();
            IList<RoadSession> ss = GetRoadSessionFromAddress(AddressNumber, StreetName);
            RoadSession selected = ss.First<RoadSession>();
            float fraction = (float)(AddressNumber - selected.AddressLower) / (selected.AddressUpper - selected.AddressLower);
            p = InterpolatePoint(selected.PositionLower, selected.PositionUpper, fraction);
            return p;
        }

        private Point InterpolatePoint(Point point, Point point_2, float fraction)
        {
            OpenConnection();
            Point p = new Point();
            string sqlcmd = String.Format("SELECT ST_X(ST_Line_Interpolate_Point(the_line, :fraction)::geometry) as X, ST_Y(ST_Line_Interpolate_Point(the_line, :fraction)::geometry) as Y FROM (SELECT ST_GeomFromEWKT('LINESTRING({0} {1}, {2} {3})') as the_line) As foo", point.lat, point.lng, point_2.lat, point_2.lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("fraction", fraction);
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

        public IList<BusStation> GetNeighborStations(Point p, int distance)
        {
            IList<BusStation> list = new List<BusStation>();
            OpenConnection();
            string sqlcmd = string.Format("SELECT ID,RoadSessionID,StationName,ST_X(Position::geometry) as X, ST_Y(Position::geometry) as Y  FROM BusStation WHERE ST_DWithin(geocolumn, 'POINT({0} {1})', :distance);", p.lat, p.lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("distance", distance);
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

        public IList<BusRoute> BusRoutesThroughAStation(BusStation s)
        {
            IList<BusRoute> list = new List<BusRoute>();
            OpenConnection();
            string sqlcmd = string.Format("select distinct R.RouteID, R.RouteName from BusRoute R, BusMovement M where R.RouteID = M.RouteID and (BusStationFrom = :StationID or BusStationTo = :StationID)");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("StationID", s.ID);
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
        public IList<BusStation> BusStationsOfARoute(BusRoute s)
        {
            IList<BusStation> list = new List<BusStation>();
            OpenConnection();
            string sqlcmd = string.Format("select distinct S.ID, S.RoadSessionID, S.StationName,  ST_X(S.Position::geometry) as X, ST_Y(S.Position::geometry) as Y from BusStation S, BusMovement M where (S.ID = MBusStationFrom or S.ID = MBusStationTo) and M.RouteID = :RouteID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RouteID", s.RouteID);
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
        public IList<BusRoute> BusRoutesBetween2Stations(BusStation s1, BusStation s2)
        {
            IList<BusRoute> list1 = BusRoutesThroughAStation(s1);
            IList<BusRoute> list2 = BusRoutesThroughAStation(s2);
            return list1.Intersect(list2).ToList<BusRoute>();
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
        public IList<Point> GetAllStationPositionsOfARouteInOrderWithIntermediatePoints(Guid RouteID, Boolean Direction)
        {
            IList<Point> list = new List<Point>();
            IList<Guid> Stations = new List<Guid>();
            OpenConnection();
            string sqlcmd = string.Format("select BusStationFrom, BusStationTo, ID from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RouteID", RouteID);
                command.Parameters.Add("Direction", Direction);
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


        public IList<BusMovement> GetAllBusMovements(Guid RouteID, bool bDirection)
        {
            IList<BusMovement> list = new List<BusMovement>();
            OpenConnection();
            string sqlcmd = string.Format("select * from BusMovement where RouteID = :RouteID and Direction = :Direction Order By OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("RouteID", RouteID);
                command.Parameters.Add("Direction", bDirection);
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


        public IList<Point> GetIntermediatePoints(Guid MovementID)
        {
            IList<Point> list = new List<Point>();
            OpenConnection();
            string sqlcmd = string.Format("select ST_X(P.Position::geometry) as X, ST_Y(P.Position::geometry) as Y from IntermediatePoint P where BusMovementID = :MovementID order by OrderNumber");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("MovementID", MovementID);
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
                command.Parameters.Add("MovementID", MovementID);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new IntermediatePoint()
                        {
                            ID = Guid.Parse(dr["ID"].ToString()),
                            lat = float.Parse(dr["X"].ToString()),
                            lng = float.Parse(dr["Y"].ToString()),
                            OrderNumber = int.Parse(dr["OrderNumber"].ToString())
                        });
                    }
                }
            }
            return list;
        }
        public void InsertIntermediatePoint(Guid MovementID, double lat, double lng, int order)
        {
            OpenConnection();
            string sqlcmd = string.Format("insert into IntermediatePoint values(:newID, :MovementID, ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), :OrderNumber)", lat, lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("newID", Guid.NewGuid());
                command.Parameters.Add("MovementID", MovementID);
                //command.Parameters.Add("lat", lat);
                //command.Parameters.Add("lng", lng);
                command.Parameters.Add("OrderNumber", order);
                command.ExecuteNonQuery();
            }
        }

        #region Save Entity
        public void SaveIntermediatePoint(IntermediatePoint p)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update IntermediatePoint set Position = ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), OrderNumber = :OrderNumber where ID= :ID", p.lat, p.lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", p.ID);
                //command.Parameters.Add("lat", lat);
                //command.Parameters.Add("lng", lng);
                command.Parameters.Add("OrderNumber", p.OrderNumber);
                command.ExecuteNonQuery();
            }
        }
        public void SaveRoadSession(RoadSession r)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update RoadSession set RoadID=:RoadID, AddressLower = :AddressLower, AddressUpper= :AddressUpper, Description=:Description, PositionLower = ST_GeographyFromText(\'SRID=4326;POINT({0} {1})\'), PositionUpper = ST_GeographyFromText(\'SRID=4326;POINT({2} {3})\') where ID= :ID", r.PositionLower.lat, r.PositionLower.lng, r.PositionUpper.lat, r.PositionUpper.lng);
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", r.ID);
                command.Parameters.Add("RoadID", r.RoadID);
                command.Parameters.Add("AddressLower", r.AddressLower);
                command.Parameters.Add("AddressUpper", r.AddressUpper);
                command.Parameters.Add("Description", r.Description);
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
        #endregion

        #region Bus Movement
        public void SaveBusMovement(BusMovement movement)
        {
            OpenConnection();
            string sqlcmd = string.Format("Update BusMovement set OrderNumber=:OrderNumber where ID= :ID");
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", movement.ID);
                command.Parameters.Add("OrderNumber", movement.OrderNumber);
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
                command.Parameters.Add("ID", movementID);
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
        public void InsertBusMovement(BusMovement movement)
        {
            OpenConnection();
            string sqlcmd = "insert into BusMovement(ID,RouteID,BusStationFrom,BusStationTo,Direction,OrderNumber) values(:ID, :RouteID,:BusStationFrom, :BusStationTo,:Direction ,:OrderNumber)";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", Guid.NewGuid());
                command.Parameters.Add("RouteID", movement.RouteID);
                command.Parameters.Add("BusStationFrom", movement.BusStationFrom);
                command.Parameters.Add("BusStationTo", movement.BusStationTo);
                command.Parameters.Add("Direction", movement.Direction);
                command.Parameters.Add("OrderNumber", movement.OrderNumber);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBusMovement(Guid MovementId)
        {
            OpenConnection();
            string sqlcmd = "Delete from BusMovement where ID = :ID";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, conn))
            {
                command.Parameters.Add("ID", MovementId);
                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}


CREATE TABLE BusRoute(
RouteID UUID PRIMARY KEY,
RouteName character varying(100)
);


CREATE TABLE Road(
RoadId UUID PRIMARY KEY,
RoadName character varying(100)
);
CREATE TABLE RoadSession(
ID UUID PRIMARY KEY,
RoadId UUID,
AddressLower Integer,
AddressUpper Integer,
Description text
);
ALTER TABLE RoadSession ADD  CONSTRAINT FK_RoadSession_Road FOREIGN KEY(RoadId)
REFERENCES Road (RoadId)
ON UPDATE CASCADE
ON DELETE CASCADE;

ALTER TABLE RoadSession ADD COLUMN PositionLower GEOGRAPHY ( POINT ,4326);
ALTER TABLE RoadSession ADD COLUMN PositionUpper GEOGRAPHY ( POINT ,4326);

CREATE TABLE BusStation(
ID UUID PRIMARY KEY,
RoadSessionID UUID,
StationName character varying(100),
Position GEOGRAPHY ( POINT ,4326)
);
ALTER TABLE BusStation ADD  CONSTRAINT FK_BusStation_RoadSession FOREIGN KEY(RoadSessionID)
REFERENCES RoadSession (ID)
ON UPDATE CASCADE
ON DELETE CASCADE;

CREATE TABLE BusMovement(
ID UUID PRIMARY KEY,
RouteID UUID,
BusStationFrom UUID,
BusStationTo UUID,
Direction boolean,
OrderNumber Integer
);
ALTER TABLE BusMovement ADD  CONSTRAINT FK_BusMovement_BusRoute FOREIGN KEY(RouteID)
REFERENCES BusRoute (RouteID)
ON UPDATE CASCADE
ON DELETE CASCADE;
ALTER TABLE BusMovement ADD  CONSTRAINT FK_BusMovement_BusStation_From FOREIGN KEY(BusStationFrom)
REFERENCES BusStation (ID)
ON UPDATE CASCADE
ON DELETE CASCADE;
ALTER TABLE BusMovement ADD  CONSTRAINT FK_BusMovement_BusStation_To FOREIGN KEY(BusStationFrom)
REFERENCES BusStation (ID)
ON UPDATE CASCADE
ON DELETE CASCADE;

CREATE TABLE IntermediatePoint(
ID UUID PRIMARY KEY,
BusMovementID UUID,
Position GEOGRAPHY ( POINT ,4326),
OrderNumber Integer
);
ALTER TABLE IntermediatePoint ADD  CONSTRAINT FK_IntermediatePoint_BusMovement FOREIGN KEY(BusMovementID)
REFERENCES BusMovement (ID)
ON UPDATE CASCADE
ON DELETE CASCADE;
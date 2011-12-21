package cse.it.data;

import android.util.Log;
import cse.it.parse.ParsedData;

public class BusStation {
	public String id_tuyen;
	public String stationName;
	public String Position_X;
	public String Position_Y;
	public String StreetName;
	public String Routes;
	
	public BusStation(String id_tuyen, String stationName, String Position_X, String Position_Y, String StreetName, String Routes){
		super();
		this.id_tuyen = id_tuyen;
		this.stationName = stationName;
		this.Position_X = Position_X;
		this.Position_Y = Position_Y;
		this.StreetName = StreetName;
		this.Routes = Routes;
		ParsedData.busStation.add(this);
	}
}

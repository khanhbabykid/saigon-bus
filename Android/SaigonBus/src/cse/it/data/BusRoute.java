package cse.it.data;

import android.util.Log;
import cse.it.parse.ParsedData;

public class BusRoute {
	public String Station_Src;
	public String BusRoute;
	public String Station_Dst;
	
	public BusRoute( String Station_Src,String BusRoute, String Station_Dst){
		super();
		this.Station_Src = Station_Src;
		this.BusRoute = BusRoute;
		this.Station_Dst = Station_Dst;
		ParsedData.timduong.add(this);
		Log.d("a", "check"+ParsedData.tuyenXe);
	}
}

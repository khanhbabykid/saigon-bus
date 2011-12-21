package cse.it.parse;

import java.util.ArrayList;

import cse.it.data.BusRoute;
import cse.it.data.BusStation;
import cse.it.data.MyPos;
import cse.it.data.TuyenXe;

public class ParsedData {
	//intilizial for save data
	public static ArrayList<TuyenXe> tuyenXe;
	public static ArrayList<BusStation> busStation;
	public static ArrayList<MyPos> myPos;
	public static ArrayList<BusRoute> timduong;
	public ParsedData(){
		tuyenXe = new ArrayList<TuyenXe>();
		busStation = new ArrayList<BusStation>();
		myPos = new ArrayList<MyPos>();
		timduong = new ArrayList<BusRoute>();
		
	}
	
}

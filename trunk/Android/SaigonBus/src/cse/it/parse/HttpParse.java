package cse.it.parse;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import cse.it.data.BusStation;
import cse.it.data.MyPos;
import cse.it.data.TuyenXe;

public class HttpParse {

	public void HttpParseArrary(JSONArray json){
		String id_tuyen;
		String ten_tuyen;
		String huong_di;
		String huong_ve;
		for (int i = 0; i < json.length(); i++) {
			try {
				JSONObject tuyenJson = json.getJSONObject(i);
				id_tuyen = tuyenJson.getString("ID");
				ten_tuyen = tuyenJson.getString("Name");
				try {
					huong_di = tuyenJson.getString("Movements1");
					huong_ve = tuyenJson.getString("Movements2");
				} catch (JSONException e) {
					huong_di = "";
					huong_ve = "";
					e.printStackTrace();
				}
				
				TuyenXe tuyen_xe = new TuyenXe(id_tuyen, ten_tuyen,huong_di, huong_ve);
			} catch (JSONException e) {
				// TODO: handle exception
				e.printStackTrace();
			}
		}
	}
	public void HttpParseStationArrary(JSONArray json){
		 String id_tuyen;
		 String stationName;
		 String Position_X;
		 String Position_Y;
		 String StreetName;
		 String Routes;
		for (int i = 0; i < json.length(); i++) {
			try {
				JSONObject tuyenJson = json.getJSONObject(i);
				id_tuyen = tuyenJson.getString("ID");
				stationName = tuyenJson.getString("StationName");
				Position_X = tuyenJson.getString("Position_X");
				Position_Y = tuyenJson.getString("Position_Y");
				StreetName = tuyenJson.getString("StreetName");
				Routes = tuyenJson.getString("Routes");
								
				new BusStation(id_tuyen, stationName, Position_X, Position_Y, StreetName, Routes);
				
			} catch (JSONException e) {
				// TODO: handle exception
				e.printStackTrace();
			}
		}
	}
	
	public void HttpParsePosArrary(JSONArray json){
		String id_tuyen;
		String ten_tram;
		String ten_duong;
		String tuyen_di_qua;
		for (int i = 0; i < json.length(); i++) {
			try {
				JSONObject tuyenJson = json.getJSONObject(i);
				id_tuyen = tuyenJson.getString("ID");
				ten_tram = tuyenJson.getString("StationName");
				ten_duong = tuyenJson.getString("StreetName");
				tuyen_di_qua = tuyenJson.getString("Routes");
				
				new MyPos(id_tuyen, ten_tram, ten_duong, tuyen_di_qua);
			} catch (JSONException e) {
				// TODO: handle exception
				e.printStackTrace();
			}
		}
	}
	public void HttpParseTimDuong(String sTemp){
		String Station_Src;
		String BusRoute;
		String Station_Dst;
		
		new cse.it.data.BusRoute("DHBK LTK", sTemp, "Au Co");
		new cse.it.data.BusRoute("DHBK LTK", sTemp, "Nguyen Thi Nho - From Lu Gia to Au Co");
		new cse.it.data.BusRoute("Lu Gia", sTemp, "Au Co");
		new cse.it.data.BusRoute("Lu Gia", sTemp, "Nguyen Thi Nho - From Lu Gia to Au Co");
		
//		for (int i = 0; i < json.length(); i++) {
//			try {
//				JSONObject tuyenJson = json.getJSONObject(i);
//				BusRoute = tuyenJson.getString("BusRoute");
//				Station_Src = tuyenJson.getString("Station_Src");
//				Station_Dst = tuyenJson.getString("Station_Dst");
//				
//				new cse.it.data.BusRoute("DHBK LTK", "số 38", "Au Co");
//				new cse.it.data.BusRoute("DHBK LTK", "số 38", "Nguyen Thi Nho - From Lu Gia to Au Co");
//				new cse.it.data.BusRoute("Lu Gia", "số 38", "Au Co");
//				new cse.it.data.BusRoute("Lu Gia", "số 38", "Nguyen Thi Nho - From Lu Gia to Au Co");
//					
//			} catch (JSONException e) {
//				// TODO: handle exception
//				e.printStackTrace();
//			}
//		}
	}
}

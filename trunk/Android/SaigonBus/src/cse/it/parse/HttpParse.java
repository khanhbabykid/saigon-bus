package cse.it.parse;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

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
				id_tuyen = tuyenJson.getString("RouteID");
				ten_tuyen = tuyenJson.getString("RouteName");
				try {
					huong_di = tuyenJson.getString("Movements1");
					huong_ve = tuyenJson.getString("Movements2");
				} catch (JSONException e) {
					// TODO Auto-generated catch block
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
	
}

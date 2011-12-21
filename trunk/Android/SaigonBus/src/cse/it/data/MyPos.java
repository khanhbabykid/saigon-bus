package cse.it.data;

import android.util.Log;
import cse.it.parse.ParsedData;

public class MyPos {
	public String id_tuyen;
	public String ten_tram;
	public String ten_duong;
	public String tuyen_di_qua;
	
	public MyPos(String id_tuyen, String ten_tram,String ten_duong, String tuyen_di_qua){
		super();
		this.id_tuyen = id_tuyen;
		this.ten_tram = ten_tram;
		this.ten_duong = ten_duong;
		this.tuyen_di_qua = tuyen_di_qua;
		ParsedData.myPos.add(this);
		Log.d("a", "check"+ParsedData.tuyenXe);
	}
}

package cse.it.data;

import android.util.Log;
import cse.it.SaigonBus;
import cse.it.parse.ParsedData;

public class TuyenXe {

	public String id_tuyen;
	public String ten_tuyen;
	public String huong_di;
	public String huong_ve;
	
	public TuyenXe(String id_tuyen, String ten_tuyen,String huong_di, String huong_ve){
		super();
		this.id_tuyen = id_tuyen;
		this.ten_tuyen = ten_tuyen;
		this.huong_di = huong_di;
		this.huong_ve = huong_ve;
		ParsedData.tuyenXe.add(this);
		Log.d("a", "check"+ParsedData.tuyenXe);
	}
}

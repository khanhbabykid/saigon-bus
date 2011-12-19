package cse.it;

import cse.it.parse.ParsedData;
import android.app.Activity;
import android.os.Bundle;
import android.widget.TextView;

public class Lotrinh extends Activity{

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.lo_trinh);
		
		TextView txt_Huongdi = (TextView) findViewById(R.id.txt_huongdi);
		TextView txt_Huongve = (TextView) findViewById(R.id.txt_huongve);
		TextView txt_TuyenXe = (TextView) findViewById(R.id.TextTuyenXe);
		
		int iPos = getIntent().getIntExtra("POS_LOTRINH", 0);
		txt_TuyenXe.setText(ParsedData.tuyenXe.get(iPos).ten_tuyen);
		if (ParsedData.tuyenXe.get(iPos).huong_di != null) {
			txt_Huongdi.setText(ParsedData.tuyenXe.get(iPos).huong_di);
		}
		if (ParsedData.tuyenXe.get(iPos).huong_ve != null) {
			txt_Huongve.setText(ParsedData.tuyenXe.get(iPos).huong_ve);
		}
	}

}

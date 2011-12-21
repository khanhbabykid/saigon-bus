package cse.it;

import java.util.ArrayList;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import cse.it.parse.HttpParse;
import cse.it.parse.ParsedData;
import cse.it.parse.RestClient;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

public class TimDuong extends Activity{

	protected JSONObject resp = null;
	protected ProgressDialog dialog;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.tim_duong);
		final EditText tu_add = (EditText)findViewById(R.id.edit_tu_add);
		
		final EditText tu_street = (EditText)findViewById(R.id.edit_tu_street);
		
		final EditText de_add= (EditText)findViewById(R.id.edit_de_add);
	
		final EditText de_street = (EditText)findViewById(R.id.edit_de_street);
		
		Button btnTim = (Button) findViewById(R.id.idButtonTim);
		btnTim.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				String stu_add = tu_add.getText().toString();
				String stu_street = tu_street.getText().toString();
				String sde_street = de_street.getText().toString();
				String sde_add = de_add.getText().toString();
								//if (!stu_add.equals("") && !stu_street.equals("") && !sde_add.equals("")&& !sde_street.equals("")){
				HttpParse parse = new HttpParse();
				parse.HttpParseTimDuong(getString(R.string.so38));
				Intent intent = new Intent(TimDuong.this, Timduong_detail.class);
				intent.putExtra("POS_VALUE", stu_add+" "+stu_street +" - "+sde_add+" "+sde_street);
				startActivity(intent);
//					final Handler uiThreadCallback = new Handler();
//					final Runnable runInUIThread = new Runnable() {
//						public void run() {
//							getResponse();
//						}
//					};
//					
//					// we use a thread to query the server, for signin
//					new Thread() {
//						@Override
//						public void run() {						
//							ArrayList<NameValuePair> params = new ArrayList<NameValuePair>();
//							params.add(new BasicNameValuePair("AddNo1", stu_add));
//							params.add(new BasicNameValuePair("Street1", stu_street));
//							
//							params.add(new BasicNameValuePair("AddNo2", sde_add));
//							params.add(new BasicNameValuePair("Street2", sde_street));
//							resp = RestClient.connectJson(Def.URL_TIM_DUONG, params);
//							Log.d("JSON", "return "+resp);
//							uiThreadCallback.post(runInUIThread);		
//							
//						}
//					}.start();
				}
			//}
		});
	}
//	public void getResponse() {
//		JSONArray statusCode = null;
//		// dismiss the progress dialog when received the response
//		if (dialog != null)
//			dialog.dismiss();
//		
//		if(resp != null){
//			HttpParse parse = new HttpParse();
//			try {
//				JSONArray respArray = resp.getJSONArray("Path_OneRoute");
//				parse.HttpParseTimDuong(respArray);
//				Intent intent = new Intent(TimDuong.this, Timduong_detail.class);
//				startActivity(intent);
//			} catch (JSONException e) {
//				// TODO Auto-generated catch block
//				e.printStackTrace();
//			}
//			//parse.HttpParsePosArrary(resp);
//			Log.d("aaa", "resp "+ParsedData.myPos.get(0).ten_tram);
//		}
//	}
}

package cse.it;
import java.util.ArrayList;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONArray;

import android.app.ListActivity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import cse.it.parse.Downloader;
import cse.it.parse.HttpParse;
import cse.it.parse.ParsedData;
import cse.it.parse.RestClient;

public class TramXe extends ListActivity{

	public static String stringDuong;
	public static String sTram;
	protected ProgressDialog dialog;
	protected JSONArray resp = null;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		if (Def.isConnectionAvailable(this)) {
			dialog = ProgressDialog.show(TramXe.this, "",
					"Loading", true);
			final Handler uiThreadCallback = new Handler();
			final Runnable runInUIThread = new Runnable() {
				public void run() {
					getResponse();
				}
			};
			
			// we use a thread to query the server, for signin
			new Thread() {
				@Override
				public void run() {				
					
					
					resp = RestClient.connect(Def.URL_TRAM_XE, null);
					Log.d("JSON", "return "+resp);
					uiThreadCallback.post(runInUIThread);		
					
				}
			}.start();
		}
		else{
			ToastUtil.show(this, "No interner");
		}
		
  }

	public void getResponse() {
		JSONArray statusCode = null;
		// dismiss the progress dialog when received the response
		if (dialog != null)
			dialog.dismiss();
		
		if(resp != null){
			HttpParse parse = new HttpParse();
			
			parse.HttpParseStationArrary(resp);
			Log.d("fgda", "da"+ParsedData.busStation.size());
		}
		updateView();
	}

	private void updateView (){
		setContentView(R.layout.tram_xe);
		TramXeAdapter tripAdapter = new TramXeAdapter(this);
		setListAdapter(tripAdapter);
		
		sTram = getString(R.string.station);
		stringDuong = getString(R.string.street);
	}
	private static class TramXeAdapter extends BaseAdapter {
		 private LayoutInflater mInflater;
		 public TramXeAdapter(Context mContext){
			 mInflater = LayoutInflater.from(mContext);
		 }
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			
			if (ParsedData.busStation != null) {
				return ParsedData.busStation.size();
			}
			return 0;
		}
	
		@Override
		public Object getItem(int arg0) {
			// TODO Auto-generated method stub
			if (ParsedData.busStation != null) {
				return ParsedData.busStation.get(arg0);
			}
			return null;
		}
	
		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return 0;
		}
	
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			// TODO Auto-generated method stub
			
			if (convertView == null) {
				convertView = mInflater.inflate(R.layout.tram_item, parent, false);
			}	
			
			ImageView imageTrip = (ImageView)convertView.findViewById(R.id.IconList);
			imageTrip.setBackgroundResource(R.drawable.iconcurrenttrip);
			
			((TextView) convertView.findViewById(R.id.TextList2)).setText(sTram+" "+(ParsedData.busStation.get(position)).stationName);
			((TextView) convertView.findViewById(R.id.Textlist3)).setText(stringDuong+" "+(ParsedData.busStation.get(position)).StreetName);
			return convertView;
		}
	}
	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) 
	{
	 
		Intent intent = new Intent(TramXe.this, TramXe_detail.class);
		startActivity(intent);
		if (!ParsedData.busStation.get(position).stationName.equals("") ) {
			intent.putExtra("POS_STATION_NAME", position);
			startActivity(intent);
		}
		else{
			ToastUtil.show(this, "No Imformation");
		}
		
	}

}

package cse.it;

import cse.it.parse.ParsedData;

import android.app.Activity;
import android.app.ListActivity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

public class StationBus extends ListActivity {
	 @Override
	    public void onCreate(Bundle savedInstanceState) {
	        super.onCreate(savedInstanceState);
	        setContentView(R.layout.currenttrip);
	        
	        TuyenXeAdapter tripAdapter = new TuyenXeAdapter(this);
			  setListAdapter(tripAdapter);
	    }
	 
	 private static class TuyenXeAdapter extends BaseAdapter {
		 private LayoutInflater mInflater;
		 public TuyenXeAdapter(Context mContext){
			 mInflater = LayoutInflater.from(mContext);
		 }
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			
			if (ParsedData.tuyenXe != null) {
				return ParsedData.tuyenXe.size();
			}
			return 0;
		}

		@Override
		public Object getItem(int arg0) {
			// TODO Auto-generated method stub
			if (ParsedData.tuyenXe != null) {
				return ParsedData.tuyenXe.get(arg0);
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
				convertView = mInflater.inflate(R.layout.trip_item, parent, false);
			}	
			
			ImageView imageTrip = (ImageView)convertView.findViewById(R.id.IconList);
			imageTrip.setBackgroundResource(R.drawable.iconcurrenttrip);
			
			((TextView) convertView.findViewById(R.id.TextList2)).setText((ParsedData.tuyenXe.get(position)).ten_tuyen);
			return convertView;
		}
	 }
	 @Override
		protected void onListItemClick(ListView l, View v, int position, long id) 
		{
		 
			Intent intent = new Intent(StationBus.this, Lotrinh.class);
			
			if (!ParsedData.tuyenXe.get(position).huong_di.equals("") ) {
				intent.putExtra("POS_LOTRINH", position);
				startActivity(intent);
			}
			else{
				ToastUtil.show(this, "No Imformation");
			}
			
		}
}

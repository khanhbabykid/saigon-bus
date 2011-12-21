package cse.it;


import cse.it.parse.ParsedData;
import android.app.ListActivity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

public class Timduong_detail extends ListActivity{
	
	public static String stringDibo;
	public static String sDung;
	public static String sBatxe;
	public static String sConlai;
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
	  super.onCreate(savedInstanceState);  
	
	  setContentView(R.layout.timduong_detail);
	 String sImfo = getIntent().getStringExtra("POS_VALUE");
	 TextView textTemp = (TextView)findViewById(R.id.TextView01);
	 textTemp.setText(sImfo);
	  stringDibo = getString(R.string.dibo);
	  sDung = getString(R.string.dung_tram);
	  sBatxe = getString(R.string.bat_xe);
	  sConlai = getString(R.string.conlai);
		
	  TramXeAdapter tripAdapter = new TramXeAdapter(this);
		setListAdapter(tripAdapter);
		
		
	}
	private static class TramXeAdapter extends BaseAdapter {
		 private LayoutInflater mInflater;
		 public TramXeAdapter(Context mContext){
			 mInflater = LayoutInflater.from(mContext);
		 }
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			
			if (ParsedData.timduong != null) {
				return ParsedData.timduong.size();
			}
			return 0;
		}
	
		@Override
		public Object getItem(int arg0) {
			// TODO Auto-generated method stub
			if (ParsedData.timduong != null) {
				return ParsedData.timduong.get(arg0);
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
				convertView = mInflater.inflate(R.layout.tim_item, parent, false);
			}	
			
			ImageView imageTrip = (ImageView)convertView.findViewById(R.id.IconList);
			imageTrip.setBackgroundResource(R.drawable.iconcurrenttrip);
			
			((TextView) convertView.findViewById(R.id.TextList2)).setText(
					stringDibo+" "+(ParsedData.timduong.get(position)).Station_Src
					+" "+sBatxe +"["+(ParsedData.timduong.get(position)).BusRoute+"]"
					+" "+sDung + " "+(ParsedData.timduong.get(position)).Station_Dst
					+"."+sConlai					
			);
			
			return convertView;
		}
	}
	
}

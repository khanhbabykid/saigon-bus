package cse.it;

import java.util.List;

import android.app.Activity;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;

import com.google.android.maps.GeoPoint;
import com.google.android.maps.MapActivity;
import com.google.android.maps.MapController;
import com.google.android.maps.MapView;
import com.google.android.maps.Overlay;
import com.google.android.maps.OverlayItem;

import cse.it.map.MyItemizedOverlay;
import cse.it.parse.ParsedData;

public class TramXe_detail extends MapActivity{

	public static int lat = 10808689;
	public static int lng = 106710033;
	public static String nameMarket;
	public static String nameTrip;
	public static String startDate;

	MapView mapView;
	GeoPoint mPoint;
	MapController mapController;
	List<Overlay> mapOverlays;
	Drawable drawable;
	MyItemizedOverlay itemizedOverlay;
	int pos;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.tramxe_detail);
		
		pos = getIntent().getIntExtra("POS_STATION_NAME", 0);
		
		TextView txt_Huongve = (TextView) findViewById(R.id.TextView02);
		TextView txt_TuyenXe = (TextView) findViewById(R.id.TextTuyenXe);
		
		int iPos = getIntent().getIntExtra("POS_STATION_NAME", 0);
		if (ParsedData.busStation.get(iPos).stationName != null) {
			txt_TuyenXe.setText(getString(R.string.station)+" "+ParsedData.busStation.get(iPos).stationName);
			nameMarket = ParsedData.busStation.get(iPos).stationName;
		}
		if (ParsedData.busStation.get(iPos).StreetName != null) {
			txt_Huongve.setText(getString(R.string.street)+" "+ParsedData.busStation.get(iPos).StreetName 
								+" - "+getString(R.string.tuyen_qua)+" "+ParsedData.busStation.get(iPos).Routes);
		}
		
//		lat = Double.valueOf(ParsedData.busStation.get(iPos).Position_X);
//		lng = Double.valueOf(ParsedData.busStation.get(iPos).Position_Y);
//		int fromLatitude = (int)(lat * 1E6);
//		int fromLongitude = (int)(lng * 1E6);
		//Log.d("te", "la"+fromLatitude+"lo"+fromLongitude);
		mapView = (MapView) findViewById(R.id.mapview);
		mapView.setBuiltInZoomControls(true);
		
		mapOverlays = mapView.getOverlays();
        drawable = this.getResources().getDrawable(R.drawable.pinmarker1);

		mPoint = new GeoPoint(lat,lng);
		itemizedOverlay = new MyItemizedOverlay(drawable, mapView);
        
        mapView.setBuiltInZoomControls(true);
		mapController = mapView.getController();
		mapController.setZoom(16); // Zoon 1 is world view		

		setGPSPosition();
		itemizedOverlay.onTap(0);
	}
	public void setGPSPosition()
    {
    	mapController.animateTo(mPoint);
		mapController.setCenter(mPoint);	
		OverlayItem overlayitem = new OverlayItem(mPoint, nameMarket, "");
		itemizedOverlay.addOverlay(overlayitem);
		mapOverlays.add(itemizedOverlay);		
    }
	@Override
	protected boolean isRouteDisplayed() {
		// TODO Auto-generated method stub
		return false;
	}
}

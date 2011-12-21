package cse.it;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONArray;

import android.app.ProgressDialog;
import android.content.Context;
import android.graphics.drawable.Drawable;
import android.location.Address;
import android.location.Criteria;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.maps.GeoPoint;
import com.google.android.maps.MapActivity;
import com.google.android.maps.MapController;
import com.google.android.maps.MapView;
import com.google.android.maps.Overlay;
import com.google.android.maps.OverlayItem;

import cse.it.map.MyItemizedOverlay;
import cse.it.parse.Downloader;
import cse.it.parse.HttpParse;
import cse.it.parse.ParsedData;
import cse.it.parse.RestClient;


public class Vitri extends MapActivity{
	protected JSONArray resp = null;
	public double fromLat = 50.619278, fromLon = 3.041678;
	private static Location currentLocation;
	protected ProgressDialog dialog;
	
	MapView mapView;
	GeoPoint mPoint;
	MapController mapController;
	List<Overlay> mapOverlays;
	Drawable drawable;
	MyItemizedOverlay itemizedOverlay;
	int pos;
	String stringAddress = "your position";
	Context mContext;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		
		mContext = this;
		fromLat = getCurrentPissition(this).getLatitude();
		fromLon = getCurrentPissition(this).getLongitude();
		if (fromLat == (double)0 | fromLon == (double)0){
			fromLat = 10808689;
			fromLon = 106710033;
		}
		if (Def.isConnectionAvailable(this)) {
			dialog = ProgressDialog.show(Vitri.this, "",
					"Loading", true);
//			updateView();
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
					ArrayList<NameValuePair> params = new ArrayList<NameValuePair>();
					params.add(new BasicNameValuePair("lng", Double.toString(fromLon)));
					params.add(new BasicNameValuePair("lat", Double.toString(fromLat)));
					
					resp = RestClient.connect(Def.URL_VITRI, params);
					Log.d("JSON", "return "+resp);
					uiThreadCallback.post(runInUIThread);		
					
				}
			}.start();
			
		}
		else{
			ToastUtil.show(this, "No interner");
		}
		//getAddressFromLatLgn(fromLat, fromLon);
		
		
		Log.d("asdas", "a"+fromLat + "ff"+fromLon);
	}
	public void getResponse() {
		JSONArray statusCode = null;
		// dismiss the progress dialog when received the response
		if (dialog != null)
			dialog.dismiss();
		
		if(resp != null){
			HttpParse parse = new HttpParse();
			
			parse.HttpParsePosArrary(resp);
			Log.d("aaa", "resp "+ParsedData.myPos.get(0).ten_tram);
		}
		updateView();
	}

	private void updateView (){
		setContentView(R.layout.vi_tri);		
		
		Log.d("check", "aaa"+ParsedData.myPos.size());
		
		ImageView imageTrip = (ImageView) findViewById(R.id.IconList);
		imageTrip.setBackgroundResource(R.drawable.iconcurrenttrip);
		((TextView) findViewById(R.id.TextList2)).setText(getString(R.string.ten_tram)+" "+(ParsedData.myPos.get(0)).ten_tram);
		((TextView) findViewById(R.id.Textlist3)).setText(getString(R.string.nam_tren_duong)+" "+(ParsedData.myPos.get(0)).ten_duong);
		((TextView) findViewById(R.id.Textlist4)).setText(getString(R.string.tuyen_qua)+" "+(ParsedData.myPos.get(0)).tuyen_di_qua);
		
		mapView = (MapView) findViewById(R.id.mapview);
		mapView.setBuiltInZoomControls(true);
		
		mapOverlays = mapView.getOverlays();
        drawable = this.getResources().getDrawable(R.drawable.pinmarker1);

        int fromLatitude = (int)(fromLat * 1E6);
		int fromLongitude = (int)(fromLon * 1E6);
		mPoint = new GeoPoint(fromLatitude,fromLongitude);
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
		OverlayItem overlayitem = new OverlayItem(mPoint, getString(R.string.vitri_temp), "");
		itemizedOverlay.addOverlay(overlayitem);
		mapOverlays.add(itemizedOverlay);		
    }
    public Location getCurrentPissition(Context context){
    	LocationManager locationManager;
    	Criteria criteria = new Criteria();
    	String bestProvider = "";
    	
		locationManager = (LocationManager)context.getSystemService(Context.LOCATION_SERVICE);
		locationManager.getAllProviders();
		bestProvider = locationManager.getBestProvider(criteria, false);
		if(!locationManager.isProviderEnabled(bestProvider)){
    		bestProvider = LocationManager.GPS_PROVIDER;    		
    	}
		Log.i( "ZZZ", "bestProvider = " + bestProvider);
		locationManager.requestLocationUpdates(bestProvider, 0, 0, locationListener);
		currentLocation = locationManager.getLastKnownLocation(bestProvider);
		if (currentLocation == null)
		{
			// sometimes, the location was not initialed will cause problems, dump_provider is nothing
			currentLocation = new Location("dump_provider");
		}
		// Always beware that listening for a long time consumes a lot of battery power, 
		// so as soon as you have the information you need, you should stop listening
		locationManager.removeUpdates(locationListener);    	
		
    	return currentLocation;
	}
    private static final LocationListener locationListener = new LocationListener() {
        public void onLocationChanged(Location location) {
        	currentLocation = location; 
        }

        public void onProviderDisabled(String provider) {
        }

        public void onProviderEnabled(String provider) {
        }

        public void onStatusChanged(String provider, int status, Bundle extras) {
        }
    };
	@Override
	protected boolean isRouteDisplayed() {
		// TODO Auto-generated method stub
		return false;
	}
	
	public void getAddressFromLatLgn(double lat, double log){
		Locale loc = new Locale("en");
		List<Address> result = new ArrayList<Address>();
	  Geocoder geocoder = new Geocoder(this, loc);
	  try {
		result = geocoder.getFromLocation(lat, log, 1);
		if (result != null) {
			stringAddress = result.get(0) +" "+result.get(0).getThoroughfare();
		}
		
	} catch (IOException e) {
		// TODO Auto-generated catch block
		e.printStackTrace();
	}
	 
	}
	
}

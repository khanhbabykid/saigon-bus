package cse.it;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class Def {

	public static String URL_TUYEN_XE = "http://saigonbus.giupviecnha24h.vn/Mobile/BusRouteList_2";
	public static String URL_TRAM_XE = "http://saigonbus.giupviecnha24h.vn/Mobile/StationList";
	public static String URL_VITRI = "http://saigonbus.giupviecnha24h.vn/Mobile/UserPosition";
	public static String URL_TIM_DUONG = "http://saigonbus.giupviecnha24h.vn/Mobile/UserPosition";
	
	public static boolean isConnectionAvailable(Context context)
    {
    	ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
    	NetworkInfo lNetInfo = cm.getActiveNetworkInfo();
        if ( lNetInfo != null )
        {
        	if ( lNetInfo.isConnected() ) 
        	{
        		return true;
        	}
        }
    	return false;
    }
}

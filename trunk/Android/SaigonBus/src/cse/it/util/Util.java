package cse.it.util;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutput;
import java.io.ObjectOutputStream;
import java.util.regex.Matcher;
import java.util.regex.Pattern;


import android.content.Context;
import android.location.LocationManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Environment;

public class Util {

    private static String TAG = "Util";
    public static File getCacheDir(){
        File sdcard = getSdcard();
        if (sdcard == null) {
            return null;
        }
        
        File cacheFolder = new File(sdcard, Def.CACHE_FOLDER_NAME);
        if (!cacheFolder.exists()) {
            if(!cacheFolder.mkdir()){
                return null;
            }
        }

        return cacheFolder;
    }
    
    public static File getSdcard(){
        return Environment.getExternalStorageDirectory();
    }
    

	 public static boolean isConnectionAvailable(Context context)
    {
		boolean isNetWork = false;
    	ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
    	NetworkInfo network_wifi = cm.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
    	NetworkInfo network_mobile = cm.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
    	if( network_wifi.isAvailable() ){
    		isNetWork = true;

        }
        return isNetWork;
    }

    public static byte[] serializeObject(Object o) {
        ByteArrayOutputStream bos = new ByteArrayOutputStream();

        try {
            ObjectOutput out = new ObjectOutputStream(bos);
            out.writeObject(o);
            out.close();

            // Get the bytes of the serialized object
            byte[] buf = bos.toByteArray();

            return buf;
        } catch (IOException ioe) {

            return null;
        }
    }

    public static Object deserializeObject(byte[] b) {
        try {
            ObjectInputStream in = new ObjectInputStream(
                    new ByteArrayInputStream(b));

            Object object = in.readObject();
            CLog.e(TAG, "check " + in + "ob " + object);
            in.close();

            return object;
        } catch (ClassNotFoundException cnfe) {

            return null;
        } catch (IOException ioe) {

            return null;
        }
    }


    /**
     * Detect if location device is available or not
     * 
     * @param aContext
     * @return
     */
    public static boolean isLocationDeviceAvailable(Context aContext) {
        boolean locationEnable = false;

        try {
            final LocationManager manager = (LocationManager) aContext
                    .getSystemService(Context.LOCATION_SERVICE);

            if (manager.isProviderEnabled(LocationManager.GPS_PROVIDER)
                    || manager
                            .isProviderEnabled(LocationManager.NETWORK_PROVIDER)) {
                locationEnable = true;
            }
        } catch (SecurityException e) {
            CLog.e(TAG, e.toString());
        }

        return locationEnable;
    }

	 public static boolean isCheckEmail(String email){
			Pattern p = Pattern.compile(
					"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$",
					Pattern.CASE_INSENSITIVE);
			Matcher m = p.matcher(email);
			if (!m.matches()) {
				return false;
			}
			else
				return true;
		}
	  

}

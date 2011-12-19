package cse.it.parse;

import java.io.IOException;

import org.apache.http.HttpResponse;
import org.apache.http.ParseException;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import cse.it.Def;

import android.content.Context;
import android.util.Log;

public class Downloader {

	public static void getData(){
		DefaultHttpClient client = new DefaultHttpClient();
    	HttpGet get = new HttpGet(Def.URL_TUYEN_XE);
    	// Execute the GET call and obtain the response
    	HttpResponse getResponse = null;
		try {
			getResponse = client.execute(get);
			Log.e("HTTPS", "try" + getResponse);
		} catch (ClientProtocolException e) {	
			Log.e("HTTPS", "ClientProtocolException" + e);
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			Log.e("HTTPS", "IOException" + e);
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		String responseString = null;
		try {
			responseString = EntityUtils.toString(getResponse.getEntity());
			Log.d("Temp", "resp" +responseString);
		} catch (ParseException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		JSONObject responseJSON;
		JSONArray jsonArray;
		try {
			//responseJSON = new JSONObject(responseString);
			jsonArray = new JSONArray(responseString);
			HttpParse parse = new HttpParse();
			parse.HttpParseArrary(jsonArray);
			Log.e("HTTPS", "data"+jsonArray);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}	
	}
}

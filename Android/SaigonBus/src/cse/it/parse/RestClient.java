/**
 * 
 */
package cse.it.parse;

import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.ArrayList;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;

/**
 * @author Tuan Phan
 * mail : minh-tuan.phan@playsoft.fr
 */
public class RestClient {
	static private int TIMEOUT_CONNECTION = 120000; // 2 minutes
	static private int TIMEOUT_SOCKET = 60000; // 1 minutes
    /* 
     * To connect to a specified WS and pass a list of parameters
     * 
     */
    public static JSONArray connect(String url, ArrayList<NameValuePair> params)
    {
    	HttpGet httpGet;
    	//prepare the params to appen it with url
    	if(params != null)
    	{
	    	String combinedParams = "";
	        if(!params.isEmpty()){
	            combinedParams += "?";
	            for(NameValuePair p : params)
	            {
	                String paramString = null;
					try {
						paramString = p.getName() + "=" + URLEncoder.encode(p.getValue(),"utf-8");
					} catch (UnsupportedEncodingException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
	                if(combinedParams.length() > 1)
	                {
	                    combinedParams  +=  "&" + paramString;
	                }
	                else
	                {
	                    combinedParams += paramString;
	                }
	            }
	        }
	        Log.d("RestClient:url=", url+combinedParams);
	        httpGet = new HttpGet(url+combinedParams);
    	}
    	else
    		httpGet = new HttpGet(url);

        HttpParams httpParameters = new BasicHttpParams();
        // Set the timeout in milliseconds until a connection is established.
        int timeoutConnection = TIMEOUT_CONNECTION;
        HttpConnectionParams.setConnectionTimeout(httpParameters, timeoutConnection);
        // Set the default socket timeout (SO_TIMEOUT) 
        // in milliseconds which is the timeout for waiting for data.
        int timeoutSocket = TIMEOUT_SOCKET;
        HttpConnectionParams.setSoTimeout(httpParameters, timeoutSocket);

        DefaultHttpClient httpClient = new DefaultHttpClient(httpParameters);
        try {
			HttpResponse response = httpClient.execute(httpGet);
			
            if (response != null) {
            	// A Simple JSONObject Creation
            	HttpEntity responseEntity = response.getEntity();
            	
            	String test = EntityUtils.toString(responseEntity, "utf-8");
//            	byte[] arrByteForSpanish = testTemp.getBytes("ISO-8859-1");  
//                String test = new String(arrByteForSpanish);  
                JSONArray json = null;
				try {
					json = new JSONArray(test);
	                return json;
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

            }
            return null;				
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return null;
    }
    
    public static JSONObject connectJson(String url, ArrayList<NameValuePair> params)
    {
    	HttpGet httpGet;
    	//prepare the params to appen it with url
    	if(params != null)
    	{
	    	String combinedParams = "";
	        if(!params.isEmpty()){
	            combinedParams += "?";
	            for(NameValuePair p : params)
	            {
	                String paramString = null;
					try {
						paramString = p.getName() + "=" + URLEncoder.encode(p.getValue(),"utf-8");
					} catch (UnsupportedEncodingException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
	                if(combinedParams.length() > 1)
	                {
	                    combinedParams  +=  "&" + paramString;
	                }
	                else
	                {
	                    combinedParams += paramString;
	                }
	            }
	        }
	        Log.d("RestClient:url=", url+combinedParams);
	        httpGet = new HttpGet(url+combinedParams);
    	}
    	else
    		httpGet = new HttpGet(url);

        HttpParams httpParameters = new BasicHttpParams();
        // Set the timeout in milliseconds until a connection is established.
        int timeoutConnection = TIMEOUT_CONNECTION;
        HttpConnectionParams.setConnectionTimeout(httpParameters, timeoutConnection);
        // Set the default socket timeout (SO_TIMEOUT) 
        // in milliseconds which is the timeout for waiting for data.
        int timeoutSocket = TIMEOUT_SOCKET;
        HttpConnectionParams.setSoTimeout(httpParameters, timeoutSocket);

        DefaultHttpClient httpClient = new DefaultHttpClient(httpParameters);
        try {
			HttpResponse response = httpClient.execute(httpGet);
            if (response != null) {
            	// A Simple JSONObject Creation
            	HttpEntity responseEntity = response.getEntity();
            	String test = EntityUtils.toString(responseEntity, "utf-8");
//            	byte[] arrByteForSpanish = testTemp.getBytes("ISO-8859-1");  
//                String test = new String(arrByteForSpanish);  
                JSONObject json = null;
				try {
					json = new JSONObject(test);
	                return json;
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

            }
            return null;				
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return null;
    }
    /* 
     * To connect to a specified WS and pass a list of parameters
     * 
     */
    public static InputStream connect(String url)
    {   
    	Log.d("RestClient:url=", url);
        HttpGet httpGet = new HttpGet(url);
        HttpParams httpParameters = new BasicHttpParams();
        // Set the timeout in milliseconds until a connection is established.
        int timeoutConnection = TIMEOUT_CONNECTION;
        HttpConnectionParams.setConnectionTimeout(httpParameters, timeoutConnection);
        // Set the default socket timeout (SO_TIMEOUT) 
        // in milliseconds which is the timeout for waiting for data.
        int timeoutSocket = TIMEOUT_SOCKET;
        HttpConnectionParams.setSoTimeout(httpParameters, timeoutSocket);

        DefaultHttpClient httpClient = new DefaultHttpClient(httpParameters);
        try {
			HttpResponse response = httpClient.execute(httpGet);
            if (response != null) {
            	HttpEntity responseEntity = response.getEntity();
            	InputStream instream = responseEntity.getContent();
            	return instream;
            }				
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return null;
    }     
}

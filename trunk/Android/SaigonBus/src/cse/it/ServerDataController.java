package cse.it;


import org.json.JSONException;
import org.json.JSONObject;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Handler;
import android.os.Message;

public class ServerDataController extends AsyncTask<Void, Void, Object> {
	
	private Context context;
	private Handler handler;
	private WebserviceMess reqMessage;
	private Boolean showProgressBar;
	private String progressTitle;
	private String progressContent;
	private ProgressDialog progress = null;
	
	public ServerDataController(Context context, Handler handler, WebserviceMess reqMessage){
		this.context = context;
		this.handler = handler;
		this.reqMessage = reqMessage;
		this.showProgressBar = false;
	}
	
	public void setProgressBar(String progressTile, String progressContent){
		showProgressBar = true;
		this.progressTitle = progressTile;
		this.progressContent = progressContent;
	}
	
	@Override
	protected void onPreExecute() {
		if (showProgressBar) {
			// Show progress bar
			progress = new ProgressDialog(context);
			progress.setTitle(progressTitle);
			progress.setMessage(progressContent);
			progress.show();
		}
		
		// TODO Auto-generated method stub
		super.onPreExecute();
	}
		
	@Override
	protected Object doInBackground(Void... arg0) {
		WebserviceMess resMessage = null;
		
		switch (reqMessage.getMessageId()) {
		case DefMessageID.WS_LOGIN_REQ:
			resMessage = doLoginReq();
			break;
		default:
			break;
		}

		
		// TODO Auto-generated method stub
		return resMessage;
	}
	
	@Override
	protected void onPostExecute(Object result) {
		// TODO Auto-generated method stub
		super.onPostExecute(result);
		
		Message lResponseMess = handler.obtainMessage(reqMessage.getMessageId());
		lResponseMess.obj = result;
		lResponseMess.sendToTarget();
		
		if (progress != null) {
			progress.dismiss();
		}
	}

	private WebserviceMess doLoginReq() {
		WebserviceMess resMessage = new WebserviceMess();
		
		resMessage.setMessageId(DefMessageID.WS_LOGIN_RES);
		
				
		//HttpParse.sendHttpPost(Config.getValue(Def.URL)+Def.LOGIN_PARAMETER, req.getUserName(), req.getPass(),jsonObjSend, context,resMessage);
		// TODO Auto-generated method stub
		return resMessage;
	}

	
}

package cse.it;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.view.WindowManager;
import cse.it.data.BusStation;
import cse.it.parse.Downloader;
import cse.it.parse.ParsedData;

public class SplashActivity extends Activity{

	private String TAG = "SplashActivity";
	private final int SPLASH_DISPLAY_LENGHT = 2000;
	private Context mContext;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
		            WindowManager.LayoutParams.FLAG_FULLSCREEN);
		mContext = this;
		this.setContentView(R.layout.splash);
		
		
		
		new Handler().postDelayed(new Runnable() {

			public void run() {
				Intent intent = new Intent(SplashActivity.this, SaigonBus.class);
				startActivity(intent);
				
				finish();
			}
			
		}, SPLASH_DISPLAY_LENGHT);
	}
}

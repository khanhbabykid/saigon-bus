package cse.it;

import java.util.ArrayList;

import cse.it.data.BusStation;
import cse.it.data.TuyenXe;
import cse.it.parse.Downloader;
import cse.it.parse.HttpParse;
import cse.it.parse.ParsedData;

import android.app.Activity;
import android.app.TabActivity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ImageView;
import android.widget.TabHost;
import android.widget.TextView;
import android.widget.TabHost.OnTabChangeListener;
import android.widget.TabHost.TabSpec;

public class SaigonBus extends TabActivity implements OnTabChangeListener {

	TabHost tabHost = null;
	static boolean isFirstTime = false;
	static boolean isKeepTrackTabStatus = false;
	int selectedTabIdx = 0;
	public View tabviewHome, tabviewTrip, tabviewNetwork, tabviewTools, tabviewProfiles;

	public static SaigonBus homeScreenInstance;
	

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		/* TabHost will have Tabs */

		//requestWindowFeature(Window.FEATURE_CUSTOM_TITLE);
		setContentView(R.layout.homescreen);		

		homeScreenInstance = this;
		
		ParsedData temp = new ParsedData();
		if (Def.isConnectionAvailable(this)) {
			Downloader.getData();
		}
		else{
			ToastUtil.show(this, "No interner");
		}
		
//		Downloader.getDataStationList();
		tabHost = getTabHost();

		tabHost.getTabWidget().setDividerDrawable(R.drawable.tab_divider);
		tabHost.setOnTabChangedListener(this);
		selectedTabIdx = 0;
		
			isKeepTrackTabStatus = false;
			tabHost.setCurrentTab(selectedTabIdx);
			addNormalTab();

		// setTabColor(tabHost);
		
	}

	public void addNormalTab() {
		addTabHome();
		addTabTrip();
		addTabTool();
		addTabProfile();
		tabHost.setCurrentTab(selectedTabIdx);
	}

	

	public void clearTabs() {
		tabHost.setCurrentTab(0);
		tabHost.clearAllTabs();
	}

	public void addTabHome() {

		tabviewHome = createTabView(tabHost.getContext(), getString(R.string.home),
				R.drawable.home_tab);
		TabSpec tabTrip = tabHost.newTabSpec(getString(R.string.home)).setIndicator(tabviewHome)
				.setContent(new Intent(this, StationBus.class));
		tabHost.addTab(tabTrip);
	}

	public void addTabTrip() {
		
		tabviewTrip = createTabView(tabHost.getContext(), getString(R.string.trips),
				R.drawable.tripsselected_tab);
		TabSpec tabTrip = tabHost.newTabSpec(getString(R.string.trips)).setIndicator(tabviewTrip)
				.setContent(new Intent(this, TramXe.class));
		tabHost.addTab(tabTrip);
	}

	
	public void addTabTool() {
		tabviewTools = createTabView(tabHost.getContext(), getString(R.string.tools),
				R.drawable.toolsselected_tab);
		TabSpec tabTrip = tabHost.newTabSpec(getString(R.string.tools)).setIndicator(tabviewTools)
				.setContent(new Intent(this, Vitri.class));
		tabHost.addTab(tabTrip);
	}

	public void addTabProfile() {
		tabviewProfiles = createTabView(tabHost.getContext(), getString(R.string.profile),
				R.drawable.profile_tab);

		TabSpec tabTrip = tabHost.newTabSpec(getString(R.string.profile)).setIndicator(tabviewProfiles)
				.setContent(new Intent(this, TimDuong.class));
		tabHost.addTab(tabTrip);
	}

	private static View createTabView(final Context context, final String text,
			final int resId) {
		View view = LayoutInflater.from(context)
				.inflate(R.layout.tabs_bg, null);
		TextView tv = (TextView) view.findViewById(R.id.tabsText);
		tv.setText(text);
		ImageView image = (ImageView) view.findViewById(R.id.idImageTab);
		image.setImageResource(resId);
		return view;
	}

	public void onTabChanged(String tabId) {
		if (isKeepTrackTabStatus == true) // Keep track previous index of tab before the
									// downloading is finished.
		{
			if (tabId.equals("Home"))
				selectedTabIdx = 0;
			else if (tabId.equals("Trips"))
				selectedTabIdx = 1;
			else if (tabId.equals("NetWork"))
				selectedTabIdx = 2;
			else if (tabId.equals("Tools"))
				selectedTabIdx = 3;
			else if (tabId.equals("Profile"))
				selectedTabIdx = 4;
		}
	}	
}
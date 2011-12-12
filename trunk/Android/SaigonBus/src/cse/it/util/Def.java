package cse.it.util;

public interface Def {
	public static final int RST_CODE_OK = 1;
	public static final int RST_CODE_FAIL = 2;
	public static final int RST_CODE_CANCEL = 3;
	
	//Error code
	public static final int NETWORK_CODE_FAIL = 199;
	public static final int NETWORK_CODE_OK = 200;
	public static final int CREATED = 201;
	public static final int ACCEPTED = 202;
	public static final int NO_CONTENT = 204;
	
	public static final int BAD_REQUEST = 400;
	public static final int UNAUTHORIZED = 401;
	public static final int FORBIDDEN = 403;
	public static final int NOT_FOUND = 404;
	public static final int METHOD_NOT_ALLOWED = 405;
	public static final int CONFLICT = 406;
	
	public static final int INTERNAL_SERVER_ERROR = 500;
	public static final int NOT_IMPLEMENTED = 501;
	public static final int BAD_GETWAY = 502;
	public static final int SERVICE_UNAVAILABLE = 503;
	public static final int GETWAY_TIMEOUT = 504;
	
	public static final int ERR_NETWORK_01 = 0;
	public static final int ERR_SERVER_01 = 1;
	public static final int ERR_REGISTER_01 = 2;
	public static final int ERR_LOGIN_PASSWORD_01 = 3;
	public static final int ERR_PASSWORD_CHANGED = 4;
	
	/** Enables all android.util.Logs from CCLog.java **/
	public static final boolean ENABLE_LOG = true;		//should be: false
    public static final String CACHE_FOLDER_NAME = ".nissan.cache";
	
	//link login user
	public String URL_LOGIN = "https://mobileapps.integration.nissan.eu/youPlusApps/1.0.0-SNAPSHOT/API/loginUser";
	
	public static String HOME_ACTION_MYPROFILE = "com.digitas.nissaneu.mobile.yp.android.ui.my_profile.MyProfileActivity";
	public static String HOME_ACTION_APP_LIBRARY = "com.digitas.nissaneu.mobile.yp.android.ui.app_library.AppLibrariActivity";
	public static String HOME_ACTION_ASSISTANCE = "com.digitas.nissaneu.mobile.yp.android.ui.assistance.AssistanceActivity";
	public static String HOME_ACTION_FIND_DEALER = "com.digitas.nissaneu.mobile.yp.android.ui.find_dealer.FindDealerActivity";
	public static String HOME_ACTION_MAINTENANCE = "com.digitas.nissaneu.mobile.yp.android.ui.maintenace.MaintenanceActivity";
    public static String HOME_ACTION_NISSAN_RANGE = "com.digitas.nissaneu.mobile.yp.android.ui.nissan_range.NissanRangeActivity";
    public static String HOME_ACTION_USER_GUIDE = "com.digitas.nissaneu.mobile.yp.android.ui.user_guide.UserGuideActivity";
}

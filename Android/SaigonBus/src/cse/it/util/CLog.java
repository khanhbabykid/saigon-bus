package cse.it.util;

import android.util.Log;


/**
 * Log extension - with easy way to switch all Logs on and off.
 * 
 * @author Olgierd Uzieblo
 */
@SuppressWarnings("unused")
public class CLog implements Def
{
	public static int d(String tag, String msg)
	{
		if ( ENABLE_LOG )
		{
			return Log.d(tag, msg);
		}
		
		return 0;
	}
	
	public static int d(String tag, String msg, Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.d(tag, msg, tr);
		}
		
		return 0;
	}

	public static int e(String tag, String msg)
	{
		if ( ENABLE_LOG )
		{
			return Log.e(tag, msg);
		}
		
		return 0;
	}
	
	public static int e(String tag, String msg, Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.e(tag, msg, tr);
		}
		
		return 0;
	}
	
	public static String getStackTraceString(Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.getStackTraceString(tr);
		}
		
		return new String("");
	}
	
	public static int i(String tag, String msg)
	{
		if ( ENABLE_LOG )
		{
			return Log.i(tag, msg);
		}
		
		return 0;
	}
	
	public static int i(String tag, String msg, Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.i(tag, msg, tr);
		}
		
		return 0;
	}
	
	public static boolean isLoggable(String tag, int level)
	{
		if ( ENABLE_LOG )
		{
			return Log.isLoggable(tag, level);
		}
		
		return false;
	}
	
	public static int println(int priority, String tag, String msg)
	{
		if ( ENABLE_LOG )
		{
			return Log.println(priority, tag, msg);
		}
		
		return 0;
	}
	
	public static int v(String tag, String msg)
	{
		if ( ENABLE_LOG )
		{
			return Log.v(tag, msg);
		}
		
		return 0;
	}
	
	public static int v(String tag, String msg, Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.v(tag, msg, tr);
		}
		
		return 0;
	}
	
	public static int w(String tag, Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.w(tag, tr);
		}
		
		return 0;
	}
	
	public static int w(String tag, String msg, Throwable tr)
	{
		if ( ENABLE_LOG )
		{
			return Log.w(tag, msg, tr);
		}
		
		return 0;
	}

	public static int w(String tag, String msg)
	{
		if ( ENABLE_LOG )
		{
			return Log.w(tag, msg);
		}
		
		return 0;
	}
}

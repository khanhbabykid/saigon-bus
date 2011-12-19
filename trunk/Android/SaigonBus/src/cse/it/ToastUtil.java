package cse.it;

import android.app.AlertDialog;
import android.content.Context;


/**
 * Show dialogs utilities.
 * @author hieu.le
 */
public final class ToastUtil {

    public static void showByErrorCode(final Context context,
            final int errorCode) {
        final AlertDialog.Builder ad = new AlertDialog.Builder(context);

        ad.setMessage(errorCode);
        showDetailMess(errorCode, ad);
    }

   
    public static void show(final Context aContext, final String mess) {
        final AlertDialog.Builder ad = new AlertDialog.Builder(aContext);

        ad.setMessage(mess);
        showDetailMess(0, ad);
    }

    /**
     * @param aiResponseMssId
     *            response message id
     * @param ad
     *            dialog builder
     */
    private static void showDetailMess(final int aiResponseMssId,
            final AlertDialog.Builder ad) {
        ad.setPositiveButton(android.R.string.ok, null);
        ad.setIcon(android.R.drawable.ic_dialog_alert);
        // switch (aiResponseMssId) {
        // case 0:
        // // Do nothing
        // break;
        // case R.string.error_network_unavailable:
        // ad.setTitle(R.string.error_title);
        // break;
        // case R.string.inform_no_direction:
        // ad.setTitle(R.string.error_title);
        // break;
        //
        // case R.string.inform_favorite_no_result:
        // case R.string.inform_location_not_exist:
        // ad.setTitle(R.string.inform_title);
        // ad.setIcon(android.R.drawable.ic_dialog_info);
        // break;
        //
        // default:
        // break;
        // }

        ad.create();
        ad.show();
    }

    /**
     * Get error code for network error.
     * @param errorCode
     *            Network error code
     * @return Application error code
     */
    
}

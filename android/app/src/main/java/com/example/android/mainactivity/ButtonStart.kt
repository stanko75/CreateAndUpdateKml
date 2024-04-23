package com.example.android.mainactivity

import android.app.Activity
import android.content.*
import android.content.pm.PackageManager
import android.os.Build
import android.view.View
import android.view.inputmethod.InputMethodManager
import android.widget.TextView
import androidx.annotation.RequiresApi
import com.example.android.foregroundservice.*
import android.content.ComponentName
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.example.android.ForegroundServiceBroadcastReceiver
import com.example.android.R

class ButtonStart(private val activity: Activity, private val context: Context, private val broadCastReceiver: ForegroundServiceBroadcastReceiver) {
    @RequiresApi(Build.VERSION_CODES.O)
    fun onClick(view: View) {

        LocalBroadcastManager.getInstance(context)
            .registerReceiver(broadCastReceiver, IntentFilter(IntentAction.NUM_OF_TICKS))
        LocalBroadcastManager.getInstance(context)
            .registerReceiver(broadCastReceiver, IntentFilter(IntentAction.RETROFIT_ON_RESPONSE))

        val log: TextView =
            activity.findViewById<View>(R.id.editTextTextMultiLineLog) as TextView
        log.text = ""

        val inputMethodManager =
            context.getSystemService(Activity.INPUT_METHOD_SERVICE) as InputMethodManager
        inputMethodManager.hideSoftInputFromWindow(view.windowToken, 0)

        val component = ComponentName(context, BroadcastTickReceiver::class.java)
        activity.packageManager.setComponentEnabledSetting(
            component,
            PackageManager.COMPONENT_ENABLED_STATE_ENABLED,
            PackageManager.DONT_KILL_APP
        )

        val numberOfTicks: TextView =
            activity.findViewById<View>(R.id.textViewNumberOfTicks) as TextView
        numberOfTicks.text = "0"

        val numOfSecondsForTick: TextView =
            activity.findViewById<View>(R.id.txtRequestUpdates) as TextView
        var strNumOfSecondsForTick: String = numOfSecondsForTick.text.toString()
        if (strNumOfSecondsForTick.isEmpty()) strNumOfSecondsForTick = "30"

        val intentStartForegroundTickService =
            Intent(context, ForegroundTickService::class.java)
        intentStartForegroundTickService.action = IntentAction.START_FOREGROUND_TICK_SERVICE
        intentStartForegroundTickService.putExtra(
            IntentExtras.NUM_OF_SECONDS_FOR_TICK,
            strNumOfSecondsForTick.toLong()
        )

        val sharedPreferences = context.getSharedPreferences("settings", Context.MODE_PRIVATE)
        val fileName = sharedPreferences.getString("fileName", "default")
        val folderName = sharedPreferences.getString("folderName", "default")

        intentStartForegroundTickService.putExtra(
            IntentExtras.KML_FILE_NAME,
            fileName
        )

        intentStartForegroundTickService.putExtra(
            IntentExtras.FOLDER_NAME,
            folderName
        )

        activity.startForegroundService(intentStartForegroundTickService)
    }
}
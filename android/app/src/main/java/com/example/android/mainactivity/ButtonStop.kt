package com.example.android.mainactivity

import android.app.Activity
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.content.pm.PackageManager
import android.os.Build
import androidx.annotation.RequiresApi
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.example.android.ForegroundServiceBroadcastReceiver
import com.example.android.foregroundservice.BroadcastTickReceiver
import com.example.android.foregroundservice.ForegroundTickService
import com.example.android.foregroundservice.IntentAction

class ButtonStop(private val activity: Activity, private val context: Context, private val broadCastReceiver: ForegroundServiceBroadcastReceiver) {

    @RequiresApi(Build.VERSION_CODES.O)
    fun onClick() {
        val component = ComponentName(context, BroadcastTickReceiver::class.java)
        context.packageManager.setComponentEnabledSetting(
            component,
            PackageManager.COMPONENT_ENABLED_STATE_DISABLED,
            PackageManager.DONT_KILL_APP
        )

        LocalBroadcastManager.getInstance(context).unregisterReceiver(broadCastReceiver)

        val intentStopForegroundTickService = Intent(context, ForegroundTickService::class.java)
        intentStopForegroundTickService.action = IntentAction.STOP_FOREGROUND_TICK_SERVICE
        activity.startForegroundService(intentStopForegroundTickService)
    }
}
package com.example.android.foregroundservice

import android.content.Context
import android.content.Intent
import android.os.Build
import androidx.annotation.RequiresApi
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.example.android.mainactivity.MainActivity

class ReceiveListener: IReceiveListener {
    override fun startForegroundTickService(context: Context) {
        val startBroadcastTickReceiverIntent =
            Intent(context, MainActivity::class.java).setAction(IntentAction.MAIN_ACTIVITY_RECEIVER)
        LocalBroadcastManager.getInstance(context)
            .sendBroadcast(startBroadcastTickReceiverIntent)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun restartForegroundTickService(context: Context) {
        val restartBroadcastTickReceiverIntent =
            Intent(context, ForegroundTickService::class.java)
        context.startForegroundService(restartBroadcastTickReceiverIntent)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun stopForegroundTickService(context: Context) {
        val stopForegroundTickServiceIntent = Intent(
            context,
            ForegroundTickService::class.java
        ).setAction(IntentAction.STOP_FOREGROUND_TICK_SERVICE)
        context.startForegroundService(stopForegroundTickServiceIntent)
    }

    override fun numOfTicks(context: Context, intent: Intent) {
        val mainActivityIntent = Intent(context, MainActivity::class.java).setAction(
            IntentAction.NUM_OF_TICKS
        )
        val numOfTicks = intent.getIntExtra (IntentExtras.NUM_OF_TICKS, 30)
        mainActivityIntent.putExtra(IntentExtras.NUM_OF_TICKS, numOfTicks)
        LocalBroadcastManager.getInstance(context).sendBroadcast(mainActivityIntent)
    }

    override fun retrofitOnResponse(context: Context, intent: Intent) {
        val mainActivityIntent = Intent(context, MainActivity::class.java).setAction(
            IntentAction.RETROFIT_ON_RESPONSE
        )
        val retrofitOnResponse = intent.getStringExtra (IntentExtras.RETROFIT_ON_RESPONSE)
        mainActivityIntent.putExtra (IntentExtras.RETROFIT_ON_RESPONSE, retrofitOnResponse)
        LocalBroadcastManager.getInstance(context).sendBroadcast(mainActivityIntent)
    }

    override fun fileName(context: Context, intent: Intent) {
        val mainActivityIntent = Intent(context, MainActivity::class.java).setAction(
            IntentAction.FILE_NAME
        )
        val fileName = intent.getStringExtra(IntentExtras.FILE_NAME)
        mainActivityIntent.putExtra(IntentExtras.FILE_NAME, fileName)
        LocalBroadcastManager.getInstance(context).sendBroadcast(mainActivityIntent)
    }
}
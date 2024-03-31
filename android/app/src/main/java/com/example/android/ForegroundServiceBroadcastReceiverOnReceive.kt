package com.example.android

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.view.View
import android.widget.TextView
import com.example.android.foregroundservice.IntentAction
import com.example.android.foregroundservice.IntentExtras
import com.example.android.logger.ActivityLogger
import com.example.android.logger.LoggerExtensions

class ForegroundServiceBroadcastReceiverOnReceive(private val activity: Activity): IForegroundServiceBroadcastReceiverOnReceive {
    override fun onReceive(context: Context, intent: Intent) {
        when (intent.action) {
            IntentAction.NUM_OF_TICKS -> {
                val numOfTicks = intent.getIntExtra(IntentExtras.NUM_OF_TICKS, 30)
                onNumOfTicksReceived(numOfTicks)
            }
            IntentAction.RETROFIT_ON_RESPONSE -> {
                val retrofitMessage = intent.getStringExtra(IntentExtras.RETROFIT_ON_RESPONSE)
                onRetrofitResponseReceived(retrofitMessage, context)
            }
            else -> {
                onUnknownActionReceived(context)
            }
        }
    }

    override fun onNumOfTicksReceived(numOfTicks: Int) {
        val numberOfTicks: TextView =
            activity.findViewById<View>(R.id.textViewNumberOfTicks) as TextView
        numberOfTicks.text = numOfTicks.toString()
    }

    override fun onRetrofitResponseReceived(retrofitMessage: String?, context: Context) {
        if (retrofitMessage != null) {
            LoggerExtensions().log(ActivityLogger(activity), retrofitMessage)
        }
    }

    override fun onUnknownActionReceived(context: Context) {
        LoggerExtensions().log(ActivityLogger(activity), "else")
    }
}
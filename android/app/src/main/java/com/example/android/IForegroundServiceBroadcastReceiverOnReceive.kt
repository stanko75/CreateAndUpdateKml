package com.example.android

import android.content.Context
import android.content.Intent

interface IForegroundServiceBroadcastReceiverOnReceive {
    fun onReceive(context: Context, intent: Intent)
    fun onNumOfTicksReceived(numOfTicks: Int)
    fun onRetrofitResponseReceived(retrofitMessage: String?, context: Context)
    fun onUnknownActionReceived(context: Context)
}
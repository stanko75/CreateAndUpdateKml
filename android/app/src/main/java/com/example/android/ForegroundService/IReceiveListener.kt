package com.example.android.foregroundservice

import android.content.Context
import android.content.Intent

interface IReceiveListener {
    fun startForegroundTickService(context: Context)
    fun restartForegroundTickService(context: Context)
    fun stopForegroundTickService(context: Context)
    fun numOfTicks(context: Context, intent: Intent)
    fun retrofitOnResponse(context: Context, intent: Intent)
    fun fileName(context: Context, intent: Intent)
}
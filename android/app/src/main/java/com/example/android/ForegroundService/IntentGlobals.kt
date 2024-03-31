package com.example.android.foregroundservice

object IntentAction {
    const val FILE_NAME = "fileName"
    const val FOLDER_NAME = "folderName"
    const val START_FOREGROUND_TICK_SERVICE = "startForegroundTickService"
    const val STOP_FOREGROUND_TICK_SERVICE = "stopForegroundTickService"
    const val RESTART_FOREGROUND_TICK_SERVICE = "restartForegroundTickService"
    const val MAIN_ACTIVITY_RECEIVER = "mainActivityReceiver"
    const val NUM_OF_TICKS = "numOfTicks"
    const val RETROFIT_ON_RESPONSE = "retrofitOnResponse"
}

object IntentExtras {
    const val FOLDER_NAME = "folderName"
    const val FILE_NAME = "fileName"
    const val NUM_OF_TICKS = "numOfTicks"
    const val NUM_OF_SECONDS_FOR_TICK = "numOfSecondsForTick"
    const val RETROFIT_ON_RESPONSE = "retrofitOnResponse"
}
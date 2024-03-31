package com.example.android.mainactivity

import android.content.Context
import android.content.Intent
import android.os.Build
import android.provider.Settings

class ButtonOpenBatteryOptimization(private val context: Context) {
    fun onClick() {
        openBatteryOptimization(context);
    }

    private fun openBatteryOptimization(context: Context) =
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            val intent = Intent()
            intent.action = Settings.ACTION_IGNORE_BATTERY_OPTIMIZATION_SETTINGS
            context.startActivity(intent)
        } else {
            //Timber.d("Battery optimization not necessary")
        }
}
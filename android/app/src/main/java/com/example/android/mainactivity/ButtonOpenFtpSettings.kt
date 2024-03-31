package com.example.android.mainactivity

import android.content.Context
import android.content.Intent
import androidx.core.content.ContextCompat
import com.example.android.ftpsettingsactivity.FtpSettingsActivity

class ButtonOpenFtpSettings(private val context: Context) {
    fun onClick() {
        val intent = Intent(context, FtpSettingsActivity::class.java)
        ContextCompat.startActivity(context, intent, null)
    }
}
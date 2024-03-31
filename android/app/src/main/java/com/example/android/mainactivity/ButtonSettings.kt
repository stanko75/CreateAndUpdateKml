package com.example.android.mainactivity

import android.content.Context
import android.content.Intent
import androidx.core.content.ContextCompat.startActivity
import com.example.android.settingsactivity.SettingsActivity

class ButtonSettings(private val context: Context) {
    fun onClick() {
        val intent = Intent(context, SettingsActivity::class.java)
        startActivity(context, intent, null)
    }
}

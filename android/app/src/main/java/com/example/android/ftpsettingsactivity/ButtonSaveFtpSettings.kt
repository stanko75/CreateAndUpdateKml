package com.example.android.ftpsettingsactivity

import android.content.Context
import android.os.Build
import androidx.annotation.RequiresApi

class ButtonSaveFtpSettings(private val context: Context, private val host: String, private val user: String, private val pass: String) {
    @RequiresApi(Build.VERSION_CODES.O)
    fun onClick() {

        val sharedPreferences = context.getSharedPreferences("ftpSettings", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putString("host", host)
        editor.putString("user", user)
        editor.putString("pass", pass)
        editor.apply()
    }
}
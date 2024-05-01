package com.example.android.settingsactivity

import android.content.Context
import android.os.Build
import androidx.annotation.RequiresApi

class ButtonSaveSettings(private val context: Context, private val fileName: String, private val folderName: String) {
    @RequiresApi(Build.VERSION_CODES.O)
    fun onClick() {

        val sharedPreferences = context.getSharedPreferences("settings", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putString("kmlFileName", fileName)
        editor.putString("folderName", folderName)
        editor.apply()
    }
}
package com.example.android.settingsactivity

import android.content.Context
import android.os.Build
import android.os.Bundle
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import androidx.databinding.DataBindingUtil.setContentView
import com.example.android.R
import com.example.android.databinding.ActivitySettingsBinding

class SettingsActivity : AppCompatActivity() {
    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_settings)

        val bindingSettingsActivity: ActivitySettingsBinding = setContentView(this,
            R.layout.activity_settings
        )
        bindingSettingsActivity.lifecycleOwner = this

        val activityContext = this

        //ToDo do it in MVVM
        val sharedPreferences = activityContext.getSharedPreferences("settings", Context.MODE_PRIVATE)

        val fileName = sharedPreferences.getString("kmlFileName", "default")
        val folderName = sharedPreferences.getString("folderName", "default")

        bindingSettingsActivity.editTextFileName.setText(fileName)
        bindingSettingsActivity.editTextFolderName.setText(folderName)

        bindingSettingsActivity.btnSaveSettings.setOnClickListener {
            ButtonSaveSettings(activityContext, bindingSettingsActivity.editTextFileName.text.toString(), bindingSettingsActivity.editTextFolderName.text.toString()).onClick()
            finish()
        }
    }
}
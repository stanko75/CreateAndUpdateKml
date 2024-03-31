package com.example.android.ftpsettingsactivity

import android.content.Context
import android.os.Build
import android.os.Bundle
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import com.example.android.R
import com.example.android.databinding.ActivityFtpSettingsBinding
import androidx.databinding.DataBindingUtil.setContentView
import com.example.android.settingsactivity.ButtonSaveSettings

class FtpSettingsActivity : AppCompatActivity() {
    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_ftp_settings)

        val bindingFtpSettingsActivity: ActivityFtpSettingsBinding = setContentView(
            this,
            R.layout.activity_ftp_settings
        )

        bindingFtpSettingsActivity.lifecycleOwner = this

        val activityContext = this

        //ToDo do it in MVVM
        val sharedPreferences =
            activityContext.getSharedPreferences("ftpSettings", Context.MODE_PRIVATE)

        val host = sharedPreferences.getString("host", "ftp.host.com")
        val user = sharedPreferences.getString("user", "user")
        val pass = sharedPreferences.getString("pass", "pass")

        bindingFtpSettingsActivity.editTextHost.setText(host)
        bindingFtpSettingsActivity.editTextUser.setText(user)
        bindingFtpSettingsActivity.editTextPass.setText(pass)

        bindingFtpSettingsActivity.btnSaveSettings.setOnClickListener {
            ButtonSaveFtpSettings(
                activityContext,
                bindingFtpSettingsActivity.editTextHost.text.toString(),
                bindingFtpSettingsActivity.editTextUser.text.toString(),
                bindingFtpSettingsActivity.editTextPass.text.toString()
            ).onClick()
            finish()
        }

    }
}
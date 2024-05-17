package com.example.android.mainactivity

import android.app.Activity
import android.content.Context
import android.os.Build
import androidx.annotation.RequiresApi
import com.example.android.Config
import com.example.android.CreateRetrofitBuilder
import com.example.android.location.FileFolderLocationModel
import com.example.android.logger.ActivityLogger
import com.example.android.logger.LogEntry
import com.example.android.logger.LoggingEventType
import com.example.android.retrofit.ftp.IUploadToBlogApiService
import com.example.android.retrofit.ftp.UploadToBlog
import com.example.android.retrofit.ftp.UploadToBlogCallbacks
import com.google.gson.Gson
import com.google.gson.GsonBuilder

class ButtonFtpUpload(private val activity: Activity, private val context: Context) {
    @RequiresApi(Build.VERSION_CODES.O)
    fun onClick() {

        val builder = GsonBuilder()
        val gson: Gson = builder.create()

        val ftpModel = FtpModel()

        val ftpSharedPreferences =
            activity.getSharedPreferences("ftpSettings", Context.MODE_PRIVATE)

        val fileAndFolderNameSharedPreferences =
            context.getSharedPreferences("settings", Context.MODE_PRIVATE)

        ftpModel.host = ftpSharedPreferences.getString("host", "ftp.host.com")
        ftpModel.user = ftpSharedPreferences.getString("user", "")
        ftpModel.pass = ftpSharedPreferences.getString("pass", "")
        ftpModel.folderName = fileAndFolderNameSharedPreferences.getString("folderName", "")
        ftpModel.kmlFileName = fileAndFolderNameSharedPreferences.getString("kmlFileName","")

        val activityLogger = ActivityLogger(activity)
        activityLogger.Log(
            LogEntry(
                LoggingEventType.Information,
                "Ftp upload executed, sending fileName: ${ftpModel.kmlFileName}"
            )
        )

        val uploadToBlogCallbacks =
            UploadToBlogCallbacks(activityLogger, activity, ftpModel.folderName);

        var ok = UploadToBlog(
            CreateRetrofitBuilder().createRetrofitBuilder(Config(context).webHost)
                .create(IUploadToBlogApiService::class.java), uploadToBlogCallbacks
        ).uploadToBlogHttpPost(gson.toJson(ftpModel));
    }

    class FtpModel: FileFolderLocationModel() {
        var host: String? = null
        var user: String? = null
        var pass: String? = null
    }
}
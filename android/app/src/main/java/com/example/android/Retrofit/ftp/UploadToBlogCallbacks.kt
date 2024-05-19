package com.example.android.retrofit.ftp

import android.app.Activity
import android.content.Intent
import android.net.Uri
import com.example.android.logger.ILogger
import com.example.android.logger.LoggerExtensions
import retrofit2.Response

class UploadToBlogCallbacks(var logger: ILogger, var activity: Activity, var folderName: String?): IUploadToBlogCallbacks {

    override fun onResponse(response: Response<String>) {
        if (response.isSuccessful) {
            val url = "http://www.milosev.com/gallery/allWithPics/travelBuddies/$folderName/www/index.html"
            val intent = Intent(Intent.ACTION_VIEW, Uri.parse(url))
            activity.startActivity(intent)

            LoggerExtensions().log(logger, "Response message: ${response.message()}, body: ${response.body()}")
        }
        else {
            LoggerExtensions().log(logger, Exception(response.errorBody()?.string() ?: ""))
        }
    }

    override fun onFailure(t: Throwable) {
        LoggerExtensions().log(logger, t.message.toString())
    }
}
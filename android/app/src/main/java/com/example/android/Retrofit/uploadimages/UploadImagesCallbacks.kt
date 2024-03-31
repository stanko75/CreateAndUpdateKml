package com.example.android.retrofit.uploadimages

import android.app.Activity
import com.example.android.logger.ILogger
import com.example.android.logger.LoggerExtensions
import retrofit2.Call
import retrofit2.Response

class UploadImagesCallbacks(var logger: ILogger, var activity: Activity): IUploadImagesCallbacks {
    override fun onResponse(call: Call<UploadImagesResponse>, response: Response<UploadImagesResponse>) {
        if (response.isSuccessful) {
            LoggerExtensions().log(logger, "Response message: ${response.message()}, body: ${response.body()}")
        }
        else {
            LoggerExtensions().log(logger, Exception(response.errorBody()?.string() ?: ""))
        }
    }

    override fun onFailure(call: Call<UploadImagesResponse>, t: Throwable) {
        LoggerExtensions().log(logger, t.message.toString())
    }
}
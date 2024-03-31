package com.example.android.retrofit.ftp

import android.content.Context
import retrofit2.Response

interface IUploadToBlogCallbacks {
    fun onResponse(response: Response<String>)
    fun onFailure(t: Throwable)
}
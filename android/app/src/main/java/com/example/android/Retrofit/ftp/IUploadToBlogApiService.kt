package com.example.android.retrofit.ftp

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.Headers
import retrofit2.http.POST

interface IUploadToBlogApiService {
    @Headers("Content-Type: text/json")
    @POST("/api/UpdateCoordinates/UploadToBlog")
    fun postMethod(@Body value: String): Call<String>
}
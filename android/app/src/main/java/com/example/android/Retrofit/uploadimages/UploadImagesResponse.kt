package com.example.android.retrofit.uploadimages

import com.google.gson.annotations.SerializedName

data class UploadImagesResponse(
    @SerializedName("message")
    val message: String
)
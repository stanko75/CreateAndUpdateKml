package com.example.android.retrofit.coordinates

import android.content.Context

interface IUpdateCoordinatesOnWeb {
    fun updateCoordinatesHttpPost(value: String, context: Context)
}
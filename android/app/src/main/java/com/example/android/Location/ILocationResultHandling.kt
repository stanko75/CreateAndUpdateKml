package com.example.android.location

import android.content.Context

interface ILocationResultHandling {
    fun execute(context: Context, lat: String, lng: String)
}
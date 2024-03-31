package com.example.android.foregroundservice

import android.content.Context

interface ILocationClass {
    fun requestLocationUpdates(context: Context)
}
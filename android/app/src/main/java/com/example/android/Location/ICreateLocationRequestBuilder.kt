package com.example.android.location

import com.google.android.gms.location.LocationRequest

interface ICreateLocationRequestBuilder {
    fun buildLocationRequest(interval: Long?): LocationRequest
}
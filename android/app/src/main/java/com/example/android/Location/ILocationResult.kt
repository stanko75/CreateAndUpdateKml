package com.example.android.location

import com.google.android.gms.location.LocationResult

interface ILocationResult {
    fun onLocationResult(locationResult: LocationResult)
}
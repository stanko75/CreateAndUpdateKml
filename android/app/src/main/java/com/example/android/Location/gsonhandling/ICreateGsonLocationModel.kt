package com.example.android.location.gsonhandling

import com.example.android.location.LocationModel

interface ICreateGsonLocationModel {
    fun execute(lat: String, lng: String): String?
    fun <T : LocationModel> serializeToJSON(model: T): String
}
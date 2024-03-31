package com.example.android.GsonHandlingTest

import com.example.android.location.gsonhandling.CreateGsonLocationModel
import com.example.android.location.LocationModel
import com.google.gson.Gson
import org.junit.Assert
import org.junit.Test

class CreateGsonLocationModelTest {
    @Test
    fun checkIfGsonIsCorrect() {
        val createGsonLocationModel = CreateGsonLocationModel()
        val lat = "44.1063052"
        val lng = "9.7281864"
        val result = createGsonLocationModel.execute(lat, lng)

        var gson = Gson()

        val locationModel = gson.fromJson(result, LocationModel::class.java)
        Assert.assertEquals(locationModel.lat, lat)
        Assert.assertEquals(locationModel.lng, lng)
    }
}
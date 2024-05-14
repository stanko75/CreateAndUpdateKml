package com.example.android.location

import android.content.Context
import com.example.android.filehandling.IWriteFileOnInternalStorage
import com.example.android.location.gsonhandling.ICreateGsonLocationModel
import com.example.android.retrofit.coordinates.IUpdateCoordinatesOnWeb
import java.io.File
import java.text.SimpleDateFormat
import java.util.*

class LocationResultHandling(
    private val createGsonLocationModel: ICreateGsonLocationModel
    , private val writeFileOnInternalStorage: IWriteFileOnInternalStorage
    , private val updateCoordinatesOnWeb: IUpdateCoordinatesOnWeb
    , private val fileFolderLocationModel: FileFolderLocationModel
): ILocationResultHandling {
    override fun execute(context: Context, lat: String, lng: String) {
        val sdf = SimpleDateFormat("ddMyyyyhhmmss", Locale.GERMANY)
        val currentDate = sdf.format(Date())
        val path = File(context.getExternalFilesDir(null), "locations")
        val fileName = "test$currentDate"

        fileFolderLocationModel.Latitude = lat
        fileFolderLocationModel.Longitude = lng

        val json = createGsonLocationModel.serializeToJSON(fileFolderLocationModel)
        writeFileOnInternalStorage.execute(path, fileName, json)
        updateCoordinatesOnWeb.updateCoordinatesHttpPost(json, context)
    }
}
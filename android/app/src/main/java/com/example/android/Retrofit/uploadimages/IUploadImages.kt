package com.example.android.retrofit.uploadimages

import android.content.Context
import android.net.Uri

interface IUploadImages {
    fun uploadImage(imgUri: Uri, fileName: String, folderName: String, context: Context): String?
}
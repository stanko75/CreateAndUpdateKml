package com.example.android.mainactivity

import android.content.Context
import android.net.Uri
import android.provider.OpenableColumns
import androidx.activity.result.ActivityResultLauncher
import com.example.android.retrofit.uploadimages.IUploadImages

class ButtonUploadPictures(
    private var uploadImages: IUploadImages
    , private val context: Context
) {
    fun onClick(galleryLauncher: ActivityResultLauncher<String>) {
        galleryLauncher.launch("image/*")
    }

    fun uploadImages(images: List<Uri>) {
        val fileAndFolderNameSharedPreferences =
            context.getSharedPreferences("settings", Context.MODE_PRIVATE)

        val folderName: String = fileAndFolderNameSharedPreferences.getString("folderName", "").toString()

        for (image in images) {
            val cursor = context.contentResolver.query(image, null, null, null, null)
            cursor?.use { innerCursor ->
                val nameIndex = innerCursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                innerCursor.moveToFirst()
                val imageName = innerCursor.getString(nameIndex)
                uploadImages.uploadImage(image, imageName, folderName, context)
            }
        }
    }
}
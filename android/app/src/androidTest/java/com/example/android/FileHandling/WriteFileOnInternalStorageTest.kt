package com.example.android.filehandling

import androidx.test.ext.junit.runners.AndroidJUnit4
import androidx.test.platform.app.InstrumentationRegistry
import org.junit.Assert
import org.junit.Test
import org.junit.runner.RunWith
import java.io.File

@RunWith(AndroidJUnit4::class)
class WriteFileOnInternalStorageTest {
    @Test
    fun checkIfFileSaved() {
        val appContext = InstrumentationRegistry.getInstrumentation().targetContext
        val path = File(appContext.getExternalFilesDir(null), "locations")
        val fileName = "testFileName"

        val writeFileOnInternalStorage = WriteFileOnInternalStorage()
        writeFileOnInternalStorage.execute(path, fileName, "testBody")

        Assert.assertTrue(path.exists())
    }
}
package com.example.android.mainactivity

import android.os.Build
import android.os.Bundle
import androidx.activity.result.contract.ActivityResultContracts
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import androidx.core.text.HtmlCompat
import androidx.databinding.DataBindingUtil.setContentView
import com.example.android.Config
import com.example.android.CreateRetrofitBuilder
import com.example.android.ForegroundServiceBroadcastReceiver
import com.example.android.ForegroundServiceBroadcastReceiverOnReceive
import com.example.android.LocationPermissionHelper
import com.example.android.R
import com.example.android.databinding.ActivityMainBinding
import com.example.android.logger.ActivityLogger
import com.example.android.retrofit.GsonConverter
import com.example.android.retrofit.uploadimages.IUploadImagesApiService
import com.example.android.retrofit.uploadimages.UploadImages
import com.example.android.retrofit.uploadimages.UploadImagesCallbacks


class MainActivity : AppCompatActivity() {

    private lateinit var locationPermissionHelper: LocationPermissionHelper
    private lateinit var buttonUploadPictures: ButtonUploadPictures

    private val galleryLauncher =
        this.registerForActivityResult(ActivityResultContracts.GetMultipleContents()) { images ->
            buttonUploadPictures.uploadImages(images)
        }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        val activityContext = this

        locationPermissionHelper = LocationPermissionHelper(this)
        if (!locationPermissionHelper.isLocationPermissionGranted()) {
            locationPermissionHelper.requestLocationPermission()
        }

        val bindingMainActivity: ActivityMainBinding = setContentView(this, R.layout.activity_main)
        bindingMainActivity.lifecycleOwner = this

        val liveLinkHtml = getString(R.string.liveLink)
        bindingMainActivity.textViewLiveLink.text =
            HtmlCompat.fromHtml(liveLinkHtml, HtmlCompat.FROM_HTML_MODE_COMPACT)
        bindingMainActivity.textViewLiveLink.movementMethod =
            android.text.method.LinkMovementMethod.getInstance()

        val broadCastReceiver = ForegroundServiceBroadcastReceiver(
            ForegroundServiceBroadcastReceiverOnReceive(activityContext)
        )

        bindingMainActivity.btnStop.setOnClickListener {
            ButtonStop(this, activityContext, broadCastReceiver).onClick()
        }

        bindingMainActivity.btnStart.setOnClickListener {
            ButtonStart(this, activityContext, broadCastReceiver).onClick(it)
        }

        bindingMainActivity.btnSettings.setOnClickListener {
            ButtonSettings(this).onClick()
        }

        bindingMainActivity.btnOpenBatteryOptimization.setOnClickListener {
            ButtonOpenBatteryOptimization(this).onClick()
        }

        bindingMainActivity.btnOpenFtpSettings.setOnClickListener {
            ButtonOpenFtpSettings(this).onClick()
        }

        bindingMainActivity.btnFtpUpload.setOnClickListener {
            ButtonFtpUpload(this, activityContext).onClick()
        }

        bindingMainActivity.btnUploadPictures.setOnClickListener {

            buttonUploadPictures = ButtonUploadPictures(
                UploadImages(
                    CreateRetrofitBuilder().createRetrofitBuilder(Config(this).webHost, GsonConverter()).create(
                        IUploadImagesApiService::class.java
                    ), UploadImagesCallbacks(ActivityLogger(this), this)
                )
                , this
            )

            buttonUploadPictures.onClick(galleryLauncher)
        }

    }
}
package com.example.android.foregroundservice

import android.app.Notification
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.os.Binder
import android.os.Build
import android.os.IBinder
import android.os.PowerManager
import androidx.annotation.RequiresApi
import androidx.core.app.NotificationCompat
import com.example.android.Config
import com.example.android.CreateRetrofitBuilder
import com.example.android.retrofit.coordinates.IUpdateCoordinatesApiService
import com.example.android.location.CreateLocationRequestBuilder
import com.example.android.location.LocationClass
import com.example.android.location.LocationResult
import com.example.android.R
import com.example.android.SendBroadcastTickReceiver
import com.example.android.retrofit.coordinates.UpdateCoordinatesOnWeb
import com.example.android.retrofit.coordinates.UpdateCoordinatesOnWebCallbacks
import com.example.android.filehandling.WriteFileOnInternalStorage
import com.example.android.location.gsonhandling.CreateGsonLocationModel
import com.example.android.location.FileFolderLocationModel
import com.example.android.location.LocationResultHandling
import kotlinx.coroutines.*

class ForegroundTickService : Service(), CoroutineScope by MainScope() {

    private var job: Job? = null

    override fun onBind(intent: Intent): IBinder {
        return Binder()
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        val context = this

        when (intent?.action) {

            IntentAction.START_FOREGROUND_TICK_SERVICE -> {
                startForeground(101, createNotification())
                val numOfSecondsForTick =
                    intent.getLongExtra (IntentExtras.NUM_OF_SECONDS_FOR_TICK, 30)

                val kmlFileName =
                    intent.getStringExtra (IntentExtras.KML_FILE_NAME)
                val folderName =
                    intent.getStringExtra (IntentExtras.FOLDER_NAME)

                val fileFolderLocationModel = FileFolderLocationModel()
                fileFolderLocationModel.kmlFileName = kmlFileName
                fileFolderLocationModel.folderName = folderName
                //Thread.sleep(5000L) //us this for debug

                val sendBroadcastTickReceiver = SendBroadcastTickReceiver()
                val startForegroundTickService =
                    LocationClass(
                        context
                        , LocationResult(
                                context
                                , sendBroadcastTickReceiver
                                , LocationResultHandling(
                                    CreateGsonLocationModel()
                                    , WriteFileOnInternalStorage()
                                    , UpdateCoordinatesOnWeb(
                                        CreateRetrofitBuilder().createRetrofitBuilder(Config(context).webHost)
                                            .create(IUpdateCoordinatesApiService::class.java)
                                        , UpdateCoordinatesOnWebCallbacks(sendBroadcastTickReceiver)
                                        , sendBroadcastTickReceiver
                                )
                                , fileFolderLocationModel
                            )
                        )
                        , CreateLocationRequestBuilder().buildLocationRequest(numOfSecondsForTick)
                )
                startForegroundTickService.requestLocationUpdates(context)
            }
            IntentAction.STOP_FOREGROUND_TICK_SERVICE -> {
                stopSelf(startId)
                stopSelfResult(startId)
            }
        }
        return START_STICKY
    }

    @RequiresApi(Build.VERSION_CODES.O)
    fun createNotification(): Notification {
        val channelId =
            createNotificationChannel()
        val notificationBuilder = NotificationCompat.Builder(this, channelId)

        return notificationBuilder.setOngoing(true)
            .setContentTitle("test")
            .setContentText("test")
            .setSmallIcon(R.mipmap.ic_launcher)
            .setPriority(1)
            .setCategory(Notification.CATEGORY_SERVICE)
            .build()
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun createNotificationChannel(): String {
        val channelId = "Foreground_TickService"
        val channel = NotificationChannel(
            channelId, "Foreground tick service", NotificationManager.IMPORTANCE_LOW
        )
        channel.lightColor = Color.RED
        channel.lockscreenVisibility = Notification.VISIBILITY_PRIVATE
        val service = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        service.createNotificationChannel(channel)
        return channelId
    }

    override fun onCreate() {
        super.onCreate()
        acquireWakelock()
    }

    override fun onDestroy() {
        super.onDestroy()
        val restartForegroundTickService = Intent(this, BroadcastTickReceiver::class.java).setAction(IntentAction.RESTART_FOREGROUND_TICK_SERVICE)
        sendBroadcast(restartForegroundTickService)
        releaseWakelock()
    }

    override fun onTaskRemoved(rootIntent: Intent?) {
        super.onTaskRemoved(rootIntent)
        val restartForegroundTickService = Intent(this, BroadcastTickReceiver::class.java).setAction(IntentAction.RESTART_FOREGROUND_TICK_SERVICE)
        sendBroadcast(restartForegroundTickService)
    }

    private val wakeLock: PowerManager.WakeLock by lazy {
        (getSystemService(Context.POWER_SERVICE) as PowerManager).run {
            newWakeLock(PowerManager.PARTIAL_WAKE_LOCK, "justanotherrealtimetrackingapp:ServiceWakelock")
        }
    }

    private fun acquireWakelock() {
        try {
            wakeLock.let {
                wakeLock.setReferenceCounted(false)
                if (!wakeLock.isHeld) {
                    wakeLock.acquire()
                }
            }
        } catch (e: RuntimeException) {
        }
    }

    private fun releaseWakelock() {
        try {
            wakeLock.let {
                if (it.isHeld) {
                    it.release()
                }
            }
        } catch (e: RuntimeException) {
        }
    }

}
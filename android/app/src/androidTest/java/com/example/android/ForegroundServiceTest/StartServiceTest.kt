package com.example.android.ForegroundServiceTest

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import androidx.core.content.ContextCompat.startForegroundService
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import androidx.test.platform.app.InstrumentationRegistry
import com.example.android.foregroundservice.BroadcastTickReceiver
import com.example.android.foregroundservice.ForegroundTickService
import com.example.android.foregroundservice.IntentAction
import org.junit.Test

class StartServiceTest {

    private val broadcastTickReceiver: BroadcastReceiver = BroadcastTickReceiver()

    @Test
    fun startService() {
        val broadCastReceiver = object : BroadcastReceiver() {
            override fun onReceive(contxt: Context?, intent: Intent?) {
                when (intent?.action) {
                    "MyAction" -> {
                    }
                }
            }
        }

        val appContext = InstrumentationRegistry.getInstrumentation().targetContext
        val filter = IntentFilter(IntentAction.NUM_OF_TICKS)
        LocalBroadcastManager.getInstance(appContext).registerReceiver(broadcastTickReceiver, filter)

        val intent = Intent(appContext, ForegroundTickService::class.java)
        intent.action = "startForeground"
        startForegroundService(appContext, intent)
    }
}
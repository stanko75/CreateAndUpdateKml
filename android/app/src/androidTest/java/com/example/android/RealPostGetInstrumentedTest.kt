package com.example.android

import android.app.Activity
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.content.pm.PackageManager
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import androidx.test.ext.junit.rules.activityScenarioRule
import androidx.test.ext.junit.runners.AndroidJUnit4
import androidx.test.platform.app.InstrumentationRegistry
import com.example.android.foregroundservice.BroadcastTickReceiver
import com.example.android.foregroundservice.ForegroundTickService
import com.example.android.foregroundservice.IntentAction
import com.example.android.foregroundservice.IntentExtras
import com.example.android.mainactivity.MainActivity
import org.junit.Assert
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith

var received: Boolean = false
var numOfTicks: Int = 0

@RunWith(AndroidJUnit4::class)
class RealPostGetInstrumentedTest {

    @get:Rule
    val activityScenarioRule = activityScenarioRule<MainActivity>()

    @Test
    fun checkNumberOfTicks() {

        val appContext = InstrumentationRegistry.getInstrumentation().targetContext

        val activityScenario = activityScenarioRule.scenario
        activityScenario.onActivity { activity ->
            val foregroundServiceBroadcastReceiverOnReceiveTest = ForegroundServiceBroadcastReceiverOnReceiveTest(activity)
            val broadCastReceiver = ForegroundServiceBroadcastReceiver(foregroundServiceBroadcastReceiverOnReceiveTest)

            LocalBroadcastManager.getInstance(appContext)
                .registerReceiver(broadCastReceiver, IntentFilter(IntentAction.NUM_OF_TICKS))
            LocalBroadcastManager.getInstance(appContext)
                .registerReceiver(broadCastReceiver, IntentFilter(IntentAction.RETROFIT_ON_RESPONSE))

            val component = ComponentName(appContext, BroadcastTickReceiver::class.java)
            activity.packageManager.setComponentEnabledSetting(
                component,
                PackageManager.COMPONENT_ENABLED_STATE_ENABLED,
                PackageManager.DONT_KILL_APP
            )

            val intentStartForegroundTickService = Intent(activity, ForegroundTickService::class.java)
            intentStartForegroundTickService.action = IntentAction.START_FOREGROUND_TICK_SERVICE
            intentStartForegroundTickService.putExtra(
                IntentExtras.NUM_OF_SECONDS_FOR_TICK,
                10.toLong()
            )
            activity.startForegroundService(intentStartForegroundTickService)
        }

        while (!received) {
        }

        Assert.assertEquals(numOfTicks, 1)
    }
}

class ForegroundServiceBroadcastReceiverOnReceiveTest(private val activity: Activity): IForegroundServiceBroadcastReceiverOnReceive {
    override fun onReceive(context: Context, intent: Intent) {
        when (intent.action) {
            IntentAction.NUM_OF_TICKS -> {
                numOfTicks = intent.getIntExtra(IntentExtras.NUM_OF_TICKS, 30)
                received = true
            }
        }
    }

    override fun onNumOfTicksReceived(numOfTicks: Int) {
        TODO("Not yet implemented")
    }

    override fun onRetrofitResponseReceived(retrofitMessage: String?, context: Context) {
        TODO("Not yet implemented")
    }

    override fun onUnknownActionReceived(context: Context) {
        TODO("Not yet implemented")
    }

}
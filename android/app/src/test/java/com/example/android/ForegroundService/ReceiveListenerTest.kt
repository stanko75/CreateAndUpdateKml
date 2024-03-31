package com.example.android.foregroundservice

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import com.example.android.mainactivity.MainActivity
import org.junit.Before
import org.junit.Test
import org.mockito.Mock
import org.mockito.Mockito
import org.mockito.Mockito.`when`
import org.mockito.MockitoAnnotations


internal class ReceiveListenerTest {

    @Mock
    private var context: Context = Mockito.mock(Context::class.java)

    @Mock
    private val androidIntent: Intent = Mockito.mock(Intent(context, MainActivity::class.java)::class.java)

    @Mock
    private val intentReturn: Intent = Mockito.mock(Intent::class.java)

    @Mock
    private val localBroadcastManager: LocalBroadcastManager = Mockito.mock(LocalBroadcastManager::class.java)

    var receiveListener = ReceiveListener()
    private lateinit var receiver: BroadcastReceiverTester

    inner class BroadcastReceiverTester : BroadcastReceiver() {

        override fun onReceive(p0: Context?, intent: Intent?) {
            intent?.let {
                println("test")
            }
        }
    }

    @Before
    fun setUp() {
        MockitoAnnotations.openMocks(this)
    }

    @Test
    fun startForegroundTickService() {
        receiver = BroadcastReceiverTester()
        context.registerReceiver(
            receiver,
            IntentFilter(
                IntentAction.START_FOREGROUND_TICK_SERVICE
            )
        )

        `when`(LocalBroadcastManager.getInstance(context))
            .thenReturn(localBroadcastManager)

        val localBroadcastManager2: LocalBroadcastManager = LocalBroadcastManager.getInstance(context)

        `when`(androidIntent.setAction(IntentAction.MAIN_ACTIVITY_RECEIVER))
            .thenReturn(intentReturn)


        //receiveListener.StartForegroundTickService(context, androidIntent, localBroadcastManager2)
    }

    @Test
    fun restartForegroundTickService() {
    }

    @Test
    fun stopForegroundTickService() {

    }

    @Test
    fun numOfTicks() {
    }
}
package com.example.android.location

import android.content.Context
import com.example.android.foregroundservice.IntentAction
import com.example.android.ISendBroadcastTickReceiver
import com.google.android.gms.location.LocationResult

class LocationResult(val context: Context
                    , private val broadcastTickReceiver: ISendBroadcastTickReceiver
                    , private val locationResultHandling: ILocationResultHandling): ILocationResult {

    private var numOfTicks = 0

    override fun onLocationResult(locationResult: LocationResult) {
        if (locationResult.locations.isNotEmpty()) {
            val location =
                locationResult.lastLocation

            locationResultHandling.execute(
                context,
                location?.latitude.toString(),
                location?.longitude.toString()
            )

            numOfTicks += 1
            broadcastTickReceiver.execute(context, IntentAction.NUM_OF_TICKS, numOfTicks)
        }
    }
}
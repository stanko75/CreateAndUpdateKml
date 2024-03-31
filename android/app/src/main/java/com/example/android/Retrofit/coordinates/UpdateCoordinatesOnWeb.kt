package com.example.android.retrofit.coordinates

import android.content.Context
import com.example.android.ISendBroadcastTickReceiver
import com.example.android.foregroundservice.IntentAction
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class UpdateCoordinatesOnWeb(
    var updateCoordinatesApiService: IUpdateCoordinatesApiService
, var updateCoordinatesOnWebCallbacks: IUpdateCoordinatesOnWebCallbacks
, private val broadcastTickReceiver: ISendBroadcastTickReceiver
): IUpdateCoordinatesOnWeb {

    private lateinit var webApiRequest: Call<String>

    override fun updateCoordinatesHttpPost(value: String, context: Context) {
        broadcastTickReceiver.execute(
            context,
            IntentAction.RETROFIT_ON_RESPONSE,
            "Sending: $value"
        )
        webApiRequest = updateCoordinatesApiService.postMethod(value)

        webApiRequest.enqueue(object : Callback<String> {
            override fun onResponse(call: Call<String>, response: Response<String>) {
                updateCoordinatesOnWebCallbacks.onResponse(response, context)
            }

            override fun onFailure(call: Call<String>, t: Throwable) {
                updateCoordinatesOnWebCallbacks.onFailure(t, context)
            }
        })
    }
}
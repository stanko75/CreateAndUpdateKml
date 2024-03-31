package com.example.android

import android.content.Context
import android.util.Log
import androidx.test.ext.junit.runners.AndroidJUnit4
import androidx.test.platform.app.InstrumentationRegistry
import com.example.android.mainactivity.MainActivity
import com.example.android.retrofit.coordinates.IUpdateCoordinatesApiService
import com.example.android.retrofit.coordinates.IUpdateCoordinatesOnWebCallbacks
import com.example.android.retrofit.coordinates.UpdateCoordinatesOnWeb
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import okhttp3.OkHttpClient
import org.junit.Assert
import org.junit.Test
import org.junit.runner.RunWith
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.scalars.ScalarsConverterFactory
import java.security.cert.X509Certificate
import javax.net.ssl.SSLContext
import javax.net.ssl.TrustManager
import javax.net.ssl.X509TrustManager

var message = ""
var errorMessage = ""
var responseCode = 0
var responseReceived = false

@RunWith(AndroidJUnit4::class)
class UpdateCoordinatesOnWebTest {
    @Test
//this test work only in Azure - non https
    fun testWithoutCertificate() {
        val appContext = InstrumentationRegistry.getInstrumentation().targetContext
        val webHost: String = Config(appContext).webHost
        val updateCoordinatesApiService: IUpdateCoordinatesApiService =
            CreateRetrofitBuilderWithoutCertificateTest().createRetrofitBuilder(webHost)
                .create(IUpdateCoordinatesApiService::class.java)

        val updateCoordinatesOnWeb = UpdateCoordinatesOnWeb(updateCoordinatesApiService, UpdateCoordinatesOnWebCallbacksTest(SendBroadcastTickReceiver()), SendBroadcastTickReceiver())
        updateCoordinatesOnWeb.updateCoordinatesHttpPost("7.0881042, 50.7541783, 2357", appContext)

        while (!responseReceived) {
        }

        Assert.assertEquals(message, "Bad Request")
        Assert.assertEquals(responseCode, 400)
    }

    @Test
    fun testPostFileFolderWithCertificate() {
        val appContext = InstrumentationRegistry.getInstrumentation().targetContext
        val webHost: String = Config(appContext).webHost
        val updateCoordinatesApiService: IUpdateCoordinatesApiService =
            CreateRetrofitBuilderWithCertificateTest().createRetrofitBuilder(webHost)
                .create(IUpdateCoordinatesApiService::class.java)

        val builder = GsonBuilder()
        val gson: Gson = builder.create()

        val locationModelFileFolder = LocationModelFileFolder()
        locationModelFileFolder.lat = "7.0881042"
        locationModelFileFolder.lng = "50.7541783"
        locationModelFileFolder.fileName = "fileName"
        locationModelFileFolder.folder = "folder"

        gson.toJson(locationModelFileFolder)

        val updateCoordinatesOnWeb = UpdateCoordinatesOnWeb(updateCoordinatesApiService, UpdateCoordinatesOnWebCallbacksTest(SendBroadcastTickReceiver()), SendBroadcastTickReceiver())
        updateCoordinatesOnWeb.updateCoordinatesHttpPost(gson.toJson(locationModelFileFolder), appContext)

        while (!responseReceived) {
        }

        Assert.assertEquals(responseCode, 200)
    }

    @Test
    fun testPostFileFolderWithCertificateEmptyCoordinates() {
        val appContext = InstrumentationRegistry.getInstrumentation().targetContext
        val webHost: String = Config(appContext).webHost
        val updateCoordinatesApiService: IUpdateCoordinatesApiService =
            CreateRetrofitBuilderWithCertificateTest().createRetrofitBuilder(webHost)
                .create(IUpdateCoordinatesApiService::class.java)

        val builder = GsonBuilder()
        val gson: Gson = builder.create()

        val locationModelFileFolder = LocationModelFileFolder()
        locationModelFileFolder.lat = ""
        locationModelFileFolder.lng = ""
        locationModelFileFolder.fileName = "fileName"
        locationModelFileFolder.folder = "folder"

        gson.toJson(locationModelFileFolder)

        val updateCoordinatesOnWeb = UpdateCoordinatesOnWeb(updateCoordinatesApiService, UpdateCoordinatesOnWebCallbacksTest(SendBroadcastTickReceiver()), SendBroadcastTickReceiver())
        updateCoordinatesOnWeb.updateCoordinatesHttpPost(gson.toJson(locationModelFileFolder), appContext)

        while (!responseReceived) {
        }

        Assert.assertEquals(responseCode, 500)
        Assert.assertTrue(errorMessage.contains("Coordinates cannot be empty"))
    }

    class LocationModelFileFolder {
        var lat: String? = null
        var lng: String? = null
        var dateTime: String? = null
        var folder: String? = null
        var fileName: String? = null
    }

    @Test
    fun testWrongLink() {
        val appContext = InstrumentationRegistry.getInstrumentation().targetContext
        val updateCoordinatesApiService: IUpdateCoordinatesApiService =
            CreateRetrofitBuilderWithoutCertificateTest().createRetrofitBuilder("http://wrong.link")
                .create(IUpdateCoordinatesApiService::class.java)

        val updateCoordinatesOnWeb =  UpdateCoordinatesOnWeb(updateCoordinatesApiService, UpdateCoordinatesOnWebCallbacksTest(SendBroadcastTickReceiver()), SendBroadcastTickReceiver())
        updateCoordinatesOnWeb.updateCoordinatesHttpPost("7.0881042, 50.7541783, 2357", appContext)

        while (!responseReceived) {
        }

        Assert.assertEquals(message, "Unable to resolve host \"wrong.link\": No address associated with hostname")
    }
}

class CreateRetrofitBuilderWithoutCertificateTest: ICreateRetrofitBuilder {

    override fun createRetrofitBuilder(baseUrl: String): Retrofit {
        return Retrofit.Builder()
            .baseUrl(baseUrl)
            .addConverterFactory(ScalarsConverterFactory.create())
            .build()
    }
}

class CreateRetrofitBuilderWithCertificateTest: ICreateRetrofitBuilder {
    override fun createRetrofitBuilder(baseUrl: String): Retrofit {
        return Retrofit.Builder()
            .baseUrl(baseUrl)
            .client(trustAllCertificates())
            .addConverterFactory(ScalarsConverterFactory.create())
            .build()
    }
}

private fun trustAllCertificates(): OkHttpClient {
    val trustAllCerts = arrayOf<TrustManager>(object : X509TrustManager {
        override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?) {
            Log.i(MainActivity::class.simpleName, "checkClientTrusted")
        }

        override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?) {
            Log.i(MainActivity::class.simpleName, "checkServerTrusted")
        }

        override fun getAcceptedIssuers() = arrayOf<X509Certificate>()
    })
    val sslContext = SSLContext.getInstance("SSL")
    sslContext.init(null, trustAllCerts, java.security.SecureRandom())

// Create an ssl socket factory with our all-trusting manager
    val sslSocketFactory = sslContext.socketFactory

// connect to server
    return OkHttpClient.Builder().sslSocketFactory(sslSocketFactory, trustAllCerts[0] as X509TrustManager).hostnameVerifier{ _, _ -> true }.build()
}

class UpdateCoordinatesOnWebCallbacksTest(private val broadcastTickReceiver: ISendBroadcastTickReceiver):
    IUpdateCoordinatesOnWebCallbacks {
    override fun onResponse(response: Response<String>, context: Context) {
        responseCode = response.code()
        errorMessage = response.errorBody()!!.charStream().readText()
        message = response.message()
        responseReceived = true
    }

    override fun onFailure(t: Throwable, context: Context) {
        message = t.message.toString()
        responseReceived = true
    }
}
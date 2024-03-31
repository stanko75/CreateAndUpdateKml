package com.example.android

import com.example.android.retrofit.IConverterType
import com.example.android.retrofit.ScalarsConverter
import retrofit2.Retrofit

interface ICreateRetrofitBuilder {
    fun createRetrofitBuilder(baseUrl: String, converterType: IConverterType = ScalarsConverter()): Retrofit
}
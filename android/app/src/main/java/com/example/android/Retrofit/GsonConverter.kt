package com.example.android.retrofit

import retrofit2.Converter
import retrofit2.converter.gson.GsonConverterFactory

class GsonConverter: IConverterType {
    override fun getFactory(): Converter.Factory = GsonConverterFactory.create()
}
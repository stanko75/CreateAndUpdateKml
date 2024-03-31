package com.example.android.retrofit

import retrofit2.Converter

interface IConverterType {
    fun getFactory(): Converter.Factory
}
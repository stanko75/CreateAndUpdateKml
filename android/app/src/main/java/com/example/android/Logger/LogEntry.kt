package com.example.android.logger

data class LogEntry (var Severity: LoggingEventType, var Message: String?, var Exception: Exception? = null)

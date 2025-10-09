using System;
using System.Configuration;

namespace WFApp_Electronic_Scale
{
    public static class AppConfiguration
    {
        // Serial Port Configuration
        public static class SerialPort
        {
            public const string DefaultPort = "COM3";
            public const int DefaultBaudRate = 9600;
            public const int DefaultDataBits = 8;
            public const int DefaultReadTimeout = 3000;
            public const int DefaultWriteTimeout = 3000;
            public const int BufferSize = 4096;
        }

        // UHF Configuration
        public static class UHF
        {
            public const string DefaultPort = "COM5";
            public const int DefaultBaudRate = 115200;
            public const int FrameLength = 24;
            public const int MaxBufferSize = 1024;
            public const int ReadIntervalSeconds = 1;
        }

        // API Configuration
        public static class Api
        {
            public const string DefaultUrl = "https://stage-masarak.frappe.cloud/api/method/get_trip_by_tag_id";
            public const string DefaultUsername = "f39a13dc264037d";
            public const string DefaultPassword = "42ec5dcfc6e0d58";
            public const int DefaultTimeoutSeconds = 30;
        }

        // Database Configuration
        public static class Database
        {
            public const string DefaultServer = "localhost";
            public const string DefaultDatabase = "WeightScaleDB";
            public const string DefaultTable = "Tartim1";
            public const int ConnectionTimeoutSeconds = 30;
            public const int MinPoolSize = 1;
            public const int MaxPoolSize = 10;
            public const int ConnectionLifetimeSeconds = 300;
        }

        // Application Configuration
        public static class Application
        {
            public const string DefaultWeightUnit = "KG";
            public const int MaxLogEntries = 1000;
            public const int LogFlushIntervalMs = 100;
            public const int MaxHistoryRecords = 1000;
        }

        // File Paths
        public static class FilePaths
        {
            public const string SettingsFile = "setting.json";
            public const string UsersFile = "users.json";
            public const string LogFile = "log.txt";
        }

        // UI Configuration
        public static class UI
        {
            public const int SplashScreenDelayMs = 2000;
            public const int PopupTimeoutMs = 5000;
            public const int ProgressUpdateIntervalMs = 500;
        }

        // Character Constants for Serial Communication
        public static class SerialChars
        {
            public const char STX = (char)2;  // Start of Text
            public const char SOH = (char)1;  // Start of Header
            public const char ETX = (char)3;  // End of Text
            public const char ACK = (char)4;  // Acknowledge
            public const char NAK = (char)5;  // Negative Acknowledge
            public const char CR = (char)13;  // Carriage Return
            public const char LF = (char)10;  // Line Feed
        }

        // Weight Processing Configuration
        public static class WeightProcessing
        {
            public const int MinWeightStringLength = 41;
            public const int WeightStartIndex = 26;
            public const int WeightLength = 6;
            public const int ProcessingDelayMs = 500;
        }

        // Error Messages
        public static class ErrorMessages
        {
            public const string DatabaseConnectionFailed = "فشل في الاتصال بقاعدة البيانات";
            public const string SerialPortError = "خطأ في الاتصال بالمنفذ التسلسلي";
            public const string ApiError = "خطأ في الاتصال بالخدمة";
            public const string InvalidWeight = "لا يمكن تحويل الوزن إلى رقم";
            public const string SettingsFileNotFound = "ملف الإعدادات غير موجود";
            public const string UsersFileNotFound = "ملف المستخدمين غير موجود";
        }

        // Success Messages
        public static class SuccessMessages
        {
            public const string DatabaseConnected = "تم الاتصال بقاعدة البيانات بنجاح";
            public const string WeightSaved = "تم حفظ الوزن بنجاح";
            public const string LoginSuccess = "تم تسجيل الدخول بنجاح";
            public const string DataLoaded = "تم تحميل البيانات بنجاح";
        }
    }
}

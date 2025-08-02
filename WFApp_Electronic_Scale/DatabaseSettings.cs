using System;

namespace WFApp_Electronic_Scale
{
    public static class DatabaseSettings
    {
        // إعدادات الاتصال بقاعدة البيانات
        public static string ServerName { get; set; } = "localhost";
        public static string DatabaseName { get; set; } = "WeightScaleDB";
        public static string Username { get; set; } = "sa";
        public static string Password { get; set; } = "your_password";
        public static bool UseWindowsAuthentication { get; set; } = true;

        // إعدادات الجدول
        public static string TableName { get; set; } = "Weights";
        public static int MaxHistoryRecords { get; set; } = 1000;

        // إعدادات التطبيق
        public static bool AutoSaveWeight { get; set; } = true;
        public static string DefaultWeightUnit { get; set; } = "KG";
        public static bool LogDatabaseOperations { get; set; } = true;

        /// <summary>
        /// الحصول على سلسلة الاتصال
        /// </summary>
        public static string GetConnectionString()
        {
            if (UseWindowsAuthentication)
            {
                return $"Server={ServerName};Database={DatabaseName};Integrated Security=true;";
            }
            else
            {
                return $"Server={ServerName};Database={DatabaseName};User Id={Username};Password={Password};";
            }
        }

        /// <summary>
        /// تحديث إعدادات الاتصال
        /// </summary>
        public static void UpdateConnectionSettings(string serverName, string databaseName, 
            string username = null, string password = null, bool useWindowsAuth = false)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UseWindowsAuthentication = useWindowsAuth;

            if (!useWindowsAuth)
            {
                Username = username ?? "sa";
                Password = password ?? "your_password";
            }
        }

        /// <summary>
        /// حفظ الإعدادات في ملف التكوين
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                Properties.Settings.Default.ServerName = ServerName;
                Properties.Settings.Default.DatabaseName = DatabaseName;
                Properties.Settings.Default.Username = Username;
                Properties.Settings.Default.Password = Password;
                Properties.Settings.Default.UseWindowsAuth = UseWindowsAuthentication;
                Properties.Settings.Default.AutoSaveWeight = AutoSaveWeight;
                Properties.Settings.Default.DefaultWeightUnit = DefaultWeightUnit;
                Properties.Settings.Default.MaxHistoryRecords = MaxHistoryRecords;
                Properties.Settings.Default.LogDatabaseOperations = LogDatabaseOperations;
                
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"خطأ في حفظ الإعدادات: {ex.Message}", 
                    "خطأ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// تحميل الإعدادات من ملف التكوين
        /// </summary>
        public static void LoadSettings()
        {
            try
            {
                ServerName = Properties.Settings.Default.ServerName ?? "localhost";
                DatabaseName = Properties.Settings.Default.DatabaseName ?? "WeightScaleDB";
                Username = Properties.Settings.Default.Username ?? "sa";
                Password = Properties.Settings.Default.Password ?? "your_password";
                UseWindowsAuthentication = Properties.Settings.Default.UseWindowsAuth;
                AutoSaveWeight = Properties.Settings.Default.AutoSaveWeight;
                DefaultWeightUnit = Properties.Settings.Default.DefaultWeightUnit ?? "KG";
                MaxHistoryRecords = Properties.Settings.Default.MaxHistoryRecords;
                LogDatabaseOperations = Properties.Settings.Default.LogDatabaseOperations;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"خطأ في تحميل الإعدادات: {ex.Message}", 
                    "خطأ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
} 
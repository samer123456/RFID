using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace WFApp_Electronic_Scale
{
    public class DatabaseManager : IDisposable
    {
        private readonly string connectionString;
        private static readonly object _lock = new object();
        private bool _disposed = false;

        public DatabaseManager()
        {
            // تحميل الإعدادات
            DatabaseSettings.LoadSettings();

            // استخدام الإعدادات المحملة مع تحسينات الاتصال
            connectionString = DatabaseSettings.GetConnectionString() +
                ";Connection Timeout=30;" +
                "Pooling=true;" +
                "Min Pool Size=1;" +
                "Max Pool Size=10;" +
                "Connection Lifetime=300;";
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في الاتصال بقاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في الاتصال بقاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        //todo 
        //IP:1023268102
        //port:8091
        //op  post tag, weight and plaka
        // get op
        //metro framework
        //public bool CreateDatabaseAndTable()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            // إنشاء جدول الأوزان إذا لم يكن موجوداً
        //            string createTableQuery = @"
        //                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Weights' AND xtype='U')
        //                CREATE TABLE Weights (
        //                    Id INT IDENTITY(1,1) PRIMARY KEY,
        //                    Weight DECIMAL(10,3) NOT NULL,
        //                    WeightUnit NVARCHAR(10) DEFAULT 'KG',
        //                    ReadingTime DATETIME DEFAULT GETDATE(),
        //                    UserId NVARCHAR(50),
        //                    UserName NVARCHAR(100),
        //                    City NVARCHAR(100),
        //                    Notes NVARCHAR(500)
        //                )";

        //            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
        //            {
        //                command.ExecuteNonQuery();
        //            }

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"خطأ في إنشاء قاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //}



        public async Task<bool> SaveWeightAsync(int weight, string userId = null,
     string userName = null, string city = null, string kart = null, string plaka = null)
        {
            var tableName = DatabaseSettings.TableName;
            var kantar = DatabaseSettings.Kantar;
            var acklama = DatabaseSettings.Acklama;
            var kullanici = DatabaseSettings.Kullanici;

            try
            {
                using (var db = new ScaleDbContext(connectionString, tableName))
                {
                    if (tableName.ToLower().Equals("tartim1"))
                    {
                        var record = new WeightRecord1
                        {
                            Kart = string.IsNullOrWhiteSpace(kart) ? null : kart,
                            Plaka = string.IsNullOrWhiteSpace(plaka) ? null : plaka,
                            Tartim1 = weight,
                            Tarih1 = DateTime.UtcNow,
                            Saat1 = DateTime.UtcNow.TimeOfDay.ToString(),
                            Sorgu1 = "todo",
                            Sorgu2 = "todo",
                            Kullanici1 = string.IsNullOrEmpty(userName) ? userName : kullanici,
                            Aciklama1 = acklama,
                            Kantar1 = kantar
                        };

                        db.Weights1.Add(record);
                    }
                    else
                    {

                        var record = new WeightRecord2
                        {
                            // No =   ,
                            Kart = string.IsNullOrWhiteSpace(kart) ? null : kart,
                            Plaka = string.IsNullOrWhiteSpace(plaka) ? null : plaka,
                            //Tartim1 = weight,
                            Tartim2 = weight,
                            //Net = ,
                           // Tarih1 = ,
                            //Saat1 = ,
                            Tarih2 = DateTime.UtcNow,
                            Saat2 = DateTime.UtcNow.TimeOfDay.ToString(),
                            Sorgu1 = "todo",
                            Sorgu2 = "todo",
                            Kullanici2 = string.IsNullOrEmpty(userName) ? userName : kullanici,
                            //Kullanici1 = ,
                            Aciklama2 = acklama,
                            Aciklama1 = acklama,
                            Kantar2 = kantar,
                            //Kantar1 = kantar
                        };

                        db.Weights2.Add(record);
                    }

                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ الوزن: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable GetWeightsHistory(DateTime? fromDate = null, DateTime? toDate = null,
            string userId = null, int limit = 100)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Use parameterized query with StringBuilder for better performance
                    var queryBuilder = new System.Text.StringBuilder();
                    queryBuilder.Append("SELECT TOP (@Limit) ");
                    queryBuilder.Append("Id, Weight, ReadingTime, UserId, UserName, City, NoPlate, LetterPlate ");
                    queryBuilder.Append("FROM Weights WHERE 1=1");

                    if (fromDate.HasValue)
                        queryBuilder.Append(" AND ReadingTime >= @FromDate");
                    if (toDate.HasValue)
                        queryBuilder.Append(" AND ReadingTime <= @ToDate");
                    if (!string.IsNullOrEmpty(userId))
                        queryBuilder.Append(" AND UserId = @UserId");

                    queryBuilder.Append(" ORDER BY ReadingTime DESC");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@Limit", limit);

                        if (fromDate.HasValue)
                            command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                        if (toDate.HasValue)
                            command.Parameters.AddWithValue("@ToDate", toDate.Value);
                        if (!string.IsNullOrEmpty(userId))
                            command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في جلب سجل الأوزان: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public async Task<DataTable> GetWeightsHistoryAsync(DateTime? fromDate = null, DateTime? toDate = null,
            string userId = null, int limit = 100)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new System.Text.StringBuilder();
                    queryBuilder.Append("SELECT TOP (@Limit) ");
                    queryBuilder.Append("Id, Weight, ReadingTime, UserId, UserName, City, NoPlate, LetterPlate ");
                    queryBuilder.Append("FROM Weights WHERE 1=1");

                    if (fromDate.HasValue)
                        queryBuilder.Append(" AND ReadingTime >= @FromDate");
                    if (toDate.HasValue)
                        queryBuilder.Append(" AND ReadingTime <= @ToDate");
                    if (!string.IsNullOrEmpty(userId))
                        queryBuilder.Append(" AND UserId = @UserId");

                    queryBuilder.Append(" ORDER BY ReadingTime DESC");

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@Limit", limit);

                        if (fromDate.HasValue)
                            command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                        if (toDate.HasValue)
                            command.Parameters.AddWithValue("@ToDate", toDate.Value);
                        if (!string.IsNullOrEmpty(userId))
                            command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            await Task.Run(() => adapter.Fill(dataTable));
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في جلب سجل الأوزان: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }



        public async Task<bool> DeleteWeightAsync(int weightId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string deleteQuery = "DELETE FROM Weights WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", weightId);
                        await command.ExecuteNonQueryAsync();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حذف الوزن: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here if any
                }
                _disposed = true;
            }
        }
    }
}
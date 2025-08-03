using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager()
        {
            // تحميل الإعدادات
            DatabaseSettings.LoadSettings();
            
            // استخدام الإعدادات المحملة
            connectionString = DatabaseSettings.GetConnectionString();
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


        //todo 
        //IP:1023268102
        //port:8091
        //op  post tag, weight and plaka
        // get op
        //metro framework
        public bool CreateDatabaseAndTable()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // إنشاء جدول الأوزان إذا لم يكن موجوداً
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Weights' AND xtype='U')
                        CREATE TABLE Weights (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Weight DECIMAL(10,3) NOT NULL,
                            WeightUnit NVARCHAR(10) DEFAULT 'KG',
                            ReadingTime DATETIME DEFAULT GETDATE(),
                            UserId NVARCHAR(50),
                            UserName NVARCHAR(100),
                            City NVARCHAR(100),
                            Notes NVARCHAR(500)
                        )";

                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إنشاء قاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool SaveWeight(decimal weight, string weightUnit = null, string userId = null, 
            string userName = null, string city = null, string notes = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = @"
                        INSERT INTO Weights (Weight, WeightUnit, ReadingTime, UserId, UserName, City, Notes)
                        VALUES (@Weight, @WeightUnit, @ReadingTime, @UserId, @UserName, @City, @Notes)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Weight", weight);
                        command.Parameters.AddWithValue("@WeightUnit", weightUnit ?? DatabaseSettings.DefaultWeightUnit);
                        command.Parameters.AddWithValue("@ReadingTime", DateTime.Now);
                        command.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@UserName", (object)userName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@City", (object)city ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);

                        command.ExecuteNonQuery();
                    }

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

                    string selectQuery = @"
                        SELECT TOP (@Limit) 
                            Id, Weight, WeightUnit, ReadingTime, UserId, UserName, City, Notes
                        FROM Weights 
                        WHERE 1=1";

                    if (fromDate.HasValue)
                        selectQuery += " AND ReadingTime >= @FromDate";
                    if (toDate.HasValue)
                        selectQuery += " AND ReadingTime <= @ToDate";
                    if (!string.IsNullOrEmpty(userId))
                        selectQuery += " AND UserId = @UserId";

                    selectQuery += " ORDER BY ReadingTime DESC";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
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

        public bool DeleteWeight(int weightId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Weights WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", weightId);
                        command.ExecuteNonQuery();
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
    }
} 
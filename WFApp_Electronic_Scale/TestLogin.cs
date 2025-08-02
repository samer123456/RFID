using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    public static class TestLogin
    {
        public static void TestUserValidation()
        {
            try
            {
                string usersFilePath = "users.json";
                if (File.Exists(usersFilePath))
                {
                    string json = File.ReadAllText(usersFilePath);
                    var users = JsonConvert.DeserializeObject<List<User>>(json);
                    
                    string userInfo = "=== اختبار بيانات المستخدمين ===\n";
                    foreach (var user in users)
                    {
                        userInfo += $"المستخدم: {user.Username}, النوع: {user.UserType}\n";
                    }
                    
                    MessageBox.Show(userInfo, "اختبار المستخدمين", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ملف المستخدمين غير موجود!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في اختبار المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 
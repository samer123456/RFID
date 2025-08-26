using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show($"Unhandled UI exception: {e.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    MessageBox.Show($"Unhandled exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // عرض شاشة ترحيبية قصيرة
            using (var splash = new SplashForm())
            {
                splash.ShowDialog();
            }

            // عرض نموذج تسجيل الدخول أولاً
            using (var loginForm = new LoginForm())
            {
                var result = loginForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        Application.Run(new Form1());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"فشل تشغيل الواجهة الرئيسية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // لا تغلق بصمت، أعرض سبب الإغلاق
                    MessageBox.Show("تم إلغاء تسجيل الدخول.", "إغلاق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}

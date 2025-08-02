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
            
            // عرض نموذج تسجيل الدخول أولاً
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // إذا كان تسجيل الدخول ناجح، قم بتشغيل التطبيق الرئيسي
                    Application.Run(new Form1());
                }
                else
                {
                    // إذا تم إلغاء تسجيل الدخول، أغلق التطبيق
                    Application.Exit();
                }
            }
        }
    }
}

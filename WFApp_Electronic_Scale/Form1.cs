using System;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Diagnostics;
using MetroFramework;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework.Components;
using System.Drawing;
using System.Threading.Tasks;


namespace WFApp_Electronic_Scale
{
    public partial class Form1 : MetroForm
    {
        private MetroStyleManager metroStyleManager;
        private MetroButton btnSettings;
        private MetroProgressSpinner metroProgressSpinner;
        private SerialPort port;
        private DatabaseManager dbManager;
        //SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        string logFilePath = "log.txt";
        private string ReadData = "";
        public Form1()
        {
            InitializeComponent();
            InitializeDefaults();
            MetroStyleManager metroStyleManager = new MetroStyleManager(this.Container);
            metroStyleManager.Theme = MetroThemeStyle.Light;
            metroStyleManager.Style = MetroColorStyle.Blue;
            btnTestLogin.Visible = false;
            CheckUserPermissions();
            port = new SerialPort();
            port.DataReceived += Port_DataReceived;

            // تحميل إعدادات قاعدة البيانات
            DatabaseSettings.LoadSettings();

            // تهيئة مدير قاعدة البيانات
            dbManager = new DatabaseManager();

            // إنشاء قاعدة البيانات والجدول إذا لم تكن موجودة
            if (dbManager.TestConnection())
            {
                dbManager.CreateDatabaseAndTable();
                Log("تم الاتصال بقاعدة البيانات بنجاح");
            }
            else
            {
                Log("فشل في الاتصال بقاعدة البيانات");
            }
        }

        private void InitializeDefaults()
        {
           

            cmbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cmbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                cmbLetters.Items.Add(letter.ToString());
            }

            //for (char letter = 'أ'; letter <= 'ي'; letter++)
            //{
            //    cmbLetters.Items.Add(letter.ToString());
            //}

            string arabicLetters = "أبجدهوزحطيكلمنسعفصقرشتثخذضظغ";
            foreach (char letter in arabicLetters)
            {
                cmbLetters.Items.Add(letter.ToString());
            }
            cmbParity.SelectedItem = Parity.None.ToString();// "None";
            cmbStopBits.SelectedItem = StopBits.One.ToString();// "One";
            txtPort.Text = "COM3";
            txtBaudRate.Text = "9600";
            txtDataBits.Text = "8";
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string portName = string.IsNullOrWhiteSpace(txtPort.Text) ? "COM3" : txtPort.Text;

                int baudRate = 9600;
                if (!string.IsNullOrWhiteSpace(txtBaudRate.Text) && int.TryParse(txtBaudRate.Text, out int parsedBaud))
                {
                    baudRate = parsedBaud;
                }

                Parity parity = Parity.None;
                if (Enum.TryParse(cmbParity.SelectedItem.ToString(), out Parity parsedParity))
                {
                    parity = parsedParity;
                }

                int dataBits = 8;
                if (!string.IsNullOrWhiteSpace(txtDataBits.Text) && int.TryParse(txtDataBits.Text, out int parsedBits))
                {
                    dataBits = parsedBits;
                }

                StopBits stopBits = StopBits.One;
                if (Enum.TryParse(cmbStopBits.SelectedItem.ToString(), out StopBits parsedStopBits))
                {
                    stopBits = parsedStopBits;
                }

                port.PortName = portName;
                port.BaudRate = baudRate;
                port.Parity = parity;
                port.DataBits = dataBits;
                port.StopBits = stopBits;
                // MessageBox.Show($"Port Configured: {port.PortName}, {port.BaudRate}, {port.Parity}, {port.DataBits}, {port.StopBits}");

                try
                {
                    //port.ReadTimeout = 3000;
                    //port.ReceivedBytesThreshold = 40;
                    port.Open();
                    Log("port is oppen");
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();

                    while (string.IsNullOrWhiteSpace(ReadData))
                    {
                        VeriTalepGonder();
                        System.Threading.Thread.Sleep(1000); // cihazın cevap vermesi için bekleme
                    }
                    //Log("Closing port...");
                    //port.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (port.IsOpen)
                    {
                        port.Close();
                        Log("Closed port...");
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }
        }

        private void btnSendTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                char CHR = Convert.ToChar(2);
                string STRG1 = CHR.ToString();
                CHR = Convert.ToChar(1);
                string STRG2 = CHR.ToString();
                string STRG3 = "DNG";
                CHR = Convert.ToChar(13);
                string STRG4 = CHR.ToString();
                if (!string.IsNullOrWhiteSpace(txtCommand.Text))
                {
                    STRG3 = txtCommand.Text;
                }

                //Log($"STX: {Convert.ToInt32(STRG1[0])}, SOH: {Convert.ToInt32(STRG2[0])}, {STRG3}, CR: {Convert.ToInt32(STRG4[0])}");
                //txtWeight.Text = (STRG1 + STRG2 + STRG3 + STRG4);
                //txtWeight.Text = $"STX: {Convert.ToInt32(STRG1[0])}, SOH: {Convert.ToInt32(STRG2[0])}, {STRG3}, CR: {Convert.ToInt32(STRG4[0])}";
                port.Write(STRG1 + STRG2 + STRG3 + STRG4);

                string data = port.ReadExisting();
                // Log("ReadExisting" + port.ReadExisting());
                // Log("ReadBufferSize" + port.ReadBufferSize); // 4096 حجم البفر
                // Log("ReadLine" + port.ReadLine());
                // Log("BytesToRead" + port.BytesToRead);
                // Log("ReceivedBytesThreshold" + port.ReceivedBytesThreshold); // 1 يعني أن الحدث DataReceived سيتم إطلاقه بمجرد استلام أول بايت من البيانات
                // Log("ReadTimeout" + port.ReadTimeout); // -1  انتظر للأبد حتى تصلك البيانات
            }

            catch (Exception EX)
            {
                MessageBox.Show("Error : " + EX.Message);
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (port.IsOpen && port.BytesToRead != 0)
            {
                ReadData = port.ReadExisting();
                string weight = ReadWeight(ReadData);
                Invoke(new Action(() =>
                {
                    txtWeight.Text = weight;
                    Log("received from Port_DataReceived string: " + weight);
                }));
            }
        }
        private string ReadWeight(string weight)
        {
            try
            {
                //foreach (char c in weight)
                //{
                //    Log($"{c} - {(int)c}");
                //}
                //if (port.IsOpen /*&& port.BytesToRead != 0*/)
                //{
                // weight += port.ReadExisting();
                Log("weight:" + weight);
                Log("weight Lenght:" + weight.Length.ToString());

                if (weight == Convert.ToChar(5).ToString())
                {
                    txtWeight.Text = "WRONG WEIGHING!  ";
                    string ilk_satir = "WRONG WEIGHING! ";
                    Thread.Sleep(500);
                    ReadData = "";
                }
                if (weight == Convert.ToChar(6).ToString())
                {
                    Thread.Sleep(500);
                    ReadData = "";
                }
                if (weight.Length >= 41)
                {
                    char[] array = weight.ToCharArray(0, 41);
                    if (array[0] == Convert.ToChar(2) && (array[40] == Convert.ToChar(13) || array[40] == Convert.ToChar(3)))
                    {
                        ReadData = weight.Substring(26, 6);
                        txtWeight.Text = ReadData;

                        // حفظ الوزن في قاعدة البيانات
                        SaveWeightToDatabase(ReadData);

                        //ReadData = "";
                        port.Write(Convert.ToChar(4).ToString());
                    }
                }
                // }
                return ReadData.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void SaveWeightToDatabase(string weightString)
        {
            try
            {
                // التحقق من إعداد الحفظ التلقائي
                if (!DatabaseSettings.AutoSaveWeight)
                {
                    Log("الحفظ التلقائي معطل في الإعدادات");
                    return;
                }

                // تحويل النص إلى رقم عشري
                if (decimal.TryParse(weightString, out decimal weight))
                {
                    // الحصول على معلومات المستخدم الحالي
                    string userId = LoginForm.CurrentUser?.UserId ?? "";
                    string userName = LoginForm.CurrentUser?.Username ?? "";
                    string city = cmbCities.SelectedItem?.ToString() ?? "";

                    // حفظ الوزن في قاعدة البيانات
                    if (dbManager.SaveWeight(weight, userId, userName, city))
                    {
                        if (DatabaseSettings.LogDatabaseOperations)
                        {
                            /*{DatabaseSettings.DefaultWeightUnit}*/
                            Log($"تم حفظ الوزن: {weight}  في قاعدة البيانات");
                        }
                    }
                    else
                    {
                        Log("فشل في حفظ الوزن في قاعدة البيانات");
                    }
                }
                else
                {
                    Log($"لا يمكن تحويل الوزن '{weightString}' إلى رقم");
                }
            }
            catch (Exception ex)
            {
                Log($"خطأ في حفظ الوزن: {ex.Message}");
            }
        }

        //private void Log(string message)
        //{
        //    string logEntry = DateTime.Now + " - " + message + "\n";
        //    txtLog.AppendText(logEntry + Environment.NewLine);
        //    File.AppendAllText(logFilePath, logEntry);
        //}

      
        private void Log(string message)
        {
            string logEntry = DateTime.Now + " - " + message + "\n";

            // إضافة سطر جديد مع تمييز الأخطاء
            if (message.Contains("Error") || message.Contains("خطأ") || message.Contains("فشل"))
            {
                txtLog.BackColor = Color.FromArgb(255, 230, 230);
                txtLog.AppendText(logEntry, Color.Red);
            }
            else
            {
                txtLog.BackColor = Color.FromArgb(240, 240, 240);
                txtLog.AppendText(logEntry, Color.Black);
            }

            File.AppendAllText(logFilePath, logEntry);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (port != null && port.IsOpen == true)
            {
                port.Close();
                Log("Port closed.");
            }
        }

        private void VeriTalepGonder()
        {
            try
            {
                char CHR = Convert.ToChar(2);
                string STRG1 = CHR.ToString();
                CHR = Convert.ToChar(1);
                string STRG2 = CHR.ToString();
                string STRG3 = "DNG";
                CHR = Convert.ToChar(13);
                string STRG4 = CHR.ToString();
                port.Write(STRG1 + STRG2 + STRG3 + STRG4);
            }
            catch (Exception EX)
            {
                MessageBox.Show("Error : " + EX.Message);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            CityLoader cityLoader = new CityLoader(cmbCities);
            //await cityLoader.LoadCitiesAsync();

            // اختبار بيانات المستخدمين (مؤقت للتصحيح)
            //TestLogin.TestUserValidation();

            // التحقق من نوع المستخدم وعرض/إخفاء زر إدارة المستخدمين
            // CheckUserPermissions();
        }

        private void CheckUserPermissions()
        {
            try
            {
                string debugInfo = "";

                if (LoginForm.CurrentUser != null)
                {
                    debugInfo = $"المستخدم الحالي: {LoginForm.CurrentUser.Username}\nالنوع: {LoginForm.CurrentUser.UserType}";

                    // عرض معلومات المستخدم في العنوان (اختياري)
                    //this.Text = $"{LoginForm.CurrentUser.Username} ({LoginForm.CurrentUser.UserType})";
                    this.metroLabelInfo.Text = $"{LoginForm.CurrentUser.Username} مرحباُ بك";
                    // إظهار زر إدارة المستخدمين للمديرين فقط
                    if (LoginForm.CurrentUser.UserType == "Admin")
                    {
                        btnManageUsers.Visible = true;
                        btnManageUsers.Enabled = true;
                        btnViewHistory.Visible = true;
                        btnViewHistory.Enabled = true;
                        btnStart.Enabled = true;
                        cmbLetters.Enabled = true;
                        cmbStopBits.Enabled = true;
                        txtDataBits.Enabled = true;
                        cmbParity.Enabled = true;
                        txtBaudRate.Enabled = true;
                        btnTestLogin.Visible = false;
                        txtCommand.Enabled = true;
                        btnSendTrigger.Enabled = true;
                        txtPort.Enabled = true;
                        cmbCities.Enabled = true;
                        btnManageUsers.Style = MetroColorStyle.Green;
                        btnManageUsers.Theme = MetroThemeStyle.Light;
                        btnManageUsers.BackColor = Color.Red;

                        //debugInfo += "\n✅ تم إظهار زر إدارة المستخدمين";
                    }
                    else
                    {
                        btnManageUsers.Visible = false;
                        btnManageUsers.Enabled = false;
                        btnViewHistory.Visible = false;
                        btnViewHistory.Enabled = false;
                        txtPort.Enabled = false;
                        btnStart.Enabled = true;
                        cmbStopBits.Enabled = false;
                        txtDataBits.Enabled = false;
                        cmbParity.Enabled = false;
                        txtBaudRate.Enabled = false;
                        btnTestLogin.Visible = false;
                        txtCommand.Visible = false;
                        btnSendTrigger.Visible = false;
                        cmbLetters.Visible = false;
                        cmbCities.Visible = false;
                        this.Style = MetroColorStyle.Orange;
                        txtPort.Style = MetroColorStyle.Orange;
                        gbSetting.Visible = false;
                        gbSetting.Enabled = false;
                        gbPort.Visible = true;
                        gbPort.Enabled = false;
                        gbtrigger.Visible = false;
                        gbtrigger.Enabled = false;
                        // debugInfo += "\n❌ تم إخفاء زر إدارة المستخدمين (ليس مدير)";
                    }
                }
                else
                {
                    debugInfo = "❌ لا يوجد مستخدم مسجل دخوله";
                    // إذا لم يكن هناك مستخدم مسجل دخوله، إخفاء الزر
                    btnManageUsers.Visible = false;
                    btnManageUsers.Enabled = false;
                    // عرض معلومات التصحيح
                    MessageBox.Show(debugInfo, "معلومات التصحيح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، إخفاء الزر للسلامة
                btnManageUsers.Visible = false;
                btnManageUsers.Enabled = false;
                Log($"خطأ في التحقق من صلاحيات المستخدم: {ex.Message}");
                MessageBox.Show($"خطأ في التحقق من الصلاحيات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            try
            {
                // عرض معلومات المستخدم الحالي للتأكد
                string userInfo = LoginForm.CurrentUser != null
                    ? $"المستخدم الحالي: {LoginForm.CurrentUser.Username} - النوع: {LoginForm.CurrentUser.UserType}"
                    : "لا يوجد مستخدم مسجل دخوله";

                MessageBox.Show(userInfo, "معلومات المستخدم", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (var userManagementForm = new UserManagementForm())
                {
                    userManagementForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في فتح نافذة إدارة المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // اختبار بيانات المستخدمين
                TestLogin.TestUserValidation();

                // اختبار المستخدم الحالي
                string currentUserInfo = LoginForm.CurrentUser != null
                    ? $"المستخدم الحالي: {LoginForm.CurrentUser.Username} - النوع: {LoginForm.CurrentUser.UserType}"
                    : "لا يوجد مستخدم مسجل دخوله";

                MessageBox.Show(currentUserInfo, "اختبار المستخدم الحالي", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // إعادة التحقق من الصلاحيات
                CheckUserPermissions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في اختبار تسجيل الدخول: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (var historyForm = new WeightHistoryForm())
                {
                    historyForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في فتح نافذة سجل الأوزان: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSettingsMenu()
        {
            ContextMenuStrip settingsMenu = new ContextMenuStrip();

            ToolStripMenuItem themeItem = new ToolStripMenuItem("تغيير السمة");
            themeItem.DropDownItems.Add("فاتحة", null, (s, e) => metroStyleManager.Theme = MetroThemeStyle.Light);
            themeItem.DropDownItems.Add("داكنة", null, (s, e) => metroStyleManager.Theme = MetroThemeStyle.Dark);

            ToolStripMenuItem colorItem = new ToolStripMenuItem("تغيير اللون");
            colorItem.DropDownItems.Add("أزرق", null, (s, e) => metroStyleManager.Style = MetroColorStyle.Blue);
            colorItem.DropDownItems.Add("أخضر", null, (s, e) => metroStyleManager.Style = MetroColorStyle.Green);
            colorItem.DropDownItems.Add("أحمر", null, (s, e) => metroStyleManager.Style = MetroColorStyle.Red);

            settingsMenu.Items.Add(themeItem);
            settingsMenu.Items.Add(colorItem);

            btnSettings.ContextMenuStrip = settingsMenu;
        }
        private void ShowProgress(bool show)
        {
            metroProgressSpinner.Visible = show;
            metroProgressSpinner.Spinning = show;
        }

    









        //// استخدامها في العمليات
        //ShowProgress(true);
        //await Task.Delay(1000); // عملية مثلا
        //ShowProgress(false);

        //    MetroNotification.Show(
        //this,
        //"تم الإتصال بالميزان بنجاح",
        //"النظام",
        //MessageBoxButtons.OK,
        //icon: MessageBoxIcon.Information);
    }
}

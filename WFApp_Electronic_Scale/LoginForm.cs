using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace WFApp_Electronic_Scale
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
        private Label lblUsername;
        private Label lblPassword;
        private string usersFilePath = "users.json";
        public static User CurrentUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            InitializeUsersFile();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.btnCancel = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();

            // Form
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "تسجيل الدخول";
            this.ResumeLayout(false);
            this.PerformLayout();

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(70, 13);
            this.lblUsername.Text = "اسم المستخدم:";


            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(20, 40);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(260, 20);
            this.txtUsername.TabIndex = 0;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 70);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 13);
            this.lblPassword.Text = "كلمة المرور:";

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(20, 90);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(260, 20);
            this.txtPassword.TabIndex = 1;

            // btnLogin
            this.btnLogin.Location = new System.Drawing.Point(20, 130);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(120, 30);
            this.btnLogin.Text = "تسجيل الدخول";
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            this.btnLogin.TabIndex = 2;


            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(160, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnCancel.TabIndex = 3;

            this.AcceptButton = this.btnLogin;
            this.CancelButton = this.btnCancel;
        }

        private void InitializeUsersFile()
        {
            if (!File.Exists(usersFilePath))
            {
                var defaultUsers = new List<User>
                {
                    new User { Username = "admin", Password = "admin123", UserType = "Admin" },
                    new User { Username = "user", Password = "user123", UserType = "User" }
                };

                string json = JsonConvert.SerializeObject(defaultUsers, Formatting.Indented);
                File.WriteAllText(usersFilePath, json);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateUser(username, password))
            {
                // رسالة تأكيد للمدير
                if (CurrentUser?.UserType == "Admin")
                {

                    MessageBox.Show($"مرحباً بك {CurrentUser.Username}! تم تسجيل الدخول كمدير.", "تسجيل دخول ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"مرحباً بك {CurrentUser?.Username}!", "تسجيل دخول ناجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                // رسالة تأكيد إضافية للتأكد من تعيين المستخدم
               // MessageBox.Show($"تم تعيين المستخدم: {CurrentUser?.Username} - النوع: {CurrentUser?.UserType}", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateUser(string username, string password)
        {
            try
            {
                if (!File.Exists(usersFilePath))
                {
                    return false;
                }

                string json = File.ReadAllText(usersFilePath);
                var users = JsonConvert.DeserializeObject<List<User>>(json);

                var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                                                   u.Password.Equals(password, StringComparison.Ordinal));
                
                if (user != null)
                {
                    CurrentUser = user;
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في قراءة ملف المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
} 
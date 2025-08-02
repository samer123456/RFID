using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace WFApp_Electronic_Scale
{
    public partial class UserManagementForm : Form
    {
        private ListBox lstUsers;
        private TextBox txtNewUsername;
        private TextBox txtNewPassword;
        private ComboBox cmbUserType;
        private Button btnAddUser;
        private Button btnDeleteUser;
        private Button btnClose;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblUserType;
        private string usersFilePath = "users.json";
        private List<User> users;

        public UserManagementForm()
        {
            InitializeComponent();
            
            // التحقق من أن المستخدم الحالي هو مدير
            if (LoginForm.CurrentUser?.UserType != "Admin")
            {
                MessageBox.Show("عذراً، هذه الصفحة متاحة للمديرين فقط", "غير مصرح", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.lstUsers = new ListBox();
            this.txtNewUsername = new TextBox();
            this.txtNewPassword = new TextBox();
            this.cmbUserType = new ComboBox();
            this.btnAddUser = new Button();
            this.btnDeleteUser = new Button();
            this.btnClose = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblUserType = new Label();

            // Form
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.lblUserType);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.cmbUserType);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtNewUsername);
            this.Controls.Add(this.lstUsers);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserManagementForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "إدارة المستخدمين";
            this.ResumeLayout(false);
            this.PerformLayout();

            // lstUsers
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(20, 20);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(360, 150);
            this.lstUsers.SelectedIndexChanged += new EventHandler(this.lstUsers_SelectedIndexChanged);

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 190);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(70, 13);
            this.lblUsername.Text = "اسم المستخدم:";

            // txtNewUsername
            this.txtNewUsername.Location = new System.Drawing.Point(20, 210);
            this.txtNewUsername.Name = "txtNewUsername";
            this.txtNewUsername.Size = new System.Drawing.Size(360, 20);

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 240);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 13);
            this.lblPassword.Text = "كلمة المرور:";

            // txtNewPassword
            this.txtNewPassword.Location = new System.Drawing.Point(20, 260);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(360, 20);

            // lblUserType
            this.lblUserType.AutoSize = true;
            this.lblUserType.Location = new System.Drawing.Point(20, 290);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(70, 13);
            this.lblUserType.Text = "نوع المستخدم:";

            // cmbUserType
            this.cmbUserType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUserType.FormattingEnabled = true;
            this.cmbUserType.Location = new System.Drawing.Point(20, 310);
            this.cmbUserType.Name = "cmbUserType";
            this.cmbUserType.Size = new System.Drawing.Size(360, 24);
            this.cmbUserType.Items.AddRange(new object[] { "User", "Admin" });
            this.cmbUserType.SelectedIndex = 0;

            // btnAddUser
            this.btnAddUser.Location = new System.Drawing.Point(20, 350);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(100, 30);
            this.btnAddUser.Text = "إضافة مستخدم";
            this.btnAddUser.Click += new EventHandler(this.btnAddUser_Click);

            // btnDeleteUser
            this.btnDeleteUser.Location = new System.Drawing.Point(140, 350);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteUser.Text = "حذف مستخدم";
            this.btnDeleteUser.Click += new EventHandler(this.btnDeleteUser_Click);

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(280, 350);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.Text = "إغلاق";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
        }

        private void LoadUsers()
        {
            try
            {
                if (File.Exists(usersFilePath))
                {
                    string json = File.ReadAllText(usersFilePath);
                    users = JsonConvert.DeserializeObject<List<User>>(json);
                }
                else
                {
                    users = new List<User>();
                }

                RefreshUserList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في قراءة ملف المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                users = new List<User>();
            }
        }

        private void RefreshUserList()
        {
            lstUsers.Items.Clear();
            foreach (var user in users)
            {
                lstUsers.Items.Add($"{user.Username} ({user.UserType}) - ****");
            }
        }

        private void SaveUsers()
        {
            try
            {
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(usersFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ ملف المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text.Trim();
            string userType = cmbUserType.SelectedItem?.ToString() ?? "User";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("اسم المستخدم موجود بالفعل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            users.Add(new User { Username = username, Password = password, UserType = userType });
            SaveUsers();
            RefreshUserList();

            txtNewUsername.Clear();
            txtNewPassword.Clear();
            cmbUserType.SelectedIndex = 0;
            txtNewUsername.Focus();

            MessageBox.Show("تم إضافة المستخدم بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار مستخدم للحذف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedUser = users[lstUsers.SelectedIndex];
            
            if (MessageBox.Show($"هل أنت متأكد من حذف المستخدم '{selectedUser.Username}'؟", "تأكيد الحذف", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                users.RemoveAt(lstUsers.SelectedIndex);
                SaveUsers();
                RefreshUserList();
                MessageBox.Show("تم حذف المستخدم بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteUser.Enabled = lstUsers.SelectedIndex != -1;
        }
    }
} 
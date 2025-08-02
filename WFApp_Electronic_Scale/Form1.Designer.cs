using System.IO.Ports;
using System;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtBaudRate;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.TextBox txtDataBits;
        private System.Windows.Forms.ComboBox cmbStopBits;
        private System.Windows.Forms.ComboBox cmbLetters;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSendTrigger;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ComboBox cmbCities;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnTestLogin;
        private System.Windows.Forms.Button btnViewHistory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtBaudRate = new System.Windows.Forms.TextBox();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.txtDataBits = new System.Windows.Forms.TextBox();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSendTrigger = new System.Windows.Forms.Button();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cmbLetters = new System.Windows.Forms.ComboBox();
            this.cmbCities = new System.Windows.Forms.ComboBox();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnTestLogin = new System.Windows.Forms.Button();
            this.btnViewHistory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(30, 34);
            this.txtPort.MaximumSize = new System.Drawing.Size(200, 30);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(200, 22);
            this.txtPort.TabIndex = 0;
            // 
            // txtBaudRate
            // 
            this.txtBaudRate.Location = new System.Drawing.Point(236, 34);
            this.txtBaudRate.Name = "txtBaudRate";
            this.txtBaudRate.Size = new System.Drawing.Size(200, 22);
            this.txtBaudRate.TabIndex = 1;
            // 
            // cmbParity
            // 
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Location = new System.Drawing.Point(442, 34);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(200, 24);
            this.cmbParity.TabIndex = 2;
            // 
            // txtDataBits
            // 
            this.txtDataBits.Location = new System.Drawing.Point(648, 34);
            this.txtDataBits.Name = "txtDataBits";
            this.txtDataBits.Size = new System.Drawing.Size(200, 22);
            this.txtDataBits.TabIndex = 3;
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopBits.FormattingEnabled = true;
            this.cmbStopBits.Location = new System.Drawing.Point(854, 34);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.Size = new System.Drawing.Size(200, 24);
            this.cmbStopBits.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(1061, 34);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(205, 32);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Open Port";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSendTrigger
            // 
            this.btnSendTrigger.Location = new System.Drawing.Point(238, 70);
            this.btnSendTrigger.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendTrigger.Name = "btnSendTrigger";
            this.btnSendTrigger.Size = new System.Drawing.Size(200, 32);
            this.btnSendTrigger.TabIndex = 7;
            this.btnSendTrigger.Text = "Send Trigger";
            this.btnSendTrigger.UseVisualStyleBackColor = true;
            this.btnSendTrigger.Click += new System.EventHandler(this.btnSendTrigger_Click);
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(30, 70);
            this.txtCommand.Margin = new System.Windows.Forms.Padding(4);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(200, 22);
            this.txtCommand.TabIndex = 6;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(30, 119);
            this.txtWeight.Margin = new System.Windows.Forms.Padding(4);
            this.txtWeight.Multiline = true;
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWeight.Size = new System.Drawing.Size(1236, 148);
            this.txtWeight.TabIndex = 8;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(30, 275);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1236, 167);
            this.txtLog.TabIndex = 9;
            // 
            // cmbLetters
            // 
            this.cmbLetters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLetters.FormattingEnabled = true;
            this.cmbLetters.Location = new System.Drawing.Point(463, 77);
            this.cmbLetters.Name = "cmbLetters";
            this.cmbLetters.Size = new System.Drawing.Size(200, 24);
            this.cmbLetters.TabIndex = 10;
            // 
            // cmbCities
            // 
            this.cmbCities.FormattingEnabled = true;
            this.cmbCities.Location = new System.Drawing.Point(670, 77);
            this.cmbCities.Name = "cmbCities";
            this.cmbCities.Size = new System.Drawing.Size(178, 24);
            this.cmbCities.TabIndex = 11;
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Location = new System.Drawing.Point(854, 77);
            this.btnManageUsers.Margin = new System.Windows.Forms.Padding(4);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(150, 32);
            this.btnManageUsers.TabIndex = 12;
            this.btnManageUsers.Text = "إدارة المستخدمين";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnTestLogin
            // 
            this.btnTestLogin.Location = new System.Drawing.Point(1012, 79);
            this.btnTestLogin.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestLogin.Name = "btnTestLogin";
            this.btnTestLogin.Size = new System.Drawing.Size(96, 32);
            this.btnTestLogin.TabIndex = 13;
            this.btnTestLogin.Text = "اختبار تسجيل الدخول";
            this.btnTestLogin.UseVisualStyleBackColor = true;
            this.btnTestLogin.Click += new System.EventHandler(this.btnTestLogin_Click);
            // 
            // btnViewHistory
            // 
            this.btnViewHistory.Location = new System.Drawing.Point(1116, 79);
            this.btnViewHistory.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Size = new System.Drawing.Size(150, 32);
            this.btnViewHistory.TabIndex = 14;
            this.btnViewHistory.Text = "سجل الأوزان";
            this.btnViewHistory.UseVisualStyleBackColor = true;
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 455);
            this.Controls.Add(this.btnViewHistory);
            this.Controls.Add(this.btnTestLogin);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.cmbCities);
            this.Controls.Add(this.cmbLetters);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtBaudRate);
            this.Controls.Add(this.cmbParity);
            this.Controls.Add(this.txtDataBits);
            this.Controls.Add(this.cmbStopBits);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.btnSendTrigger);
            this.Controls.Add(this.btnStart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Serial Port Weight Reader";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}

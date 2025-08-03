using System.IO.Ports;
using System;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Components;
using MetroFramework;
using System.Drawing;

namespace WFApp_Electronic_Scale
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private MetroComboBox cmbParity;
        private MetroComboBox cmbStopBits;
        private MetroComboBox cmbLetters;
        private MetroComboBox cmbCities;
        private MetroTextBox txtPort;
        private MetroTextBox txtBaudRate;
        private MetroTextBox txtDataBits;
        private MetroTextBox txtCommand;
        private MetroTextBox txtWeight;
        private MetroTextBox txtLog;
        private MetroButton btnStart;
        private MetroButton btnSendTrigger;
        private MetroButton btnManageUsers;
        private MetroButton btnViewHistory;
        private MetroButton btnTestLogin;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtPort = new MetroFramework.Controls.MetroTextBox();
            this.txtBaudRate = new MetroFramework.Controls.MetroTextBox();
            this.cmbParity = new MetroFramework.Controls.MetroComboBox();
            this.txtDataBits = new MetroFramework.Controls.MetroTextBox();
            this.cmbStopBits = new MetroFramework.Controls.MetroComboBox();
            this.btnStart = new MetroFramework.Controls.MetroButton();
            this.btnSendTrigger = new MetroFramework.Controls.MetroButton();
            this.txtCommand = new MetroFramework.Controls.MetroTextBox();
            this.txtWeight = new MetroFramework.Controls.MetroTextBox();
            this.txtLog = new MetroFramework.Controls.MetroTextBox();
            this.cmbLetters = new MetroFramework.Controls.MetroComboBox();
            this.cmbCities = new MetroFramework.Controls.MetroComboBox();
            this.btnManageUsers = new MetroFramework.Controls.MetroButton();
            this.btnTestLogin = new MetroFramework.Controls.MetroButton();
            this.btnViewHistory = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.gbPort = new System.Windows.Forms.GroupBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.gbSetting = new System.Windows.Forms.GroupBox();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.gblog = new System.Windows.Forms.GroupBox();
            this.gbweight = new System.Windows.Forms.GroupBox();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            this.gbtrigger = new System.Windows.Forms.GroupBox();
            this.gbPort.SuspendLayout();
            this.gbSetting.SuspendLayout();
            this.gblog.SuspendLayout();
            this.gbweight.SuspendLayout();
            this.gbtrigger.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            // 
            // 
            // 
            this.txtPort.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.txtPort.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.txtPort.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.txtPort.CustomButton.Name = "";
            this.txtPort.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.txtPort.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPort.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.txtPort.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPort.CustomButton.UseSelectable = true;
            this.txtPort.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.txtPort.Lines = new string[0];
            resources.ApplyResources(this.txtPort, "txtPort");
            this.txtPort.MaxLength = 32767;
            this.txtPort.Name = "txtPort";
            this.txtPort.PasswordChar = '\0';
            this.txtPort.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPort.SelectedText = "";
            this.txtPort.SelectionLength = 0;
            this.txtPort.SelectionStart = 0;
            this.txtPort.ShortcutsEnabled = true;
            this.txtPort.UseSelectable = true;
            this.txtPort.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPort.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtBaudRate
            // 
            // 
            // 
            // 
            this.txtBaudRate.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.txtBaudRate.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.txtBaudRate.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.txtBaudRate.CustomButton.Name = "";
            this.txtBaudRate.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.txtBaudRate.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBaudRate.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.txtBaudRate.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBaudRate.CustomButton.UseSelectable = true;
            this.txtBaudRate.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.txtBaudRate.Lines = new string[0];
            resources.ApplyResources(this.txtBaudRate, "txtBaudRate");
            this.txtBaudRate.MaxLength = 32767;
            this.txtBaudRate.Name = "txtBaudRate";
            this.txtBaudRate.PasswordChar = '\0';
            this.txtBaudRate.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBaudRate.SelectedText = "";
            this.txtBaudRate.SelectionLength = 0;
            this.txtBaudRate.SelectionStart = 0;
            this.txtBaudRate.ShortcutsEnabled = true;
            this.txtBaudRate.UseSelectable = true;
            this.txtBaudRate.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBaudRate.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cmbParity
            // 
            this.cmbParity.FormattingEnabled = true;
            resources.ApplyResources(this.cmbParity, "cmbParity");
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.UseSelectable = true;
            // 
            // txtDataBits
            // 
            // 
            // 
            // 
            this.txtDataBits.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.txtDataBits.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.txtDataBits.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.txtDataBits.CustomButton.Name = "";
            this.txtDataBits.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.txtDataBits.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtDataBits.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.txtDataBits.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtDataBits.CustomButton.UseSelectable = true;
            this.txtDataBits.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.txtDataBits.Lines = new string[0];
            resources.ApplyResources(this.txtDataBits, "txtDataBits");
            this.txtDataBits.MaxLength = 32767;
            this.txtDataBits.Name = "txtDataBits";
            this.txtDataBits.PasswordChar = '\0';
            this.txtDataBits.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtDataBits.SelectedText = "";
            this.txtDataBits.SelectionLength = 0;
            this.txtDataBits.SelectionStart = 0;
            this.txtDataBits.ShortcutsEnabled = true;
            this.txtDataBits.UseSelectable = true;
            this.txtDataBits.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtDataBits.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.FormattingEnabled = true;
            resources.ApplyResources(this.cmbStopBits, "cmbStopBits");
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.UseSelectable = true;
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.Style = MetroFramework.MetroColorStyle.Blue;
            this.btnStart.UseSelectable = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSendTrigger
            // 
            resources.ApplyResources(this.btnSendTrigger, "btnSendTrigger");
            this.btnSendTrigger.Name = "btnSendTrigger";
            this.btnSendTrigger.Style = MetroFramework.MetroColorStyle.Green;
            this.btnSendTrigger.UseSelectable = true;
            this.btnSendTrigger.Click += new System.EventHandler(this.btnSendTrigger_Click);
            // 
            // txtCommand
            // 
            // 
            // 
            // 
            this.txtCommand.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.txtCommand.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.txtCommand.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.txtCommand.CustomButton.Name = "";
            this.txtCommand.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.txtCommand.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCommand.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.txtCommand.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCommand.CustomButton.UseSelectable = true;
            this.txtCommand.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
            this.txtCommand.Lines = new string[0];
            resources.ApplyResources(this.txtCommand, "txtCommand");
            this.txtCommand.MaxLength = 32767;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.PasswordChar = '\0';
            this.txtCommand.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCommand.SelectedText = "";
            this.txtCommand.SelectionLength = 0;
            this.txtCommand.SelectionStart = 0;
            this.txtCommand.ShortcutsEnabled = true;
            this.txtCommand.UseSelectable = true;
            this.txtCommand.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCommand.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtWeight
            // 
            // 
            // 
            // 
            this.txtWeight.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image4")));
            this.txtWeight.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode4")));
            this.txtWeight.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location4")));
            this.txtWeight.CustomButton.Name = "";
            this.txtWeight.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size4")));
            this.txtWeight.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtWeight.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex4")));
            this.txtWeight.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtWeight.CustomButton.UseSelectable = true;
            this.txtWeight.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible4")));
            this.txtWeight.Lines = new string[0];
            resources.ApplyResources(this.txtWeight, "txtWeight");
            this.txtWeight.MaxLength = 32767;
            this.txtWeight.Multiline = true;
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.PasswordChar = '\0';
            this.txtWeight.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWeight.SelectedText = "";
            this.txtWeight.SelectionLength = 0;
            this.txtWeight.SelectionStart = 0;
            this.txtWeight.ShortcutsEnabled = true;
            this.txtWeight.UseSelectable = true;
            this.txtWeight.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtWeight.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // 
            // 
            this.txtLog.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image5")));
            this.txtLog.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode5")));
            this.txtLog.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location5")));
            this.txtLog.CustomButton.Name = "";
            this.txtLog.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size5")));
            this.txtLog.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtLog.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex5")));
            this.txtLog.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtLog.CustomButton.UseSelectable = true;
            this.txtLog.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible5")));
            this.txtLog.ForeColor = System.Drawing.Color.Black;
            this.txtLog.Lines = new string[0];
            resources.ApplyResources(this.txtLog, "txtLog");
            this.txtLog.MaxLength = 32767;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.PasswordChar = '\0';
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.SelectedText = "";
            this.txtLog.SelectionLength = 0;
            this.txtLog.SelectionStart = 0;
            this.txtLog.ShortcutsEnabled = true;
            this.txtLog.UseSelectable = true;
            this.txtLog.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtLog.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cmbLetters
            // 
            this.cmbLetters.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLetters, "cmbLetters");
            this.cmbLetters.Name = "cmbLetters";
            this.cmbLetters.PromptText = "اختر حرف من فضلك";
            this.cmbLetters.UseSelectable = true;
            // 
            // cmbCities
            // 
            this.cmbCities.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCities, "cmbCities");
            this.cmbCities.Name = "cmbCities";
            this.cmbCities.PromptText = "اختر المدينة من فضلك";
            this.cmbCities.UseSelectable = true;
            // 
            // btnManageUsers
            // 
            resources.ApplyResources(this.btnManageUsers, "btnManageUsers");
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Style = MetroFramework.MetroColorStyle.Purple;
            this.btnManageUsers.UseSelectable = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnTestLogin
            // 
            resources.ApplyResources(this.btnTestLogin, "btnTestLogin");
            this.btnTestLogin.Name = "btnTestLogin";
            this.btnTestLogin.UseSelectable = true;
            this.btnTestLogin.Click += new System.EventHandler(this.btnTestLogin_Click);
            // 
            // btnViewHistory
            // 
            resources.ApplyResources(this.btnViewHistory, "btnViewHistory");
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Style = MetroFramework.MetroColorStyle.Teal;
            this.btnViewHistory.UseSelectable = true;
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);
            // 
            // metroLabel1
            // 
            resources.ApplyResources(this.metroLabel1, "metroLabel1");
            this.metroLabel1.Name = "metroLabel1";
            // 
            // gbPort
            // 
            this.gbPort.Controls.Add(this.metroLabel7);
            this.gbPort.Controls.Add(this.metroLabel6);
            this.gbPort.Controls.Add(this.metroLabel5);
            this.gbPort.Controls.Add(this.metroLabel4);
            this.gbPort.Controls.Add(this.metroLabel3);
            this.gbPort.Controls.Add(this.txtPort);
            this.gbPort.Controls.Add(this.txtBaudRate);
            this.gbPort.Controls.Add(this.cmbParity);
            this.gbPort.Controls.Add(this.txtDataBits);
            this.gbPort.Controls.Add(this.cmbStopBits);
            resources.ApplyResources(this.gbPort, "gbPort");
            this.gbPort.Name = "gbPort";
            this.gbPort.TabStop = false;
            // 
            // metroLabel7
            // 
            resources.ApplyResources(this.metroLabel7, "metroLabel7");
            this.metroLabel7.Name = "metroLabel7";
            // 
            // metroLabel6
            // 
            resources.ApplyResources(this.metroLabel6, "metroLabel6");
            this.metroLabel6.Name = "metroLabel6";
            // 
            // metroLabel5
            // 
            resources.ApplyResources(this.metroLabel5, "metroLabel5");
            this.metroLabel5.Name = "metroLabel5";
            // 
            // metroLabel4
            // 
            resources.ApplyResources(this.metroLabel4, "metroLabel4");
            this.metroLabel4.Name = "metroLabel4";
            // 
            // metroLabel3
            // 
            resources.ApplyResources(this.metroLabel3, "metroLabel3");
            this.metroLabel3.Name = "metroLabel3";
            // 
            // gbSetting
            // 
            this.gbSetting.Controls.Add(this.metroLabel10);
            this.gbSetting.Controls.Add(this.metroLabel9);
            this.gbSetting.Controls.Add(this.cmbLetters);
            this.gbSetting.Controls.Add(this.cmbCities);
            resources.ApplyResources(this.gbSetting, "gbSetting");
            this.gbSetting.Name = "gbSetting";
            this.gbSetting.TabStop = false;
            // 
            // metroLabel10
            // 
            resources.ApplyResources(this.metroLabel10, "metroLabel10");
            this.metroLabel10.Name = "metroLabel10";
            // 
            // metroLabel9
            // 
            resources.ApplyResources(this.metroLabel9, "metroLabel9");
            this.metroLabel9.Name = "metroLabel9";
            // 
            // metroLabel8
            // 
            resources.ApplyResources(this.metroLabel8, "metroLabel8");
            this.metroLabel8.Name = "metroLabel8";
            // 
            // gblog
            // 
            this.gblog.Controls.Add(this.metroLabel1);
            this.gblog.Controls.Add(this.txtLog);
            resources.ApplyResources(this.gblog, "gblog");
            this.gblog.Name = "gblog";
            this.gblog.TabStop = false;
            // 
            // gbweight
            // 
            this.gbweight.Controls.Add(this.metroLabel11);
            this.gbweight.Controls.Add(this.txtWeight);
            resources.ApplyResources(this.gbweight, "gbweight");
            this.gbweight.Name = "gbweight";
            this.gbweight.TabStop = false;
            // 
            // metroLabel11
            // 
            resources.ApplyResources(this.metroLabel11, "metroLabel11");
            this.metroLabel11.Name = "metroLabel11";
            // 
            // gbtrigger
            // 
            this.gbtrigger.Controls.Add(this.metroLabel8);
            this.gbtrigger.Controls.Add(this.txtCommand);
            this.gbtrigger.Controls.Add(this.btnSendTrigger);
            resources.ApplyResources(this.gbtrigger, "gbtrigger");
            this.gbtrigger.Name = "gbtrigger";
            this.gbtrigger.TabStop = false;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbtrigger);
            this.Controls.Add(this.gbweight);
            this.Controls.Add(this.gblog);
            this.Controls.Add(this.gbSetting);
            this.Controls.Add(this.gbPort);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnViewHistory);
            this.Controls.Add(this.btnTestLogin);
            this.Controls.Add(this.btnManageUsers);
            this.Name = "Form1";
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbPort.ResumeLayout(false);
            this.gbPort.PerformLayout();
            this.gbSetting.ResumeLayout(false);
            this.gbSetting.PerformLayout();
            this.gblog.ResumeLayout(false);
            this.gblog.PerformLayout();
            this.gbweight.ResumeLayout(false);
            this.gbweight.PerformLayout();
            this.gbtrigger.ResumeLayout(false);
            this.gbtrigger.PerformLayout();
            this.ResumeLayout(false);

        }
        private MetroLabel metroLabel1;
        private GroupBox gbPort;
        private MetroLabel metroLabel7;
        private MetroLabel metroLabel6;
        private MetroLabel metroLabel5;
        private MetroLabel metroLabel4;
        private MetroLabel metroLabel3;
        private GroupBox gbSetting;
        private GroupBox gblog;
        private GroupBox gbweight;
        private MetroLabel metroLabel8;
        private MetroLabel metroLabel10;
        private MetroLabel metroLabel9;
        private MetroLabel metroLabel11;
        private GroupBox gbtrigger;
    }
}

using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    // FormPopup.cs
    public partial class FormPopup : MetroForm
    {
        private System.Windows.Forms.Timer closeTimer;
        private Label lblContent;

        public FormPopup()
        {
            InitializeComponent();
            InitializeAutoCloseTimer();
        }

        private void InitializeAutoCloseTimer()
        {
            closeTimer = new System.Windows.Forms.Timer
            {
                Interval = 5000 // 5 ثواني
            };
            closeTimer.Tick += (sender, e) => ClosePopup();
        }

        // دالة لتعيين البيانات في عناصر التحكم
        public void SetData(string title, string content)
        {
            this.Text = title;
            lblContent.Text = content;
            closeTimer.Start();
        }
        private void ClosePopup()
        {
            closeTimer.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            closeTimer?.Dispose();
        }
        private void InitializeComponent()
        {
            this.lblContent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(238, 62);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(44, 16);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "label1";
            // 
            // FormPopup
            // 
            this.ClientSize = new System.Drawing.Size(845, 265);
            this.Controls.Add(this.lblContent);
            this.Name = "FormPopup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

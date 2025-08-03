using System;
using System.Data;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    public partial class WeightHistoryForm : Form
    {
        private DatabaseManager dbManager;
        private DataTable weightsData;

        // UI Controls
        private System.Windows.Forms.DataGridView dgvWeights;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Label lblTitle;

        public WeightHistoryForm()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            LoadWeightsHistory();
        }

        private void InitializeComponent()
        {
            this.dgvWeights = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeights)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvWeights
            // 
            this.dgvWeights.AllowUserToAddRows = false;
            this.dgvWeights.AllowUserToDeleteRows = false;
            this.dgvWeights.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWeights.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWeights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWeights.Location = new System.Drawing.Point(12, 80);
            this.dgvWeights.MultiSelect = false;
            this.dgvWeights.Name = "dgvWeights";
            this.dgvWeights.ReadOnly = true;
            this.dgvWeights.RowHeadersWidth = 51;
            this.dgvWeights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWeights.Size = new System.Drawing.Size(776, 300);
            this.dgvWeights.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(12, 400);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(118, 400);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف المحدد";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(688, 400);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Location = new System.Drawing.Point(12, 50);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(150, 22);
            this.dtpFromDate.TabIndex = 4;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(200, 50);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(150, 22);
            this.dtpToDate.TabIndex = 5;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(12, 30);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(54, 16);
            this.lblFromDate.TabIndex = 6;
            this.lblFromDate.Text = "من تاريخ:";
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(200, 30);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(58, 16);
            this.lblToDate.TabIndex = 7;
            this.lblToDate.Text = "إلى تاريخ:";
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(370, 45);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 30);
            this.btnFilter.TabIndex = 8;
            this.btnFilter.Text = "تصفية";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(460, 45);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(80, 30);
            this.btnClearFilter.TabIndex = 9;
            this.btnClearFilter.Text = "مسح التصفية";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(107, 25);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "سجل الأوزان";
            // 
            // WeightHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvWeights);
            this.Name = "WeightHistoryForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "سجل الأوزان";
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeights)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoadWeightsHistory()
        {
            try
            {
                weightsData = dbManager.GetWeightsHistory();
                dgvWeights.DataSource = weightsData;
                
                // تخصيص أسماء الأعمدة
                if (dgvWeights.Columns.Count > 0)
                {
                    dgvWeights.Columns["Id"].HeaderText = "الرقم";
                    dgvWeights.Columns["Weight"].HeaderText = "الوزن";
                    //dgvWeights.Columns["WeightUnit"].HeaderText = "الوحدة";
                    dgvWeights.Columns["ReadingTime"].HeaderText = "وقت القراءة";
                    dgvWeights.Columns["UserId"].HeaderText = "معرف المستخدم";
                    dgvWeights.Columns["UserName"].HeaderText = "اسم المستخدم";
                    dgvWeights.Columns["LetterPlate"].HeaderText = "محرف اللوحة";
                    dgvWeights.Columns["NoPlate"].HeaderText = "رقم اللوحة";
                    dgvWeights.Columns["City"].HeaderText = "المدينة";
                    //dgvWeights.Columns["Notes"].HeaderText = "ملاحظات";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل سجل الأوزان: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadWeightsHistory();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvWeights.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("هل أنت متأكد من حذف السجل المحدد؟", "تأكيد الحذف", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    int weightId = Convert.ToInt32(dgvWeights.SelectedRows[0].Cells["Id"].Value);
                    if (dbManager.DeleteWeight(weightId))
                    {
                        MessageBox.Show("تم حذف السجل بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadWeightsHistory();
                    }
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد سجل للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                weightsData = dbManager.GetWeightsHistory(dtpFromDate.Value, dtpToDate.Value);
                dgvWeights.DataSource = weightsData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تطبيق التصفية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now.AddDays(-30);
            dtpToDate.Value = DateTime.Now;
            LoadWeightsHistory();
        }
    }
} 
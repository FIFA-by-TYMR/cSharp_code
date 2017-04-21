namespace ProjectFifaV2
{
    partial class frmAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadData = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.dgvAdminData = new System.Windows.Forms.DataGridView();
            this.btnAdminLogOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(663, 33);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(81, 20);
            this.btnLoadData.TabIndex = 0;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(142, 33);
            this.txtPath.MaxLength = 256;
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(515, 20);
            this.txtPath.TabIndex = 1;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(55, 33);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(81, 20);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(55, 139);
            this.txtQuery.MaxLength = 150;
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(515, 84);
            this.txtQuery.TabIndex = 4;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(576, 168);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(81, 20);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // dgvAdminData
            // 
            this.dgvAdminData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdminData.Location = new System.Drawing.Point(12, 229);
            this.dgvAdminData.Name = "dgvAdminData";
            this.dgvAdminData.Size = new System.Drawing.Size(935, 271);
            this.dgvAdminData.TabIndex = 6;
            // 
            // btnAdminLogOut
            // 
            this.btnAdminLogOut.Location = new System.Drawing.Point(841, 33);
            this.btnAdminLogOut.Name = "btnAdminLogOut";
            this.btnAdminLogOut.Size = new System.Drawing.Size(106, 40);
            this.btnAdminLogOut.TabIndex = 7;
            this.btnAdminLogOut.Text = "Log Out";
            this.btnAdminLogOut.UseVisualStyleBackColor = true;
            this.btnAdminLogOut.Click += new System.EventHandler(this.btnAdminLogOut_Click);
            // 
            // frmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 512);
            this.Controls.Add(this.btnAdminLogOut);
            this.Controls.Add(this.dgvAdminData);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnLoadData);
            this.Name = "frmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.DataGridView dgvAdminData;
        private System.Windows.Forms.Button btnAdminLogOut;
    }
}
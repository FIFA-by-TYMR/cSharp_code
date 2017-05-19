namespace ProjectFifaV2
{
    partial class frmRanking
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
            this.btnRankingBack = new System.Windows.Forms.Button();
            this.lvRanking = new System.Windows.Forms.ListView();
            this.clmRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmScore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnRankingBack
            // 
            this.btnRankingBack.Location = new System.Drawing.Point(454, 12);
            this.btnRankingBack.Name = "btnRankingBack";
            this.btnRankingBack.Size = new System.Drawing.Size(84, 29);
            this.btnRankingBack.TabIndex = 1;
            this.btnRankingBack.Text = "Back";
            this.btnRankingBack.UseVisualStyleBackColor = true;
            this.btnRankingBack.Click += new System.EventHandler(this.btnRankingBack_Click);
            // 
            // lvRanking
            // 
            this.lvRanking.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmRank,
            this.clmName,
            this.clmScore});
            this.lvRanking.Location = new System.Drawing.Point(12, 12);
            this.lvRanking.Name = "lvRanking";
            this.lvRanking.Size = new System.Drawing.Size(436, 588);
            this.lvRanking.TabIndex = 2;
            this.lvRanking.UseCompatibleStateImageBehavior = false;
            this.lvRanking.View = System.Windows.Forms.View.Details;
            // 
            // clmRank
            // 
            this.clmRank.Text = "Rank";
            this.clmRank.Width = 45;
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 300;
            // 
            // clmScore
            // 
            this.clmScore.Text = "Score";
            this.clmScore.Width = 80;
            // 
            // frmRanking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 612);
            this.Controls.Add(this.lvRanking);
            this.Controls.Add(this.btnRankingBack);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRanking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ranking";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRankingBack;
        private System.Windows.Forms.ListView lvRanking;
        private System.Windows.Forms.ColumnHeader clmRank;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmScore;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectFifaV2
{
    public partial class frmPlayer : Form
    {
        private Form frmRanking;
        private DatabaseHandler dbh;
        private string userName;

        List<TextBox> txtBoxList;

        public frmPlayer(Form frm, string un)
        {
            this.ControlBox = false;
            frmRanking = frm;
            dbh = new DatabaseHandler();

            InitializeComponent();
            if (DisableEditButton())
            {
                btnEditPrediction.Enabled = false;
            }
            ShowResults();
            ShowScoreCard();
            this.Text = "Welcome " + un;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnShowRanking_Click(object sender, EventArgs e)
        {
            frmRanking.Show();
        }

        private void btnClearPrediction_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear your prediction?", "Clear Predictions", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                // Clear predections
                // Update DB
            }
        }

        private bool DisableEditButton()
        {
            bool hasPassed;
            //This is the deadline for filling in the predictions
            DateTime deadline = new DateTime(2014, 06, 12);
            DateTime curTime = DateTime.Now;
            int result = DateTime.Compare(deadline, curTime);

            if (result < 0)
            {
                hasPassed = true;
            }
            else
            {
                hasPassed = false;
            }

            return hasPassed;
        }

        private void ShowResults()
        {
            dbh.TestConnection();
            dbh.OpenConnectionToDB();

            DataTable hometable = dbh.FillDT("SELECT tblTeams.TeamName, tblGames.HomeTeamScore FROM tblGames INNER JOIN tblTeams ON tblGames.HomeTeam = tblTeams.Team_ID");
            DataTable awayTable = dbh.FillDT("SELECT tblTeams.TeamName, tblGames.AwayTeamScore FROM tblGames INNER JOIN tblTeams ON tblGames.AwayTeam = tblTeams.Team_ID");

            dbh.CloseConnectionToDB();

            for (int i = 0; i < hometable.Rows.Count; i++)
            {
                DataRow dataRowHome = hometable.Rows[i];
                DataRow dataRowAway = awayTable.Rows[i];
                ListViewItem lstItem = new ListViewItem(dataRowHome["TeamName"].ToString());
                lstItem.SubItems.Add(dataRowHome["HomeTeamScore"].ToString());
                lstItem.SubItems.Add(dataRowAway["AwayTeamScore"].ToString());
                lstItem.SubItems.Add(dataRowAway["TeamName"].ToString());
                lvOverview.Items.Add(lstItem);
            }
        }

        private void ShowScoreCard()
        {
            dbh.TestConnection();
            dbh.OpenConnectionToDB();

            DataTable hometable = dbh.FillDT("SELECT tblTeams.TeamName FROM tblGames INNER JOIN tblTeams ON tblGames.HomeTeam = tblTeams.Team_ID");
            DataTable awayTable = dbh.FillDT("SELECT tblTeams.TeamName FROM tblGames INNER JOIN tblTeams ON tblGames.AwayTeam = tblTeams.Team_ID");

            dbh.CloseConnectionToDB();

            for (int i = 0; i < hometable.Rows.Count; i++)
            {
                DataRow dataRowHome = hometable.Rows[i];
                DataRow dataRowAway = awayTable.Rows[i];
                
                Label lblHomeTeam = new Label();
                Label lblAwayTeam = new Label();
                TextBox txtHomePred = new TextBox();
                TextBox txtAwayPred = new TextBox();

                lblHomeTeam.TextAlign = ContentAlignment.BottomRight;
                lblHomeTeam.Text = dataRowHome["TeamName"].ToString();
                lblHomeTeam.Location = new Point(15, txtHomePred.Bottom + (i * 30));
                lblHomeTeam.AutoSize = true;

                txtHomePred.Text = "0";
                txtHomePred.Location = new Point(lblHomeTeam.Width, lblHomeTeam.Top - 3);
                txtHomePred.Width = 40;

                txtAwayPred.Text = "0";
                txtAwayPred.Location = new Point(txtHomePred.Width + lblHomeTeam.Width, txtHomePred.Top);
                txtAwayPred.Width = 40;

                lblAwayTeam.Text = dataRowAway["TeamName"].ToString();
                lblAwayTeam.Location = new Point(txtHomePred.Width + lblHomeTeam.Width + txtAwayPred.Width, txtHomePred.Top + 3);
                lblAwayTeam.AutoSize = true;

                pnlPredCard.Controls.Add(lblHomeTeam);
                pnlPredCard.Controls.Add(txtHomePred);
                pnlPredCard.Controls.Add(txtAwayPred);
                pnlPredCard.Controls.Add(lblAwayTeam);
                //ListViewItem lstItem = new ListViewItem(dataRowHome["TeamName"].ToString());
                //lstItem.SubItems.Add(dataRowHome["HomeTeamScore"].ToString());
                //lstItem.SubItems.Add(dataRowAway["AwayTeamScore"].ToString());
                //lstItem.SubItems.Add(dataRowAway["TeamName"].ToString());
                //lvOverview.Items.Add(lstItem);
            }
        }

        internal void GetUsername(string un)
        {
            userName = un;
        }
    }
}

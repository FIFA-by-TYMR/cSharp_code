using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectFifaV2
{
    public partial class frmPlayer : Form
    {
        // This is letting the user to bet in the numericupdowns, creates an databasehandler to execute some query's if he/she presses an button and remember the users id and username.

        internal DatabaseHandler dbh = new DatabaseHandler();
     
        const int lengthInnerArray = 2;
        const int lengthOutterArray = 2;

        private Form frmRanking;
        private DataTable tblUsers;
        private DataRow rowUser;

        private string userName;

        internal int resultId;
        internal int counter = 0;
       
        List<NumericUpDown> txtBoxList;
        List<NumericUpDown>[,] newRows = new List<NumericUpDown>[2, 2];

        NumericUpDown[,] rows;


        public frmPlayer(Form frm, string un)
        {
            // This is letting the user to see the preditions, result and scorecard.

            int amount = dbh.DTInt("SELECT COUNT(*) FROM TblGames");

            rows = new NumericUpDown[amount, lengthInnerArray];

            this.ControlBox = false;

            frmRanking = frm;

            dbh = new DatabaseHandler();

            InitializeComponent();

            if (DisableEditButton())
            {
                btnEditPrediction.Enabled = false;
                btnClearPrediction.Enabled = false;
                SaveButton.Enabled = false;
            }

            ShowResults();
            ShowScoreCard();

            this.Text = un;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            // This hides the form from the user, to make sure if he wants to log in again or to close the application.

            Hide();
        }

        private void btnShowRanking_Click(object sender, EventArgs e)
        {
            // This shows the frmRanking for the user.

            frmRanking.Show();
        }

        private void btnClearPrediction_Click(object sender, EventArgs e)
        {
            // This is letting the user to clear his/her preditions.

            DialogResult result = MessageBox.Show("Are you sure you want to clear your prediction?", "Clear Predictions", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result.Equals(DialogResult.OK))
            {
                DataTable tblUsers = dbh.FillDT("SELECT * FROM TblUsers WHERE (Username='" + this.Text + "')");

                using (SqlCommand cmd = new SqlCommand("SELECT id FROM TblUsers WHERE Username =  @Username", dbh.GetCon()))
                {
                    cmd.Parameters.AddWithValue("Username", this.Text);

                    dbh.OpenConnectionToDB();

                    string sql = Convert.ToString(cmd.ExecuteScalar());

                    int.TryParse(sql, out this.resultId);
                }

                int userId = resultId;
                int zero = 0;

                string home = "";
                string away = "";
                string sqlStr = "DELETE FROM TblPredictions WHERE user_id ='" + userId + "'";

                for (; zero < lengthOutterArray; zero++)
                {
                    for (int k = 0; k < lengthInnerArray; k++)
                    {
                        if (k == 0)
                        {
                            home = rows[zero, k].Text;
                        }
                        else
                        {
                            away = rows[zero, k].Text;
                        }
                    }
                }
                dbh.CloseConnectionToDB();
                dbh.Execute(sqlStr);
            }
        }

        private bool DisableEditButton()
        {
            // This is the deadline for filling in the predictions.

            bool hasPassed;

            if (!iselton())
            {

                DateTime deadline = new DateTime(2019, 06, 12);
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
            else
            {
                hasPassed = false;
                return hasPassed;
            }
        }

        private void ShowResults()
        {
            // This shows your predition result.

            dbh.OpenConnectionToDB();

            DataTable homeTable = dbh.FillDT("SELECT TblTeams.TeamName, TblGames.HomeTeamScore FROM TblGames INNER JOIN TblTeams ON TblGames.HomeTeam = TblTeams.Team_ID");
            DataTable awayTable = dbh.FillDT("SELECT TblTeams.TeamName, TblGames.AwayTeamScore FROM TblGames INNER JOIN TblTeams ON TblGames.AwayTeam = TblTeams.Team_ID");

            dbh.CloseConnectionToDB();

            for (int i = 0; i < homeTable.Rows.Count; i++)
            {
                DataRow dataRowHome = homeTable.Rows[i];
                DataRow dataRowAway = awayTable.Rows[i];

                ListViewItem lstItem = new ListViewItem(dataRowHome["teamName"].ToString());

                lstItem.SubItems.Add(dataRowHome["HomeTeamScore"].ToString());
                lstItem.SubItems.Add(dataRowAway["AwayTeamScore"].ToString());
                lstItem.SubItems.Add(dataRowAway["teamName"].ToString());

                lvOverview.Items.Add(lstItem);
            }
        }

        private void ShowScoreCard()
        {
            // This allows the user to make his/her bet.

            DataTable homeTable = dbh.FillDT("SELECT TblTeams.Teamname FROM TblGames INNER JOIN TblTeams ON TblGames.HomeTeam = TblTeams.Team_id");
            DataTable awayTable = dbh.FillDT("SELECT TblTeams.Teamname FROM TblGames INNER JOIN TblTeams ON TblGames.AwayTeam = TblTeams.Team_id");

            dbh.CloseConnectionToDB();

            for (int i = 0; i < homeTable.Rows.Count; i++)
            {
                DataRow dataRowHome = homeTable.Rows[i];
                DataRow dataRowAway = awayTable.Rows[i];

                Label lblHomeTeam = new Label();
                Label lblAwayTeam = new Label();

                NumericUpDown txtHomePred = new NumericUpDown();
                NumericUpDown txtAwayPred = new NumericUpDown();

                lblHomeTeam.TextAlign = ContentAlignment.BottomRight;
                lblHomeTeam.Text = dataRowHome["TeamName"].ToString();
                lblHomeTeam.Location = new Point(15, txtHomePred.Bottom + (i * 30));
                lblHomeTeam.AutoSize = true;

                txtHomePred.Text = "0";
                txtHomePred.Location = new Point(lblHomeTeam.Width, lblHomeTeam.Top - 3);
                txtHomePred.Width = 40;
                rows[i, 0] = txtHomePred;

                txtAwayPred.Text = "0";
                txtAwayPred.Location = new Point(txtHomePred.Width + lblHomeTeam.Width, txtHomePred.Top);
                txtAwayPred.Width = 40;
                rows[i, 1] = txtAwayPred;

                lblAwayTeam.Text = dataRowAway["TeamName"].ToString();
                lblAwayTeam.Location = new Point(txtHomePred.Width + lblHomeTeam.Width + txtAwayPred.Width, txtHomePred.Top + 3);
                lblAwayTeam.AutoSize = true;

                pnlPredCard.Controls.Add(lblHomeTeam);
                pnlPredCard.Controls.Add(txtHomePred);
                pnlPredCard.Controls.Add(txtAwayPred);
                pnlPredCard.Controls.Add(lblAwayTeam);

                this.counter++;

                ListViewItem lstItem = new ListViewItem(dataRowHome["TeamName"].ToString());
            }
        }

        internal void GetUsername(string un)
        {
            // This getter gets the users username.

            userName = un;
        }


        private void btnEditPrediction_Click(object sender, EventArgs e)
        {
            DataTable tblUsers = dbh.FillDT("SELECT * FROM TblUsers WHERE (Username='" + this.Text + "')");

            using (SqlCommand cmd = new SqlCommand("SELECT id FROM TblUsers WHERE Username =  @Username", dbh.GetCon()))
            {
                cmd.Parameters.AddWithValue("Username", this.Text);

                dbh.OpenConnectionToDB();

                string strSql = Convert.ToString(cmd.ExecuteScalar());

                int.TryParse(strSql, out this.resultId);
            }

            int id = resultId;
            int zero = 0;

            string home = "";
            string away = "";

            for (; zero < this.counter; zero++)
            {
                for (int k = 0; k < this.counter; k++)
                {
                    if (k == 0)
                    {
                        home = rows[zero, k].Text;

                        string sql = "UPDATE tblPredictions SET PredictedHomeScore = " + home + ", PredictedAwayScore = " + away + " WHERE(User_id = " + id + " AND Game_id=" + Convert.ToInt32(zero) + ")";

                        dbh.Execute(sql);
                    }
                    else
                    {
                        away = rows[zero, k].Text;

                        string sql = "UPDATE tblPredictions SET PredictedHomeScore = " + home + ", PredictedAwayScore = " + away + " WHERE(User_id = " + id + " AND Game_id=" + Convert.ToInt32(zero) + ")";

                        dbh.Execute(sql);
                    }
                }
            }
        }
        private bool iselton()
        {
            dbh.OpenConnectionToDB();
            
            bool admin;

            string userName = this.Text;

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [tblUsers] WHERE Username = @Username AND IsAdmin = 2", dbh.GetCon()))
            {
                cmd.Parameters.AddWithValue("Username", userName);
                admin = (int)cmd.ExecuteScalar() > 0;
            }

            dbh.CloseConnectionToDB();

            if (admin)
            {
                return true;   
            }
            else
            {
                return false;
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            DataTable tblUsers = dbh.FillDT("SELECT * FROM tblUsers WHERE (Username='" + this.Text + "')");

            using (SqlCommand cmd = new SqlCommand("SELECT id FROM TblUsers WHERE Username =  @Username", dbh.GetCon()))
            {
                cmd.Parameters.AddWithValue("Username", this.Text);

                dbh.OpenConnectionToDB();

                string sqlStr = Convert.ToString(cmd.ExecuteScalar());

                int.TryParse(sqlStr, out this.resultId);
            }

            int userId = resultId;
            int zero = 0;

            string home = "1";
            string away = "2";
    
            string sqlex = "INSERT INTO tblPredictions (PredictedHomeScore, PredictedAwayScore, User_id, Game_id  ) VALUES('" + home + "','" + away + "','" + userId+ "','" + Convert.ToInt32(zero) + "')";

            this.counter--;

            for (; zero <= this.counter; zero++)
            {
                for (int k = 1; k < 2 ; k++)
                {
                    if (k == 0)
                    {
                        home = rows[zero, k].Text;
                    }
                    else
                    {
                        away = rows[zero, k].Text;
                    }
                }
                string sql = "INSERT INTO tblPredictions (User_id, Game_id, PredictedHomeScore, PredictedAwayScore) VALUES ('" + userId + "', " + Convert.ToInt32(zero) + ", '" + home + "', '" + away + "')";

                dbh.CloseConnectionToDB();
                dbh.Execute(sql);
            }
            ShowResults();
        }
    }
}
﻿using System;
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
        internal DatabaseHandler dbh = new DatabaseHandler();
     
        const int lengthInnerArray = 2;
        const int lengthOutterArray = 2;

        private Form frmRanking;
        private string userName;
        private DataTable tblUsers;
        private DataRow rowUser;
        internal int counter = 0;
       
        List<TextBox> txtBoxList;
        TextBox[,] rows;
        List<TextBox>[,] newRows = new List<TextBox>[2, 2];


        public frmPlayer(Form frm, string un)
        {
            int amount = dbh.DTInt("SELECT COUNT(*) FROM TblGames ");
            rows = new TextBox[amount, lengthInnerArray];

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
            this.Text = un;
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
                DataTable tblUsers = dbh.FillDT("select * from tblUsers WHERE (Username='" + this.Text + "')");
                DataRow rowUser = tblUsers.Rows[0];
                int j = 0;
                string home = "";
                string away = "";
                string sqlex = "DELETE FROM tblPredictions WHERE user_id ='"+rowUser[0]+"'";

                for (; j < lengthOutterArray; j++)
                {
                    for (int k = 0; k < lengthInnerArray; k++)
                    {
                        if (k == 0)
                        {
                            home = rows[j, k].Text;
                        }
                        else
                        {
                            away = rows[j, k].Text;
                        }
                    }
                }
                dbh.Execute(sqlex);
                string sqlexX = "UPDATE tblPredictions WHERE user_id =  '" + this.Text + "' ";
            }
        }

        private bool DisableEditButton()
        {
            bool hasPassed;
            if (!iselton())
            {
                
                //This is the deadline for filling in the predictions
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
            dbh.TestConnection();
            dbh.OpenConnectionToDB();

            DataTable hometable = dbh.FillDT("SELECT TblTeams.TeamName, TblGames.HomeTeamScore FROM TblGames INNER JOIN TblTeams ON TblGames.HomeTeam = TblTeams.Team_ID");
            DataTable awayTable = dbh.FillDT("SELECT TblTeams.TeamName, TblGames.AwayTeamScore FROM TblGames INNER JOIN TblTeams ON TblGames.AwayTeam = TblTeams.Team_ID");

            dbh.CloseConnectionToDB();

            for (int i = 0; i < hometable.Rows.Count; i++)
            {
                DataRow dataRowHome = hometable.Rows[i];
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
            DataTable hometable = dbh.FillDT("SELECT TblTeams.Teamname FROM TblGames INNER JOIN TblTeams ON TblGames.HomeTeam = TblTeams.Team_id");
            DataTable awayTable = dbh.FillDT("SELECT TblTeams.Teamname FROM TblGames INNER JOIN TblTeams ON TblGames.AwayTeam = TblTeams.Team_id");

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
            userName = un;
        }

        //private void btnEditPrediction_Click(object sender, EventArgs e)
        //{
        //    DataTable tblUsers = dbh.FillDT("select * from tblUsers WHERE (Username='test')");
        //    DataRow rowUser = tblUsers.Rows[0];
        //    int j = 0;
        //    string home = "0";
        //    string away = "2";
        //    //string sqlex = "UPDATE tblPredictions SET PredictedHomeScore = " + home + ", PredictedAwayScore = " + away + " WHERE(User_id = " + rowUser["id"] + " AND Game_id=" + Convert.ToInt32(j) + ")";
        //    string sqlex = "insert into tblPredictions (PredictedHomeScore, PredictedAwayScore, User_id, Game_id  ) values('" + home + "','" + away + "','" + rowUser[0] + "','" + Convert.ToInt32(j) + "')";


        //    for (; j < lengthOutterArray; j++)
        //    {
        //        for (int k = 0; k < lengthInnerArray; k++)
        //        {
        //            if (k == 0)
        //            {
        //                home = rows[j, k].Text;
        //            }
        //            else
        //            {
        //                away = rows[j, k].Text;
        //            }
        //        }
        //    }
        //    dbh.Execute(sqlex);
        //}

        private void btnEditPrediction_Click(object sender, EventArgs e)
        {
            DataTable tblUsers = dbh.FillDT("select * from tblUsers WHERE (Username='test')");
            DataRow rowUser = tblUsers.Rows[0];
            int j = 0;
            string home = "";
            string away = "";

            //string sqlex = "insert into tblPredictions (PredictedHomeScore, PredictedAwayScore, User_id, Game_id  ) values('" + home + "','" + away + "','" + rowUser[0] + "','" + Convert.ToInt32(j) + "')";


            for (; j < this.counter; j++)
            {
                for (int k = 0; k < this.counter; k++)
                {
                    if (k == 0)
                    {
                        home = rows[j, k].Text;
                        string sqlex = "UPDATE tblPredictions SET PredictedHomeScore = " + home + ", PredictedAwayScore = " + away + " WHERE(User_id = " + rowUser["id"] + " AND Game_id=" + Convert.ToInt32(j) + ")";
                        dbh.Execute(sqlex);
                    }
                    else
                    {
                        away = rows[j, k].Text;
                        string sqlex = "UPDATE tblPredictions SET PredictedHomeScore = " + home + ", PredictedAwayScore = " + away + " WHERE(User_id = " + rowUser["id"] + " AND Game_id=" + Convert.ToInt32(j) + ")";
                        dbh.Execute(sqlex);
                    }
                }
            }
        }
        private bool iselton()
        {
            dbh.OpenConnectionToDB();
            bool admin;
            string userName = this.Text;

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from [tblUsers] WHERE Username = @Username AND IsAdmin = 2", dbh.GetCon()))
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
            DataTable tblUsers = dbh.FillDT("SELECT * from tblUsers WHERE (Username='" + this.Text + "')");
            DataRow rowUser = tblUsers.Rows[0];
            int j = 0;
            
            string home = "1";
            string away = "2";
            //string sqlex = "UPDATE tblPredictions SET PredictedHomeScore = " + home + ", PredictedAwayScore = " + away + " WHERE(User_id = " + rowUser["id"] + " AND Game_id=" + Convert.ToInt32(j) + ")";
            string sqlex = "insert into  tblPredictions (PredictedHomeScore, PredictedAwayScore, User_id, Game_id  ) values('" + home + "','" + away + "','" + rowUser[0] + "','" + Convert.ToInt32(j) + "')";

            this.counter--;
            for (; j <= this.counter; j++)
            {
                for (int k = 1; k < 2 ; k++)
                {
                    if (k == 0)
                    {
                        home = rows[j, k].Text;
                    }
                    else
                    {
                        away = rows[j, k].Text;
                    }
                }
                string fag = "Insert Into tblPredictions (User_id, Game_id, PredictedHomeScore, PredictedAwayScore) VALUES ('" + rowUser["id"] + "', " + Convert.ToInt32(j) + ", '" + home + "', '" + away + "')";
                dbh.Execute(fag);
            }
            ShowResults();
        }

        private void lvOverview_Click(object sender, EventArgs e)
        {
            ShowResults();
        }
    }
}
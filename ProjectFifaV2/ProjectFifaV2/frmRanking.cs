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
    public partial class frmRanking : Form
    {
        DatabaseHandler dbh;

        public frmRanking()
        {
            // Creates an databasehandler to show the scores.

            this.ControlBox = false;

            dbh = new DatabaseHandler();

            InitializeComponent();

            SetListColumnWidth();

            ShowScore();
        }

        private void btnRankingBack_Click(object sender, EventArgs e)
        {
            // This lets the user go back to his/her form.

            Hide();
        }

        private void SetListColumnWidth()
        {
            // This sets the widths in the listview.

            clmRank.Width = 45;
            clmName.Width = 300;
            clmScore.Width = 80;
        }

        private void ShowScore()
        {
            // This shows the scores per username.

            dbh.OpenConnectionToDB();

            DataTable table = dbh.FillDT("SELECT Username, Score FROM tblUsers WHERE (IsAdmin = 0 OR IsAdmin = 2) ORDER BY Score DESC");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dataRow = table.Rows[i];

                ListViewItem lstItem = new ListViewItem((i + 1).ToString());

                lstItem.SubItems.Add(dataRow["Username"].ToString());
                lstItem.SubItems.Add(dataRow["Score"].ToString());

                lvRanking.Items.Add(lstItem);
            }
            dbh.CloseConnectionToDB();
        }
    }
}

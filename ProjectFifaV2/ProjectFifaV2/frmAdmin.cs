using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProjectFifaV2
{
    public partial class frmAdmin : Form
    {
        private DatabaseHandler dbh;
        private OpenFileDialog opfd;

        DataTable table;

        public frmAdmin()
        {
            dbh = new DatabaseHandler();
            table = new DataTable();

            this.ControlBox = false;

            InitializeComponent();

            // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

            btnLoadData.Enabled = false;
            btnExecute.Enabled = false;
            txtQuery.Enabled = false;
        }

        private void btnAdminLogOut_Click(object sender, EventArgs e)
        {
            txtQuery.Text = null;
            txtPath.Text = " ";

            dgvAdminData.DataSource = null;

            // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

            btnSelectFile.Enabled = true;
            btnLoadData.Enabled = false;
            btnExecute.Enabled = false;

            txtQuery.Enabled = false;
            txtPath.Enabled = true;

            Hide();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (txtQuery.TextLength > 0)
            {
                ExecuteSQL(txtQuery.Text);
            }
        }

        private void ExecuteSQL(string selectCommandText)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommandText, dbh.GetCon());

            dataAdapter.Fill(table);
            dgvAdminData.DataSource = table;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            txtPath.Text = null;
            
            string path = GetFilePath();

            if (CheckExtension(path, "csv"))
            {
                txtPath.Text = path;

                // This disables a button to be sure that an exception won't happen.

                btnLoadData.Enabled = true;
            }
            else
            {
                MessageHandler.ShowMessage("The wrong filetype is selected.");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (!(txtPath.Text == null && rb_Teams.Checked == true))
            {
                string drop = "DROP TABLE TblTeams ";
                string create =
                    "CREATE TABLE TblTeams(" +
                    "id INTEGER  NOT NULL PRIMARY KEY AUTO_INCREMENT " +
                    ",team_id_a INTEGER  NOT NULL" +
                    ", team_id_b      INTEGER NOT NULL" +
                    ",score_team_a VARCHAR(4) NOT NULL" +
                    ", score_team_b   VARCHAR(4) NOT NULL" +
                    ", match_duration BIT  NOT NULL" +
                    ", empty bit null"+
                    "); ";
                string insert = "BULK INSERT TblTeams" +
               " FROM '"+txtPath.Text+"'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ");";

                //dbh.OpenConnectionToDB();
                ExecuteSQL(drop);
                dbh.ExecuteAdmin(create);
                dbh.ExecuteAdmin(insert);
                //dbh.CloseConnectionToDB();

                // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

                btnExecute.Enabled = true;
                btnLoadData.Enabled = false;
                btnSelectFile.Enabled = false;

                txtQuery.Enabled = true;
                txtPath.Enabled = false;
            }
            else
            {
                MessageHandler.ShowMessage("No filename selected.");
            }
            if (!(txtPath.Text == null && rb_players.Checked == true))
            {
                string insert = "BULK INSERT TblPlayers" +
               " FROM '" + txtPath.Text + "'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ")";

                //dbh.OpenConnectionToDB();
                dbh.ExecuteAdmin(insert);
                //dbh.CloseConnectionToDB();

                // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

                btnExecute.Enabled = true;
                btnLoadData.Enabled = false;
                btnSelectFile.Enabled = false;

                txtQuery.Enabled = true;
                txtPath.Enabled = false;
            }
            else
            {
                MessageHandler.ShowMessage("No filename selected.");
            }
            if (!(txtPath.Text == null && rb_Games.Checked == true))
            {
                string insert = "BULK INSERT TblGames" +
               " FROM '" + txtPath.Text + "'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ")";

                dbh.OpenConnectionToDB();
                dbh.ExecuteAdmin(insert);
                dbh.CloseConnectionToDB();

                // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

                btnExecute.Enabled = true;
                btnLoadData.Enabled = false;
                btnSelectFile.Enabled = false;

                txtQuery.Enabled = true;
                txtPath.Enabled = false;
            }
            else
            {
                MessageHandler.ShowMessage("No filename selected.");
            }
        }
        
        private string GetFilePath()
        {
            string filePath = "";
            opfd = new OpenFileDialog();

            opfd.Multiselect = false;

            if (opfd.ShowDialog() == DialogResult.OK)
            {
                filePath = opfd.FileName;
            }

            return filePath;
        }

        private bool CheckExtension(string fileString, string extension)
        {
            int extensionLength = extension.Length;
            int strLength = fileString.Length;

            // This makes sure that there need to be a minimum path of 3 chars (Example: "C:\" and "D:\").

            if (fileString.Length < 3 )
            {
                return false;
            }

            string ext = fileString.Substring(strLength - extensionLength, extensionLength);

            if (ext == extension)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
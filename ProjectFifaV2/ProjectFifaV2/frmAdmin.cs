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
            // Creates an its own databasehandler and datatable to make sure that we could execute query's.

            dbh = new DatabaseHandler();
            table = new DataTable();

            this.ControlBox = false;

            InitializeComponent();

            // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

            btnLoadData.Enabled = false;
        }

        private void btnAdminLogOut_Click(object sender, EventArgs e)
        {
            // Its sets the path the query text and path to " ".

            txtQuery.Text = null;
            txtPath.Text = " ";

            dgvAdminData.DataSource = null;

            // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

            btnSelectFile.Enabled = true;
            btnLoadData.Enabled = false;
            btnExecute.Enabled = false;

            txtQuery.Enabled = false;
            txtPath.Enabled = true;

            // This hides the form.

            Hide();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            // This sets the char length for the execute button.

            if (txtQuery.TextLength > 8)
            {
                ExecuteSQL(txtQuery.Text);
            }
        }

        private void ExecuteSQL(string selectCommandText)
        {
            // This executes the query and sets it in the database.

            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommandText, dbh.GetCon());

            dataAdapter.Fill(table);

            dgvAdminData.DataSource = table;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            // This enables the option to select a file from the file explorer aka "Verkenner".

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
                // This shows a message if the file isn't a CSV file.

                MessageHandler.ShowMessage("The wrong filetype is selected.");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            // This disables a couple of buttons and/or text boxes to be sure that an exception won't happen.

            btnExecute.Enabled = true;
            btnLoadData.Enabled = false;
            btnSelectFile.Enabled = false;

            txtQuery.Enabled = true;
            txtPath.Enabled = false;

            // This is letting us to load in a CSV file.

            if ((txtPath.Text != null && rb_Teams.Checked == true))
            {
                string drop = "DROP TABLE TblTeams ";
                string create =
                    "CREATE TABLE TblTeams(" +
                    "    id             INT NOT NULL," +
                    "    poule_id      INT NULL," +
                    "    name     VARCHAR(255)  NULL," +
                    "    created_at   VARCHAR(255) NULL," +
                    "    deleted_at   VARCHAR(255) NULL," +
                    ");";

                string insert = "BULK INSERT TblTeams" +
               " FROM '"+txtPath.Text+"'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ");";

                ExecuteSQL(drop);

                dbh.ExecuteAdmin(create);
                dbh.ExecuteAdmin(insert);
            }
            else
            {
                // This shows a message if nothing is selected.

                MessageHandler.ShowMessage("No filename selected.");
            }

            if ((txtPath.Text != null && rb_players.Checked == true))
            {
                string drop1 = "DROP TABLE TblPLayers ";
                string create1 =
                    "CREATE TABLE TblPlayers(" +
                    "    id             INT NOT NULL," +
                    "    student_id     VARCHAR(255) NULL," +
                    "    Team_id      INT NULL," +
                    "    first_name     VARCHAR(255)  NULL," +
                    "    last_name     VARCHAR(255)  NULL," +
                    "    created_at   VARCHAR(255) NULL," +
                    "    deleted_at   VARCHAR(255) NULL," +
                    ");";

                string insert1 = "BULK INSERT TblPlayers" +
               " FROM '" + txtPath.Text + "'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ");";

                ExecuteSQL(drop1);

                dbh.ExecuteAdmin(create1);
                dbh.ExecuteAdmin(insert1);
            }
            else
            {
                // This shows a message if nothing is selected.

                MessageHandler.ShowMessage("No filename selected.");
            }

            if ((txtPath.Text != null && rb_Games.Checked == true))
            {
                string drop2 = "DROP TABLE TblGames ";
                string create2 =
                    "CREATE TABLE TblGames(" +
                    "    id             INT NOT NULL," +
                    "    team_id_a     VARCHAR(255) NULL," +
                    "    team_id_b     VARCHAR(255) NULL," +
                    "    score_team_a     VARCHAR(255)  NULL," +
                    "    score_team_b     VARCHAR(255)  NULL," +
                    "    score_team_b     VARCHAR(255)  NULL," +
                    "    start_time   VARCHAR(255) NULL," +
                    ");";

                string insert2 = "BULK INSERT TblGames" +
               " FROM '" + txtPath.Text + "'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ");";

                ExecuteSQL(drop2);

                dbh.ExecuteAdmin(create2);
                dbh.ExecuteAdmin(insert2);

                dbh.CloseConnectionToDB();
            }
            else
            {
                // This shows a message if nothing is selected.

                MessageHandler.ShowMessage("No filename selected.");
            }
        }

        private string GetFilePath()
        {
            // This gets the file path.

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
            // This check the extension to make sure its an CSV file.

            int extensionLength = extension.Length;
            int strLength = fileString.Length;

            // This makes sure that there need to be a minimum path of 3 chars (Example: "C:\" and "D:\").

            if (fileString.Length < 3)
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
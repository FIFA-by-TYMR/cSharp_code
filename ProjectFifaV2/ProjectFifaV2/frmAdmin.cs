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
            if (!(txtPath.Text == null))
            {
                string insert = "BULK INSERT TblTeams" +
               " FROM '"+txtPath.Text+"'" +
                "WITH" +
                "(" +
                   " FIRSTROW = 2," +
                   " FIELDTERMINATOR = ',', " +
                   " ROWTERMINATOR = '\n', " +
                   " TABLOCK" +
                ")";

                dbh.OpenConnectionToDB();
                ExecuteSQL(insert);
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
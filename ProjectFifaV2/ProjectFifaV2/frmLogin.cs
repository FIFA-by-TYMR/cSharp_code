using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ProjectFifaV2
{
    public partial class frmLogin : Form
    {
        private DatabaseHandler dbh;

        private Form frmAdmin;
        private Form frmPlayer;
        private Form frmRanking;

        internal string gettingHashedPassword;

        public frmLogin()
        {
            // Creates an its own databasehandler, creates an frmAdmin if an admin is going to log in and creates an frmRanking to see all users ranks.

            InitializeComponent();

            dbh = new DatabaseHandler();

            frmAdmin = new frmAdmin();
            frmRanking = new frmRanking();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Gives the option to quit the application.

            DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result.Equals(DialogResult.Yes))
            {
                if (dbh.GetCon().State == ConnectionState.Open)
                {
                    dbh.CloseConnectionToDB();
                }

                Application.Exit();
            }
        }

        private void btnShowRanking_Click(object sender, EventArgs e)
        {
            // Shows the scores from all users.

            frmRanking.Show();
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            // This is letting the user to make an account.

            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                // This shows a message is the files are empty.

                MessageBox.Show("Both fields are required");
            }
            else
            {
                // Making sure that the username doesn't exists in the database.

                dbh.TestConnection();
                dbh.OpenConnectionToDB();

                bool exist = false;

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [tblUsers] WHERE Username = @Username", dbh.GetCon()))
                {
                    cmd.Parameters.AddWithValue("Username", txtUsername.Text);

                    exist = (int)cmd.ExecuteScalar() > 0;
                }

                if (exist)
                {
                    // This shows a message if the user already exists.

                    MessageHandler.ShowMessage("This user already exists.");
                }
                else
                {
                    // This is Elton's secret account.

                    string user = txtUsername.Text.ToLower();

                    if (user == "Ninja")
                    {
                        dbh.CloseConnectionToDB();

                        string password = txtPassword.Text;
                        string userName = txtUsername.Text;

                        int admin = 2;
                        int score = 0;

                        // Preparing array to initialize later.
                        byte[] salt;
                        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                        // The hashing formula is executed 10000 times just to be sure that the security level is high.
                        Rfc2898DeriveBytes passwordToHash = new Rfc2898DeriveBytes(password, salt, 10000);
                        byte[] hashArray = passwordToHash.GetBytes(20);

                        // Copys the value of an byte array and paste them in an other array.
                        byte[] hashBytes = new byte[36];
                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hashArray, 0, hashBytes, 16, 20);

                        // Converting hashed password to a string
                        string savedPasswordHash = Convert.ToBase64String(hashBytes);

                        string sql = "INSERT INTO [tblUsers] ([Username], [Password], [IsAdmin], [Score]) VALUES ('" + userName + "', '" + savedPasswordHash + "', '" + admin + "', '" + score + "')";
                        
                        dbh.Execute(sql);
                    }
                    else
                    {
                        // Password hashing for registering.

                        dbh.CloseConnectionToDB();

                        string password = txtPassword.Text;
                        string userName = txtUsername.Text;

                        // Preparing array to initialize later.
                        byte[] salt;
                        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                        // The hashing formula is executed 10000 times just to be sure that the security level is high.
                        Rfc2898DeriveBytes passwordToHash = new Rfc2898DeriveBytes(password, salt, 10000);
                        byte[] hashArray = passwordToHash.GetBytes(20);

                        // Copys the value of an byte array and paste them in an other array.
                        byte[] hashBytes = new byte[36];
                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hashArray, 0, hashBytes, 16, 20);

                        // Converting hashed password to a string
                        string savedPasswordHash = Convert.ToBase64String(hashBytes);

                        int admin = 0;
                        int score = 0;

                        string sql = "INSERT INTO [tblUsers] ([Username], [Password], [IsAdmin], [Score]) VALUES ('" + userName + "', '" + savedPasswordHash + "', '" + admin + "', '" + score + "')";

                        dbh.Execute(sql);
                    }
                }
                dbh.CloseConnectionToDB();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // This is letting the user to log in and go towards the form where he/she belongs.

            dbh.TestConnection();
            dbh.OpenConnectionToDB();

            bool exist = false;

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Getting hashed password from database.

            using (SqlCommand cmd = new SqlCommand("SELECT Password FROM TblUsers WHERE Username =  @Username", dbh.GetCon()))
            {
                cmd.Parameters.AddWithValue("Username", username);

                this.gettingHashedPassword = Convert.ToString(cmd.ExecuteScalar());
            }

            // Comparing password from user input and saved password in database.
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [tblUsers] WHERE Username = @Username AND Password = @Password", dbh.GetCon()))
            {
                cmd.Parameters.AddWithValue("Username", username);
                cmd.Parameters.AddWithValue("Password", this.gettingHashedPassword);

                exist = (int)cmd.ExecuteScalar() > 0;
            }

            if (exist)
            {
                bool admin;

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [tblUsers] WHERE Username = @Username AND IsAdmin = 1", dbh.GetCon()))
                {
                    cmd.Parameters.AddWithValue("Username", username);

                    admin = (int)cmd.ExecuteScalar() > 0;
                }

                dbh.CloseConnectionToDB();

                if (admin)
                {
                    frmAdmin.Show();
                }
                else
                {
                    frmPlayer = new frmPlayer(frmRanking, username);
                    frmPlayer.Show();
                }
            }
            else
            {
                // This shows a message if the username and or password is wrong.

                MessageHandler.ShowMessage("Wrong username and/or password.");
            }
            dbh.CloseConnectionToDB();
        }
    }
}
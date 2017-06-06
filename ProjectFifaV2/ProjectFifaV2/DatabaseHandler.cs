using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace ProjectFifaV2
{
    class DatabaseHandler
    {
        private SqlConnection con;

        public DatabaseHandler()
        {
            // This is the database connection.

            string Path = Environment.CurrentDirectory;
            string[] appPath = Path.Split(new string[] { "bin" }, StringSplitOptions.None);
            AppDomain.CurrentDomain.SetData("DataDirectory", appPath[0]);

            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\db.mdf';Integrated Security=True;Connect Timeout=30");
        }

        public void TestConnection()
        {
            // This tests the database connection.

            bool open = false;

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    open = true;
                }

                con.Close();
            }

            if (!open)
            {
                Application.Exit();
            }
        }

        public void OpenConnectionToDB()
        {
            // This opens the database connection.

            con.Open();
        }

        public void CloseConnectionToDB()
        {
            // This is closes the database connection.
            
            con.Close();
        }


        public SqlConnection GetCon()
        {
            // This a getter that gets the connection.

            return con;
        }

        public System.Data.DataTable FillDT(string query)
        {
            // This fills the datatable.

            TestConnection();
            OpenConnectionToDB();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, GetCon());
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            CloseConnectionToDB();

            return dt;
        }


        public int DTInt(string query)
        {
            // This returns an int from the database.

            TestConnection();
            OpenConnectionToDB();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, GetCon());
            DataTable dt = new DataTable();
            SqlCommand cmd1 = new SqlCommand(query, con);
            int ret = Convert.ToInt32(cmd1.ExecuteScalar());

            CloseConnectionToDB();

            return ret;
        }

        public void Execute(string query)
        {
            // This executes the query and returns a message if its passed/failed.

            SqlCommand queryExecute = new SqlCommand(query, con);
            MessageBox.Show(query);

            try
            {
                TestConnection();
                OpenConnectionToDB();

                queryExecute.Prepare();
                queryExecute.ExecuteReader();

                MessageBox.Show("Saving succesvol!");

                CloseConnectionToDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");

                CloseConnectionToDB();
            }
        }
        public void ExecuteAdmin(string query)
        {
            SqlCommand queryExecute = new SqlCommand(query, con);
            MessageBox.Show(query);

            try
            {
                TestConnection();
                OpenConnectionToDB();

                queryExecute.Prepare();
                queryExecute.ExecuteNonQuery();

                CloseConnectionToDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                CloseConnectionToDB();
            }
        }
    }
}
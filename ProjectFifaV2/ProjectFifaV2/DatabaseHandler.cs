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
            if (con.State == ConnectionState.Open)
            {
            //    this.CloseConnectionToDB();
                int i = 0;
            }
            con.Open();
        }

        public void CloseConnectionToDB()
        {
            // This is closes the database connection.
            
            con.Close();

            if (con.State != ConnectionState.Closed)
            {
                int i = 0;
            }
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

        public SqlConnection GetCon()
        {
            // This a getter that gets the connection.

            return con;
        }
        public void Execute(string query)
        {
            // This executes the query and returns a message if its passed/failed.

            SqlCommand queryExecute = new SqlCommand(query, con);
            MessageBox.Show(query);

            try
            {
                OpenConnectionToDB();
                queryExecute.Prepare();
                queryExecute.ExecuteReader();
                MessageBox.Show("Success saving to database");
                CloseConnectionToDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                CloseConnectionToDB();
            }
        }

        public int ExecuteInt(string query)
        {
            // This executes the query and returns an int, if its failed it returns a message.

            SqlCommand queryExecute = new SqlCommand(query, con);
            MessageBox.Show(query);

            try
            {
                OpenConnectionToDB();

                int amountOfRows = queryExecute.ExecuteNonQuery();
                return amountOfRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                CloseConnectionToDB();
                return 0;
            }
        }
        public void ExecuteAdmin(string query)
        {
            SqlCommand queryExecute = new SqlCommand(query, con);
            MessageBox.Show(query);

            try
            {
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
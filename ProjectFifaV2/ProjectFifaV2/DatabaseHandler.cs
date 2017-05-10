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
            //SqlCeEngine engine = new SqlCeEngine(@"Data Source=.\DB.sdf");
            //engine.Upgrade(@"Data Source=.\DB2.sdf");


            string Path = Environment.CurrentDirectory;
            string[] appPath = Path.Split(new string[] { "bin" }, StringSplitOptions.None);
            AppDomain.CurrentDomain.SetData("DataDirectory", appPath[0]);

            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\db.mdf';Integrated Security=True;Connect Timeout=30");
        }

        public void TestConnection()
        {
            bool open = false;
            
            try
            {
                con.Open();
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
            con.Open();
        }

        public void CloseConnectionToDB()
        {
            con.Close();
        }

        public System.Data.DataTable FillDT(string query)
        {
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
            return con;
        }
        public void Execute(string query)
        {
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
        public void Execute1(string query)
        {
            SqlCommand queryExecute = new SqlCommand(query, con);


            try
            {
                OpenConnectionToDB();
                queryExecute.Prepare();
                queryExecute.ExecuteReader();

                CloseConnectionToDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                CloseConnectionToDB();
            }
        }
        public string ExecuteString(string query)
        {
            SqlCommand queryExecute = new SqlCommand(query, con);
            MessageBox.Show(query);

            try
            {
                OpenConnectionToDB();
                SqlDataReader dr = queryExecute.ExecuteReader();
                string treatment = dr[0].ToString();
                return treatment;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                CloseConnectionToDB();
                return "failed";
            }


        }
        public int ExecuteInt(string query)
        {
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
    }
}

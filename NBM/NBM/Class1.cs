using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace NBM
{
    class Class1
    {
        public static string constring = "Server=115.68.182.142;Database=nbm_board; Uid=root; pwd=nbm1033";
        public MySqlConnection conn = new MySqlConnection();
        private string sConnString = "";

        //DB접속
        public void ConnectDB()
        {
            try
            {
                sConnString = constring;
            }
            catch
            {
            }

            if (conn.State.ToString().Equals("Closed"))
            {
                conn.ConnectionString = sConnString;
                conn.Open();
            }
        }

        //DB나오기
        public void CloseDB()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        public DataTable GetDBTable(string sql)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

    }
}

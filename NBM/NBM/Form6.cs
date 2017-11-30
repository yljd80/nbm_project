using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace NBM
{
    public partial class Form6 : Form
    {
        Class1 db;
        DataTable dt;
        MySqlCommand cmd;

        static int datey1;
        static int datem1;
        static int dated1;

        public Form6()
        {
            InitializeComponent();

            db = new Class1();
            db.ConnectDB();

            ReadData();

            db.CloseDB();
        }
        private void ReadData()
        {
            String resultsql = "SELECT * FROM result";
            dt = db.GetDBTable(resultsql);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                db.conn.Open();
                DateTime date1 = dateTimePicker1.Value;
                datey1 = date1.Year;
                datem1 = date1.Month;
                dated1 = date1.Day;

                db.ConnectDB();

                //MessageBox.Show(Convert.ToString(datey1));
                int r_id = 20000;
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (row.Cells[0].Value == null)
                        continue;
                    if (int.Parse(row.Cells[0].Value.ToString()) > r_id)
                    {
                        r_id = int.Parse(row.Cells[0].Value.ToString());
                    }
                }
                r_id++;
                cmd = new MySqlCommand();
                cmd.Connection = db.conn;
                String savesql = "INSERT INTO result (r_id, result) VALUES (" + r_id + ",(SELECT COUNT(*) FROM sensed_data WHERE date_Y = " + datey1 + " and date_M = " + datem1 + " and date_D = " + dated1 + ")); ";
                cmd.CommandText = savesql;
                cmd.ExecuteNonQuery();
                db.CloseDB();
            }
            catch
            {

            }
            //다시 그리드뷰 띄워줌
            db.ConnectDB();
            ReadData();
            db.CloseDB();
        }
    }
}

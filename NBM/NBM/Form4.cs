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
    public partial class Form4 : Form
    {
        Class1 db;
        private int p_id;

        public Form4()
        {
            InitializeComponent();
            db = new Class1();
            db.ConnectDB(); //디비연결

            PrintAllPropertyData();

            db.CloseDB();
        }

        private void PrintAllPropertyData()
        {
            string sql = "SELECT * FROM property";
            DataTable dt = db.GetDBTable(sql);
            dataGridView1.DataSource = dt;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //추가
            string LastNum = (int.Parse(dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[0].Value.ToString()) + 1).ToString();
            db.ConnectDB();

            MySqlCommand InsertCommand = new MySqlCommand();

            InsertCommand.Connection = db.conn;
            InsertCommand.CommandText = "INSERT INTO property(p_id, p_name, p_unit, sensor_id2) VALUES(@p_id, @p_name, @p_unit, @s_id)";

            InsertCommand.Parameters.Add("@p_id", MySqlDbType.Int32, 11);
            InsertCommand.Parameters.Add("@p_name", MySqlDbType.VarChar, 45);
            InsertCommand.Parameters.Add("@p_unit", MySqlDbType.VarChar, 45);
            InsertCommand.Parameters.Add("@s_id", MySqlDbType.Int32, 11);

            //1센서명 2센서종류 3센서아이디
            InsertCommand.Parameters[0].Value = LastNum;
            InsertCommand.Parameters[1].Value = textBox1.Text;
            InsertCommand.Parameters[2].Value = textBox2.Text;
            InsertCommand.Parameters[3].Value = textBox3.Text;

            InsertCommand.ExecuteNonQuery();

            MySqlDataAdapter dda = new MySqlDataAdapter("select * from property", db.conn);
            DataTable ddata = new DataTable();
            dda.Fill(ddata);
            dataGridView1.DataSource = ddata;
            dataGridView1.Refresh();
            db.CloseDB();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //삭제
            String query = "DELETE FROM property WHERE p_id = @p_id ";

            db.ConnectDB();

            try
            {
                MySqlCommand DeleteCommand = new MySqlCommand();
                DeleteCommand.Connection = db.conn;
                DeleteCommand.CommandText = query;
                DeleteCommand.Parameters.Add("@p_id", MySqlDbType.Int32, 11);
                DeleteCommand.Parameters[0].Value = p_id;

                if (DeleteCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("삭제 성공");
                }
                else
                {
                    MessageBox.Show("삭제 실패");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            PrintAllPropertyData();

            db.CloseDB();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //수정
            String query = "UPDATE property SET p_name = @p_name, p_unit = @p_unit, sensor_id2 = @sensor_id2 "
              + "WHERE p_id = @p_id ";

            db.ConnectDB();

            try
            {
                MySqlCommand UpdateCommand = new MySqlCommand();
                UpdateCommand.Connection = db.conn;
                UpdateCommand.CommandText = query;

                UpdateCommand.Parameters.Add("@p_name", MySqlDbType.VarChar, 45);
                UpdateCommand.Parameters.Add("@p_unit", MySqlDbType.VarChar, 45);
                UpdateCommand.Parameters.Add("@sensor_id2", MySqlDbType.Int32, 11);
                UpdateCommand.Parameters.Add("@p_id", MySqlDbType.Int32, 11);

                UpdateCommand.Parameters[0].Value = textBox1.Text;
                UpdateCommand.Parameters[1].Value = textBox2.Text;
                UpdateCommand.Parameters[2].Value = textBox3.Text;
                UpdateCommand.Parameters[3].Value = p_id;

                if (UpdateCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("수정 성공");
                }
                else
                {
                    MessageBox.Show("수정 실패");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.CloseDB();
            }


            // 커밋 문제
            db.ConnectDB();
            PrintAllPropertyData();
            db.CloseDB();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            p_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["p_id"].Value.ToString());

            textBox1.Text = dataGridView1.CurrentRow.Cells["p_name"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["p_unit"].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells["s_id"].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //검색
            if (comboBox1.Text == "p_id")
            {
                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.Int32, 11);
                SearchCommand.Parameters[0].Value = textBox4.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM property WHERE p_id = " + textBox4.Text, db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
            }
            else if (comboBox1.Text == "p_name")
            {
                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.VarChar, 45);
                SearchCommand.Parameters[0].Value = textBox4.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM property WHERE p_name = '" + textBox4.Text + "'", db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
            }
            else if (comboBox1.Text == "p_unit")
            {

                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.VarChar, 45);
                SearchCommand.Parameters[0].Value = textBox4.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM property WHERE p_unit = '" + textBox4.Text + "'", db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
            }
            else if (comboBox1.Text == "sensor_id2")
            {

                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.Int32, 11);
                SearchCommand.Parameters[0].Value = textBox4.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM property WHERE sensor_id2 = " + textBox4.Text, db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
            }

        }

    }
}

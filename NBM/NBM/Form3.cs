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
    public partial class Form3 : Form
    {
        Class1 db;
        private int s_id;

        public Form3()
        {
            InitializeComponent();
            db = new Class1();
            db.ConnectDB(); //디비연결

            PrintAllSensorData();
          

            db.CloseDB();
        }

        private void PrintAllSensorData()
        {
            string sql = "SELECT * FROM sensor";
            DataTable dt = db.GetDBTable(sql);
            dataGridView1.DataSource = dt;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //추가
            string LastNum = (int.Parse(dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[0].Value.ToString()) + 1).ToString();
            db.ConnectDB();

                MySqlCommand InsertCommand = new MySqlCommand();

                InsertCommand.Connection = db.conn;
                InsertCommand.CommandText = "INSERT INTO sensor(s_id, s_model, latitude, longitude, manager_id1) VALUES(@s_id, @s_model, @latitude, @longitude, @manager_id)";

                InsertCommand.Parameters.Add("@s_id", MySqlDbType.Int32, 11);
                InsertCommand.Parameters.Add("@s_model", MySqlDbType.VarChar, 45);
                InsertCommand.Parameters.Add("@latitude", MySqlDbType.Float);
                InsertCommand.Parameters.Add("@longitude", MySqlDbType.Float);
                InsertCommand.Parameters.Add("@manager_id", MySqlDbType.Int32, 11);
                //1모델명 2 위도 3 경도 4 관리자 아이디
                InsertCommand.Parameters[0].Value = LastNum;
                InsertCommand.Parameters[1].Value = txtModel.Text;
                InsertCommand.Parameters[2].Value = txtTx.Text;
                InsertCommand.Parameters[3].Value = txtLx.Text;



                InsertCommand.Parameters[4].Value = txtManager.Text;


                InsertCommand.ExecuteNonQuery();
            MySqlDataAdapter dda = new MySqlDataAdapter("select * from sensor", db.conn);
            DataTable ddata = new DataTable();
            dda.Fill(ddata);
            dataGridView1.DataSource = ddata;
            dataGridView1.Refresh();
            db.CloseDB();
            
        }
        private void button2_Click(object sender, EventArgs e)
        {   //삭제
            
            String query = "DELETE FROM sensor WHERE s_id = @s_id ";

            db.ConnectDB();

            try
            {
                MySqlCommand DeleteCommand = new MySqlCommand();
                DeleteCommand.Connection = db.conn;
                DeleteCommand.CommandText = query;
                DeleteCommand.Parameters.Add("@s_id", MySqlDbType.Int32, 11);
                DeleteCommand.Parameters[0].Value = s_id;

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

            PrintAllSensorData();

            db.CloseDB();
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            String query = "UPDATE sensor SET s_model=@s_model, latitude = @latitude , longitude = @longitude, manager_id1 = @manager_id1 " 
                          +"WHERE s_id = @s_id ";
            db.ConnectDB();

            try
            {
                MySqlCommand UpdateCommand = new MySqlCommand();
                UpdateCommand.Connection = db.conn;
                UpdateCommand.CommandText = query;

                UpdateCommand.Parameters.Add("@s_model", MySqlDbType.VarChar, 45);
                UpdateCommand.Parameters.Add("@latitude", MySqlDbType.Float, 20);
                UpdateCommand.Parameters.Add("@longitude", MySqlDbType.Float, 20);
                UpdateCommand.Parameters.Add("@manager_id1", MySqlDbType.Int32, 11);
                UpdateCommand.Parameters.Add("@s_id", MySqlDbType.Int32, 11);

                UpdateCommand.Parameters[0].Value = txtModel.Text;
                UpdateCommand.Parameters[1].Value = txtTx.Text;
                UpdateCommand.Parameters[2].Value = txtLx.Text;
                UpdateCommand.Parameters[3].Value = txtManager.Text;
                UpdateCommand.Parameters[4].Value = s_id;


             

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
            PrintAllSensorData();
            db.CloseDB();
            
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            s_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["s_id"].Value.ToString());

            txtModel.Text = dataGridView1.CurrentRow.Cells["s_model"].Value.ToString();
            txtTx.Text = dataGridView1.CurrentRow.Cells["latitude"].Value.ToString();
            txtLx.Text = dataGridView1.CurrentRow.Cells["longitude"].Value.ToString();
            txtManager.Text = dataGridView1.CurrentRow.Cells["manager_id1"].Value.ToString();
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            
            if (comboBox1.Text == "manager_id")
            {

                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.Int32, 11);
                SearchCommand.Parameters[0].Value = Search.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM sensor WHERE manager_id1 = " + Search.Text, db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
            }
            else if (comboBox1.Text == "s_id")
            {

                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.Int32, 11);
                SearchCommand.Parameters[0].Value = Search.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM sensor WHERE s_id = " + Search.Text, db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
            }
            else if (comboBox1.Text == "s_model")
            {

                db.ConnectDB();
                MySqlCommand SearchCommand = new MySqlCommand();
                SearchCommand.Connection = db.conn;


                SearchCommand.Parameters.Add("@Search", MySqlDbType.VarChar, 45);
                SearchCommand.Parameters[0].Value = Search.Text;

                MySqlDataAdapter dda = new MySqlDataAdapter("SELECT * FROM sensor WHERE s_model = '" + Search.Text + "'", db.conn);

                DataTable ddata = new DataTable();

                dda.Fill(ddata);
                dataGridView1.DataSource = ddata;
                dataGridView1.Refresh();
                db.CloseDB();
                
            }
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form form = new Form4();
            form.Show();
        }
    }
}

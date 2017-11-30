using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace NBM
{
    public partial class Form2 : Form
    {
        Class1 db;
        DataTable dt;
        MySqlCommand cmd;
        private int m_id;

        public Form2()
        {
            InitializeComponent();
            db = new Class1();
            db.ConnectDB();

            ReadData();

            db.CloseDB();
        }
        private void ReadData()
        {
            string sql = "SELECT * FROM manager";
            dt = db.GetDBTable(sql);
            dataGridView1.DataSource = dt;
        }
        //삽입
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                db.conn.Open();
                int max1 = 4000;
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (row.Cells[0].Value == null)
                        continue;
                    if (int.Parse(row.Cells[0].Value.ToString()) > max1)
                    {
                        max1 = int.Parse(row.Cells[0].Value.ToString());
                    }
                }
                max1++;
                dt.Rows.Add(max1, textBox1.Text, textBox2.Text, textBox3.Text);

                cmd = new MySqlCommand();
                cmd.Connection = db.conn;
                string insertsql = "INSERT INTO manager VALUES(" + max1 + ",'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')";
                cmd.CommandText = insertsql;
                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = dt;

                MessageBox.Show("관리자가 추가되었습니다.");
                db.conn.Close();
            }
            catch
            {
                MessageBox.Show("삽입실패");
            }
        }
        //삭제
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                db.conn.Open();
                cmd = new MySqlCommand();
                cmd.Connection = db.conn;
                String deletesql = "delete from manager where m_id = " + m_id;

                cmd.CommandText = deletesql;
                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = dt;

                MessageBox.Show("해당 관리자가 삭제되었습니다.");
                db.conn.Close();
            }
            catch
            {
                MessageBox.Show("삭제 실패");
            }
            //다시 그리드뷰 띄워줌
            db.ConnectDB();
            ReadData();
            db.CloseDB();

        }
        //수정
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                db.conn.Open();
                cmd = new MySqlCommand();
                cmd.Connection = db.conn;
                String adjustsql = "UPDATE manager SET m_first = '" + textBox1.Text +"', m_name = '" + textBox2.Text + "', m_location = '" + textBox3.Text +"' WHERE m_id =" + m_id;

                cmd.CommandText = adjustsql;
                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = dt;

                if (cmd.ExecuteNonQuery() == 1)
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
            //다시 그리드뷰 띄워줌
            db.ConnectDB();
            ReadData();
            db.CloseDB();

        }

        //검색
        private void button4_Click(object sender, EventArgs e)
        {
            db.ConnectDB();
            cmd = new MySqlCommand();
            cmd.Connection = db.conn;

            string comboitem = comboBox1.Text.ToString();
            if (comboitem == "m_id")
            {
                string searchsql = "SELECT * FROM manager WHERE m_id = " + textBox4.Text;
                dt = db.GetDBTable(searchsql);
                dataGridView1.DataSource = dt;
            }
            else if (comboitem == "m_first")
            {
                string searchsql = "SELECT * FROM manager WHERE m_first = " + "'" + textBox4.Text + "'";
                dt = db.GetDBTable(searchsql);
                dataGridView1.DataSource = dt;
            }
            else if (comboitem == "m_name")
            {
                string searchsql = "SELECT * FROM manager WHERE m_name = " + "'" + textBox4.Text + "'";
                dt = db.GetDBTable(searchsql);
                dataGridView1.DataSource = dt;
            }
            else if (comboitem == "m_location")
            {
                string searchsql = "SELECT * FROM manager WHERE m_location = " + "'" + textBox4.Text + "'";
                dt = db.GetDBTable(searchsql);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//클릭시 정보 텍스트 상자에 띄우기
        {
            m_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["m_id"].Value.ToString());
            textBox1.Text = dataGridView1.CurrentRow.Cells["m_first"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["m_name"].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells["m_location"].Value.ToString();
        }
    }
}

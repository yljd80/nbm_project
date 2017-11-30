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
    public partial class Form5 : Form
    {
        Class1 db;
        DataTable dt;
        MySqlCommand cmd;

        static int datey1;
        static int datey2;
        static int datem1;
        static int datem2;
        static int dated1;
        static int dated2;

        public Form5()
        {
            InitializeComponent();
            db = new Class1();
            db.ConnectDB(); //디비연결

            PrintAllSensorData();


            db.CloseDB();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Form form = new Form6();
            form.Show();
        }

        private void PrintAllSensorData()
        {
            string sql = "SELECT * FROM sensed_data";
            DataTable dt = db.GetDBTable(sql);
            dataGridView1.DataSource = dt;
        }

        //비교탭
        public string getMonthData(int start, int end)
        {
            db.ConnectDB();
            MySqlCommand command = new MySqlCommand();
            string query, resultStr;
            int count;
            // 연도와 월을 인자로 전달해서 해당 년도 월의 일수를 반환
            double day = DateTime.DaysInMonth(2016, start);
            double sResult, eResult;

            query = String.Format("SELECT count(*) FROM sensed_data WHERE date_M = {0} ", start);
            command.Connection = db.conn;
            command.CommandText = query;
            // 실행되는 첫 컬럼의 값을 반환 command.ExecuteScalar()
            count = Convert.ToInt32(command.ExecuteScalar());
            sResult = count / day;


            query = String.Format("SELECT count(*) FROM sensed_data WHERE date_M = {0} ", end);
            command.CommandText = query;
            count = Convert.ToInt32(command.ExecuteScalar());
            day = DateTime.DaysInMonth(2016, end);
            eResult = count / day;


            // 마이너스 데이터 방지
            //if(sResult > eResult)
            resultStr = String.Format("{0}월:{2} {1}월:{3} "
                                        , start, end, Math.Round(sResult, 2), Math.Round(eResult, 2));
            //else
            //    resultStr = String.Format("{0}월:{3} {1}월:{4} / {0}월이 {1}월보다 {2} 만큼 큽니다."
            //        , end, start, Math.Round(eResult - sResult,2), Math.Round(sResult, 2), Math.Round(eResult,2));


            return resultStr;
            db.CloseDB();
        }
        public string compare(int start, int end)
        {
            db.ConnectDB();
            MySqlCommand command = new MySqlCommand();
            string query, resultStr;
            int count;
            // 연도와 월을 인자로 전달해서 해당 년도 월의 일수를 반환
            double day = DateTime.DaysInMonth(2016, start);
            double sResult, eResult;

            query = String.Format("SELECT count(*) FROM sensed_data WHERE date_M = {0} ", start);
            command.Connection = db.conn;
            command.CommandText = query;
            // 실행되는 첫 컬럼의 값을 반환 command.ExecuteScalar()
            count = Convert.ToInt32(command.ExecuteScalar());
            sResult = count / day;


            query = String.Format("SELECT count(*) FROM sensed_data WHERE date_M = {0} ", end);
            command.CommandText = query;
            count = Convert.ToInt32(command.ExecuteScalar());
            day = DateTime.DaysInMonth(2016, end);
            eResult = count / day;


            // 마이너스 데이터 방지
            //if(sResult > eResult)
            resultStr = String.Format("{0}월이 {1}월보다 {2} 만큼 큽니다."
                                        , start, end, Math.Round(sResult - eResult, 2));
            //else
            //    resultStr = String.Format("{0}월:{3} {1}월:{4} / {0}월이 {1}월보다 {2} 만큼 큽니다."
            //        , end, start, Math.Round(eResult - sResult,2), Math.Round(sResult, 2), Math.Round(eResult,2));


            return resultStr;
            db.CloseDB();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            // 콤보박스에서 데이터 추출 comboBox9.SelectedItem.ToString()
            if (comboBox9.SelectedItem.ToString() == "" ||
                comboBox11.SelectedItem.ToString() == "" ||
                comboBox9.SelectedItem.ToString().CompareTo(comboBox11.SelectedItem.ToString()) == 0)
                return;

            // 비교 날짜1
            int startMonth = Convert.ToInt32(comboBox9.SelectedItem.ToString());

            // 비교 날짜2
            int endMonth = Convert.ToInt32(comboBox11.SelectedItem.ToString());

            string result;
            string result_2;
            try
            {
                // 결과 값 스트링 반환
                result = getMonthData(startMonth, endMonth);
                result_2 = compare(startMonth, endMonth);

                // 결과 값 리스트박스에 추가
                listBox1.Items.Add(result);
                listBox2.Items.Add(result_2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //기간탭

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime date1 = dateTimePicker1.Value;
            datey1 = date1.Year;
            datem1 = date1.Month;
            dated1 = date1.Day;

            DateTime date2 = dateTimePicker2.Value;
            datey2 = date2.Year;
            datem2 = date2.Month;
            dated2 = date2.Day;

            //MessageBox.Show(Convert.ToString(datem1));
            //MessageBox.Show(Convert.ToString(dated1));
            
            db.ConnectDB();
            cmd = new MySqlCommand();
            cmd.Connection = db.conn;
            String countsql = "SELECT COUNT(*) FROM sensed_data WHERE(date_Y between " + datey1 + " and "  + datey2 + ") and (date_M between " + datem1 + " and " +  datem2 + ") and (date_D between " + dated1 +" and " + dated2 +")";
            cmd.CommandText = countsql;
            cmd.ExecuteNonQuery();

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            textBox2.Text = Convert.ToString(count);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime date1 = dateTimePicker1.Value;
            datey1 = date1.Year;
            datem1 = date1.Month;
            dated1 = date1.Day;

            DateTime date2 = dateTimePicker2.Value;
            datey2 = date2.Year;
            datem2 = date2.Month;
            dated2 = date2.Day;

            db.ConnectDB();

            //MessageBox.Show(Convert.ToString(datey1));
            String ranksql = "SELECT date_H, COUNT(date_H) as count FROM sensed_data WHERE(date_Y between " + datey1 + " and " + datey2 + ") and (date_M between " + datem1 + " and " + datem2 + ") and (date_D between " + dated1 + " and " + dated2 + ") GROUP BY date_H ORDER BY COUNT(date_H) DESC LIMIT 5";
            dt = db.GetDBTable(ranksql);
            dataGridView3.DataSource = dt;
            db.CloseDB();
        }


        //지역탭

        private void button1_Click(object sender, EventArgs e)
        {
            String area = comboBox1.Text;
 
            db.ConnectDB();

            cmd = new MySqlCommand();
            cmd.Connection = db.conn;
            String countsql = "SELECT count(*) "
                                + " FROM nbm_board.sensed_data "
                                + " WHERE sensor_id1 IN (SELECT s_id "
                                                     + " FROM nbm_board.sensor "
                                                     + " WHERE manager_id1 IN (SELECT m_id "
                                                                           + " FROM nbm_board.manager "
                                                                           + " WHERE m_location = '" + area + "'));";
            cmd.CommandText = countsql;
            cmd.ExecuteNonQuery();

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            textBox1.Text = Convert.ToString(count);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String area = comboBox1.Text;

            db.ConnectDB();

            String ranksql = "SELECT sensor_id1,count(date_Y) "
                          + " FROM nbm_board.sensed_data "
                          + " WHERE sensor_id1 IN (SELECT s_id "
                                               + " FROM nbm_board.sensor "
                                               + " WHERE manager_id1 IN (SELECT m_id "
                                                                     + " FROM nbm_board.manager "
                                                                     + " WHERE m_location = '" + area + "')) "
                          + " GROUP BY sensor_id1 "
                          + " ORDER BY COUNT(date_Y) DESC LIMIT 5;";//////
            dt = db.GetDBTable(ranksql);
            dataGridView2.DataSource = dt;
            db.CloseDB();
        }

    }
}

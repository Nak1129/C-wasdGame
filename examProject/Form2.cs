using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace examProject
{
    public partial class Form2 : Form
    {
        string str;
        float rate;
        int [] GetPoint = new int[2];

        public Form2()
        {
            InitializeComponent();
            label2.Text = Interaction.InputBox("您的名字", "名字輸入框");

            label3.Text = "";
            label4.Text = "";
            
        }

        //抓正確和錯誤 次數
        public int[] GetFrom1Point
        {
            set
            {
                GetPoint[0] = value[0];
                GetPoint[1] = value[1];
            }
        }
        //抓正確率
        public float GetFrom1Rate
        {
            set
            {
                rate = value;
            }
        }

        public void SetValue()
        {
            this.label1.Text = GetPoint[0].ToString();
            this.label12.Text = GetPoint[1].ToString();
            this.label13.Text = rate.ToString();
        }


        
        public SqlConnection GetSqlCon()
        {
            //待修改
            // string str = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename "+ Application.StartupPath+ @"\rank.mdf;";
            string str = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nak\Downloads\C-wasdGame-master\C-wasdGame-master\examProject\rank.mdf;";
            SqlConnection sqlCon = new SqlConnection(str);
            sqlCon.Open();
            return sqlCon;            
        }      
       
        
        

        //新增
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string name = label2.Text;

                int correct = int.Parse(label1.Text);
                int uncorrect = int.Parse(label12.Text);                
                //float test_float = (rate);                
                
                SqlConnection sqlCon = GetSqlCon();              
                string sql_insert = "insert into player (Name,Correct,Uncorrect,rate) values(N'" + name + "','" + correct + "','" + uncorrect + "','" + rate + "')";
                SqlCommand cmd = new SqlCommand(sql_insert, sqlCon);
                //執行指令 並讓系統讀取                
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("新增成功");
                    //限制新增一次
                    button2.Enabled = false;
                }
                else
                {
                    MessageBox.Show("新增失敗");
                }
              
                string sql_searchOwn = "select * from player where Name= N'" + name + "' and Correct='" + correct + "' and Uncorrect='" + uncorrect + "'and rate='"+ rate + "'" ;
                SqlCommand cmd_searchOwn = new SqlCommand(sql_searchOwn, sqlCon);
                SqlDataReader sql_dr = cmd_searchOwn.ExecuteReader();
                while (sql_dr.Read())
                {
                    label4.Text = string.Format("ID:{0} 名字:{1}正確:{2} 錯誤:{3}正確率{4} ", sql_dr.GetInt32(0), sql_dr.GetString(1), sql_dr.GetInt32(2), sql_dr.GetInt32(3), sql_dr.GetDouble(4));
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        //查詢自己
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int tb_id = int.Parse(textBox2.Text);
                string tb_name = textBox3.Text;

                SqlConnection sqlCon = GetSqlCon();
                string sql_searchOwn = "select * from player where Id='"+ tb_id +"' and Name = N'"+ tb_name +"'";
                SqlCommand cmd_searchOwn = new SqlCommand(sql_searchOwn, sqlCon);
                SqlDataReader sql_dr = cmd_searchOwn.ExecuteReader();
                if (!sql_dr.HasRows)
                {
                    MessageBox.Show("查無此人", "未達到");
                    label7.Text = "";
                }
                while (sql_dr.Read())
                {
                    //string.Format("自己想加標頭", 抓取table 內容)
                    label7.Text = string.Format("ID:{0}名字:{1}正確:{2}錯誤:{3}正確率{4}",sql_dr.GetInt32(0),sql_dr.GetString(1),sql_dr.GetInt32(2),sql_dr.GetInt32(3),sql_dr.GetDouble(4));
                }

                //清空
                textBox2.Text = "";
                textBox3.Text = "";
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        //修改
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int db_id = int.Parse(textBox1.Text);                
                string changeNameAfter = textBox5.Text;

                SqlConnection sqlCon = GetSqlCon();
                string updateOwn = "update player set Name= N'" + changeNameAfter + "'where Id='" + db_id + "'";
                SqlCommand cmd = new SqlCommand(updateOwn, sqlCon);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("修改成功");
                    textBox1.Text = "";
                    textBox5.Text = "";
                    label2.Text = changeNameAfter ;
                }
                else
                {
                    MessageBox.Show("修改失敗");
                    label9.Text = "";
                    textBox1.Text = "";
                    textBox5.Text = "";
                }
                string sql_search = "select * from player where Id='" + db_id + "'";
                SqlCommand cmd_d = new SqlCommand(sql_search, sqlCon);
                //執行指令 並讓系統讀取
                SqlDataReader sql_dr = cmd_d.ExecuteReader();
                while (sql_dr.Read())
                {
                    //string.Format("自己想加標頭", 抓取table 內容)
                    label4.Text = string.Format("ID:{0} 名字:{1}",sql_dr.GetInt32(0),sql_dr.GetString(1));
                    label9.Text = "更改後的名字  "+label4.Text;
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
        }
        //刪除指定自己
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int db_id = int.Parse(textBox4.Text);
                SqlConnection sqlCon = GetSqlCon();

                string deleteOwn = "DELETE FROM player WHERE Id='" + db_id + "'";
                SqlCommand cmd = new SqlCommand(deleteOwn, sqlCon);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("刪除成功");
                    textBox4.Text = "";
                    label4.Text = "資料被刪除了";
                    label7.Text = "";
                    label2.Text = "?_?";
                }
                else
                {
                    MessageBox.Show("刪除失敗");
                    textBox4.Text = "";
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        //正確rank
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                //前置作業
                label3.Text = "     Rank CorrectPoint Top 5 \n";
                SqlConnection sqlCon = GetSqlCon();

                string sql_search = "select top 5* from player  ORDER BY Correct DESC";
                SqlCommand cmd = new SqlCommand(sql_search, sqlCon);
                //執行指令 並讓系統讀取
                SqlDataReader sql_dr = cmd.ExecuteReader();
                while (sql_dr.Read())
                {
                    str = string.Format("ID:{0}名字:{1}正確:{2}", sql_dr.GetInt32(0), sql_dr.GetString(1), sql_dr.GetInt32(2));
                    label3.Text += str + "\n";
                }
                sqlCon.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }
        //錯誤rank
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //前置作業
                label3.Text = "     Rank UncorrectPoint Top 5 \n";
                SqlConnection sqlCon = GetSqlCon();  
                
                string sql_search = "select top 5* from player  ORDER BY UNcorrect DESC";
                SqlCommand cmd = new SqlCommand(sql_search, sqlCon);
                //執行指令 並讓系統讀取
                SqlDataReader sql_dr = cmd.ExecuteReader();
                while (sql_dr.Read())
                {
                    str = string.Format("ID:{0}名字:{1}正確:{2}錯誤:{3}",sql_dr.GetInt32(0),sql_dr.GetString(1),sql_dr.GetInt32(2), sql_dr.GetInt32(3));
                    label3.Text += "ID:"+ sql_dr.GetInt32(0)+ "名字:" + sql_dr.GetString(1)+ "錯誤:" + sql_dr.GetInt32(3) + "\n";
                }
                  sqlCon.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        //正確率排行版
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                //前置作業
                label3.Text = "     Rank Rate Top 5 \n";
                SqlConnection sqlCon = GetSqlCon();               
                string sql_search = "select top 5* from player  ORDER BY rate DESC";
                SqlCommand cmd = new SqlCommand(sql_search, sqlCon);
                //執行指令 並讓系統讀取
                SqlDataReader sql_dr = cmd.ExecuteReader();
                while (sql_dr.Read())
                {
                    str = string.Format("ID:{0}名字:{1}正確:{2}錯誤:{3}正確率{4}", sql_dr.GetInt32(0), sql_dr.GetString(1), sql_dr.GetInt32(2), sql_dr.GetInt32(3), sql_dr.GetDouble(4));
                    label3.Text += "ID:" + sql_dr.GetInt32(0) + "名字:" + sql_dr.GetString(1) + "正確率:" + sql_dr.GetDouble(4) + "\n";
                }
                sqlCon.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}

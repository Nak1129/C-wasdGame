using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;

namespace examProject
{
    public partial class Form1 : Form
    {
        
        int time;
        int correct;
        int uncorrect;   
        
        public Form1()
        {
            InitializeComponent();
            //介面  
            timer1.Enabled = false;
            label8.Visible = false;
            texchage();
            
        }
        
        public void texchage()
        {
            Random random = new Random();
            string[] characters = { "W", "S", "A", "D" };
            string rd_char = characters[random.Next(0, characters.Length)];
            label8.Text = rd_char;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form2 lForm = new Form2();//實例化一個Form2窗口  

            int[] pointArray = { int.Parse(label9.Text) , int.Parse(label10.Text) };
            lForm.GetFrom1Point = pointArray;  //設置Form2中string1的值  
            
            lForm.GetFrom1Rate = float.Parse(label11.Text);

            lForm.SetValue();//設置Form2中Label1的  
            lForm.ShowDialog();
            
        }

        //計算
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            {
                //壓值變色
                if (e.KeyCode == Keys.W)
                {
                    label1.ForeColor = Color.Red;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                }
                if (e.KeyCode == Keys.S)
                {
                    label2.ForeColor = Color.Red;
                }
                else
                {
                    label2.ForeColor = Color.Black;
                }
                if (e.KeyCode == Keys.A)
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }
                if (e.KeyCode == Keys.D)
                {
                    label4.ForeColor = Color.Red;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }
            }
            if (timer1.Enabled == true)
            {
                //如按鈕 == rd_char就馬上換字
                if (e.KeyCode.ToString() == label8.Text)
                {
                    //正確次數
                    correct++;                    
                    texchage();
                    label9.Text = correct.ToString();
                }
                else if (e.KeyCode.ToString() != label8.Text)
                {
                    uncorrect++;
                    label10.Text = uncorrect.ToString();
                }              
               
            }
        }

        //倒數計時
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;             
            if (time > 0)
            {
                time--;
                label7.Text = time.ToString();

                //先蓋住答案
                label8.Visible = true;

                float  correct   = int.Parse(label9.Text);
                float uncorrect = int.Parse(label10.Text);
                float rate  = (correct / (correct + uncorrect))*100 ;
                string rate_string = rate.ToString("#.##");
                label11.Text  = rate_string;
            }            
            else
            {                               
                timer1.Enabled = false;
                MessageBox.Show("time up");
                button1.Enabled = true;
            }         

        }       

        //開始按鈕
        private void button2_Click(object sender, EventArgs e)
        {
            //外表
            label1.Text = "W";
            label2.Text = "S";
            label3.Text = "A";
            label4.Text = "D";

            //初始化
            correct = 0;
            label9.Text = correct.ToString();
            uncorrect = 0;
            label10.Text = uncorrect.ToString();

            timer1.Enabled = true;            
            button1.Enabled = false; 
            
            time = 10;            
            label7.Text = time.ToString();
        }
               
    }
}

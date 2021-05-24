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
using MyDataBase;

namespace gis_test1
{
    public partial class launch : Form
    { 
        DataTable dt;
        public launch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usr = textBox1.Text;
            string pw = textBox2.Text;
            string sql = "select * from users0322 where 用户名=\'" + usr + "\' and 密码=\'" + pw + "\'";
            dt = MyDatabase.QueryFromDt(sql);
            if (dt == null)
                return;
            int row = dt.Rows.Count;
            if (row == 1)
            {
                Form1.usr = (string)dt.Rows[0][0];  //用户名
                Form1.pipe = (int)dt.Rows[0][2];    //管线编号
                Form1.segment = (int)dt.Rows[0][3]; //分段
                //如果管理权限和部门管理员都为1
                if ((int)dt.Rows[0][4] == 1 && (int)dt.Rows[0][6] == 1)
                {
                    Form1.manage = 1;
                }
                //只有管理权限为1
                else if ((int)dt.Rows[0][4] == 1)
                {
                    Form1.manage = 1;
                }
                //只有部门管理员为1
                else if ((int)dt.Rows[0][6] == 1)
                {
                    Form1.manage = 2;
                }
                //超级管理员为1
                else if((int)dt.Rows[0][8] == 1)
                {
                    Form1.manage = 3;
                }
                else Form1.manage = 0;
                Form1.frm1.refresh_label_launch();
                MessageBox.Show("登录成功", "登录提示\n");
                this.Close();
            }
            else
            {
                if (row == 0)
                {
                    MessageBox.Show("用户名或密码错误", "登录提示\n");
                }
            }
        }

        private int return_pipe()
        {
            return (int)dt.Rows[0][2];
        }
        private int return_segment()
        {
            return (int)dt.Rows[0][3];
        }

        private void launch_Load(object sender, EventArgs e)
        {

        }
    }
}

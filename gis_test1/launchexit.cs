using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gis_test1
{
    public partial class launchexit : Form
    {
        public launchexit()
        {
            InitializeComponent();
            label2.Text = Form1.usr;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.usr = null;
            Form1.pipe = -2;
            Form1.segment = -2;
            Form1.manage = 0;       
            MessageBox.Show("退出登录成功", "登录提示\n");
            Form1.frm1.refresh_label_launchout();
            this.Close();
        }

        private void launchexit_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void launchexit_Load_1(object sender, EventArgs e)
        {

        }
    }
}

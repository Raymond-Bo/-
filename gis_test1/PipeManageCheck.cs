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
    public partial class PipeManageCheck : Form
    {
        private int operation;
        private string sector = null;
        private string pipe = null;
        private string segment = null;
        private string oil = null;
        public PipeManageCheck(int op, string sector, string pipe, string segment,string oil)
        {
            this.operation = op;
            this.sector = sector;
            this.pipe = pipe;
            this.segment = segment;
            this.oil = oil;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection Conn = MyDatabase.Static_myConnectionToMySql();
            string sql = null;
            if (Conn == null)
            {
                MessageBox.Show("数据库连接异常");
                return;
            }

            if (operation==1)
            {
                //添加
                sql = "INSERT INTO sector0322  (部门,管线编号,分段,油田) VALUES (\'"+sector+ "\',"+pipe+"," + segment + ",\'"+oil+ "\') ";
            }
            else if(operation==2)
            {
                //删除
                sql = "DELETE FROM sector0322 where 部门=\'" + sector + "\' and 管线编号=" + pipe + " and 分段=" + segment;// +"and 油田=\'"+oil+"\'";
            }
            MySqlCommand cmd = new MySqlCommand(sql, Conn);
            cmd.ExecuteNonQuery();//对无返回值的命令一定要执行此方法
            MessageBox.Show("部门信息更改成功");
            Conn.Close();
            Close();
        }

        private void PipeManageCheck_Load(object sender, EventArgs e)
        {

        }
    }
}

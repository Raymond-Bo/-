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
    public partial class PipeManageOilCheck : Form
    {
        private int operation;
        private string sector = null;
        private string pipe = null;
        //private string segment = null;
        public PipeManageOilCheck(int op, string sector, string pipe)
        {
            this.operation = op;
            this.sector = sector;
            this.pipe = pipe;
            //this.segment = segment;
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
                sql = "INSERT INTO oil0322  (油田,管线编号) VALUES (\'"+sector+ "\',"+pipe+ ") ";
            }
            else if(operation==2)
            {
                //删除
                sql = "DELETE FROM oil0322 where 油田=\'" + sector + "\' and 管线编号=" + pipe;
            }
            MySqlCommand cmd = new MySqlCommand(sql, Conn);
            cmd.ExecuteNonQuery();//对无返回值的命令一定要执行此方法
            MessageBox.Show("部门信息更改成功");
            Conn.Close();
            Close();
        }

        private void PipeManageOilCheck_Load(object sender, EventArgs e)
        {

        }
    }
}

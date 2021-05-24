using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using MyDataBase;
using MyFile;

namespace gis_test1
{
    public partial class PipeManage : Form
    {
        private string operation = null;
        private string sector = null;
        private string pipe = null;
        private string segment = null;
        private string usr = null;

        public PipeManage(string usr)
        {
            this.usr = usr;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "select * from sector0322 where 油田=(select 油田 from users0322 where 用户名=\'"+usr+"\')";
            DataTable data = MyDatabase.QueryFromDt(sql);
            if (data == null)
                return;
            //创建新的dataset
            DataSet display = new DataSet("sector");//括号里面是数据库的名称
            DataTable dt = new DataTable("sectortb");//括号里面是表的名称
            display.Tables.Add(dt);
            DataColumn dcAutoId1 = new DataColumn("部门", typeof(string));
            dt.Columns.Add(dcAutoId1);
            DataColumn dcAutoId2 = new DataColumn("管线", typeof(string));
            dt.Columns.Add(dcAutoId2);
            DataColumn dcAutoId3 = new DataColumn("分段", typeof(string));
            dt.Columns.Add(dcAutoId3);

            //管道和分段的可以通过查询管道和分段具体的表，使用循环动态建立哈希表，日后可考虑

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                display.Tables[0].Rows[i][0] = (string)data.Rows[i][1];//部门
                if ((int)data.Rows[i][3] == -1)
                    display.Tables[0].Rows[i][1] = "全部";//管线
                else display.Tables[0].Rows[i][1] = data.Rows[i][3].ToString();
                if ((int)data.Rows[i][4] == -1)
                    display.Tables[0].Rows[i][2] = "全部";//编号
                else display.Tables[0].Rows[i][2] = data.Rows[i][4].ToString();
            }
            dataGridView1.DataSource = display;//设置数据源为dataSet
            dataGridView1.DataMember = "sectortb";//绑定dataSet的表名
            dataGridView1.Columns[0].HeaderText = "部门";
            dataGridView1.Columns[1].HeaderText = "管线";
            dataGridView1.Columns[2].HeaderText = "分段";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sec = textBox1.Text;
            string sql = "select * from sector0322 where 部门=\'" + sec + "\' ";
            //先确认输入的用户是否存在
            DataTable data = MyDatabase.QueryFromDt(sql);
            if (data == null)
                return;
            int row = data.Rows.Count;
            if (row > 0)
            {
                DataSet display = new DataSet("sector");//括号里面是数据库的名称
                DataTable dt = new DataTable("sectortb");//括号里面是表的名称
                display.Tables.Add(dt);
                //DataColumn dcAutoId1 = new DataColumn("部门", typeof(string));
                //dt.Columns.Add(dcAutoId1);
                DataColumn dcAutoId2 = new DataColumn("管线", typeof(string));
                dt.Columns.Add(dcAutoId2);
                DataColumn dcAutoId3 = new DataColumn("分段", typeof(string));
                dt.Columns.Add(dcAutoId3);
                //管道和分段的可以通过查询管道和分段具体的表，使用循环动态建立哈希表，日后可考虑
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    if ((int)data.Rows[i][3] == -1)
                        display.Tables[0].Rows[i][0] = "全部";//管线
                    else display.Tables[0].Rows[i][0] = data.Rows[i][3].ToString();
                    if ((int)data.Rows[i][4] == -1)
                        display.Tables[0].Rows[i][1] = "全部";//编号
                    else display.Tables[0].Rows[i][1] = data.Rows[i][4].ToString();
                }
                dataGridView1.DataSource = display;//设置数据源为dataSet
                dataGridView1.DataMember = "sectortb";//绑定dataSet的表名
                dataGridView1.Columns[0].HeaderText = "管线";
                dataGridView1.Columns[1].HeaderText = "分段";
            }
            else MessageBox.Show("不存在该部门");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "添加":
                    operation = "添加";
                    if (textBox3.Text == null)
                    {
                        textBox3.Text = "请输入数字或\'全部\'";
                    }
                    if (textBox4.Text == null)
                    {
                        textBox4.Text = "请输入数字或\'全部\'";
                    }
                    break;
                case "删除":
                    operation = "删除";
                    if (textBox3.Text == null)
                    {
                        textBox3.Text = "请输入数字或\'全部\'";
                    }
                    if (textBox4.Text == null)
                    {
                        textBox4.Text = "请输入数字或\'全部\'";
                    }
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (operation == null)
            {
                MessageBox.Show("请选择操作类型");
            }
            else if (operation == "添加")
            {
                sector = textBox2.Text;
                pipe = textBox3.Text;
                segment = textBox4.Text;
                if (!IsNumAndchCh(pipe) | !IsNumAndchCh(segment))
                {
                    MessageBox.Show("管道或分段信息格式有误");
                    return;
                }
                if (sector == null | pipe == null | segment == null)
                {
                    MessageBox.Show("请将信息补充完整");
                    return;
                }
                if (pipe == "全部")
                    pipe = "-1";
                if (segment == "全部")
                    segment = "-1";
                if (!CheckOilRecord(pipe))
                {
                    return;
                }
                string oil = null;
                string sql = "select 油田 from users0322 where 用户名=\'" + usr+"\'";
                //获取当前用户的所在油田
                DataTable dt = MyDatabase.QueryFromDt(sql);
                if (dt == null)
                    return;
                int row = dt.Rows.Count;
                if (row == 1)
                {
                    oil = (string)dt.Rows[0][0];
                }
                Form frmExportDlg = new PipeManageCheck(1, sector, pipe, segment,oil);
                frmExportDlg.Show();
            }
            else
            {
                //删除操作要检查记录是否存在
                sector = textBox2.Text;
                pipe = textBox3.Text;
                segment = textBox4.Text;
                if (!IsNumAndchCh(pipe) | !IsNumAndchCh(segment))
                {
                    MessageBox.Show("管道或分段信息格式有误");
                    return;
                }
                if (pipe == "全部")
                    pipe = "-1";
                if (segment == "全部")
                    segment = "-1";
                if (CheckTextBlank(sector, pipe, segment))
                {
                    //获取当前用户的所在油田
                    string oil = null;
                    string sql = "select 油田 from users0322 where 用户名=\'" + usr + "\'";
                    DataTable dt = MyDatabase.QueryFromDt(sql);
                    if (dt == null)
                        return;
                    int row = dt.Rows.Count;
                    if (row == 1)
                    {
                        oil = (string)dt.Rows[0][0];
                    }
                    MessageBox.Show("请注意，若存在多个相同的部门、管线和分段记录，使用此功能将全部删除，不做任一记录保留");
                    Form frmExportDlg = new PipeManageCheck(2, sector, pipe, segment, oil);
                    frmExportDlg.Show();
                }
            }
        }

        /// <summary>
        /// 用来核查是否存在记录
        /// </summary>
        /// <returns></returns>
        public bool CheckTextBlank(string sector, string pipe, string segment)
        {
            string sql = "select * from sector0322 where 部门=\'" + sector + "\' and 管线编号=" + pipe + " and 分段=" + segment;
            DataTable dt = MyDatabase.QueryFromDt(sql);
            if (dt == null)
                return false;
            int row = dt.Rows.Count;
            if (row > 0)
                return true;
            MessageBox.Show("输入的记录不存在或信息不完整");
            return false;
        }

        /// <summary>
        /// 用来检查添加的数据是否再范围内
        /// </summary>
        /// <param name="pipe"></param>
        /// <returns></returns>
        public bool CheckOilRecord(string pipe)
        {
            string sql = "select 管线编号 from oil0322 where 油田=(select 油田 from users0322 where 用户名=\'" + usr + "\') and 管线编号=" + pipe;
            DataTable dt = MyDatabase.QueryFromDt(sql);
            if (dt == null)
                return false;
            int row = dt.Rows.Count;
            if (row > 0)
                return true;
            MessageBox.Show("油田无该管线管辖权");
            return false;
        }

        /// <summary>
        /// 检查是否只包含数字和全部
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndchCh(string input)
        {
            if (input == "全部")
                return true;
            string pattern = @"^[0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySqlConnection Conn = MyDatabase.Static_myConnectionToMySql();
            if (Conn == null)
            {
                MessageBox.Show("数据库连接异常");
                return;
            }
            List<String> ls = MyDatabase.SelectField("authority0322");
            string col = null;
            for (int i = 0; i < ls.Count; ++i)
            {
                col += @"`" + ls[i] + "`,";
            }
            col = col.Substring(0, col.Length - 1);
            string title = "选择 .csv 文件";
            string filter = ".csv文件|*.csv";
            string filePath = Myfile.GetOpenFile(title, filter);
            filePath = filePath.Replace(@"\", @"\\");
            string tableName = "authority0322";
            string sql = @"LOAD DATA LOCAL INFILE '" + @filePath +
                @"' REPLACE INTO TABLE " + @tableName +
                @" CHARACTER SET UTF8 FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' IGNORE 1 LINES (" +
                @col + @");";
            Console.WriteLine("1399 sql = " + sql);
            MySqlCommand cmd = new MySqlCommand(sql, Conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                if (err.ToString().Contains(@"Invalid utf8 character string"))
                {
                    MessageBox.Show("请将文件转换为UTF8编码格式");
                }
                else if (err.ToString().Contains(@"Incorrect") && err.ToString().Contains(@"value"))
                {
                    MessageBox.Show("文件中包含类型不正确的数据");
                }
                else MessageBox.Show(err.ToString());
                return;
            }

            MessageBox.Show("导入数据成功！");
        }

        private void PipeManage_Load(object sender, EventArgs e)
        {

        }
    }
}

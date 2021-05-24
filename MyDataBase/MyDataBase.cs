using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDataBase
{
    public class MyDatabase
    {      
        /// <summary>
        /// 连接到数据库并打开，默认参数为数据库名 和 密码
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static MySqlConnection Static_myConnectionToMySql(String database = "gis_test", String password = "xidianlink")
        {
            string SCon = null;
            SCon = String.Format("Database = {0}; Data Source = 119.254.155.248; User Id = link; Password = {1}; Port = 6306; CharSet = gb2312; AllowLoadLocalInfile = true; Allow User Variables = true; Connect Timeout=100", database, password);
            MySqlConnection Conn = new MySqlConnection(SCon);
            try
            {
                Conn.Open();
                if (Conn.State == ConnectionState.Open)
                {
                    return Conn;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "数据库连接失败！");
            }
            return null;
        }

        public static DataTable QueryFromDt(string sql)
        {
            MySqlConnection Conn = Static_myConnectionToMySql();
            if (Conn == null)
            {
                MessageBox.Show("数据库连接异常");
                return null;
            }
            DataTable dt = new DataTable();
            while (dt.Rows.Count == 0)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, Conn);
                    cmd.CommandTimeout = 99999;
                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    sda.SelectCommand.CommandTimeout = 60;
                    sda.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                     dt.Clear();
                }
            }
            Conn.Close();
            return dt;
        }

        /// <summary>
        /// 从数据库中 返回离给定位置最近的一个点 的记录， 利用街区距离计算
        /// </summary>
        /// <param name="x">经度</param>
        /// <param name="y">纬度</param>
        /// <param name="TableName">数据库表名</param>
        /// <returns></returns>
        public static DataTable SelectProperty(double x, double y, String TableName = "gis0322")
        {
            List<String> ls = SelectField(TableName);
            string col = null;
            for (int i = 0; i < ls.Count; ++i)
            {
                col += @"`" + ls[i] + "`,";
            }
            col = col.Substring(0, col.Length - 1);
            string sql = String.Format(@"select a.* from ( select " + @col +
                @" from gis0322 where abs(`经度（小数）` -{0:N20}) +abs(`纬度（小数）`-{1:N20})<0.0005 order by abs(`经度（小数）` -{2:N20}) +abs(`纬度（小数）`-{3:N20}) asc )a limit 0,1",
                @x, @y, @x, @y);
            DataTable dt = QueryFromDt(sql);
            return dt;
        }

        /// <summary>
        /// 从数据库中 返回给定id分段的线的记录
        /// </summary>
        /// <param name="id">分段</param>
        /// <param name="TableName">数据库表名</param>
        /// <returns></returns>
        public static DataTable SelectProperty(int id, String TableName = "gis0322")
        {
            string sql = "select * from gis0322 where 分段=\'" + id + "\' order by 序号";
            DataTable dt = QueryFromDt(sql);
            return dt;
        }

        /// <summary>
        /// 返回 数据库某个表中 所有的列字段名
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <returns></returns>
        public static List<String> SelectField(String TableName = "gis0322")
        {
            List<String> fields = new List<string>();
            string sql = @"show columns from " + @TableName + @";";
            DataTable dt = QueryFromDt(sql);
            if (dt == null)
            {
                return null;
            }
            int row = dt.Rows.Count;
            for (int i = 0; i < row; ++i)
            {
                fields.Add(Convert.ToString(dt.Rows[i][0]));
            }
            return fields;
        }
    }
}

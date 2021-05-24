using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFile
{
    public class Myfile
    {
        /// <summary>
        /// 返回打开文件的路径
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="filter">说明|文件类型过滤</param>
        /// <returns></returns>
        public static string GetOpenFile(string title = "导入excel文件", string filter = "excel文件|*.xls;*.xlsx")
        {
            OpenFileDialog OpenFdlg = new OpenFileDialog();
            OpenFdlg.CheckFileExists = false; //设置不弹出警告            
            OpenFdlg.Title = title;
            OpenFdlg.Filter = filter;
            OpenFdlg.ShowDialog();
            string filePath = OpenFdlg.FileName;
            return filePath;
        }
    }
}

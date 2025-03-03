using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lucifer.XCalender;

namespace 洒下一片深情
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private static DateTime WinTime = new DateTime(2023, 4, 28, 17, 30, 0);
        private void timer1_Tick(object sender, EventArgs e)
        {
            XLunisolarCalender xx = new XLunisolarCalender(DateTime.Now);
            StringBuilder newDate = new StringBuilder();
            newDate.Append(DateTime.Now.ToString("yyyy-MM-dd dddd HH:mm:ss "));
            newDate.Append(xx.ChineseZodiac);
            newDate.Append("年 ");
            newDate.Append(xx.ChineseEra);
            newDate.Append(" ");
            newDate.Append(xx.LunisolarDate);
            string result = newDate.ToString();
            toolStripStatusLabel1.Text= result;
            listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
            listView1.Items.Clear();
            foreach (var date in GetHoliday())   //添加行数据
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = date.Value;
                lvi.SubItems.Add(date.Key.Item1.ToString("yyyy-MM-dd dddd"));
                lvi.SubItems.Add(GetSpan(date.Key));

                listView1.Items.Add(lvi);
            }
            listView1.Columns[0].Width = 100;
            listView1.Columns[1].Width = 200;
            listView1.Columns[2].Width = listView1.Width - listView1.Columns[0].Width - listView1.Columns[1].Width - 4;

            listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
        }
        public static Dictionary<(DateTime,DateTime), string> GetHoliday()
        {
            // 获取当前年份
            int year = DateTime.Now.Year;

            // 定义法定节假日
            Dictionary<(DateTime,DateTime), string> holidays = new Dictionary<(DateTime, DateTime), string> {
                { (new DateTime(year, 1, 1),new DateTime(year, 1, 2)), "元旦" },
              // { new DateTime(year, 2, 2), "世界湿地日" },
              // { new DateTime(year, 2, 14), "情人节" },
              // { new DateTime(year, 3, 8), "妇女节" },
              // { new DateTime(year, 3, 12), "植树节" },
              // { new DateTime(year, 4, 1), "愚人节" },
               { (new DateTime(year, 4, year==2023?5:4),new DateTime(year, 4, year==2023?6:5)), "清明节" },
              // { new DateTime(year, 4, 22), "世界地球日" },
                { (new DateTime(year, 5, 1),new DateTime(year, 5, 4)), "劳动节" },
              // { new DateTime(year, 5, 4), "青年节" },
              // { new DateTime(year, 6, 1), "儿童节" },
              // { new DateTime(year, 6, 5), "世界环境日" },
              // { new DateTime(year, 7, 1), "建党节" },
              // { new DateTime(year, 8, 1), "建军节" },
              // { new DateTime(year, 9, 3), "抗战胜利日" },
              // { new DateTime(year, 9, 10), "教师节" },
               { (new DateTime(year, 10, 1),new DateTime(year, 10, 8)), "国庆节" },
              // { new DateTime(year, 12, 1), "世界艾滋病日" },
              // { new DateTime(year, 12, 24), "平安夜" },
              // { new DateTime(year, 12, 25), "圣诞节" }
            };
           
            
            // 获取春节和中秋节的日期，因为这两个节日的日期是根据农历计算的
            ChineseLunisolarCalendar lunarCalendar = new ChineseLunisolarCalendar();
            DateTime springFestival = lunarCalendar.ToDateTime(year-1, 12, 29, 0, 0, 0, 0);
            //DateTime qingmingFestival = lunarCalendar.ToDateTime(year, 4, 4, 0, 0, 0, 0);
            DateTime duanwuFestival = lunarCalendar.ToDateTime(year, year==2023?6:5, 5, 0, 0, 0, 0);
            DateTime midAutumnFestival = lunarCalendar.ToDateTime(year, year == 2023 ? 9 : 8, 15, 0, 0, 0, 0);

            // 将春节和中秋节加入节日列表
            holidays.Add((springFestival, lunarCalendar.ToDateTime(year, 1, 10, 0, 0, 0, 0)), "春节");
            //holidays.Add(qingmingFestival, "清明节");
            holidays.Add((duanwuFestival, lunarCalendar.ToDateTime(year, year == 2023 ? 6 : 5, 6, 0, 0, 0, 0)),"端午节");
            holidays.Add((midAutumnFestival, lunarCalendar.ToDateTime(year, year == 2023 ? 9 : 8, 16, 0, 0, 0, 0)),"中秋节");

            holidays = holidays.OrderBy(p => p.Key).ToDictionary(p=>p.Key,v=>v.Value);
            return holidays;
        }

        public static string GetSpan((DateTime time1, DateTime time2) time)
        {
            var span = time.time1 - DateTime.Now - new TimeSpan(6, 30, 0);
            if (span.Milliseconds < 0)
            {
                var span2 = time.time2 - DateTime.Now;
                if (span.Milliseconds < 0)
                {
                    return "已过期（悲）";
                }
                else
                {
                    return String.Format("欢呼雀跃吧同志们！假期余额:{0:000}天{1:00}小时{2}分{3}秒{4}毫秒", span2.Days, span2.Hours, span2.Minutes, span2.Seconds, span2.Milliseconds);

                }
            }
            return String.Format("{0:000}天{1:00}小时{2}分{3}秒{4}毫秒", span.Days, span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.DoubleBufferedListView(true);
            listView1.View = View.Details;
            listView1.Columns.Add("节日");
            listView1.Columns.Add("日期");
            listView1.Columns.Add("倒计时");
            listView1.Columns[0].Width = 100;
            listView1.Columns[1].Width = 200;
            listView1.Columns[2].Width = listView1.Width-listView1.Columns[0].Width- listView1.Columns[1].Width - 4;
        }
    }

    public static class DoubleBufferListView
    {
        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        public static void DoubleBufferedListView(this ListView dgv, bool flag)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (pi != null) pi.SetValue(dgv, flag, null);
        }
    }

}



using Morin.App.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Morin.App.Common.AppSetting.Style
{
    /// <summary>
    /// LrcView.xaml 的交互逻辑
    /// </summary>
    public partial class LrcView : UserControl
    {
        public LrcView()
        {
            InitializeComponent();
        }

        #region 歌词模型
        #region 变量
        //歌词集合
        public Dictionary<double, LrcModel> Lrcs = new Dictionary<double, LrcModel>();
        //当前焦点所在歌词集合位置
        public int FoucsLrcLocation { get; set; } = -1;
        //添加当前焦点歌词变量
        public LrcModel foucslrc { get; set; }
        //非焦点歌词颜色
        public SolidColorBrush NoramlLrcColor = new SolidColorBrush(Colors.White);
        //焦点歌词颜色

        List<string> foucslrcNext = new List<string>();
        List<LrcInfo> LrcsList = new List<LrcInfo>();


        // SolidColorBrush ButtonNormal = FindResource("ButtonNormal") as SolidColorBrush;

        public SolidColorBrush FoucsLrcColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEE4848"));
        public SolidColorBrush BackLrcColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#20eeeeee"));

        #region 加载歌词
        public void LoadLrc(string lrcstr)
        {
            var main = Morin.App.MainWindow.Instance

            if (lrcstr == "" || lrcstr == null) return;
            SolidColorBrush FoucsLrc = FindResource("ThemeColor") as SolidColorBrush;

            FoucsLrcColor = FoucsLrc;
            c_lrc_items.Children.Clear();
            Lrcs.Clear();
            LrcsList.Clear();
            MainWindow.Instance.lrcShowWindow.lrcShow_next.Text = "";
            //foucslrcNext.Clear();
            //FoucsLrcLocation = -1;
            main.lrcShowWindow.lrcShow.Text = "";

            TextBlock spack = new TextBlock();
            spack.Height = 30;
            spack.Text = "-\n-";
            spack.FontSize = main.Prog_Lrc2.Value;
            spack.Foreground = NoramlLrcColor;
            spack.TextAlignment = TextAlignment.Center;
            c_lrc_items.Children.Add(spack);
            //循环以换行\n切割出歌词
            foreach (string str in lrcstr.Split('\n'))
            {
                //过滤空行，判断是否存在时间
                if (str.Length > 0 && str.IndexOf(":") != -1)
                {
                    //歌词时间
                    TimeSpan time = GetTime(str);
                    //歌词
                    try
                    {
                        string lrc = str.Split(']')[1];

                        //过滤空行
                        if (lrc.Length > 0)
                        {
                            //歌词显示textblock控件
                            TextBlock c_lrcbk = new TextBlock();
                            c_lrcbk.FontSize = main.Prog_Lrc1.Value;
                            c_lrcbk.Foreground = NoramlLrcColor;
                            c_lrcbk.MaxWidth = 500;
                            //c_lrcbk.Background = BackLrcColor;
                            c_lrcbk.TextAlignment = TextAlignment.Center;
                            c_lrcbk.TextTrimming = TextTrimming.CharacterEllipsis;
                            c_lrcbk.TextWrapping = TextWrapping.Wrap;
                            //赋值
                            c_lrcbk.Text = lrc;
                            if (c_lrc_items.Children.Count > 0)
                            {
                                //增加一些行间距，see起来不那么拥挤~
                                c_lrcbk.Margin = new Thickness(0, 10, 0, 0);
                            }

                            //添加到集合，方便日后操作
                            //Lrcs. Clear();
                            try
                            {
                                Lrcs.Add(time.TotalMilliseconds, new LrcModel()
                                {
                                    c_LrcTb = c_lrcbk,
                                    LrcText = lrc,
                                    Time = time.TotalMilliseconds.ToString()

                                });
                            }
                            catch { }
                            LrcsList.Add(new LrcInfo()
                            {
                                Lrc = lrc,
                                Time = time.TotalMilliseconds
                            });
                            //将歌词显示textblock控件添加到界面中显示
                            c_lrc_items.Children.Add(c_lrcbk);
                            foucslrcNext.Add(lrc);
                        }
                    }
                    catch { }
                }
            }

            TextBlock spack2 = new TextBlock();
            spack2.Height = 50;
            spack2.Text = "-\n-";
            spack2.FontSize = main.Prog_Lrc2.Value;
            spack2.Foreground = NoramlLrcColor;
            spack2.TextAlignment = TextAlignment.Center;
            c_lrc_items.Children.Add(spack2);
        }

        //正则表达式提取时间
        public TimeSpan GetTime(string str)
        {
            try
            {
                Regex reg = new Regex(@"\[(?<time>.*)\]", RegexOptions.IgnoreCase);
                string timestr = reg.Match(str).Groups["time"].Value;

                string ff = timestr.Split(':')[0];
                if (ff == "") return new TimeSpan(0);
                //获得分
                int m = Convert.ToInt32(ff);
                //判断是否有小数点
                int s = 0, f = 0;
                if (timestr.Split(':')[1].IndexOf(".") != -1)
                {
                    //有
                    s = Convert.ToInt32(timestr.Split(':')[1].Split('.')[0]);
                    //获得毫秒位
                    f = Convert.ToInt32(timestr.Split(':')[1].Split('.')[1]);

                }
                else
                {
                    //没有
                    s = Convert.ToInt32(timestr.Split(':')[1]);
                }
                //Debug.WriteLine(m + "-" + s + "-" + f + "->" + new TimeSpan(0, 0, m, s, f).TotalMilliseconds);
                return new TimeSpan(0, 0, m, s, f);
            }
            catch { return new TimeSpan(0, 0, 0, 0, 0); }
        }
        #region 歌词滚动

        string oldlrc = "", nextlrc = "";
        public void LrcRoll(double nowtime)
        {
            if (Lrcs.Values.Count == 0) return;

            var main = MainWindow.Instance;
            if (foucslrc == null)
            {
                foucslrc = Lrcs.Values.First();
                foucslrc.c_LrcTb.Foreground = FoucsLrcColor;
            }
            else
            {

                for (int i = 0; i < LrcsList.Count; i++)
                {
                    double TotalMilliseconds = LrcsList[i].Time;
                    if (TotalMilliseconds > nowtime)
                    { nextlrc = LrcsList[i].Lrc; if (string.IsNullOrEmpty(nextlrc)) nextlrc = ""; break; }
                }
                //查找焦点歌词
                IEnumerable<KeyValuePair<double, LrcModel>> s = Lrcs.Where(m => nowtime >= m.Key);
                if (s.Count() > 0)
                {
                    LrcModel lm = s.Last().Value;
                    foucslrc.c_LrcTb.Foreground = NoramlLrcColor;
                    foucslrc.c_LrcTb.FontSize = main.Prog_Lrc1.Value;
                    foucslrc = lm;
                    foucslrc.c_LrcTb.Foreground = FoucsLrcColor;
                    foucslrc.c_LrcTb.FontSize = main.Prog_Lrc2.Value;

                    MainWindow.Instance.lrcShowWindow.lrcShow.Text = foucslrc.c_LrcTb.Text;
                    MainWindow.Instance.lrcShowWindow.lrcShow_next.Text = nextlrc;
                    //定位歌词在控件中间区域
                    if (oldlrc != foucslrc.c_LrcTb.Text) ResetLrcviewScroll();
                    oldlrc = foucslrc.c_LrcTb.Text;
                }
            }
        }
        //计算时间差
        public double DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            double dateDiff = 0;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan t = ts1 - ts2;
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = t.Seconds + t.Milliseconds;
            }
            catch
            {
            }
            return dateDiff;
        }

        #endregion

        double osold = 0;
        #region 调整歌词控件滚动条位置
        public async void ResetLrcviewScroll()
        {

            //获得焦点歌词位置
            GeneralTransform gf = foucslrc.c_LrcTb.TransformToVisual(c_lrc_items);
            Point p = gf.Transform(new Point(0, 0));
            //滚动条当前位置
            //计算滚动位置（p.Y是焦点歌词控件(c_LrcTb)相对于父级控件c_lrc_items(StackPanel)的位置）
            //拿焦点歌词位置减去滚动区域控件高度除以2的值得到的【大概】就是歌词焦点在滚动区域控件的位置
            double os = p.Y - (c_scrollviewer.ActualHeight / 2) + 80;
            if (os - osold > 80) osold = os - 80;

            for (double i = osold; i < os; i++)
            {
                double os2 = p.Y - (c_scrollviewer.ActualHeight / 2) + 80;
                if (os != os2) return;
                c_scrollviewer.ScrollToVerticalOffset(i);
                await RequestBody(3);
            }
            osold = os;

        }
        async Task<string> RequestBody(int ms)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(ms);
                return "01";
            });
        }

        #endregion

        #endregion

        #endregion

        #endregion

        private void GT_MouseEnter(object sender, MouseEventArgs e)
        {
            //c_scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //c_scrollviewer.Opacity = 100;

        }

        private void GT_MouseLeave(object sender, MouseEventArgs e)
        {
            //c_scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //c_scrollviewer.Opacity = 0;
        }
    }



    public class LrcModel
    {
        /// <summary>
        /// 歌词所在控件
        /// </summary>
        public TextBlock c_LrcTb { get; set; }

        /// <summary>
        /// 歌词字符串
        /// </summary>
        public string LrcText { get; set; }

        /// <summary>
        /// 时间（读取格式参照网易云音乐歌词格式：xx:xx.xx，即分:秒.毫秒，秒小数点保留2位。如：00:28.64）
        /// </summary>
        public string Time { get; set; }
    }
}

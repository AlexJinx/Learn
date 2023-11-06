using Morin.App.Common.AppSetting.Style;
using Morin.App.Common.Wave;

using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Morin.App.Common.Code
{
    public class TimerKeyer
    {
        TransformType transformType = new TransformType();//实例化一个转换类
        LrcView lrcview = new LrcView();
        //播放进度条定时器
        public DispatcherTimer Progtimer = null;
        string Ta = ""; int hasCache = 0;
        public int playbuttonvi = 0;//控制按钮区显示隐藏
        public void timer_tick(object sender, EventArgs e)
        {
            var main = MainWindow.Instance;
            string position = main.play.Position.ToString();

            if (main.播放器区.Height > 0 && main.isPlayMV())
            {
                playbuttonvi++;
                if (playbuttonvi == 50) main.播放控制区.Visibility = Visibility.Hidden;
            }

            if (main.isShowPeak.IsChecked == true && main.playpath != null)
            {
                if (main.NeedDown)
                {
                    if (main.network.CacheSongDownPe == 100 && hasCache == 0)
                    { NAudioEngine.Instance.OpenFile(main.playpath); hasCache = 1; }
                }
                else
                {
                    FileInfo cachesong = new FileInfo(main.playpath);
                    if (cachesong.Exists && hasCache == 0)
                    { NAudioEngine.Instance.OpenFile(main.playpath); hasCache = 1; }
                }
            }

            if (!Progtimer.IsEnabled) return;
            if (main.Time == null)
            {
                Ta = (main.play.NaturalDuration.ToString()).ToString();
                main.Time = transformType.ToSecond(Convert.ToDateTime(Ta)).ToString();
            }

            main.Prog.Value = transformType.ToSecond(DateTime.Parse(position));
            main.Title = main.SongName;
            main.time1.Text = transformType.ToDateTime((int)main.Prog.Value);
            main.time2.Text = transformType.ToDateTime((int)Convert.ToInt64(main.Time));
            main.Prog.Maximum = Convert.ToInt64(main.Time);

            if (main.LrcText != "")
            {
                main.LrcView.LrcRoll(main.play.Position.TotalMilliseconds);
            }
        }

        public void tiemrs()
        {
            hasCache = 0;
            //媒体文件打开成功
            Progtimer = new DispatcherTimer();
            Progtimer.Interval = TimeSpan.FromSeconds(0.1);
            Progtimer.Tick += new EventHandler(timer_tick);
            Progtimer.Start();
        }
    }
}

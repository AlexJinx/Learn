using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Morin.App.Common.Page
{
    /// <summary>
    /// Update.xaml 的交互逻辑
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
        }

        MainWindow main = MainWindow.Instance;
        public void Start()
        {
            if (start.Content.ToString() == "开始下载")
            {
                string DownUrl = MainWindow.Instance.DownUrl;
                if (string.IsNullOrWhiteSpace(DownUrl))
                {
                    main.message.ShowMessage("下载地址错误，请重试");
                    return;
                }
                string path = Environment.CurrentDirectory + "/Install.exe";
                int y = FileDawn(DownUrl, path);
                if (y == 0) return;
                DownTiemrs();
                start.Content = "正在下载...";
                start.IsEnabled = false;
            }
            else if (start.Content.ToString() == "点击安装")
            {
                Process.Start(Environment.CurrentDirectory + @"\Install.exe");
                Environment.Exit(0);
            }
        }


        public int DownPercent = 0;
        public int FileDawn(string t1, string t2)
        {
            try
            {
                if (t1 != null && t2 != null)
                {
                    WebRequest request = WebRequest.Create(t1);
                    WebResponse respone = request.GetResponse();
                    //pro.Maximum = respone.ContentLength;
                    ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        Stream netStream = respone.GetResponseStream();
                        Stream fileStream = new FileStream(t2, FileMode.Create);
                        byte[] read = new byte[1024];
                        long progressBarValue = 0;
                        int realReadLen = netStream.Read(read, 0, read.Length);
                        while (realReadLen > 0)
                        {
                            fileStream.Write(read, 0, realReadLen);
                            progressBarValue += realReadLen;
                            realReadLen = netStream.Read(read, 0, read.Length);
                            DownPercent = (int)(((double)progressBarValue / (double)respone.ContentLength) * 100);
                        }
                        netStream.Close();
                        fileStream.Close();
                    }, null);

                }
            }
            catch (Exception e) { main.message.ShowMessage(e.Message); return 0; }
            return 1;
        }
        //public int ShowMessage(string text)
        //{
        //    ShowMessageBox messageBox = new ShowMessageBox();
        //    messageBox.text.Text = text;
        //    messageBox.Info.Text = "提示一下";
        //    messageBox.Button1.Visibility = Visibility.Visible;

        //    messageBox.ShowDialog();
        //    return messageBox.Resul;
        //}
        //下载进度条定时器
        public DispatcherTimer Downtimer = null;
        public void DownTimer_tick(object sender, EventArgs e)
        {
            pro.Value = DownPercent;
            if (DownPercent == 100)
            { start.Content = "点击安装"; start.IsEnabled = true; }
        }

        public void DownTiemrs()
        {
            {
                Downtimer = new DispatcherTimer();
                Downtimer.Interval = TimeSpan.FromMilliseconds(1);
                Downtimer.Tick += new EventHandler(DownTimer_tick);
                Downtimer.Start();
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Environment.Exit(0);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = MainWindow.Instance;
            Start();
        }

    }
}

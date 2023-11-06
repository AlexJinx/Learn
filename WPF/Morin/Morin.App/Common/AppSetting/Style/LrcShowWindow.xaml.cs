using Morin.App.Common.Code;

using System;
using System.Collections.Generic;
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

namespace Morin.App.Common.AppSetting.Style
{
    /// <summary>
    /// LrcShowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LrcShowWindow : Window
    {
        public LrcShowWindow()
        {
            InitializeComponent();
        }
        public static LrcShowWindow LrcShowInstance { get; set; }
        IniHelper iniHelper = new IniHelper("config.ini");

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow main = MainWindow.Instance;
            if (main == null) return;
            LrcShowInstance = this;
            double workHeight = SystemParameters.WorkArea.Height; // 屏幕工作区域高度
            double workWidth = SystemParameters.WorkArea.Width; // 屏幕工作区域高度
            Top = workHeight - (Height + 260);
            lrcShow.FontSize = main.Prog_Lrc3.Value;
            Visibility = Visibility.Hidden;
            //Left = workWidth - Width;

            try
            {
                //设置屏幕大小
                string[] size = iniHelper.ReadValue("DefaultSet", "LrcSize").Split(',');
                Width = Convert.ToDouble(size[0]);
                Height = Convert.ToDouble(size[1]);
                Top = Convert.ToDouble(size[2]);
                Left = Convert.ToDouble(size[3]);
            }
            catch { }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void grid1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MainWindow.Instance.showMain();
            }
        }

        private void Play_B_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.PlayClick();
        }

        private void B_Copy7_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.NextSong();
        }

        private void B_Copy2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LastSong();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((bool)!Lock.IsChecked)
                DragMove();

            //桌面尺寸
            string size = $"{Width},{Height},{Top},{Left}";
            iniHelper.WriteValue("DefaultSet", "LrcSize", size);
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double va = (double)e.Delta / 3500;
            MainWindow.Instance.Volume.Value += va;
        }




        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            ContBG.Visibility = Visibility.Hidden;
            LUck.Visibility = Visibility.Visible;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ContBG.Visibility = Visibility.Visible;
            LUck.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if ((bool)checkBox.IsChecked)
            { grid1.Opacity = 100; MainWindow.Instance.Prog_Lrc4.Value = 100; }
            else { grid1.Opacity = 0; MainWindow.Instance.Prog_Lrc4.Value = 0; }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            MainWindow main = MainWindow.Instance;
            if (main.Prog_Lrc4.Value == 0) bg.IsChecked = false;
            else bg.IsChecked = true;
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = MainWindow.Instance;
            if (main.WindowState == WindowState.Minimized) main.WindowState = WindowState.Normal;
            main.Visibility = Visibility.Visible;
            main.ShowInTaskbar = true;
            main.Activate();
        }
    }
}

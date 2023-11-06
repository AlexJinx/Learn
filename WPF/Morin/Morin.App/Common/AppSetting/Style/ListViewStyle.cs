using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Morin.App.Common.AppSetting.Style
{
    public partial class ListViewStyle : ResourceDictionary
    {
        int i = 0;
        public void NetSong_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {
                //MainWindow.w1.ShowPopup("双击了");

                i += 1;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
                timer.IsEnabled = true;
                if (i % 2 == 0)
                {
                    //MessageBox.Show("双击了");
                    //MainWindow.w1.ShowPopup("双击了");
                    //MainWindow main = new MainWindow();
                    //main.NetSongDouble();
                    //MainWindow.w1.NetSongDouble();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

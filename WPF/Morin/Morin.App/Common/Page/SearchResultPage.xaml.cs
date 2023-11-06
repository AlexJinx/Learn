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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Morin.App.Common.Page
{
    /// <summary>
    /// SearchResultPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearchResultPage : UserControl
    {
        public SearchResultPage()
        {
            InitializeComponent();
        }

        MainWindow main;
        private void SearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            main.SearchSongDouble();
        }

        private void SearchList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void SearchMV_Button_Click(object sender, RoutedEventArgs e)
        {
            main.SearchMV_Button_Click(sender, e);
        }

        private void SearchDown_Button_Click(object sender, RoutedEventArgs e)
        {
            main.SearchDown_Button_Click(sender, e);
        }

        private void SearchLike_Click(object sender, RoutedEventArgs e)
        {
            main.SearchLike_Click(sender, e);
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            main.AddList_Click(sender, e);
        }

        private void SingerButton_Click(object sender, RoutedEventArgs e)
        {
            main.SingerButton_Click(sender, e);
        }

        int frist = 0;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (frist == 1) return;
            main = MainWindow.Instance;
            //搜索引擎.SelectedIndex = main.DefaultEngine.SelectedIndex;
            frist = 1;
        }

        private void SearchAllPlay_Click(object sender, RoutedEventArgs e)
        {
            main.SearchAllPlay();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            main.SearchContent.Visibility = Visibility.Hidden;
        }
    }
}

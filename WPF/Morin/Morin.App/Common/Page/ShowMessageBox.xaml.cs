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

namespace Morin.App.Common.Page
{
    /// <summary>
    /// ShowMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class ShowMessageBox : Window
    {
        public ShowMessageBox()
        {
            InitializeComponent();
        }

        MainWindow main;
        public bool result = false;
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            Close();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

            }
        }
    }
}

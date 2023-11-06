using System.Windows;
using System.Windows.Input;

namespace Morin.App.Common.Page
{
    /// <summary>
    /// ShowWriteMessageRl.xaml 的交互逻辑
    /// </summary>
    public partial class ShowWriteMessageRl : Window
    {
        public ShowWriteMessageRl()
        {
            InitializeComponent();
        }

        MainWindow main;
        public new string Content = "";
        public new string Name = "";
        public int Resul = 0;
        private void B_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Resul = 0;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Resul = 0;
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Resul = 1;
            Content = text.Text;
            Name = name.Text;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

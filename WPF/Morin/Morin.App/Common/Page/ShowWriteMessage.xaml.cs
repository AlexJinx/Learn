using System.Windows;
using System.Windows.Input;

namespace Morin.App.Common.Page
{
    /// <summary>
    /// ShowWriteMessage.xaml 的交互逻辑
    /// </summary>
    public partial class ShowWriteMessage : Window
    {
        public ShowWriteMessage()
        {
            InitializeComponent();
        }

        public new string Content = "";
        public bool isOk = false;
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            isOk = true;
            Content = text.Text;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void text_Loaded(object sender, RoutedEventArgs e)
        {
            text.Focus();
        }
    }
}

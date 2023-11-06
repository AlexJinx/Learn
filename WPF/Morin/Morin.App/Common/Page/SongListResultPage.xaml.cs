using Morin.App.Model;
using Morin.App.ViewModel;

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
    /// SongListResultPage.xaml 的交互逻辑
    /// </summary>
    public partial class SongListResultPage : Window
    {
        public SongListResultPage()
        {
            InitializeComponent();
        }

        public bool showFavorites = false;
        MainWindow main;
        private void SearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            main.NetSongDouble();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            main = MainWindow.Instance;
            if (showFavorites) Favorites_btn.Visibility = Visibility.Visible;
            else Favorites_btn.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SongList.SelectedIndex = 0;
            main.NetSongDouble();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string id = main.songListSave[main.SongList.SelectedIndex, 1];
                if (string.IsNullOrEmpty(id)) { main.message.ShowMessage("链接无效"); return; }
                string url = "http://www.kuwo.cn/playlist_detail/" + id;
                Clipboard.SetDataObject(url);
                main.message.ShowMessage("歌单分享链接已复制到剪切板");
            }
            catch (Exception ex)
            {
                main.message.ShowMessage(ex.Message);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (Favorites_btn.Content.ToString() == "收藏歌单")
            {
                Favorites_btn.Content = "已收藏";
            }
            else
            {
                Favorites_btn.Content = "收藏歌单";
                return;
            }

            MusicCardInfo musicCardInfo = new MusicCardInfo();
            musicCardInfo.Musics = main.musicSearchList;
            musicCardInfo.Name = PLName.Text;
            musicCardInfo.Info = PLInt.Text;
            musicCardInfo.Pic = PLImg.Source.ToString();
            MyMusicCard myMusicCard = new MyMusicCard();
            myMusicCard.CreateMusicCard(main.MusicCardList, musicCardInfo);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (Favorites_btn.IsVisible)
            {
                main.SearchContent.Visibility = Visibility.Hidden;
            }
            else
            {
                main.MusicCardContent.Visibility = Visibility.Hidden;
            }
        }

        private void SongList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyCountextMenu myCountextMenu = new MyCountextMenu();
            if (main.slRPage.Favorites_btn.Visibility == Visibility.Visible)
            {
                main.slRPage.SongList.ContextMenu = myCountextMenu.CountextMenu();
            }
            else
            {
                main.slRPage.SongList.ContextMenu = myCountextMenu.CardListCountextMenu();
            }
        }


    }
}

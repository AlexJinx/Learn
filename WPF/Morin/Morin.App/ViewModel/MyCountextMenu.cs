using Morin.App.Common.Code;
using Morin.App.Model;

using Panuon.UI.Silver;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Message = Morin.App.Model.Message;

namespace Morin.App.ViewModel
{
    public class MyCountextMenu
    {
        public MyCountextMenu()
        {
            main = MainWindow.Instance;
        }
        MainWindow main;
        Message message = new Message();
        Network network = new Network();
        MyMusicCard myMusicCard = new MyMusicCard();


        //右键菜单
        public ContextMenu CountextMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.BorderThickness = new Thickness(0);
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.A = 1; color.R = 240; color.G = 240; color.B = 240;
            ContextMenuHelper.SetShadowColor(cm, color);
            ContextMenuHelper.SetCornerRadius(cm, new CornerRadius(5));

            addCM(cm, "播放", Play_CM);
            addCM(cm, "播放MV", PlayMV_CM);
            addCM(cm, "下载", Down_CM);
            addCM(cm, "喜欢这首歌", Like_CM);
            addCM(cm, "下一首播放", AddToNextPlay_CM);
            addCM(cm, "收藏到歌单", getCardName());
            addCM(cm, "分享链接", Share_CM);
            addCM(cm, "复制歌曲信息", SongInfoCopy_CM);
            addCM(cm, "添加到播放列表", AddToPlayList_CM);
            return cm;
        }

        public ContextMenu CardListCountextMenu()
        {
            ContextMenu cm = CountextMenu();
            cm.Items.Add(new Separator());
            addCM(cm, "从此歌单中移除", Delete_CM);
            return cm;
        }

        //我的喜欢右键菜单
        public ContextMenu LikeCountextMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.BorderThickness = new Thickness(0);
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.A = 1; color.R = 240; color.G = 240; color.B = 240;
            ContextMenuHelper.SetShadowColor(cm, color);
            ContextMenuHelper.SetCornerRadius(cm, new CornerRadius(5));

            addCM(cm, "播放", LikePlay_CM);
            addCM(cm, "下载", LikeDown_CM);
            addCM(cm, "刷新", LikeRefresh_CM);
            addCM(cm, "收藏到歌单", getCardName());
            addCM(cm, "复制歌曲信息", LikeSongInfoCopy_CM);
            addCM(cm, "分享链接", LikeShare_CM);
            addCM(cm, "不再喜欢", LikeNoLike_CM);
            return cm;
        }

        //本地音乐右键菜单
        public ContextMenu LocalCountextMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.BorderThickness = new Thickness(0);
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.A = 1; color.R = 240; color.G = 240; color.B = 240;
            ContextMenuHelper.SetShadowColor(cm, color);
            ContextMenuHelper.SetCornerRadius(cm, new CornerRadius(5));

            addCM(cm, "播放", LocalPlay_CM);
            addCM(cm, "刷新", LocalRefresh_CM);
            addCM(cm, "打开路径", LocalPath_CM);
            addCM(cm, "删除", LocalDelete_CM);
            addCM(cm, "属性", LocalInfo_CM);
            return cm;
        }

        //本地音乐右键菜单
        public ContextMenu MusicCardCountextMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.BorderThickness = new Thickness(0);
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.A = 1; color.R = 240; color.G = 240; color.B = 240;
            ContextMenuHelper.SetShadowColor(cm, color);
            ContextMenuHelper.SetCornerRadius(cm, new CornerRadius(5));

            addCM(cm, "修改标题", ChangeTitle);
            addCM(cm, "修改简介", ChangeInfo);
            addCM(cm, "删除", DeleteCard);
            return cm;
        }

        //播放列表右键菜单
        public ContextMenu PlaylistCountextMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.BorderThickness = new Thickness(0);
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.A = 1; color.R = 240; color.G = 240; color.B = 240;
            ContextMenuHelper.SetShadowColor(cm, color);
            ContextMenuHelper.SetCornerRadius(cm, new CornerRadius(5));

            addCM(cm, "播放", Play_Playlist);
            addCM(cm, "下载", Down_Playlist);
            addCM(cm, "喜欢/取消喜欢", Like_Playlist);
            addCM(cm, "移除", Remove_Playlist);
            return cm;
        }

        //获取歌单列表
        private List<string> getCardName()
        {
            List<string> items = new List<string>();
            foreach (var item in MyMusicCard.musicCardInfos)
            {
                int le = 12;
                string name = item.Name;
                if (name.Length > le) name = ClipString(name, le) + "...";
                items.Add(name);
            }
            return items;
        }

        public string ClipString(string inputString, int len)
        {
            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "";
            return tempString;
        }


        private void addCM(ContextMenu cm, string header, RoutedEventHandler routedEvent)
        {
            MenuItem menu = new MenuItem();
            menu.Header = header;
            menu.Click += routedEvent;
            //menu.Icon = "pack://application:,,,/Resources/Icon/null.png";
            cm.Items.Add(menu);
        }

        private void addCM(ContextMenu cm, string header, List<string> name)
        {
            MenuItem menu = new MenuItem();
            menu.Header = header;

            MenuItem menuCreate = new MenuItem();
            menuCreate.Header = "创建新歌单";
            menuCreate.Icon = "pack://application:,,,/Resources/Icon/add2.png";
            menuCreate.Click += Create_Click;
            menu.Items.Add(menuCreate);
            menu.Items.Add(new Separator());
            int i = 0;
            foreach (var item in name)
            {
                MenuItem menu2 = new MenuItem();
                //menu2.Width = 500;
                menu2.Header = item;
                menu2.Click += Select_Click;
                menu2.Tag = i;
                menu.Items.Add(menu2);
                i++;
            }
            cm.Items.Add(menu);
        }

        //添加歌单选择
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            MyMusicCard myMusicCard = new MyMusicCard();
            myMusicCard.AddMusic((int)menu.Tag, main.GetSelectMusicInfo());
            //message.ShowPopup($"已添加到{menu.Header}");
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!main.CreateCard()) return;
            MyMusicCard myMusicCard = new MyMusicCard();
            myMusicCard.AddMusic(0, main.GetSelectMusicInfo());
        }

        #region 自定义歌单右键菜单
        private void ChangeTitle(object sender, RoutedEventArgs e)
        {
            if (main.MusicCardList.SelectedIndex == -1) return;
            myMusicCard.ChangeTitle(main.MusicCardList);
        }
        private void ChangeInfo(object sender, RoutedEventArgs e)
        {
            if (main.MusicCardList.SelectedIndex == -1) return;
            myMusicCard.ChangeInfo(main.MusicCardList);
        }
        private void DeleteCard(object sender, RoutedEventArgs e)
        {
            if (main.MusicCardList.SelectedIndex == -1) return;
            myMusicCard.DeleteCard(main.MusicCardList);
        }
        #endregion

        #region 排行榜和搜索列表事件
        //排行榜搜索列表右键菜单
        private void Play_CM(object sender, RoutedEventArgs e)
        {
            if (main.Interface() == 0 || main.Interface() == 5)
            {
                main.NetSongDouble();
            }
            else
            {
                main.SearchSongDouble();
            }
        }
        private void PlayMV_CM(object sender, RoutedEventArgs e)
        {
            main.MvPlayClick();
        }
        private void Down_CM(object sender, RoutedEventArgs e)
        {
            network.DownSongToList();
        }
        private void Like_CM(object sender, RoutedEventArgs e)
        {
            main.MyLikeSong();
        }
        private void Share_CM(object sender, RoutedEventArgs e)
        {
            main.SongShare();
        }
        private void SongInfoCopy_CM(object sender, RoutedEventArgs e)
        {
            main.SongInfoCopy();
        }
        private void AddToPlayList_CM(object sender, RoutedEventArgs e)
        {
            main.AddToPlayList();
        }
        private void AddToNextPlay_CM(object sender, RoutedEventArgs e)
        {
            main.AddToPlayList(main.NowPlaying + 1);
        }
        private void Delete_CM(object sender, RoutedEventArgs e)
        {
            MyMusicCard myMusicCard = new MyMusicCard();
            myMusicCard.Delete(main.slRPage.SongList.SelectedIndex, main.MusicCardList);
        }
        #endregion

        #region 我的喜欢列表事件
        private void LikePlay_CM(object sender, RoutedEventArgs e)
        {
            main.LikeList_MouseDoubleClick(null, null);
        }
        private void LikeDown_CM(object sender, RoutedEventArgs e)
        {
            network.DownSongToList();
        }
        private void LikeRefresh_CM(object sender, RoutedEventArgs e)
        {
            main.ReadMyLikeSong();
        }
        private void LikeSongInfoCopy_CM(object sender, RoutedEventArgs e)
        {
            main.SongInfoCopy();
        }
        private void LikeShare_CM(object sender, RoutedEventArgs e)
        {
            main.SongShare();
        }
        private void LikeNoLike_CM(object sender, RoutedEventArgs e)
        {
            if (main.LikeList.SelectedIndex == -1) return;
            main.musicLikeList.RemoveAt(main.LikeList.SelectedIndex);
            main.jsonRWInfo.WriteJsonFile(main.MyLikeSongPath, main.musicLikeList);
            main.ReadMyLikeSong();
        }

        #endregion 

        #region 本地音乐右键菜单
        private void LocalPlay_CM(object sender, RoutedEventArgs e)
        {
            main.LocalListPlayer();
        }
        private void LocalRefresh_CM(object sender, RoutedEventArgs e)
        {
            main.LocalListImprt(main.SavePath.Text);
        }
        private void LocalPath_CM(object sender, RoutedEventArgs e)
        {
            if (main.LocalList.SelectedIndex == -1) return;
            string path = main.localMusicList[main.LocalList.SelectedIndex].Path;
            Process.Start("ExpLorer", "/select," + path);
        }
        private void LocalDelete_CM(object sender, RoutedEventArgs e)
        {
            main.LocalDelete(sender, e);
        }
        private void LocalInfo_CM(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region 播放列表右键菜单
        private void Play_Playlist(object sender, RoutedEventArgs e)
        {
            main.PlayingList_DoubleClick();
        }
        private void Down_Playlist(object sender, RoutedEventArgs e)
        {
            network.DownSongToList();
        }
        private void Like_Playlist(object sender, RoutedEventArgs e)
        {
            main.MyLikeSong();
        }
        private void Remove_Playlist(object sender, RoutedEventArgs e)
        {
            main.PlayingListRemove();
        }
        #endregion
    }
}

using Microsoft.Win32;

using Morin.App.Common.Code;
using Morin.App.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Message = Morin.App.Model.Message;

namespace Morin.App.ViewModel
{
    class MyMusicCard
    {
        MusicCardJson MusicCardJson = new MusicCardJson();
        Message message = new Message();
        static public List<MusicCardInfo> musicCardInfos = new List<MusicCardInfo>();
        public string MySongListPath = AppDomain.CurrentDomain.BaseDirectory + "MySongList.json";

        //添加单曲到歌单
        public void AddMusic(int index, MusicInfo musicInfo)
        {
            if (musicInfo.SongId == null) return;
            bool ex = isMusicExistence(index, musicInfo);
            if (ex) { message.ShowMessage("此歌单已收藏过这首歌"); return; }
            MusicCardInfo musicCardInfo = musicCardInfos[index];
            musicCardInfo.Musics.Insert(0, musicInfo);
            musicCardInfo.Pic = musicInfo.PTUrl;
            musicCardInfo.Info = "无简介";
            musicCardInfos[index] = musicCardInfo;
            SetMusicCard();
        }

        //刷新单个歌单
        public void AddMusicCard(ListBox listBox, MusicCardInfo musicCardInfo)
        {
            listBox.ItemTemplate = MainWindow.Instance.FindResource("SongListDataTemplate") as DataTemplate;
            string img = musicCardInfo.Pic;
            if (musicCardInfo.Musics.Count == 0 || String.IsNullOrEmpty(img)) img = morinImgPath;
            listBox.Items.Add(new FMListInfo
            {
                Pic = img,
                Name = musicCardInfo.Name,
                Visibility = Visibility.Hidden,
                Total = musicCardInfo.Musics.Count + "首",
            });
            //通知右键菜单改变
            MainWindow.Instance.CountextMenrUpdate();
        }

        //检查歌曲是否重复
        private bool isMusicExistence(int index, MusicInfo musicInfo)
        {
            foreach (var item in musicCardInfos[index].Musics)
            {
                if (item.SongId == musicInfo.SongId) { return true; }
            }
            return false;
        }

        //检查歌单是否重复
        private bool isCardExistence(MusicCardInfo musicCardInfo)
        {
            foreach (var item in musicCardInfos)
            {
                if (item.Name == musicCardInfo.Name) { return true; }
            }
            return false;
        }

        //检查歌曲是否重复
        public bool isCardExistence(string cardname)
        {
            foreach (var item in musicCardInfos)
            {
                if (item.Name == cardname) return true;
            }
            return false;
        }

        //新建单个歌单
        public void CreateMusicCard(ListBox listBox, MusicCardInfo musicCardInfo)
        {
            bool ex = isCardExistence(musicCardInfo);
            if (ex) { message.ShowMessage("在列表中已存在同名歌单"); return; }
            AddMusicCard(listBox, musicCardInfo);
            musicCardInfos.Insert(0, musicCardInfo);
            SetMusicCard();
            LoadAllCard(listBox);
        }

        //移除知道项
        public void Delete(int index, ListBox listBox)
        {
            MainWindow main = MainWindow.Instance;
            int cardid = musicCardIndex(main.slRPage.PLName.Text);
            if (cardid == -1 || index == -1) return;
            musicCardInfos[cardid].Musics.RemoveAt(index);
            //main.SLRPage.SongList.Items.RemoveAt(index);
            OpenMusicCard(openIndex);
            SetMusicCard();
            LoadAllCard(listBox);
        }

        public int musicCardIndex(string name)
        {
            int i = 0;
            foreach (var item in musicCardInfos)
            {
                if (item.Name == name) return i;
                i++;
            }
            return -1;
        }

        //加载所有歌单
        public void LoadAllCard(ListBox listBox)
        {
            GetMusicCard();
            listBox.Items.Clear();
            foreach (var musicCardInfo in musicCardInfos)
            {
                AddMusicCard(listBox, musicCardInfo);
            }
            MainWindow main = MainWindow.Instance;
            main.CountextMenrUpdate();
        }

        static int openIndex = -1;
        string morinImgPath = @"pack://application:,,,/Resources/Images/MORIN.jpg";
        public void OpenMusicCard(int Index)
        {
            //try
            {
                if (Index != -1) openIndex = Index;
                MainWindow main = MainWindow.Instance;
                MusicCardInfo musicCardInfo = musicCardInfos[openIndex];
                List<MusicInfo> musicInfos = musicCardInfo.Musics;
                string img = musicCardInfo.Pic;
                if (musicInfos.Count == 0 || img == "") img = morinImgPath;
                main.slRPage.PLImg.Source = new BitmapImage(new Uri(img));
                main.slRPage.PLName.Text = musicCardInfo.Name;
                main.slRPage.PLInt.Text = musicCardInfo.Info.Replace("\n", "");

                main.slRPage.SongList.Items.Clear();
                main.musicSearchList.Clear();

                if (musicInfos.Count == 0) return;

                foreach (var musicInfo in musicInfos)
                {
                    main.musicSearchList.Add(musicInfo);
                    main.LoadToListView(main.slRPage.SongList, musicInfo);
                }
                main.slRPage.play_btn.Content = " " + musicInfos.Count + "首";
                main.slRPage.SongList.ContextMenu = new MyCountextMenu().CardListCountextMenu();
            }
            // catch { }
        }

        public void ChangeTitle(ListBox listBox)
        {
            ShowWriteMessage showWrite = new ShowWriteMessage();
            showWrite.text.Text = musicCardInfos[listBox.SelectedIndex].name;
            showWrite.ShowDialog();
            if (!showWrite.isOk) return;
            if (String.IsNullOrEmpty(showWrite.Content)) { message.ShowMessage("简介不能为空"); return; }
            string title = showWrite.Content;

            MusicCardInfo musicCardInfo = musicCardInfos[listBox.SelectedIndex];
            musicCardInfo.Name = title;
            musicCardInfos[listBox.SelectedIndex] = musicCardInfo;
            SetMusicCard();
            LoadAllCard(listBox);
        }
        public void ChangeInfo(ListBox listBox)
        {
            ShowWriteMessage showWrite = new ShowWriteMessage();
            showWrite.text.Text = musicCardInfos[listBox.SelectedIndex].Info;
            showWrite.ShowDialog();
            if (!showWrite.isOk) return;
            if (String.IsNullOrEmpty(showWrite.Content)) { message.ShowMessage("简介不能为空"); return; }
            string info = showWrite.Content;

            MusicCardInfo musicCardInfo = musicCardInfos[listBox.SelectedIndex];
            musicCardInfo.Info = info;
            musicCardInfos[listBox.SelectedIndex] = musicCardInfo;
            SetMusicCard();
            LoadAllCard(listBox);
        }
        public void DeleteCard(ListBox listBox)
        {
            musicCardInfos.RemoveAt(listBox.SelectedIndex);
            SetMusicCard();
            LoadAllCard(listBox);
        }

        public void Output(ListBox listBox)
        {
            if (listBox.Items.Count == 0) return;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //设置文件类型   
            saveFileDialog1.Filter = " json files(*.json)|*.json";
            saveFileDialog1.FileName = "MySongList.json";
            saveFileDialog1.Title = "选择导出文件位置";
            //设置默认文件类型显示顺序   
            saveFileDialog1.FilterIndex = 2;
            //保存对话框是否记忆上次打开的目录   
            saveFileDialog1.RestoreDirectory = true;
            //点了保存按钮进入   
            if (saveFileDialog1.ShowDialog() == true)
            {
                //获得文件路径   
                string localFilePath = saveFileDialog1.FileName.ToString();
                File.Copy(MySongListPath, localFilePath, true);
                message.ShowMessage("导出完成！");
            }
        }

        public void Input(ListBox listBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " json files(*.json)|*.json";
            openFileDialog.Title = "选择歌单的json文件位置";
            if (openFileDialog.ShowDialog() == true)
            {
                string localFilePath = openFileDialog.FileName.ToString();
                if (File.Exists(MySongListPath))
                {
                    bool r = message.ShowMessageOK("是否覆盖已存在的所有歌单？无法恢复");
                    if (r)
                    {
                        File.Copy(localFilePath, MySongListPath, true);
                        LoadAllCard(listBox);
                        //ShowMessage("导出完成！");
                    }
                }
                else
                {
                    File.Copy(localFilePath, MySongListPath, true);
                    LoadAllCard(listBox);
                }
            }
        }

        //读到歌单
        private void GetMusicCard()
        {
            musicCardInfos.Clear();
            if (!File.Exists(MySongListPath)) { return; }
            musicCardInfos = MusicCardJson.ReadJsonFile(MySongListPath);
        }

        //写入歌单
        private void SetMusicCard()
        {
            MusicCardJson.WriteJsonFile(MySongListPath, musicCardInfos);
        }


    }
}

<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
=======
﻿using Microsoft.Win32;

using Morin.App.Common.AppSetting.Style;
using Morin.App.Common.Code;
using Morin.App.Common.Log;
using Morin.App.Common.OtherAPI;
using Morin.App.Common.Page;
using Morin.App.Common.Wave;
using Morin.App.Model;
using Morin.App.ViewModel;

using Panuon.UI.Silver;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
>>>>>>> bc8fab203afe35b34ef6038e5f17a8751cf4e46a
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
<<<<<<< HEAD
=======
using System.Windows.Media.Animation;
>>>>>>> bc8fab203afe35b34ef6038e5f17a8751cf4e46a
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

<<<<<<< HEAD
=======
using Message = Morin.App.Model.Message;
using Path = System.IO.Path;

>>>>>>> bc8fab203afe35b34ef6038e5f17a8751cf4e46a
namespace Morin.App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
<<<<<<< HEAD
        public MainWindow()
        {
            InitializeComponent();
=======
        public static MainWindow Instance { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            soundEngine = NAudioEngine.Instance;
            waveformTimeline.RegisterSoundPlayer(soundEngine);
        }

        NAudioEngine soundEngine;
        public Message message = new Message();
        MyMusicCard myMusicCard = new MyMusicCard();

        public Network network = new Network();
        public QQMusicAPI QQAPI = new QQMusicAPI();
        public KGMusicAPI KGMusicAPI = new KGMusicAPI();
        public NTMusicAPI NTMusicAPI = new NTMusicAPI();
        public KWMusicAPI KWMusicAPI = new KWMusicAPI();

        TransformType transformType = new TransformType();
        TimerKeyer timerKeyer = new TimerKeyer();
        RingMusicAPI ringMusicAPI = new RingMusicAPI();
        MVGet mvGet = new MVGet();
        public JsonRWInfo jsonRWInfo = new JsonRWInfo();
        //电台
        FmGet fmGet = new FmGet();
        public List<FmDetailInfo> fmListInfo = new List<FmDetailInfo>();
        public List<SpeakerDetailInfo> speakerListInfo = new List<SpeakerDetailInfo>();

        //搜索信息列表
        public List<MusicInfo> musicSearchList = new List<MusicInfo>();
        //排行榜
        public List<MusicInfo> musicTopList = new List<MusicInfo>();
        //喜欢的音乐列表
        public List<MusicInfo> musicLikeList = new List<MusicInfo>();
        //播放列表
        public List<MusicInfo> musicPlayingList = new List<MusicInfo>();
        //本地音乐
        public List<MusicInfo> localMusicList = new List<MusicInfo>();
        //下载列表
        public List<MusicInfo> musicDownList = new List<MusicInfo>();

        public SearchResultPage srPage = new SearchResultPage();//搜索返回列表页
        public SongListResultPage slRPage = new SongListResultPage();//歌单搜索返回列表页

        public string[] musicParsSave = new string[20];//解析歌曲信息存储
        public string[] mvInfoSave = new string[10];//播放MV信息管理
        public string[,] singerInfoSave = new string[100, 10];//歌手信息保存
        public string[,] songListSave = new string[30, 10];//歌单信息保存


        public void CountextMenrUpdate()
        {
            MyCountextMenu myCountextMenu = new MyCountextMenu();
            HotList.ContextMenu = myCountextMenu.CountextMenu();
            srPage.SearchList.ContextMenu = myCountextMenu.CountextMenu();
            slRPage.SongList.ContextMenu = myCountextMenu.CountextMenu();
            LikeList.ContextMenu = myCountextMenu.LikeCountextMenu();
            LocalList.ContextMenu = myCountextMenu.LocalCountextMenu();
            MusicCardList.ContextMenu = myCountextMenu.MusicCardCountextMenu();
            PlayingList.ContextMenu = myCountextMenu.PlaylistCountextMenu();
        }

        public string Time = "0";//存储播放歌曲时长
                                 //缓存位置
        public string CachePath()
        {
            return CachePath_box.Text;
        }
        public string SongPath()
        {
            return CachePath() + "Song/";
        }
        public string LrcPath()
        {
            return SavePath.Text + "Lrc/";
        }
        public string Picpath()
        {
            return SavePath.Text + "SongImages/";
        }

        public string FileName = "", LrcText = "";
        public string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public string DownUrl = "";

        //魔音开始
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Instance = this;
                //lrcShowWindow.Visibility = Visibility.Hidden;//隐藏桌面歌词
                //getPower();
                ReadConfig();//读取配置
                             //StartSound();//开始音乐
                NotifyIconInit();//最小化到托盘
                Show(null, null);

                //右键菜单
                CountextMenrUpdate();
                //窗体启动置顶
                Topmost = true;
                Topmost = false;
                搜索引擎.SelectedIndex = DefaultEngine.SelectedIndex;
                RB1.IsChecked = true;
                newlist.IsChecked = true;//最新歌单

                ReadMyLikeSong();
                ReadLastPlayList();

                AllKeyboard(); //全局快捷键
                if (!network.IsConnectedInternet())
                { message.ShowMessage("无法访问网络，请检查网络是否连接，可以播放本地音乐"); return; }
                //流量统计
                UserStatistics userStatistics = new UserStatistics();
                userStatistics.StartRegister();
                Update.UpdateVersion();
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteException(ex);
                message.ShowMessage("初始化时出现了一些问题");
            }
        }

        public string MyLikeSongPath = AppDomain.CurrentDomain.BaseDirectory + "MyLikeSong.json";
        public void WriteMyLikeSong(MusicInfo musicInfo)
        {
            try
            {
                musicInfo.LikeDate = DateTime.Now.ToString("yyyy-MM-dd");
                musicLikeList.Insert(0, musicInfo);
                if (musicLikeList.Count == 0) return;
                jsonRWInfo.WriteJsonFile(MyLikeSongPath, musicLikeList);
            }
            catch
            {
                message.ShowMessage("添加到喜欢出错，请联系作者修复BUG");
            }
        }

        public void ReadMyLikeSong()
        {
            try
            {
                LikeList.Items.Clear();
                if (!File.Exists(MyLikeSongPath))
                {
                    musicLikeList.Clear();
                    return;
                }

                //读到喜欢的List
                musicLikeList = jsonRWInfo.ReadJsonFile(MyLikeSongPath);
                //加载到列表
                for (int i = 0; i < musicLikeList.Count; i++)
                {
                    MusicInfo musicInfo = musicLikeList[i];
                    LikeList.Items.Add(new
                    {
                        ID = (i + 1).ToString(),
                        Song = musicInfo.SongName,
                        Singer = musicInfo.Singer,
                        Album = musicInfo.AlbumName,
                        Duration = transformType.ToDateTime(Convert.ToInt32(musicInfo.Duration)),
                        From = musicInfo.From,
                        Data = musicInfo.LikeDate,
                    });
                }

                if (LikeList.Items.Count == 0)
                { Like_null.Visibility = Visibility.Visible; LikeList.Visibility = Visibility.Hidden; }
                else { Like_null.Visibility = Visibility.Hidden; LikeList.Visibility = Visibility.Visible; }
                LikeCount.Content = " " + LikeList.Items.Count + "首";
            }
            catch { message.ShowMessage("读取我的喜欢出错，请联系作者修复BUG"); }
        }

        private void Show(object sender, EventArgs e)
        {
            ShowMain();
        }

        //读取播放历史
        public void ReadLastPlayList()
        {
            try
            {
                if (!File.Exists(LastPlayListPath)) { return; }
                PlayingList.Items.Clear();
                //读到喜欢的List
                musicPlayingList = jsonRWInfo.ReadJsonFile(LastPlayListPath);
                if (musicPlayingList.Count == 0) return;
                //加载到列表
                foreach (MusicInfo musicInfo in musicPlayingList)
                {
                    string and = " - ";
                    if (musicInfo.Singer == "" || musicInfo.Singer == null) and = "";
                    PlayingList.Items.Add(new
                    {
                        ID = (PlayingList.Items.Count + 1).ToString(),
                        Song = musicInfo.SongName + and + musicInfo.Singer
                    });
                }

                if (PlayingList.Items.Count == 0) PlayList_null.Visibility = Visibility.Visible;
                else PlayList_null.Visibility = Visibility.Hidden;
                PlayCount.Visibility = Visibility.Visible;
                PlayCount.Text = PlayingList.Items.Count.ToString();
                PlayingListPlayer(NowPlaying);

                if (StartMusic.IsChecked == false)
                { play.Stop(); SynPlayUIState(false); }//播放UI
            }
            catch { message.ShowMessage("读取播放历史出错，请联系作者修复BUG"); }
        }

        public void AddpathPower(string pathname, string username, string power)
        {

            DirectoryInfo dirinfo = new DirectoryInfo(pathname);

            if ((dirinfo.Attributes & FileAttributes.ReadOnly) != 0)
            {
                dirinfo.Attributes = FileAttributes.Normal;
            }

            //取得访问控制列表
            DirectorySecurity dirsecurity = dirinfo.GetAccessControl();

            switch (power)
            {
                case "FullControl":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                    break;
                case "ReadOnly":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));
                    break;
                case "Write":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Write, AccessControlType.Allow));
                    break;
                case "Modify":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Modify, AccessControlType.Allow));
                    break;
            }
            dirinfo.SetAccessControl(dirsecurity);
        }

        public void StartSound()
        {
            if (StartMusic.IsChecked == true)
            {
                sound.Source = new Uri(AppPath + "Sound/audio1.mp3");
                sound.Play();
            }
        }


        public string playpath = "";
        public bool NeedDown = true;
        public string SongName = "";
        public async void PlayingListPlayer(int num)//播放列表播放器
        {
            string CacheUrlPath = "";
            try
            {
                soundEngine.OpenFile(null);
                FlushMemory();
                string UrlPath = "";
                SongLoadShow.IsRunning = false;
                if (num > musicPlayingList.Count - 1) return;
                MusicInfo musicInfo = musicPlayingList[num];
                //if (musicInfo.suffix == "ape")
                //{
                //    message.ShowMessage("暂时不支持" + musicInfo.suffix + "格式播放");
                //    return;
                //}
                string songid = musicInfo.SongId;
                string from = musicInfo.From;
                string songname = musicInfo.SongName;
                string singer = musicInfo.Singer;
                string lrcurl = musicInfo.LrcUrl;
                string mp3url = musicInfo.MP3Url;
                string pturl = musicInfo.PTUrl;



                if (from == "本地音乐") SongName = songname;
                else SongName = songname + " - " + singer;
                LrcText = noLrc;
                LrcView.LoadLrc(LrcText);
                if (PlayingList.Items.Count == 0 || num < 0 || num > PlayingList.Items.Count - 1) return;
                BlurImg.Visibility = Visibility.Visible;

                playpath = SongPath() + SongName + ".mp3";
                if (from == "QQ音乐") playpath = SongPath() + SongName + ".m4a";

                if (from != "本地音乐")
                {
                    if (!network.IsConnectedInternet()) { message.ShowMessage("无法访问网络，请检查网络是否连接"); return; }
                    Heart.IsEnabled = true;
                    Heart.Opacity = 1;
                    if (isExistLikeList(songid) != -1) Heart.Content = Likestr;
                    else Heart.Content = Nolikestr;
                    HeartColor();
                    if (!File.Exists(playpath))
                    {
                        NeedDown = true;
                        if (from == "QQ音乐")
                        {
                            CacheUrlPath = UrlPath = QQAPI.QQMusicUrlGet(songid);
                            LrcText = QQAPI.QQGetLrc(lrcurl);
                        }
                        else if (from == "网易云音乐")
                        {
                            CacheUrlPath = UrlPath = mp3url;
                            LrcText = NTMusicAPI.NTGetLrc(lrcurl);
                        }
                        else if (from == "酷我音乐")
                        {
                            //{
                            //    string kwurltext = "";//获取的信息文本
                            //    SongLoadShow.IsRunning = true;
                            //    await Task.Run(() =>
                            //    {
                            //        kwurltext = KWMusicAPI.SearchJson(mp3url);
                            //    });
                            //    SongLoadShow.IsRunning = false;
                            //    //JObject o2 = (JObject)JsonConvert.DeserializeObject(kwurltext);
                            //    //UrlPath = o2["url"].ToString();

                            //    //UrlPath = kwurltext;
                            //}
                            LrcText = KWMusicAPI.GetLrc(songid);
                            {
                                //获取128的峰值文件
                                //CacheUrlPath = "http://www.kuwo.cn/url?format=mp3&response=url&type=convert_url&br=128kmp3&from=web&rid=" + songid;

                                //CacheUrlPath = "http://antiserver.kuwo.cn/anti.s?type=convert_url&rid=" + songid + "&format=mp3&response=url";

                                string kwurltext = "";//获取的信息文本
                                SongLoadShow.IsRunning = true;
                                await Task.Run(() =>
                                {
                                    kwurltext = KWMusicAPI.SearchJson(mp3url.Replace("type=convert_url3", "type=convert_url"));
                                });
                                SongLoadShow.IsRunning = false;
                                CacheUrlPath = kwurltext;
                                UrlPath = kwurltext;
                                //JObject o2 = (JObject)JsonConvert.DeserializeObject(kwurltext);
                                //CacheUrlPath = o2["url"].ToString();
                            }
                        }
                        else if (from == "酷狗音乐")
                        {
                            SongLoadShow.IsRunning = true;
                            await Task.Run(() =>
                            {
                                KGMusicAPI.KGUrlGet(num);
                            });
                            SongLoadShow.IsRunning = false;
                            CacheUrlPath = UrlPath = musicPlayingList[num].MP3Url;
                            lrcurl = musicPlayingList[num].LrcUrl;
                            pturl = musicPlayingList[num].PTUrl;
                            LrcText = lrcurl;
                        }
                        else if (from == "酷狗铃声")
                        {
                            CacheUrlPath = UrlPath = mp3url;
                            LrcText = "";
                        }

                        if ((string.IsNullOrWhiteSpace(UrlPath) || musicInfo.fee) && from != "酷我音乐")
                        {
                            WuBanQuan();
                            return;
                        }
                        play.Source = new Uri(UrlPath.Replace("https", "http"));
                        if (!Directory.Exists(SongPath())) Directory.CreateDirectory(SongPath());
                        network.CacheSongDawn(CacheUrlPath, playpath);

                        if (!string.IsNullOrWhiteSpace(LrcText))
                        { LrcText = LrcText.Replace("//", ""); SaveLrc(LrcText, SongName); }
                        else LrcText = noLrc;
                    }
                    else
                    {
                        NeedDown = false;
                        play.Source = new Uri(playpath);
                        LrcText = ReadLrc(SongName);
                    }

                    if (Regex.Match(UrlPath, ".mp4").Success)
                    {
                        play.SetValue(Panel.ZIndexProperty, 0);
                        string Ta = (play.NaturalDuration.ToString()).ToString();
                        Time = null;
                        BlurImg.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Time = musicInfo.Duration;
                        play.SetValue(Panel.ZIndexProperty, -1);
                    }
                }
                else
                {
                    Heart.Content = Nolikestr;
                    Heart.Opacity = 0.5;
                    Heart.IsEnabled = false;
                    NeedDown = false;
                    string Ta = (play.NaturalDuration.ToString()).ToString();
                    Time = null;
                    play.Source = new Uri(mp3url);
                    playpath = mp3url;
                    pturl = Picpath() + SongName + ".jpg";
                    if (!File.Exists(pturl)) pturl = "";
                    LrcText = ReadLrc(SongName);
                }

                if (!Directory.Exists(Picpath())) Directory.CreateDirectory(Picpath());

                SongShow.Text = songname;
                if (string.IsNullOrWhiteSpace(singer))
                {
                    SingerShow.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SingerShow.Visibility = Visibility.Visible; SingerShow.Text = singer;
                }

                if (pturl.IsNullOrEmpty())
                {
                    pturl = "pack://application:,,,/Resources/Images/new Logo-alpha-2.png";
                }
                Cover.Source = BitmapFrame.Create(new Uri(pturl));
                SynPlayUIState(true);
                play.Play();
                lrcRe();
                LrcView.LoadLrc(LrcText);
                timerKeyer.tiemrs();
                //定位位置
                ShowNow();
                FlushMemory();
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
            }
        }

        private void Up_Click(object sender, EventArgs e)
        {
            Up_Click(null, null);
        }
        private void Play_Click(object sender, EventArgs e)
        {
            Play_Click(null, null);
        }
        private void Down_Click(object sender, EventArgs e)
        {
            Down_Click(null, null);
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        #region 顶栏

        private void Top_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //popup_box.IsOpen = false;
            //return;
            //DuiB += 1;
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            //timer.Tick += (s, e1) => { timer.IsEnabled = false; DuiB = 0; };
            //timer.IsEnabled = true;
            //if (DuiB % 2 == 0)
            //{
            //    Maximize_Click(null, null);
            //}
        }

        private void SearchBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (popup_box.IsOpen == false) ReadHistorySo();
            //popup_box.IsOpen = false;
            //popup_box.IsOpen = true;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (SearchBox.Text == "") { ReadHistorySo(); return; }
            //KWMusicAPI.HotSearchKey();
        }

        private void SongListSearchBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //if (SearchBox.Text == "") return;
            //if (!network.IsConnectedInternet()) { message.ShowMessage("无法访问网络，请检查网络是否连接"); return; }

            //SearchContent.Visibility = Visibility.Visible;
            //SearchContent.Content = SRPage;
            ////Retreat.IsEnabled = true;

            //Forward_Click(null, null);
            //popup_box.IsOpen = false;
            //WriteSearchHistory();
            //NTMusicAPI NTMusicAPI = new NTMusicAPI();
            //KGMusicAPI KGMusicAPI = new KGMusicAPI();
            //KWMusicAPI KWMusicAPI = new KWMusicAPI();

            //SRPage.SearchList.Items.Clear();
            //if (搜索引擎.SelectedIndex == 0) QQAPI.QQMusicSoso();
            //else if (搜索引擎.SelectedIndex == 1) { NTMusicAPI.NTMusicSoso(); }
            //else if (搜索引擎.SelectedIndex == 2) { KWMusicAPI.KWMusicSoso(); }
            //else if (搜索引擎.SelectedIndex == 3) { KGMusicAPI.KGMusicSoso(); }
            //Tab.SelectedIndex = 0;
            //SRPage.sotex.Text = "搜索到 “ " + SearchBox.Text + " ” 的相关结果";
            //if (SRPage.SearchList.Items.Count > 0) SRPage.SearchList.ScrollIntoView(SRPage.SearchList.Items[0]);//滚动条置顶
        }

        public void SearchEngines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (Tab == null) return;
            //if (((TabItem)Tab.SelectedItem).Name == "设置") return;
            //SearchButton_Click(null, null);
            //if (搜索引擎.SelectedIndex == 0) 搜索引擎.Foreground = QQ.Foreground;
            //else if (搜索引擎.SelectedIndex == 1) 搜索引擎.Foreground = NT.Foreground;
            //else if (搜索引擎.SelectedIndex == 2) 搜索引擎.Foreground = KW.Foreground;
            //else if (搜索引擎.SelectedIndex == 3) 搜索引擎.Foreground = KG.Foreground;
        }

        private void SearchHistoryList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SearchHistoryList.SelectedIndex == -1) return;
            SearchBox.Text = SearchHistoryList.SelectedItem.ToString();
            SearchButton_Click(null, null);
        }

        private void Discern_Button_Click(object sender, RoutedEventArgs e)
        {
            ////network = new Network();
            //network.MusicDiscern();
            //Down_null.Visibility = Visibility.Hidden; DownList.Visibility = Visibility.Visible;
        }

        private void Theme_btn_Click(object sender, RoutedEventArgs e)
        {
            //SolidColorBrush ButtonNormal = FindResource("ButtonNormal") as SolidColorBrush;
            //SolidColorBrush ThemeColor = FindResource("ThemeColor") as SolidColorBrush;
            //if (sTheme == "White")
            //{
            //    Theme("Black");
            //    iniHelper.WriteValue("DefaultSet", "Theme", "Black");
            //}
            //else
            //{
            //    Theme("White");
            //    iniHelper.WriteValue("DefaultSet", "Theme", "White");
            //}
            //ReForeground();
        }

        private void Button_Click_Light(object sender, RoutedEventArgs e)
        {
            //Theme("White");
            //iniHelper.WriteValue("DefaultSet", "Theme", "White");
        }

        private void Button_Click_Dark(object sender, RoutedEventArgs e)
        {
            //Theme("Black");
            //iniHelper.WriteValue("DefaultSet", "Theme", "Black");
        }

        private void ToMost_Click(object sender, RoutedEventArgs e)
        {
            //if (!Topmost)
            //{
            //    //TopB.Content = "";
            //    Topmost = true; //ToMost.Foreground = ThemeColor;// new SolidColorBrush(Color.FromRgb(200, 25, 25));
            //    istom.Visibility = Visibility.Visible;
            //    ToMost.ToolTip = "点击取消置顶";
            //}
            //else
            //{ //TopB.Content = "";
            //    Topmost = false; //ToMost.Foreground = ButtonNormal;//new SolidColorBrush(Color.FromRgb(75, 75, 75)) ; 
            //    istom.Visibility = Visibility.Hidden;
            //    ToMost.ToolTip = "将窗口置于最顶端";
            //}
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        Rect rcnormal;//定义一个全局rect记录还原状态下窗口的位置和大小。
        string minze_str = "";
        string maxze_str = "";
        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            Rect rc = SystemParameters.WorkArea;//获取工作区大小
            if (Maximize.Content.ToString() == minze_str)
            {
                rcnormal = new Rect(this.Left, this.Top, this.Width, this.Height);//保存下当前位置与大小
                Maximize.Content = maxze_str;
                Maximize.FontSize = 16;
                Left = rc.Left;//设置位置
                Top = rc.Top;
                Width = rc.Width;
                Height = rc.Height;
            }
            else
            {
                Maximize.Content = minze_str;
                Maximize.FontSize = 20;
                Left = rcnormal.Left;
                Top = rcnormal.Top;
                Width = rcnormal.Width;
                Height = rcnormal.Height;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //if (isFirstStart)
            //{
            //    bool n = message.ShowMessageOK("是否将魔音最小化到托盘？", true, "最小化到托盘", "退出程序", out isFirstStart);
            //    if (n) NotifyIcon.IsChecked = true;
            //    else NotifyIcon.IsChecked = false;
            //}
            //if (NotifyIcon.IsChecked == true) NotifyIconInit();
            //else
            //{
            //    CloseMo(null, null);
            //}
        }

        #endregion


        #region 内容区

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TabItem tabItem = (TabItem)Tab.SelectedItem;
            //if (tabItem == null) return;
            //if (tabItem.Name == "设置") inset = 1;
            //else if (inset == 1) { WriteConfig(); inset = 0; }
        }

        private void SongList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (SongList.SelectedIndex == -1) return;
            //    SLRPage.showFavorites = true;
            //    SearchContent.Content = SLRPage;
            //    SearchContent.Visibility = Visibility.Visible;
            //    MusicCardContent.Content = null;
            //    KWMusicAPI.SongListToList(SongList.SelectedIndex);
            //}
            //catch (Exception e1)
            //{
            //    Logger.Instance.WriteException(e1);
            //    message.ShowMessage("遇到错误！" + e1.Message);
            //}
        }

        private void SongListSearchButton_Click(object sender, RoutedEventArgs e)
        {
            //KWMusicAPI.SearchSongList();
        }

        private void RadioButton_Checked_11(object sender, RoutedEventArgs e)
        {
            //KWMusicAPI.SongList("hot");
        }

        private void RadioButton_Checked_12(object sender, RoutedEventArgs e)
        {
            //KWMusicAPI.SongList("new");
        }

        private void IsShowPeak_Checked(object sender, RoutedEventArgs e)
        {
            //if (!IsLoaded) return;
            //waveformTimeline.Visibility = Visibility.Visible;
        }

        private void IsShowPeak_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (!IsLoaded) return;
            //waveformTimeline.Visibility = Visibility.Hidden;
        }

        private void TopAllPlay_Click(object sender, RoutedEventArgs e)
        {
            //HotList.SelectedIndex = 0;
            //NetSongDouble();
        }

        private void TopRefresh_Click(object sender, RoutedEventArgs e)
        {
            //KWRank(AtRankId);
        }

        private void HotList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (HotList.SelectedIndex != -1) NetSongDouble();
        }

        private void HotList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void SongButton_Click(object sender, RoutedEventArgs e)
        {
            //if (HotList.SelectedIndex == -1) return;
            //Button ItemLabel = sender as Button;
            //if (Interface() == 0)
            //{
            //    HotList.SelectedItem = ItemLabel.DataContext;
            //    SelectedIndex = HotList.SelectedIndex;
            //    string text = MusicTopList[SelectedIndex].songname;
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
            //else if (Interface() == 1)
            //{
            //    SRPage.SearchList.SelectedItem = ItemLabel.DataContext;
            //    SelectedIndex = SRPage.SearchList.SelectedIndex;
            //    string text = MusicSearchList[SelectedIndex].songname;
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
            //else if (Interface() == 2)
            //{
            //    LikeList.SelectedItem = ItemLabel.DataContext;
            //    SelectedIndex = LikeList.SelectedIndex;
            //    string text = MusicLikeList[SelectedIndex].songname;
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
        }

        private void MV_Button_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //HotList.SelectedItem = ItemLabel.DataContext;
            //SelectedIndex = HotList.SelectedIndex;
            //Cover_MouseLeftButtonUp(null, null);
            //MVGet.KWMvGet(MusicTopList[SelectedIndex].mvid);
        }

        public void SearchMV_Button_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //SRPage.SearchList.SelectedItem = ItemLabel.DataContext;
            //SLRPage.SongList.SelectedItem = ItemLabel.DataContext;
            //SelectedIndex = SRPage.SearchList.SelectedIndex;
            //if (SelectedIndex == -1) SelectedIndex = SLRPage.SongList.SelectedIndex;
            //Cover_MouseLeftButtonUp(null, null);
            //MVGet.SearchMvAdd();
        }

        //喜欢这首歌
        private void Like_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //HotList.SelectedItem = ItemLabel.DataContext;
            //MyLikeSong();
        }

        public void SearchLike_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //SRPage.SearchList.SelectedItem = ItemLabel.DataContext;
            //SLRPage.SongList.SelectedItem = ItemLabel.DataContext;
            //MyLikeSong();
        }

        //添加到下载列表
        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //HotList.SelectedItem = ItemLabel.DataContext;
            //network.DownSongToList();
        }
        public void SearchDown_Button_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //SRPage.SearchList.SelectedItem = ItemLabel.DataContext;
            //SLRPage.SongList.SelectedItem = ItemLabel.DataContext;
            //network.DownSongToList();
        }

        public void AddList_Click(object sender, RoutedEventArgs e)
        {
            //MusicInfo musicInfo = new MusicInfo();
            //int f = -1;
            //Button ItemLabel = sender as Button;
            //if (Interface() == 0)
            //{
            //    HotList.SelectedItem = ItemLabel.DataContext;
            //    f = HotList.SelectedIndex;
            //    musicInfo = MusicTopList[f];
            //}
            //else if (Interface() == 1)
            //{
            //    SRPage.SearchList.SelectedItem = ItemLabel.DataContext;
            //    f = SRPage.SearchList.SelectedIndex;
            //    musicInfo = MusicSearchList[f];
            //}
            //else if (Interface() == 5)
            //{
            //    SLRPage.SongList.SelectedItem = ItemLabel.DataContext;
            //    f = SLRPage.SongList.SelectedIndex;
            //    musicInfo = MusicSearchList[f];
            //}
            ////if (musicInfo.fee && musicInfo.from != "酷我音乐")
            ////{
            ////    WuBanQuan();
            ////    return;
            ////}
            //int s = isExistPlayList(musicInfo);
            //if (s == -1) AddPlayingList(musicInfo);
            //else message.ShowPopup("已经添加过了", true);
        }

        //歌手点击
        public void SingerButton_Click(object sender, RoutedEventArgs e)
        {
            //Button ItemLabel = sender as Button;
            //if (Interface() == 0)
            //{
            //    HotList.SelectedItem = ItemLabel.DataContext;
            //    SelectedIndex = HotList.SelectedIndex;
            //    string text = MusicTopList[SelectedIndex].singer;
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
            //else if (Interface() == 1 || Interface() == 5)
            //{
            //    SRPage.SearchList.SelectedItem = ItemLabel.DataContext;
            //    SLRPage.SongList.SelectedItem = ItemLabel.DataContext;
            //    SelectedIndex = SRPage.SearchList.SelectedIndex;
            //    if (SelectedIndex == -1) SelectedIndex = SLRPage.SongList.SelectedIndex;

            //    string text = MusicSearchList[SelectedIndex].singer;
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
            //else if (Interface() == 2)
            //{
            //    LikeList.SelectedItem = ItemLabel.DataContext;
            //    SelectedIndex = LikeList.SelectedIndex;
            //    string text = MusicLikeList[SelectedIndex].singer;
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
        }

        //热门歌曲
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //KWRank(16);
            //Rank(26);
        }
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            //KWRank(17);

            //Rank(27);
        }
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            //KWRank(185);
            //Rank(36);
        }
        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            //KWRank(26);

            //Rank(52);
        }
        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            //KWRank(93);
            //Rank(4);
        }
        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            try
            {
                //RingMusicAPI.RingMusicBang();//抖音
                //HotList.ScrollIntoView(HotList.Items[0]);//滚动条置顶
                //KWRank(158);
            }
            catch (Exception)
            {
            }
        }
        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            //KWRank(154);
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            //KWRank(64);
        }

        private void RadioButton_Checked_8(object sender, RoutedEventArgs e)
        {
            //KWRank(182);
        }

        private void RadioButton_Checked_9(object sender, RoutedEventArgs e)
        {
            //KWRank(22);
        }

        private void RadioButton_Checked_10(object sender, RoutedEventArgs e)
        {
            //KWRank(184);
        }

        private void Douyin_Button1_Checked(object sender, RoutedEventArgs e)
        {
            //KWRank(183);
        }

        private void SingerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void SingerList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (SingerList.Items.Count > 0 && SingerList.SelectedIndex > -1)
            //{
            //    string text = SingerInfoSave[SingerList.SelectedIndex, 0];
            //    SearchBox.Text = text;
            //    SearchButton_Click(null, null);
            //}
        }

        private void FmList_Loaded(object sender, RoutedEventArgs e)
        {
            //FmGet fmGet = new FmGet();
            //fmGet.getFmList();
        }

        private void FmList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int s = FmList.SelectedIndex;
            if (s == -1) return;
            //FmPlayer(s);
        }

        public void LikeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    SelectedIndex = NowPlaying = LikeList.SelectedIndex;
            //    if (LikeList.Items.Count == 0 || SelectedIndex < 0) return;

            //    AllToPlayingList();
            //    ShowNow();
            //    PlayingListPlayer(NowPlaying);
            //    //RemoveNow = PlayingList.SelectedIndex;
            //}
            //catch (Exception ex)
            //{
            //    Logger.Instance.WriteException(ex);
            //}
        }

        private void LikeList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void LikeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (LikeList.Items.Count == 0)
            {
                Like_null.Visibility = Visibility.Visible;
                LikeList.Visibility = Visibility.Hidden;
            }
            else
            {
                Like_null.Visibility = Visibility.Hidden;
                LikeList.Visibility = Visibility.Visible;
            }
        }

        private void LocalList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void DownList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void B_Copy2_Click(object sender, EventArgs e)
        {
            B_Copy2_Click(null, null);
        }

        private void Play_B_Click(object sender, EventArgs e)
        {
            Play_B_Click(null, null);
        }

        private void B_Copy7_Click(object sender, EventArgs e)
        {
            B_Copy7_Click(null, null);
        }


        private void PlayingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //RemoveNow = PlayingList.SelectedIndex;
        }

        //最小化到托盘
        System.Windows.Forms.NotifyIcon _notifyIcon = new System.Windows.Forms.NotifyIcon();
        public void NotifyIconInit()
        {
            //// _notifyIcon = 
            //_notifyIcon.BalloonTipText = "Hello,Morin";
            //_notifyIcon.Text = "Morin";
            ////_notifyIcon.Icon = new System.Drawing.Icon(@"Appicon.ico");//程序图标
            //_notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);//当前程序图标
            //_notifyIcon.Visible = true;

            ////打开菜单项
            //System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("打开");
            //open.Click += new EventHandler(Show);
            ////隐藏菜单项
            //System.Windows.Forms.MenuItem hide = new System.Windows.Forms.MenuItem("隐藏");
            //hide.Click += new EventHandler(Hide);
            ////退出菜单项
            //System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            //exit.Click += new EventHandler(CloseMo);


            //System.Windows.Forms.MenuItem play = new System.Windows.Forms.MenuItem("播放/暂停");
            //play.Click += new EventHandler(Play_B_Click);
            //System.Windows.Forms.MenuItem up = new System.Windows.Forms.MenuItem("上一曲");
            //up.Click += new EventHandler(B_Copy2_Click);
            //System.Windows.Forms.MenuItem down = new System.Windows.Forms.MenuItem("下一曲");
            //down.Click += new EventHandler(B_Copy7_Click);

            ////关联托盘控件
            //System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, hide, exit, play, up, down };
            //_notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            ////单击图标
            //_notifyIcon.MouseClick += OnMouseClickHandler;
            //Hide(null, null);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            //LocalList.SelectedIndex = 0;
            //LocalList_MouseDoubleClick(null, null);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            ////刷新本地列表
            //LocalListImprt(currentPath);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            //if (currentPath == "") return;
            //Process.Start("ExpLorer", currentPath);
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            //if (currentPath == "") return;
            //bool r = message.ShowMessageOK("确定要移除这个音乐目录吗？（不会删除文件夹）");
            //if (!r) return;

            //folderPaths = iniHelper.ReadValue("DownSet", "FolderPath");
            //if (folderPaths.Contains("#")) folderPaths = folderPaths.Replace("#" + currentPath, "");
            //else folderPaths = folderPaths.Replace(currentPath, "");
            //currentPath = "";
            //LocalMusicList.Clear();
            //LocalList.Items.Clear();
            //WriteConfig();
            //AddAllPath();

            ////if (LocalList.Items.Count == 0) return;

            ////bool r = message.ShowMessageOK("此操作会永久删除全部音乐文件(不经过回收站)，你确定吗？");
            ////if (!r) return;
            ////Folder folder = new Folder();
            ////folder.DeleteFolder(currentPath);
            ////LocalList.Items.Clear();
            ////Local_null.Visibility = Visibility.Visible;
            ////message.ShowPopup("全部删除完成！");
        }

        private void SearchBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //LocalMusicSearch();
        }

        private void LocalSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (LocalSearch.Text != "")
            //{
            //    LocalMusicSearch();
            //}
            //else
            //{
            //    LocalListImprt(currentPath);
            //}
        }

        private void SingerList_Loaded(object sender, RoutedEventArgs e)
        {
            //if (音乐库Tab.SelectedIndex != 2)
            //{
            //    return;
            //}

            //if (SingerList.Items.Count == 0)
            //{
            //    SingerTop();
            //}
        }

        //我的喜欢播放
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (LikeList.Items.Count == 0)
            {
                return;
            }
            LikeList.SelectedIndex = 0;
            LikeList_MouseDoubleClick(null, null);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            //if (LikeList.Items.Count == 0)
            //{
            //    return;
            //}
            //ReadMyLikeSong();
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            //if (LikeList.Items.Count == 0)
            //{
            //    return;
            //}
            //bool r = message.ShowMessageOK("确定清空所有喜欢的音乐吗？无法恢复");
            //if (r)
            //{
            //    MusicLikeList.Clear();
            //    LikeList.Items.Clear();
            //    jsonRWInfo.WriteJsonFile(MyLikeSongPath, MusicLikeList);
            //    ReadMyLikeSong();
            //    message.ShowMessage("清空完成！");
            //}
        }

        //我的喜欢导入
        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = " json files(*.json)|*.json";
            //openFileDialog.Title = "选择我的喜欢json文件位置";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    string localFilePath = openFileDialog.FileName.ToString();
            //    if (File.Exists(MyLikeSongPath))
            //    {
            //        bool r = message.ShowMessageOK("是否覆盖已喜欢的文件？无法恢复");
            //        if (r)
            //        {
            //            File.Copy(localFilePath, MyLikeSongPath, true);
            //            ReadMyLikeSong();
            //            //message.ShowMessage("导出完成！");
            //        }
            //    }
            //    else
            //    {
            //        File.Copy(localFilePath, MyLikeSongPath, true);
            //        ReadMyLikeSong();
            //    }
            //}
        }

        //我的喜欢导出
        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            //if (LikeList.Items.Count == 0) return;
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            ////设置文件类型   
            //saveFileDialog1.Filter = " json files(*.json)|*.json";
            //saveFileDialog1.FileName = "MyLikeSong.json";
            //saveFileDialog1.Title = "选择导出文件位置";
            ////设置默认文件类型显示顺序   
            //saveFileDialog1.FilterIndex = 2;
            ////保存对话框是否记忆上次打开的目录   
            //saveFileDialog1.RestoreDirectory = true;
            ////点了保存按钮进入   
            //if (saveFileDialog1.ShowDialog() == true)
            //{
            //    //获得文件路径   
            //    string localFilePath = saveFileDialog1.FileName.ToString();
            //    File.Copy(MyLikeSongPath, localFilePath, true);
            //    message.ShowMessage("导出完成！");
            //}
        }

        private void MusicCardList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (MusicCardList.SelectedIndex == -1) return;
            //    SLRPage.showFavorites = false;
            //    MusicCardContent.Content = SLRPage;
            //    MusicCardContent.Visibility = Visibility.Visible;
            //    SearchContent.Content = null;
            //    myMusicCard.OpenMusicCard(MusicCardList.SelectedIndex);
            //}
            //catch (Exception e1)
            //{
            //    Logger.Instance.WriteException(e1);
            //    message.ShowMessage("遇到错误！" + e1.Message);
            //}
        }

        private void MusicCardList_Loaded(object sender, RoutedEventArgs e)
        {
            //myMusicCard.LoadAllCard(MusicCardList);
        }

        private void Create_btn_Click(object sender, RoutedEventArgs e)
        {
            //CreateCard();
        }

        private void CardRefresh_btn_Click(object sender, RoutedEventArgs e)
        {
            //MusicCardList.Items.Clear();
            //myMusicCard.LoadAllCard(MusicCardList);
        }

        private void CardInput_btn_Click(object sender, RoutedEventArgs e)
        {
            //myMusicCard.Input(MusicCardList);
        }

        private void CardOutput_btn_Click(object sender, RoutedEventArgs e)
        {
            //myMusicCard.Output(MusicCardList);
        }

        private void Cardurlinput_btn_Click(object sender, RoutedEventArgs e)
        {
            //network.MusicDiscern();
        }

        private void DownList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DownListPlayer();
        }

        private void DownList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Badge_Num.Text = DownList.Items.Count.ToString();
        }

        private void DownList_Loaded(object sender, RoutedEventArgs e)
        {
            if (DownList.Items.Count == 0)
            {
                Down_null.Visibility = Visibility.Visible;
                DownList.Visibility = Visibility.Hidden;
            }
            else
            {
                Down_null.Visibility = Visibility.Hidden;
                DownList.Visibility = Visibility.Visible;
            }
        }

        private void DownList_MouseMove(object sender, MouseEventArgs e)
        {
            //string[] files = new string[1];
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    if (DownList.SelectedIndex >= 0)
            //    {
            //        string path = MusicDownList[DownList.SelectedIndex].songPath;
            //        files[0] = path;
            //        DragDrop.DoDragDrop(DownList, new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, files), System.Windows.DragDropEffects.Copy /* | DragDropEffects.Link */);
            //    }
            //}
        }

        private void DownList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*
            ListView ItemLabel = sender as ListView;
            DownList.SelectedItem = ItemLabel.DataContext;
            CopyText(DownSongSave[DownList.SelectedIndex, 1]);
            message.ShowPopup("已复制到剪切板");*/
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            //if (DownList.Items.Count == 0) return;
            //network.Downing = 0;
            //network.isDown = 0;
            //DownList.Items.Clear();
            //MusicDownList.Clear();
            //Down_null.Visibility = Visibility.Visible;
            //DownCount.Visibility = Visibility.Hidden;
            ////message.ShowPopup("已清空下载列表");
        }

        private void LocalList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //LocalListPlayer();
        }

        private void LocalList_Loaded(object sender, RoutedEventArgs e)
        {
            //AddAllPath();
            //LocalListImprt(currentPath);
        }

        //文件拖出
        private void LocalList_MouseMove(object sender, MouseEventArgs e)
        {
            //string[] files = new string[1];
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    int sele = LocalList.SelectedIndex;
            //    if (sele >= 0)
            //    {
            //        string path = LocalMusicList[sele].path;
            //        files[0] = path;
            //        DragDrop.DoDragDrop(LocalList, new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, files), System.Windows.DragDropEffects.Copy /* | DragDropEffects.Link */);
            //    }
            //}
        }

        string folderPaths = "";
        string currentPath = "";
        private void Add_Path_Click(object sender, RoutedEventArgs e)
        {
            //string path = GetFolderPath();
            //if (path == "") return;
            //string savePath = iniHelper.ReadValue("DownSet", "FolderPath");
            //string[] s = savePath.Split('#');
            //foreach (string s1 in s)
            //{
            //    if (s1 == path)
            //    {
            //        message.ShowMessage("已经添加过这个目录了"); return;
            //    }
            //}

            //if (savePath.Contains("#")) folderPaths = $"{savePath}{path}#";
            //else folderPaths = $"{savePath}#{path}#";
            //WriteConfig();
            //AddLocalPath(path);
        }

        private void SearchButton1_Click(object sender, RoutedEventArgs e)
        {
            //LocalMusicSearch();
        }


        private void ClearCache_Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //SingerList.Items.Clear();
            //    FlushMemory();
            //    Folder folder = new Folder();
            //    folder.DeleteFolder(CachePath_box.Text);
            //    folder.DelTimers();
            //    message.ShowPopup("已删除没使用的缓存文件", true);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Instance.WriteException(ex);
            //    message.ShowPopup("清理失败！", false);
            //}
        }

        //缓存目录更改
        private void CacheButton_Click(object sender, RoutedEventArgs e)
        {
            ////在VS里打开Package Manager Console后输入Install-Package WindowsAPICodePack-Shell获取包后
            //var dialog = new CommonOpenFileDialog();
            //dialog.IsFolderPicker = true;
            //dialog.InitialDirectory = SavePath.Text;
            //CommonFileDialogResult result = dialog.ShowDialog();
            //if (result == CommonFileDialogResult.Cancel)
            //{
            //    return;
            //}
            //string path = dialog.FileName;
            //if (path.Substring(path.Length - 1, 1) != "\\") path += "\\";
            //CachePath_box.Text = path;
            ////CachePath() = CachePath_box.Text;
        }

        private void CachePath_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("ExpLorer", CachePath_box.Text);
        }

        //打开文件夹对话框 加大版
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //在VS里打开Package Manager Console后输入Install-Package WindowsAPICodePack-Shell获取包后
            //    var dialog = new CommonOpenFileDialog();
            //    dialog.IsFolderPicker = true;
            //    dialog.InitialDirectory = SavePath.Text;
            //    CommonFileDialogResult result = dialog.ShowDialog();
            //    if (result == CommonFileDialogResult.Cancel)
            //    {
            //        return;
            //    }
            //    string path = dialog.FileName;
            //    if (path.Substring(path.Length - 1, 1) != "\\") path += "\\";
            //    SavePath.Text = path;
            //    WriteConfig();
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void Prog_Lrc1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (LrcView != null) LrcView.LoadLrc(LrcText);
        }

        private void Prog_Lrc2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (LrcView != null) LrcView.LoadLrc(LrcText);
        }

        private void Prog_Lrc3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //try
            //{
            //    if (LrcView == null || MainWindow.Instance == null)
            //    {
            //        return;
            //    }
            //    var lrcmain = MainWindow.Instance.lrcShowWindow;
            //    if (lrcmain != null)
            //    {
            //        lrcmain.lrcShow.FontSize = Prog_Lrc3.Value;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void Prog_Lrc4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //try
            //{
            //    if (LrcView == null || MainWindow.Instance == null)
            //    {
            //        return;
            //    }
            //    var lrcmain = MainWindow.Instance.lrcShowWindow;
            //    if (lrcmain != null)
            //    {
            //        lrcmain.grid1.Opacity = Prog_Lrc4.Value / 100;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void Prog_Lrc5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Width = Prog_Lrc5.Value;
            }
            catch (Exception)
            {

            }
        }

        private void Prog_Lrc6_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Height = Prog_Lrc6.Value;
            }
            catch (Exception)
            {

            }
        }

        private void IsEnabledKey_Checked(object sender, RoutedEventArgs e)
        {
            //if (isEnabledKey.IsChecked == true) UnRegist(this,null);
            //else AllKeyboard();
        }

        private void About_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string NowVersion = Application.ResourceAssembly.GetName().Version.ToString();
            badge1.Text = "v " + NowVersion;
        }

        Update Update = new Update();
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //Update.UpdateVersion();
            //string NowVersion = Application.ResourceAssembly.GetName().Version.ToString();
            //if (!Update.CompareVersion(Update.NewVersion, NowVersion))
            //{
            //    message.ShowPopup("已是最新版本，无需升级！", true);
            //}
        }

        private void WWW_Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Process.Start("http://www.huanghunxiao.com");
            //}
            //catch
            //{
            //    message.ShowMessage("打开浏览器失败！");
            //}
        }

        //反馈
        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Process.Start("https://support.qq.com/products/175540");
            //}
            //catch (Exception)
            //{
            //    message.ShowMessage("打开浏览器失败！");
            //}
        }

        //赞助
        private void Sponsor_Click(object sender, RoutedEventArgs e)
        {
            if (SponsorGrid.Visibility == Visibility.Hidden)
            {
                SponsorGrid.Visibility = Visibility.Visible;
            }
            else
            {
                SponsorGrid.Visibility = Visibility.Hidden;
            }
        }

        private void Gzh_Click(object sender, RoutedEventArgs e)
        {
            if (GzhGrid.Visibility == Visibility.Hidden)
            {
                GzhGrid.Visibility = Visibility.Visible;
            }
            else
            {
                GzhGrid.Visibility = Visibility.Hidden;
            }
        }

        private void X3_Click(object sender, RoutedEventArgs e)
        {
            SponsorGrid.Visibility = Visibility.Hidden;
        }

        private void X4_Click(object sender, RoutedEventArgs e)
        {
            GzhGrid.Visibility = Visibility.Hidden;
        }


        #endregion

        #region Other

        public LrcShowWindow lrcShowWindow = new LrcShowWindow();
        private void Lrc_Click(object sender, RoutedEventArgs e)
        {
            if (lrcShowWindow.Visibility == Visibility.Visible) lrcShowWindow.Visibility = Visibility.Hidden;
            else { lrcShowWindow.Show(); lrcShowWindow.Visibility = Visibility.Visible; }
        }

        public void ShowMain()
        {
            if (IsLoaded)
            {
                this.Visibility = Visibility.Visible;
                this.ShowInTaskbar = true;
                this.Activate();
            }
        }

        public void PlayClick()
        {
            try
            {
                if (play.Source == null) return;
                if (Play_B.Content.ToString() == pauseui)
                {
                    play.Pause();
                    timerKeyer.Progtimer.Stop();
                    SynPlayUIState(false);
                }
                else
                {

                    if (time1.Text == time2.Text)
                    {
                        play.Stop(); play.Position = new TimeSpan(0, 0, 0); Prog.Value = 0;
                    }
                    play.Play();
                    TimeSpan ts = new TimeSpan(0, 0, (int)Prog.Value);
                    play.Position = ts;
                    timerKeyer.Progtimer.Start();

                    SynPlayUIState(true);
                }
            }
            catch { }
        }

        private void Cover_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            string pturl = "pack://application:,,,/Resources/Images/新LOGO设计-圆角.png";
            Cover.Source = BitmapFrame.Create(new Uri(pturl));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //解决拖拉窗口背景主页露出
            if (播放器区.Height != 0) UpAnimation2();
        }




        #endregion
        #region 播放

        public void UpAnimation2()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            double h = 0;
            if (播放器区.ActualHeight != 0) h = 播放器区.ActualHeight;
            doubleAnimation.From = h;
            doubleAnimation.To = window.ActualHeight - 30;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.0));
            播放器区.BeginAnimation(Grid.HeightProperty, doubleAnimation);
            播放器区.Margin = new Thickness(0);
        }

        //添加到播放列表
        public void AddPlayingListx(MusicInfo musicInfo, int index)
        {
            PlayCount.Visibility = Visibility.Visible;
            int i = index;
            string and = " - ";
            if (musicInfo.Singer == "" || musicInfo.Singer == null) and = "";
            PlayingList.Items.Add(new
            {
                ID = (i + 1).ToString(),
                Song = musicInfo.SongName + and + musicInfo.Singer
            });
            //NowPlaying = index;
            //message.ShowMessage(NowPlaying.ToString());
            //if (Interface() == 1) NowPlaying = i;//如果是搜索界面，选择最后一首播放
            PlayCount.Text = PlayingList.Items.Count.ToString();
            if (PlayingList.Items.Count == 0) PlayList_null.Visibility = Visibility.Visible;
            else PlayList_null.Visibility = Visibility.Hidden;
            //添加到历史记录
            WriteLastPlayList();
        }

        //添加到播放列表
        public void AddPlayingList(MusicInfo musicInfo)
        {
            musicPlayingList.Add(musicInfo);
            AddPlayingListx(musicInfo, PlayingList.Items.Count);
        }

        //播放历史
        string LastPlayListPath = AppDomain.CurrentDomain.BaseDirectory + "LastPlayList.json";
        public void WriteLastPlayList()
        {
            try
            {
                if (musicPlayingList.Count == 0) return;
                jsonRWInfo.WriteJsonFile(LastPlayListPath, musicPlayingList);
            }
            catch { message.ShowMessage("记录播放历史出错，请联系作者修复BUG"); }
        }

        int RemoveNow = 0;
        public void NextSong()
        {
            if (PlayingList.Items.Count == 0) return;
            //if (NowPlaying == PlayingList.Items.Count - 1) return;

            play.Source = null;
            // "随机播放";
            if (Cycle.Content.ToString() == "")
            {
                Random random = new Random();
                NowPlaying = random.Next(0, PlayingList.Items.Count - 1);
            }
            else
            {
                NowPlaying++;
            }
            if (NowPlaying > PlayingList.Items.Count - 1) NowPlaying = 0;
            if (MusicPlayingList[NowPlaying].fee) NextSong();
            PlayingListPlayer(NowPlaying);
        }

        public void LastSong()
        {
            if (PlayingList.Items.Count == 0) return;

            play.Source = null;
            // "随机播放";
            if (Cycle.Content.ToString() == "")
            {
                Random random = new Random();
                NowPlaying = random.Next(0, PlayingList.Items.Count - 1);
            }
            else NowPlaying--;
            if (NowPlaying == -1) NowPlaying = PlayingList.Items.Count - 1;
            if (musicPlayingList[NowPlaying].Fee) LastSong();
            PlayingListPlayer(NowPlaying);
        }

        public void AllToPlayingList()
        {
            //try
            {
                List<MusicInfo> listInfo = new List<MusicInfo>();
                int sele = 0;
                if (Interface() == 0)
                {
                    if (musicPlayingList.Count > 0 && musicPlayingList.Count == musicTopList.Count && musicTopList[0].SongId == musicPlayingList[0].SongId && HotList.Items.Count == PlayingList.Items.Count)
                    {
                        sele = PlayingList.SelectedIndex = HotList.SelectedIndex;
                        if (sele > -1) PlayingList.ScrollIntoView(PlayingList.Items[sele]);
                        return;
                    }
                    listInfo = musicTopList;
                }

                else if (Interface() == 5)
                {
                    if (musicPlayingList.Count > 0 && musicSearchList[0].SongName == musicPlayingList[0].SongName && slRPage.SongList.Items.Count == PlayingList.Items.Count)
                    {
                        sele = slRPage.SongList.SelectedIndex;
                        if (sele > -1) PlayingList.ScrollIntoView(PlayingList.Items[sele]);
                        return;
                    }
                    listInfo = musicSearchList;
                }
                else if (Interface() == 1)
                {
                    if (srPage.SearchList.Items.Count == 0)
                    {
                        return;
                    }
                    listInfo = musicSearchList;
                }
                else if (Interface() == 2)
                {
                    if (musicPlayingList.Count > 0 && musicLikeList[0].SongId == musicPlayingList[0].SongId && musicPlayingList != null && LikeList.Items.Count == PlayingList.Items.Count)
                    {
                        sele = PlayingList.SelectedIndex = LikeList.SelectedIndex;
                        if (sele > -1) PlayingList.ScrollIntoView(PlayingList.Items[sele]);
                        return;
                    }
                    listInfo = musicLikeList;
                }
                else if (Interface() == 4)
                {
                    if (musicPlayingList.Count > 0 && localMusicList[0].SongName == musicPlayingList[0].SongName && musicPlayingList != null && LocalList.Items.Count == PlayingList.Items.Count)
                    {
                        sele = PlayingList.SelectedIndex = LocalList.SelectedIndex;
                        if (sele > -1) PlayingList.ScrollIntoView(PlayingList.Items[sele]);
                        return;
                    }
                    listInfo = localMusicList;
                }


                musicPlayingList.Clear();
                PlayingList.Items.Clear();
                foreach (MusicInfo musicInfo in listInfo)
                {
                    AddPlayingList(musicInfo);
                }
                PlayingList.SelectedIndex = sele;
                PlayingList.ScrollIntoView(PlayingList.Items[sele]);

                //添加到历史记录
                WriteLastPlayList();
            }
        }

        public string Likestr = "\ue68a";
        public string Nolikestr = "\ue7df";
        public void HotLike(int f, int like)
        {
            SolidColorBrush Color1 = new SolidColorBrush(Color.FromRgb(255, 100, 100));
            SolidColorBrush Color2 = new SolidColorBrush(Color.FromRgb(255, 154, 99));
            SolidColorBrush Color3 = new SolidColorBrush(Color.FromRgb(255, 214, 105));
            SolidColorBrush ButtonNormal = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            SolidColorBrush Colorok;
            if (f == 0) Colorok = Color1;
            else if (f == 1) Colorok = Color2;
            else if (f == 2) Colorok = Color3;
            else Colorok = ButtonNormal;

            MusicInfo musicInfo = musicTopList[f];

            string time = transformType.ToDateTime(Convert.ToInt32(musicInfo.Duration));
            string heart = Nolikestr;
            if (like == 1) heart = Likestr;

            Visibility VIP;
            if (musicInfo.Fee) { VIP = Visibility.Visible; }
            else VIP = Visibility.Hidden;

            Visibility MV;
            if (musicInfo.HasMV) MV = Visibility.Visible;
            else MV = Visibility.Hidden;

            Visibility lossless = new Visibility();
            if (musicInfo.HasLossless) lossless = Visibility.Visible;
            else lossless = Visibility.Hidden;
            int se = HotList.SelectedIndex;
            HotList.Items[f] = new
            {
                ID = (f + 1).ToString(),
                Song = musicInfo.SongName,
                Singer = musicInfo.Singer,
                Album = musicInfo.AlbumName,
                Duration = time,
                Lossless = lossless,
                MVShow = MV,
                VipVisi = VIP,
                IDBackground = Colorok,
                Heart = heart,
            };
            HotList.SelectedIndex = se;
        }

        string pauseui = "\ue6d7";
        string playui = "\ue6dd";
        private void SynPlayUIState(bool isPlay)
        {
            if (isPlay)
            {
                Play_B.Content = pauseui;//暂停UI
                PlayIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/暂停2.png", UriKind.RelativeOrAbsolute));
                MainWindow.Instance.lrcShowWindow.Play_B.Content = pauseui;//暂停UI
            }
            else
            {
                Play_B.Content = playui;//播放UI
                PlayIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/播放2.png", UriKind.RelativeOrAbsolute));
                MainWindow.Instance.lrcShowWindow.Play_B.Content = playui;//播放UI
            }
        }

        public int NowPlaying = 0;//正在播放的歌曲序号
        public void MVPlayer(string UrlPath)//网络播放器
        {
            if (!network.IsConnectedInternet()) { message.ShowMessage("无法访问网络，请检查网络是否连接"); return; }
            try
            {
                Network network1 = new Network();
                int num = SelectedIndex;
                string extension = Path.GetExtension(UrlPath);
                if (extension != ".mp4") UrlPath = network1.GetRealUrl(UrlPath).Replace("https", "http");
                play.Source = new Uri(UrlPath);
                BlurImg.Visibility = Visibility.Hidden;//隐藏专辑背景图
                SynPlayUIState(true);
                if (!Directory.Exists(Picpath())) Directory.CreateDirectory(Picpath());

                Heart.Content = Nolikestr;
                Heart.Opacity = 0.5;
                Heart.IsEnabled = false;
                string song = "", singer = "", pturl = "";
                LrcText = "";

                if (Interface() == 1)//搜索界面
                {
                    if (搜索引擎.SelectedIndex == 3) KGMusicAPI.KGSoSoUrlGet(num);
                    song = musicSearchList[num].SongName;
                    singer = musicSearchList[num].Singer;
                    pturl = musicSearchList[num].PTUrl;
                }
                if (Interface() == 5)//搜索界面
                {
                    song = musicSearchList[num].SongName;
                    singer = musicSearchList[num].Singer;
                    pturl = musicSearchList[num].PTUrl;
                }
                else if (Interface() == 0)//排行榜界面
                {
                    song = musicTopList[num].SongName;
                    singer = musicTopList[num].Singer;
                    pturl = musicTopList[num].PTUrl;
                }

                play.SetValue(Panel.ZIndexProperty, 0);
                SongShow.Text = song;
                SingerShow.Text = singer;
                SongName = song + " - " + singer;
                if (pturl.IsNullOrEmpty())
                {
                    pturl = "pack://application:,,,/Resource/Image/new Logo-alpha-2.png";
                }
                Cover.Source = BitmapFrame.Create(new Uri(pturl));
                play.Play();
                Time = null;
                TimerKeyer.tiemrs();
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
                //message.ShowMessage("播放MV出错！请重试");
            }
        }

        public void ShowNow()
        {
            int count = PlayingList.Items.Count;
            PlayingList.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                string id;
                if (i == NowPlaying) id = "";
                else id = (i + 1).ToString();
                string and = " - ";
                if (musicPlayingList[i].Singer == "") and = "";
                PlayingList.Items.Add(new
                {
                    ID = id,
                    Song = musicPlayingList[i].SongName + and + musicPlayingList[i].Singer
                });
            }
            PlayingList.SelectedIndex = NowPlaying;
            PlayingList.ScrollIntoView(PlayingList.Items[NowPlaying]);
        }

        public int SelectedIndex = 0;
        public void NetSongDouble()
        {
            try
            {
                if (Interface() == 0)
                {
                    if (HotList.Items.Count == 0 || SelectedIndex < 0) return;
                    SelectedIndex = NowPlaying = HotList.SelectedIndex;
                }
                else if (Interface() == 5)
                {
                    if (slRPage.SongList.Items.Count == 0) return;
                    SelectedIndex = NowPlaying = slRPage.SongList.SelectedIndex;
                }
                AllToPlayingList();
                ShowNow();
                PlayingListPlayer(NowPlaying);
            }
            catch (Exception e) { Logger.Instance.WriteException(e); message.ShowPopup("播放错误，请重试或报告BUG", false); }
        }

        int infa = -1;
        public int Interface()
        {
            if (HotGrid.IsMouseOver) infa = 0;
            else if (srPage.IsMouseOver) infa = 1;
            else if (slRPage.IsMouseOver) infa = 5;
            else if (LikeGrid.IsMouseOver) infa = 2;
            else if (DownGrid.IsMouseOver) infa = 3;
            else if (LocalGrid.IsMouseOver) infa = 4;
            else if (播放列表.IsMouseOver) infa = 6;
            //message.DebugOut(infa.ToString());
            return infa;
        }

        #endregion

        #region 读写配置文件

        //配置文件
        bool isFirstStart = true;
        IniHelper iniHelper = new IniHelper("config.ini");
        string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        public void WriteConfig()
        {
            //是否开机启动
            if ((bool)Start.IsChecked) { iniHelper.WriteValue("General", "Starting up", "1"); }
            else { iniHelper.WriteValue("General", "Starting up", "0"); }
            SelfRunning((bool)Start.IsChecked);

            //最小化到托盘
            iniHelper.WriteValue("General", "NotifyIcon", NotifyIcon.IsChecked.ToString());
            iniHelper.WriteValue("General", "isFirstStart", isFirstStart.ToString());

            //播放启动音乐
            if ((bool)StartMusic.IsChecked) iniHelper.WriteValue("General", "StartMusic", "1");
            else iniHelper.WriteValue("General", "StartMusic", "0");
            //默认启动页
            iniHelper.WriteValue("General", "StartPage", StartPage.SelectedIndex.ToString());
            iniHelper.WriteValue("General", "DefaultEngine", DefaultEngine.SelectedIndex.ToString());

            iniHelper.WriteValue("Display", "DefLrcFontSize", Prog_Lrc1.Value.ToString());
            iniHelper.WriteValue("Display", "FocLrcFontSize", Prog_Lrc2.Value.ToString());
            iniHelper.WriteValue("Display", "DesLrcFontSize", Prog_Lrc3.Value.ToString());
            iniHelper.WriteValue("Display", "DesLrcBGOpacity", Prog_Lrc4.Value.ToString());

            //下载地址
            folderPaths.Replace("##", "#");
            //message.ShowPopup(folderPaths);
            if (folderPaths == "") iniHelper.WriteValue("DownSet", "FolderPath", SavePath.Text);
            else iniHelper.WriteValue("DownSet", "FolderPath", folderPaths);

            iniHelper.WriteValue("DownSet", "SavePath", SavePath.Text);

            iniHelper.WriteValue("DownSet", "CachePath()", CachePath_box.Text);
            //下载音质
            if (Quality.SelectedIndex == 0) { iniHelper.WriteValue("DownSet", "Quality", "128"); }
            else if (Quality.SelectedIndex == 1) { iniHelper.WriteValue("DownSet", "Quality", "320"); }
            else if (Quality.SelectedIndex == 2) { iniHelper.WriteValue("DownSet", "Quality", "ape"); }

            //是否下载图片
            iniHelper.WriteValue("DownSet", "isDownPic", isDownPic.IsChecked.ToString());
            iniHelper.WriteValue("DownSet", "isDownLrc", isDownLrc.IsChecked.ToString());
            iniHelper.WriteValue("DownSet", "isShowPeak", isShowPeak.IsChecked.ToString());
            iniHelper.WriteValue("KeySet", "isEnabledKey", isEnabledKey.IsChecked.ToString());

            //默认设置值
            iniHelper.WriteValue("DefaultSet", "Volume", Volume.Value.ToString());
            iniHelper.WriteValue("DefaultSet", "NowPlaying", NowPlaying.ToString());

            //循环
            iniHelper.WriteValue("DefaultSet", "Cycle", Cycle.Content.ToString());

            //提示信息
            iniHelper.WriteValue("DefaultSet", "EnginTips", EnginTips.Visibility.ToString());
            iniHelper.WriteValue("DefaultSet", "URLTips", URLTips.Visibility.ToString());

            //桌面尺寸
            string size = $"{Width},{Height},{Top},{Left}";
            iniHelper.WriteValue("DefaultSet", "Size", size);
        }

        ///isStart--是否开机自启动
        ///exeName--应用程序名
        ///path--应用程序路径
        public bool SelfRunning(bool isStart)
        {
            try
            {
                //message.ShowMessage("准备设置");
                string exeName = "魔音Morin", path = GetType().Assembly.Location;
                RegistryKey local = Registry.CurrentUser;
                RegistryKey key = local.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key == null)
                {
                    local.CreateSubKey("SOFTWARE//Microsoft//Windows//CurrentVersion//Run");
                }
                if (isStart)//若开机自启动则添加键值对
                {
                    string[] keyNames = key.GetValueNames();
                    foreach (string keyName in keyNames)
                    {
                        if (keyName.ToUpper() == exeName.ToUpper())
                        {
                            key.Close();
                            return false;
                        }
                    }
                    key.SetValue(exeName, path);
                    key.Close();
                }
                else//否则删除键值对
                {
                    string[] keyNames = key.GetValueNames();
                    foreach (string keyName in keyNames)
                    {
                        if (keyName.ToUpper() == exeName.ToUpper())
                        {
                            key.DeleteValue(exeName);
                            key.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteException(ex);
                return false;
            }
            return true;
        }

        public void ReadConfig()
        {
            try
            {
                EnginTips.Visibility = Visibility.Visible;
                URLTips.Visibility = Visibility.Visible;
                //如果没有配置文件
                if (!File.Exists(configPath))
                {
                    DefaultSet();
                    return;
                }
                //是否开机启动
                string s1 = iniHelper.ReadValue("General", "Starting up");
                if (s1 == "1") Start.IsChecked = true;
                else Start.IsChecked = false;
                //启动播放推荐音乐
                string s2 = iniHelper.ReadValue("General", "StartPlay");

                //最小化到托盘
                bool notiBo = Convert.ToBoolean(iniHelper.ReadValue("General", "NotifyIcon").ToString());
                NotifyIcon.IsChecked = notiBo;
                isFirstStart = Convert.ToBoolean(iniHelper.ReadValue("General", "isFirstStart").ToString());

                //播放启动音乐
                string s3 = iniHelper.ReadValue("General", "StartMusic");
                if (s3 == "1") StartMusic.IsChecked = true;
                else StartMusic.IsChecked = false;

                //默认启动页
                int s4 = Convert.ToInt32(iniHelper.ReadValue("General", "StartPage"));
                StartPage.SelectedIndex = s4;
                Tab.SelectedIndex = s4;

                Prog_Lrc1.Value = Convert.ToInt32(iniHelper.ReadValue("Display", "DefLrcFontSize"));
                Prog_Lrc2.Value = Convert.ToInt32(iniHelper.ReadValue("Display", "FocLrcFontSize"));
                Prog_Lrc3.Value = Convert.ToInt32(iniHelper.ReadValue("Display", "DesLrcFontSize"));
                Prog_Lrc4.Value = Convert.ToInt32(iniHelper.ReadValue("Display", "DesLrcBGOpacity"));



                //默认引擎
                int s5 = Convert.ToInt32(iniHelper.ReadValue("General", "DefaultEngine"));
                DefaultEngine.SelectedIndex = s5;

                //下载地址
                string savePath = iniHelper.ReadValue("DownSet", "SavePath");
                if (savePath == "" || savePath == null)
                {
                    //设置下载目录
                    string path = @"C:\Users\" + Environment.UserName + "\\Music\\";
                    if (!Directory.Exists(path))
                    {
                        if (Directory.Exists(@"D:\")) path = "D:\\Morin\\";
                        else path = "C:\\Morin\\";
                    }
                    SavePath.Text = path;
                }
                else SavePath.Text = savePath;



                folderPaths = iniHelper.ReadValue("DownSet", "FolderPath");
                //string[] paths = folderPath.Split('#');
                //if (folderPath != "") {
                //    folderPaths
                //}
                //else if(paths.Length > 0)
                // else if(savePath != "" && !savePath.Contains("#")) SavePath.Text = savePath;
                // else SavePath.Text = paths[0];


                //下载音质
                string Q = iniHelper.ReadValue("DownSet", "Quality");
                if (Q == "128") Quality.SelectedIndex = 0;
                else if (Q == "320") Quality.SelectedIndex = 1;
                else if (Q == "ape") Quality.SelectedIndex = 2;

                isDownPic.IsChecked = Convert.ToBoolean(iniHelper.ReadValue("DownSet", "isDownPic"));
                isDownLrc.IsChecked = Convert.ToBoolean(iniHelper.ReadValue("DownSet", "isDownLrc"));
                isShowPeak.IsChecked = Convert.ToBoolean(iniHelper.ReadValue("DownSet", "isShowPeak"));

                //循环
                string cy = iniHelper.ReadValue("DefaultSet", "Cycle");
                if (cy == "") Cycle.Content = "";
                else Cycle.Content = cy;
                switch (cy)
                {
                    case "":
                        Cycle.ToolTip = "列表播放";
                        break;
                    case "":
                        Cycle.ToolTip = "列表循环";
                        break;
                    case "":
                        Cycle.ToolTip = "单曲循环";
                        break;
                    case "":
                        Cycle.ToolTip = "随机播放";
                        break;
                }
                //默认设置值
                NowPlaying = Convert.ToInt16(iniHelper.ReadValue("DefaultSet", "NowPlaying"));
                //音量
                string vol = iniHelper.ReadValue("DefaultSet", "Volume");
                if (vol == "") vol = "0.5";
                Volume.Value = Convert.ToDouble(vol);
                //主题
                string tm = iniHelper.ReadValue("DefaultSet", "Theme");
                if (tm != "") Theme(tm);
                else Theme("White");
                //提示信息
                EnginTips.Visibility = (iniHelper.ReadValue("DefaultSet", "EnginTips") == "Hidden") ? Visibility.Hidden : Visibility.Visible;
                URLTips.Visibility = (iniHelper.ReadValue("DefaultSet", "URLTips") == "Hidden") ? Visibility.Hidden : Visibility.Visible;

                string[] size = iniHelper.ReadValue("DefaultSet", "Size").Split(',');
                Width = Convert.ToDouble(size[0]);
                Height = Convert.ToDouble(size[1]);
                Top = Convert.ToDouble(size[2]);
                Left = Convert.ToDouble(size[3]);

                Prog_Lrc5.Maximum = SystemParameters.WorkArea.Width; // 屏幕工作区域宽度
                Prog_Lrc6.Maximum = SystemParameters.WorkArea.Height; // 屏幕工作区域高度

                Prog_Lrc5.Value = Width;
                Prog_Lrc6.Value = Height;

                CachePath_box.Text = iniHelper.ReadValue("DownSet", "CachePath()");
                if (CachePath_box.Text == "") CachePath_box.Text = AppPath + "Cache\\";
                isEnabledKey.IsChecked = Convert.ToBoolean(iniHelper.ReadValue("KeySet", "isEnabledKey"));
            }
            catch (Exception ex)
            {
                //设置缓存目录
                CachePath_box.Text = AppPath + "Cache\\";

                //设置屏幕大小
                double PWidth = SystemParameters.PrimaryScreenWidth; // 屏幕宽度
                double PHeight = SystemParameters.PrimaryScreenHeight; // 屏幕高度
                double workWidth = SystemParameters.WorkArea.Width; // 屏幕工作区域宽度
                double workHeight = SystemParameters.WorkArea.Height; // 屏幕工作区域高度
                Width = PWidth - 620;
                Height = PHeight - 340;
                Left = (workWidth - Width) / 2;
                Top = (workHeight - Height) / 2;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                Prog_Lrc5.Maximum = SystemParameters.WorkArea.Width; // 屏幕工作区域宽度
                Prog_Lrc6.Maximum = SystemParameters.WorkArea.Height; // 屏幕工作区域高度
                Prog_Lrc5.Value = Width;
                Prog_Lrc6.Value = Height;

                Logger.Instance.WriteException(ex);
            }
        }

        public void DefaultSet()
        {
            try
            {
                Start.IsChecked = false;
                StartMusic.IsChecked = true;
                NotifyIcon.IsChecked = false;
                StartPage.SelectedIndex = 0;
                DefaultEngine.SelectedIndex = 2;

                Volume.Value = Convert.ToDouble(0.5);
                Theme("White");

                //设置下载目录
                string path = @"C:\Users\" + Environment.UserName + "\\Music\\";
                if (!Directory.Exists(path))
                {
                    if (Directory.Exists(@"D:\")) path = "D:\\Morin\\";
                    else path = "C:\\Morin\\";
                }
                SavePath.Text = path;
                //设置缓存目录
                CachePath_box.Text = AppPath + "Cache\\";

                //设置屏幕大小
                double PWidth = SystemParameters.PrimaryScreenWidth; // 屏幕宽度
                double PHeight = SystemParameters.PrimaryScreenHeight; // 屏幕高度
                double workWidth = SystemParameters.WorkArea.Width; // 屏幕工作区域宽度
                double workHeight = SystemParameters.WorkArea.Height; // 屏幕工作区域高度
                Width = PWidth - 620;
                Height = PHeight - 340;
                Left = (workWidth - Width) / 2;
                Top = (workHeight - Height) / 2;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                Prog_Lrc5.Maximum = SystemParameters.WorkArea.Width; // 屏幕工作区域宽度
                Prog_Lrc6.Maximum = SystemParameters.WorkArea.Height; // 屏幕工作区域高度
                Prog_Lrc5.Value = Width;
                Prog_Lrc6.Value = Height;
            }
            catch (Exception e) { Logger.Instance.WriteException(e); }
        }

        #endregion

        #region Theme

        //皮肤更换
        string sTheme = "White";
        public void Theme(string source)
        {
            sTheme = source;
            //if (source == "White") Theme_btn.Content = "暗";
            //else Theme_btn.Content = "明";

            this.Focusable = false;
            var skinDictUri = new Uri("./Theme/" + source + ".xaml", UriKind.Relative);//【X2.提供样式样本】 
            var skinDict = Application.LoadComponent(skinDictUri) as ResourceDictionary;
            var mergedDicts = Resources.MergedDictionaries;
            mergedDicts.Clear();//清空之前样式
            mergedDicts.Add(skinDict);//添加当前需要的样式 
            ReForeground();
            if (((TabItem)Tab.SelectedItem).Name == "本地音乐") AddAllPath();
        }

        public void ReForeground()
        {
            foreach (Button btn in FindVisualChildren<Button>(window))
            {
                Brush brush = Resources["ThemeColor"] as Brush;
                Brush brush2 = Resources["Black-01"] as Brush;
                Brush brush3 = Resources["White-01"] as Brush;
                string tb_s = btn.Foreground.ToString();
                if (tb_s == brush.ToString() || tb_s == brush2.ToString()) { }//|| tb_s == brush3.ToString()) { }
                else
                {
                    btn.Foreground = Resources["ButtonNormal"] as Brush;
                    btn.BorderBrush = Resources["AllBackground2"] as Brush;
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        #endregion

        #region 配置文件

        //全局快捷键
        public void AllKeyboard()
        {
            if ((bool)isEnabledKey.IsChecked) return;
            HotKey hotKey = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL, System.Windows.Forms.Keys.Left);
            hotKey.OnHotKey += new HotKey.OnHotKeyEventHandler(LastSong);

            HotKey hotKey2 = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL, System.Windows.Forms.Keys.Right);
            hotKey2.OnHotKey += new HotKey.OnHotKeyEventHandler(NextSong);

            HotKey hotKey3 = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL, System.Windows.Forms.Keys.B);
            hotKey3.OnHotKey += new HotKey.OnHotKeyEventHandler(Pause);

            HotKey hotKey4 = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL, System.Windows.Forms.Keys.L);
            hotKey4.OnHotKey += new HotKey.OnHotKeyEventHandler(LrcShow);

            HotKey hotKey5 = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL, System.Windows.Forms.Keys.Up);
            hotKey5.OnHotKey += new HotKey.OnHotKeyEventHandler(() =>
            {
                Volume.Value += 0.01;
            });
            HotKey hotKey6 = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL, System.Windows.Forms.Keys.Down);
            hotKey6.OnHotKey += new HotKey.OnHotKeyEventHandler(() =>
            {
                Volume.Value -= 0.01;
            });


            HotKey hotKey7 = new HotKey(this, System.Windows.Forms.Keys.MediaPlayPause);
            hotKey7.OnHotKey += new HotKey.OnHotKeyEventHandler(() =>
            {
                Play_B_Click(null, null);
            });
            HotKey hotKey8 = new HotKey(this, System.Windows.Forms.Keys.MediaNextTrack);
            hotKey8.OnHotKey += new HotKey.OnHotKeyEventHandler(() =>
            {
                NextSong();
            });
            HotKey hotKey9 = new HotKey(this, System.Windows.Forms.Keys.MediaPreviousTrack);
            hotKey9.OnHotKey += new HotKey.OnHotKeyEventHandler(() =>
            {
                LastSong();
            });
        }

        //本地路径
        private void AddAllPath()
        {
            path_Panel.Children.Clear();
            string savePath = iniHelper.ReadValue("DownSet", "FolderPath");
            if (savePath != "" && !savePath.Contains("#"))
            {
                AddLocalPath(SavePath.Text);
                ((RadioButton)path_Panel.Children[0]).IsChecked = true;
                return;
            }
            string[] paths = savePath.Split('#');
            if (paths.Length == 0) return;
            folderPaths = savePath;
            //currentPath = paths[0];
            foreach (string p in paths)
            {
                if (p != "") AddLocalPath(p);
            }
            if (path_Panel.Children.Count > 0) ((RadioButton)path_Panel.Children[0]).IsChecked = true;
        }

        private void AddLocalPath(string name)
        {
            SolidColorBrush ThemeColor = FindResource("ThemeColor") as SolidColorBrush;
            SolidColorBrush ButtonNormal = FindResource("ButtonNormal") as SolidColorBrush;
            SolidColorBrush LeftTabNormal = FindResource("AllBackground") as SolidColorBrush;

            RadioButton radio = new RadioButton
            {
                Margin = new Thickness(4),
                Padding = new Thickness(5, 8, 5, 8),
                Background = LeftTabNormal,
                Height = 40,
                Foreground = ButtonNormal,
                FontSize = 12,
                ToolTip = name,
                Content = name,
                //Name = "PathRadio" + path_Panel.Children.Count.ToString()
            };
            radio.Checked += Radio_Path_Checked;

            RadioButtonHelper.SetRadioButtonStyle(radio, RadioButtonStyle.Button);


            #endregion



        }

        private void Radio_Path_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            currentPath = radio.Content.ToString();
            LocalListImprt(currentPath);
        }

        //本地导入
        public void LocalListImprt(string path)
        {
            if (currentPath == "") return;
            LocalCount.Content = " 0首";
            try
            {
                //列表清空
                if (LocalList.Items.Count > 0) LocalList.Items.Clear();
                localMusicList.Clear();
                LoadLocalList(path);
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
                message.ShowPopup("本地音乐刷新有误！尝试更改下载目录", false);
                return;
            }
        }

        //加载本地
        public void LoadLocalList(string path)
        {
            FileInfo[] names;
            DirectoryInfo[] directoryInfos;
            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists) { return; }
            names = directory.GetFiles();
            directoryInfos = directory.GetDirectories();
            if (names.Length == 0) return;
            FileComparer fc = new FileComparer();
            Array.Sort(names, fc);//文件按时间排序
            foreach (var name in names)
            {
                LocalListAdd(name.FullName);
            }

            LocalCount.Content = " " + LocalList.Items.Count + "首";
            if (LocalList.Items.Count == 0) { Local_null.Visibility = Visibility.Visible; LocalList.Visibility = Visibility.Hidden; return; }
            else { Local_null.Visibility = Visibility.Hidden; LocalList.Visibility = Visibility.Visible; }

            LocalList.ScrollIntoView(LocalList.Items[0]);
        }

        //本地列表 ADD
        public void LocalListAdd(string filestr)
        {

            FileInfo name = new FileInfo(filestr);
            int i = LocalList.Items.Count;
            string ext = name.Extension;
            string data = name.CreationTime.ToString("yyyy-MM-dd");
            if (ext == ".m4a" || ext == ".mp3" || ext == ".flac" || ext == ".ape")
            {
                MusicInfo musicInfo = new MusicInfo();
                musicInfo.SongName = name.Name.Replace(ext, "");
                musicInfo.Singer = "";
                musicInfo.Ext = ext.Replace(".", "");
                musicInfo.Data = data;
                musicInfo.Path = name.FullName;
                musicInfo.PTUrl = "";
                musicInfo.MP3Url = name.FullName;
                musicInfo.From = "本地音乐";
                musicInfo.Suffix = ext.Replace(".", "");
                localMusicList.Add(musicInfo);

                LocalList.Items.Add(new
                {
                    ID = (LocalList.Items.Count + 1).ToString(),
                    Song = musicInfo.SongName,
                    Data = musicInfo.Data,
                    Ext = musicInfo.Ext,
                });
            }
        }

        #region GC

        //内存释放
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        public void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        #endregion

        public class FileComparer : IComparer//文件按时间排序
        {
            int IComparer.Compare(Object o1, Object o2)
            {
                FileInfo fi1 = o1 as FileInfo;
                FileInfo fi2 = o2 as FileInfo;
                return fi2.CreationTime.CompareTo(fi1.CreationTime);
            }
>>>>>>> bc8fab203afe35b34ef6038e5f17a8751cf4e46a
        }
    }
}

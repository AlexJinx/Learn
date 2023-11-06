using Morin.App.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Morin.App.Common.Code
{
    public class Network
    {
        MainWindow main = MainWindow.Instance;
        KGMusicAPI KGMusicAPI = new KGMusicAPI();
        Message message = new Message();


        public string JSON(string url)//获取URL地址JSON编码
        {
            main = MainWindow.Instance;
            // if (!IsConnectedInternet()) { main.message.ShowMessage("无法访问网络，请检查网络是否连接"); return ""; }
            string t = null;
            try
            {
                //ThreadPool.QueueUserWorkItem((object state) =>
                //{

                WebRequest webrequest = WebRequest.Create(url);
                //设置用于对Internet资源请求进行身份验证的网络凭据
                webrequest.Method = "GET";
                SetHeaderValue(webrequest.Headers, "Cookie", "_ga=GA1.2.1867555549.1577866116; _gid=GA1.2.897821451.1578686661; Hm_lvt_cdb524f42f0ce19b169a8071123a4797=1578332907,1578475756,1578545602,1578686661; _gat=1; Hm_lpvt_cdb524f42f0ce19b169a8071123a4797=1578688764; kw_token=N25WESU3O5");
                SetHeaderValue(webrequest.Headers, "CSRF", "N25WESU3O5");
                SetHeaderValue(webrequest.Headers, "Referer", "http://www.kuwo.cn/rankList");
                //webrequest.Timeout = 3000;
                webrequest.Credentials = CredentialCache.DefaultCredentials;
                WebResponse webresponse = webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                StreamReader sreader = new StreamReader(stream);
                t = sreader.ReadToEnd();
                sreader.Close();
                stream.Close();
                webresponse.Close();
                //Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                //{

                //}));
                //}, null);

            }
            catch (Exception ex)
            {
                message.DebugOut(ex.Message);
            }
            return t;
        }

        public string QQJSON(string u)//获取URL地址JSON编码
        {
            main = MainWindow.Instance;
            // if (!IsConnectedInternet()) { main.message.ShowMessage("无法访问网络，请检查网络是否连接"); return ""; }
            string t = null;
            try
            {
                WebRequest webrequest = WebRequest.Create(u);
                //设置用于对Internet资源请求进行身份验证的网络凭据
                webrequest.Method = "GET";
                SetHeaderValue(webrequest.Headers, "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36");
                //webrequest.Timeout = 3000;
                webrequest.Credentials = CredentialCache.DefaultCredentials;
                WebResponse webresponse = webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                StreamReader sreader = new StreamReader(stream);
                t = sreader.ReadToEnd();
                sreader.Close();
                stream.Close();
                webresponse.Close();
            }
            catch (Exception)
            {
                main.LoadShow.IsRunning = false;
            }
            return t;
        }

        public string QQLrcJson(string u)//获取URL地址JSON编码
        {
            string t = null;
            try
            {
                // string str = string.Empty;//存储信息
                //实例化一个WebRequest对象
                WebRequest webrequest = WebRequest.Create(u);
                webrequest.Method = "GET";
                SetHeaderValue(webrequest.Headers, "Referer", "https://y.qq.com/portal/1player.html");
                SetHeaderValue(webrequest.Headers, "Cookie", "skey=@LVJPZmJUX;p");
                //webrequest.Timeout = 3000;
                ////设置用于对Internet资源请求进行身份验证的网络凭据
                webrequest.Credentials = CredentialCache.DefaultCredentials;
                WebResponse webresponse = webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                //使用创建的Stream对象创建一个StreamReader流读取对象
                StreamReader sreader = new StreamReader(stream);
                //读取流中的内容，并显示在RichTestBox控件中
                t = sreader.ReadToEnd().Replace("MusicJsonCallback(", "").Replace(")", "");
                t = t.Replace("playlistinfoCallback(", "").Replace(")", "");
                sreader.Close();
                stream.Close();
                webresponse.Close();
            }
            catch (Exception)
            {
                main.LoadShow.IsRunning = false;
            }
            return t;
        }

        public string NTCookieJson(string u)//获取URL地址JSON编码
        {
            string t = null;
            try
            {
                // string str = string.Empty;//存储信息
                //实例化一个WebRequest对象
                WebRequest webrequest = WebRequest.Create(u);
                webrequest.Method = "GET";
                //SetHeaderValue(webrequest.Headers, "Referer", "http://music.163.com");
                //SetHeaderValue(webrequest.Headers, "Cookie", "os=pc; osver=Microsoft-Windows-10-Professional-build-10586-64bit; appver=2.0.2; channel=netease; __remember_me=true");
                //webrequest.Timeout = 3000;
                ////设置用于对Internet资源请求进行身份验证的网络凭据
                webrequest.Credentials = CredentialCache.DefaultCredentials;
                WebResponse webresponse = webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                //使用创建的Stream对象创建一个StreamReader流读取对象
                StreamReader sreader = new StreamReader(stream);
                //读取流中的内容，并显示在RichTestBox控件中
                t = sreader.ReadToEnd().Replace("MusicJsonCallback(", "").Replace(")", "");
                sreader.Close();
                stream.Close();
                webresponse.Close();
            }
            catch (Exception)
            {
                main.LoadShow.IsRunning = false;
            }
            return t;
        }

        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        public string GetRealUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return url;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                ServicePointManager.Expect100Continue = false;

                ((HttpWebResponse)request.GetResponse()).Close();
                return request.Address.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async void PicDawn(string t1, string t2)
        {
            try
            {
                await Task.Run(() =>
                {
                    main = MainWindow.Instance;
                    if (!IsConnectedInternet()) { main.message.ShowPopup("无法访问网络，请检查网络是否连接", false); return; }
                    t2.Replace(":", "-");
                    if (t1 != "" && t2 != "")
                    {
                        if (File.Exists(t2)) return;
                        WebRequest request = WebRequest.Create(t1);
                        //request.Timeout = 1000;
                        WebResponse respone = request.GetResponse();
                        ThreadPool.QueueUserWorkItem((obj) =>
                        {
                            try
                            {
                                Stream netStream = respone.GetResponseStream();
                                Stream fileStream = new FileStream(t2, FileMode.Create);
                                byte[] read = new byte[1024];
                                long progressBarValue = 0;
                                int realReadLen = netStream.Read(read, 0, read.Length);
                                while (realReadLen > 0)
                                {
                                    fileStream.Write(read, 0, realReadLen);
                                    progressBarValue += realReadLen;
                                    //pbDown.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                                    realReadLen = netStream.Read(read, 0, read.Length);
                                }
                                netStream.Close();
                                fileStream.Close();
                            }
                            catch
                            {
                            }
                        }, null);
                    }
                });
            }
            catch (Exception)
            {

            }
        }

        public int DownID = 0;
        public int SongDawn(string t1, string t2)
        {
            try
            {
                t2 = t2.Replace("?", "？");
                main = MainWindow.Instance;
                if (!IsConnectedInternet()) { main.message.ShowPopup("无法访问网络，请检查网络是否连接", false); return 0; }
                if (t1 != null && t2 != null)
                {
                    if (File.Exists(t2)) return 0;
                    WebRequest request = WebRequest.Create(t1);
                    //request.Timeout = 3000;
                    WebResponse respone = request.GetResponse();
                    ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        Stream netStream = respone.GetResponseStream();
                        Stream fileStream = new FileStream(t2, FileMode.Create);
                        byte[] read = new byte[1024];
                        long progressBarValue = 0;
                        int realReadLen = netStream.Read(read, 0, read.Length);
                        while (realReadLen > 0)
                        {
                            fileStream.Write(read, 0, realReadLen);
                            progressBarValue += realReadLen;
                            realReadLen = netStream.Read(read, 0, read.Length);
                            //进度信息传递给列表
                            MusicInfo musicInfo = main.musicDownList[Downing];
                            musicInfo.progress = (int)(((double)progressBarValue / (double)respone.ContentLength) * 100);
                            main.musicDownList[Downing] = musicInfo;
                        }
                        netStream.Close();
                        fileStream.Close();
                    }, null);
                    //MainWindow.Instance.ShowPopup(DownPercent.ToString());

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        public int CacheSongDownPe = 0;
        public int CacheSongDawn(string t1, string t2)
        {
            main = MainWindow.Instance;
            if (main.isShowPeak.IsChecked == false) return 0;
            try
            {
                if (t1 != null && t2 != null)
                {
                    WebRequest request = WebRequest.Create(t1);
                    WebResponse respone = request.GetResponse();
                    ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        try
                        {
                            Stream netStream = respone.GetResponseStream();
                            Stream fileStream = new FileStream(t2, FileMode.Create);
                            byte[] read = new byte[1024];
                            long progressBarValue = 0;
                            int realReadLen = netStream.Read(read, 0, read.Length);
                            while (realReadLen > 0)
                            {
                                fileStream.Write(read, 0, realReadLen);
                                progressBarValue += realReadLen;
                                realReadLen = netStream.Read(read, 0, read.Length);
                                CacheSongDownPe = (int)(((double)progressBarValue / (double)respone.ContentLength) * 100);
                            }
                            netStream.Close();
                            fileStream.Close();
                        }
                        catch { }
                    }, null);
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 1;
        }

        string Suffix = "mp3", Br = "";
        public void DownSongToList()
        {
            //try
            {
                QQMusicAPI QQMusicAPI = new QQMusicAPI();
                var main = MainWindow.Instance;
                string DownUrl = "", SongPath = "", PicPath = "", Song = "", Album = "", Singer = "", Time = "", from = "", id = "", lrcurl = "";
                Suffix = "mp3";
                MusicInfo musicInfo;
                //message.DebugOut(main.Interface().ToString());
                if (main.Interface() == 0)
                {
                    int i = main.HotList.SelectedIndex;
                    if (i == -1) return;
                    musicInfo = main.musicTopList[i];
                    DownUrl = musicInfo.mp3url;
                }
                else if (main.Interface() == 2)
                {
                    int i = main.LikeList.SelectedIndex;
                    if (i == -1) return;
                    musicInfo = main.musicLikeList[i];
                }
                else if (main.Interface() == 6)
                {
                    int i = main.PlayingList.SelectedIndex;
                    if (i == -1) return;
                    musicInfo = main.musicPlayingList[i];

                    string fromx = musicInfo.from;
                    if (fromx == "QQ音乐") DownUrl = QQMusicAPI.QQMusicUrlGet(musicInfo.songid);
                    else if (fromx == "网易云音乐") DownUrl = musicInfo.mp3url;
                    else if (fromx == "酷我音乐") DownUrl = JSON(musicInfo.mp3url);
                    else if (fromx == "酷狗音乐")
                    {
                        musicInfo = KGMusicAPI.KGSoSoUrlGet(main.SelectedIndex);
                        DownUrl = musicInfo.mp3url;
                    }
                }
                else//搜索列表或者歌单列表
                {
                    if (main.Interface() == 1) main.SelectedIndex = main.srPage.SearchList.SelectedIndex;
                    else if (main.Interface() == 5) main.SelectedIndex = main.slRPage.SongList.SelectedIndex;
                    musicInfo = main.musicSearchList[main.SelectedIndex];


                    string fromx = musicInfo.from;
                    if (fromx == "QQ音乐") DownUrl = QQMusicAPI.QQMusicUrlGet(musicInfo.songid);
                    else if (fromx == "网易云音乐") DownUrl = musicInfo.mp3url;
                    else if (fromx == "酷我音乐") DownUrl = JSON(musicInfo.mp3url);
                    else if (fromx == "酷狗音乐")
                    {
                        musicInfo = KGMusicAPI.KGSoSoUrlGet(main.SelectedIndex);
                        DownUrl = musicInfo.mp3url;
                    }

                }


                if (musicInfo.fee)
                {
                    main.WuBanQuan();
                    return;
                }
                //DownUrl = musicInfo.mp3url;
                string DownPT = musicInfo.pturl;
                Song = musicInfo.songname;
                Singer = musicInfo.singer;
                Time = musicInfo.duration;
                id = musicInfo.songid;
                from = musicInfo.from;
                Album = musicInfo.albumname;
                lrcurl = musicInfo.lrcurl;


                if (from == "酷我音乐" && id != "")
                {
                    DownUrl = DownQuality(id); lrcurl = id;
                }
                if (from == "QQ音乐")
                {
                    Suffix = "m4a";
                    Br = "96";
                }
                //else if(musicInfo.hasLossless)

                if (isExistDownList(musicInfo) != -1) return;
                string Name = Song + " - " + Singer;
                SongPath = main.SavePath.Text + Name.Replace(":", "") + "." + Suffix;
                PicPath = main.Picpath() + Name + ".jpg";

                if (!Directory.Exists(main.SavePath.Text)) Directory.CreateDirectory(main.SavePath.Text);
                if (File.Exists(SongPath))
                {
                    main.message.ShowPopup(" 已经下载过这首音乐了...", true);
                    main.LocalList.SelectedIndex = main.isExistLocalList(SongPath);
                    main.Tab.SelectedIndex = 4;
                    main.SongLoadShow.IsRunning = false;
                    return;
                } //Contrast(Name);


                int DawnListCount = main.DownList.Items.Count;
                MusicInfo downInfo = new MusicInfo()
                {
                    SongId = id,
                    SongName = Name,
                    MP3Url = DownUrl,
                    PTUrl = DownPT,
                    LrcUrl = lrcurl,
                    SongPath = SongPath,
                    PicPath = PicPath,
                    Singer = Singer,
                    Duration = Time,
                    From = from,
                    Suffix = Suffix,
                    AlbumName = Album,
                    State = "等待",
                    Quality = Br,
                };
                main.musicDownList.Add(downInfo);

                MainWindow.Instance.DownList.Items.Add(new
                {
                    ID = DawnListCount + 1,
                    ProValue = 0,
                    Song,
                    Singer,
                    Album,
                    Quality = Br,
                    From = from,
                    State = "等待"
                });

                if (main.isDownLrc.IsChecked == true)
                {
                    string LrcText = "";
                    if (from == "QQ音乐")
                    {
                        LrcText = main.QQAPI.QQGetLrc(lrcurl);
                    }
                    else if (from == "网易云音乐")
                    {
                        LrcText = main.NTMusicAPI.NTGetLrc(lrcurl);
                    }
                    else if (from == "酷我音乐")
                    {
                        LrcText = main.KWMusicAPI.GetLrc(lrcurl);
                    }
                    else if (from == "酷狗音乐")
                    {
                        LrcText = lrcurl;
                    }
                    main.SaveLrc(LrcText, Name);
                }
                if (main.isDownPic.IsChecked == true) PicDawn(DownPT, PicPath);
                DownTiemrs();
            }
        }

        //下载列表比对是否重复
        public int isExistDownList(MusicInfo musicInfo)
        {
            var main = MainWindow.Instance;
            for (int i = 0; i < main.DownList.Items.Count; i++)
            {
                if (main.musicDownList[i].songid == musicInfo.SongId)
                {
                    main.message.ShowPopup("已经在下载列表啦", false);
                    return i;
                }
            }
            return -1;
        }

        public int DownList(int i)
        {
            if (i < 0)
            {
                return 0;
            }
            var main = MainWindow.Instance;

            string DownUrl = main.musicDownList[i].mp3url;
            string SongPath = main.musicDownList[i].songPath;
            return SongDawn(DownUrl, SongPath);
        }

        //下载进度条定时器
        public DispatcherTimer Downtimer = null;
        public int Downing = 0;
        public int isDown = 0;
        string 下载中 = "";
        string 等待 = "";
        string 完成 = "";
        public void DownTimer_tick(object sender, EventArgs e)
        {
            //try
            {
                var main = MainWindow.Instance;
                int count = main.DownList.Items.Count;

                if (Downing > count - 1) return;
                if (isDown == 0)
                {
                    if (main.musicDownList[Downing].state == 等待)
                    {
                        if (DownList(Downing) == 0) isDown = 0;
                        else isDown = 1;
                    }
                }
                DownState(Downing, 下载中);
                main.DownCount.Visibility = Visibility.Visible;
                main.DownCount.Text = (count - Downing).ToString();
                if (main.musicDownList[Downing].progress == 100)
                {
                    isDown = 0;
                    DownState(Downing, 完成);
                    main.LocalListImprt(main.SavePath.Text);
                    if (Downing < count) Downing++;

                    if (Downing > count - 1)
                    {
                        main.DownCount.Visibility = Visibility.Hidden;
                        //main.message.ShowPopup("全部下载任务已完成");
                        Downtimer.Stop();
                    }
                }
            }
            //catch {}
        }

        public void DownState(int d, string s)
        {
            var main = MainWindow.Instance;
            MusicInfo musicInfo = main.musicDownList[d];
            main.DownList.Items[d] = new
            {
                ID = Downing + 1,
                ProValue = musicInfo.Progress,
                Song = musicInfo.SongName,
                Singer = musicInfo.Singer,
                Album = musicInfo.AlbumName,
                Quality = musicInfo.Quality,
                From = musicInfo.From,
                State = s,
            };
        }

        public void DownTiemrs()
        {
            var main = MainWindow.Instance;

            Downtimer = new DispatcherTimer();
            Downtimer.Interval = TimeSpan.FromMilliseconds(1);
            Downtimer.Tick += new EventHandler(DownTimer_tick);
            Downtimer.Start();
        }

        public string DownQuality(string id)
        {
            var main = MainWindow.Instance;
            string DownUrl = "";
            if (main.Quality.SelectedIndex == 0)
            { DownUrl = "http://antiserver.kuwo.cn/anti.s?format=mp3&response=url&type=convert_url3&br=128kmp3&from=web&rid=" + id; Suffix = "mp3"; Br = "128"; }
            else if (main.Quality.SelectedIndex == 1)
            { DownUrl = "http://antiserver.kuwo.cn/anti.s?format=mp3&response=url&type=convert_url3&br=320kmp3&from=web&rid=" + id; Suffix = "mp3"; Br = "320"; }
            else if (main.Quality.SelectedIndex == 2)
            { DownUrl = "http://antiserver.kuwo.cn/anti.s?format=mp3&response=url&type=convert_url3&br=1000kape&from=web&rid=" + id; Suffix = "ape"; Br = "ape"; }

            JObject o2 = (JObject)JsonConvert.DeserializeObject(JSON(DownUrl));
            string u = o2["url"].ToString();
            return u;
        }

        public void SingleDownToList()
        {
            try
            {
                QQMusicAPI QQMusicAPI = new QQMusicAPI();
                var main = MainWindow.Instance;
                string DownUrl = "", LrcPath, lrcurl, DownPT = "", Name = "", SongPath = "", PicPath = "", Song = "", Singer = "", Time = "", from, Album, id;
                Suffix = ".mp3";
                if (main.musicParsSave[17] == "1")
                {
                    main.WuBanQuan();
                    return;
                }
                if (main.musicParsSave[18] == "QQ音乐")
                {
                    Suffix = ".m4a";
                    main.musicParsSave[9] = QQMusicAPI.QQMusicUrlGet(main.musicParsSave[0]);
                }

                DownUrl = main.musicParsSave[9];
                DownPT = main.musicParsSave[10];
                lrcurl = main.musicParsSave[12];
                Name = main.musicParsSave[1] + " - " + main.musicParsSave[3];
                Song = main.musicParsSave[1];
                Singer = main.musicParsSave[3];
                Br = "128";
                SongPath = main.SavePath.Text + Name + Suffix;
                PicPath = main.Picpath() + Name + ".jpg";
                LrcPath = main.LrcPath() + Name + ".lrc";
                from = main.musicParsSave[18];
                Album = main.musicParsSave[5];
                id = main.musicParsSave[0];
                if (File.Exists(SongPath))
                {
                    main.message.ShowPopup("已经下载过了...", true);
                    main.LocalList.SelectedIndex = main.isExistLocalList(Name);
                    main.Tab.SelectedIndex = 4;
                    main.SongLoadShow.IsRunning = false;
                    return;
                } //Contrast(Name);

                int DawnListCount = main.DownList.Items.Count;
                MusicInfo downInfo = new MusicInfo()
                {
                    SongId = id,
                    SongName = Name,
                    MP3Url = DownUrl,
                    PTUrl = DownPT,
                    //lrcurl = lrcurl,
                    SongPath = SongPath,
                    PicPath = PicPath,
                    Singer = Singer,
                    Duration = Time,
                    From = from,
                    Suffix = Suffix,
                    AlbumName = Album,
                    State = "等待",
                    Quality = Br,
                };

                main.DownList.Items.Add(new
                {
                    ID = DawnListCount + 1,
                    ProValue = 0,
                    Song = Song,
                    Singer = Singer,
                    Album = Album,
                    Quality = Br,
                    From = from,
                    State = 等待
                });
                main.musicDownList.Add(downInfo);
                if (main.isDownLrc.IsChecked == true)
                {
                    string LrcText = "";
                    if (from == "QQ音乐")
                    {
                        LrcText = main.QQAPI.QQGetLrc(lrcurl);
                    }
                    else if (from == "网易云音乐")
                    {
                        LrcText = main.NTMusicAPI.NTGetLrc(lrcurl);
                    }
                    else if (from == "酷我音乐")
                    {
                        LrcText = main.KWMusicAPI.GetLrc(lrcurl);
                    }
                    else if (from == "酷狗音乐")
                    {
                        LrcText = lrcurl;
                    }
                    main.SaveLrc(LrcText, Name);
                }
                if (main.isDownPic.IsChecked == true) PicDawn(DownPT, PicPath);
                main.Down_null.Visibility = Visibility.Hidden;
                DownTiemrs();
            }
            catch (Exception ex)
            {
                main.message.ShowPopup("SingleDownToList()" + ex.Message, false);
            }
        }

        //一键智能解析
        public async void MusicDiscern()
        {
            main = MainWindow.Instance;
            Message message = new Message();
            Network network = new Network();
            if (!IsConnectedInternet()) { main.message.ShowPopup("无法访问网络，请检查网络是否连接", false); return; }

            try
            {
                IDataObject iData = Clipboard.GetDataObject();
                string url = (string)iData.GetData(DataFormats.UnicodeText);
                url = network.GetRealUrl(url);
                string wyy = "music.163.com/";//网易云
                string qu1 = "https://y.qq.com";//QQ
                string qu2 = "http://url.cn/";//QQ
                string qu3 = "https://i.y.qq.com";//
                string qu4 = "https://c.y.qq.com";//

                string parm1 = "playlist";

                if (!iData.GetDataPresent(DataFormats.Text))
                {
                    url = null;
                    main.message.ShowPopup("不是有效的音乐链接...", false);
                    return;
                }
                //网易云
                else if (url.Contains(wyy))
                {
                    //歌单
                    if (url.Contains(parm1))
                    {
                        //https://music.163.com/#/playlist?id=5122302875
                        //https://music.163.com/playlist?id=368529707&userid=85262593
                        //https://music.163.com/playlist?id=424038303&userid=85262593

                        main.我的歌单.IsSelected = true;
                        ViewModel.MyMusicCard myMusicCard = new ViewModel.MyMusicCard();

                        Match match = Regex.Match(url, "id=\\d+");
                        string id = match.Groups[0].Value.Replace("id=", "");

                        //message.ShowPopup("网易云歌单ID"+ id, false);

                        MusicCardInfo musicCardInfo = await MainWindow.Instance.NTMusicAPI.getSonglist(id);
                        if (musicCardInfo.name != null)
                            myMusicCard.CreateMusicCard(main.MusicCardList, musicCardInfo);
                        else message.ShowPopup("解析失败", false);
                    }
                    //单曲
                    else
                    {
                        main.NTMusicAPI.NTSingleResol(url);
                    }
                }
                //QQ音乐
                else if (url.Contains(qu1) || url.Contains(qu2) || url.Contains(qu3) || url.Contains(qu4))
                {
                    if (url.Contains(parm1) || url.Contains("taoge"))
                    {
                        //https://y.qq.com/n/m/detail/taoge/index.html?id=7521795190
                        //https://y.qq.com/n/yqq/playlist/7521795190.html#stat=y_new.index.playlist.name
                        //https://c.y.qq.com/base/fcgi-bin/u?__=bRNkynN

                        main.我的歌单.IsSelected = true;
                        ViewModel.MyMusicCard myMusicCard = new ViewModel.MyMusicCard();

                        Match Duration1 = Regex.Match(url, "playlist/\\d+.html");
                        string id = Duration1.Groups[0].Value.Replace("playlist/", "").Replace(".html", "");

                        if (string.IsNullOrEmpty(id))
                        {
                            Match Duration2 = Regex.Match(url, "id=\\d+");
                            id = Duration2.Groups[0].Value.Replace("id=", "");
                        }
                        MusicCardInfo musicCardInfo = await main.QQAPI.getSonglist(id);
                        if (musicCardInfo.Name != null)
                            myMusicCard.CreateMusicCard(main.MusicCardList, musicCardInfo);
                        else message.ShowPopup("解析失败", false);
                    }
                    else
                    {
                        string str = GetRealUrl(url);
                        Match Duration1 = Regex.Match(str, "songid=.+?&");
                        string u = Duration1.Groups[0].Value.Replace("songid=", "").Replace("&", "");
                        url = "https://y.qq.com/n/yqq/song/" + u + "_num.html?ADTAG=h5_playsong&no_redirect=1";
                        main.QQAPI.QQSingleResol(url);
                    }
                }
                else
                {
                    url = null;
                    main.message.ShowPopup("不是有效的音乐链接...", false); return;
                }
            }
            catch (Exception)
            {
                main.message.ShowPopup("解析失败...有时需要多尝试几次", false);
                main.LoadShow.IsRunning = false; return;
            }
        }

        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        public bool IsConnectedInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                //已联网
                return true;
            }
            else
            {
                //未联网
                return false;
            }
        }
    }
}

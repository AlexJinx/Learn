using Morin.App.Common.Code;
using Morin.App.Model;
using Morin.App.ViewModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Morin.App.Common.OtherAPI
{
    public class KWMusicAPI
    {
        TransformType transformType = new TransformType();//实例化一个转换类
        MainWindow main;
        Network network = new Network();
        int SongNum = 100;
        int retrySo = 0;

        public async void KWMusicSoso()
        {
            try
            {
                main = MainWindow.Instance;
                string rid = new TransformType().GenerateMD5(DateTime.Now.ToString());

                //列表清空
                main.srPage.SearchList.Items.Clear();
                string txt = main.SearchBox.Text;
                string SoStr2 = "http://www.kuwo.cn/api/www/search/searchMusicBykeyWord?pn=0&rn=" + SongNum + "&httpsStatus=1&key=" + txt + "&reqId=4310c701-b7c7-11eb-8e20-3337bcf7e70d";
                string SoStr = "http://www.kuwo.cn/api/www/search/searchMusicBykeyWord?pn=0&rn=" + SongNum + "&httpsStatus=1&key=" + txt + "&reqId=" + rid;
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = SearchJson(SoStr);//.Replace("{\"result\": ","");//存储信息
                });
                KWJson(str);
                main.LoadShow.IsRunning = false;
                retrySo = 0;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteException(ex);
                main.LoadShow.IsRunning = false;
                if (retrySo < 5) KWMusicSoso();
                else
                {
                    main.message.ShowMessage("酷我音乐搜索出错或无结果，请重试");
                }
                retrySo++;
            }
        }


        public int ntj = 0;//判断是一键解析还是搜索
        public void KWJson(string str1)
        {
            MainWindow main = MainWindow.Instance;
            main.message.DebugOut(str1);
            JObject o = (JObject)JsonConvert.DeserializeObject(str1);
            //歌曲列表
            JArray jar = JArray.Parse(o["data"]["list"].ToString());
            ntj = 0;
            int mn;
            if (jar.Count < SongNum) mn = jar.Count;
            else mn = SongNum;
            main.musicSearchList.Clear();
            for (int i = 0; i < mn; i++)
            {
                //JArray jar = JArray.Parse(o["songlist"].ToString());
                JObject job = JObject.Parse(jar[i].ToString());
                string songname = job["name"].ToString().Replace("/", "-").Replace("|", " ");//歌名
                                                                                             //string songmid = job["songmid"].ToString();
                string songid = job["musicrid"].ToString();
                //string subname = job["lyric"].ToString();//副名

                string albumname = job["album"].ToString();//专辑
                string albumid = job["albumid"].ToString();
                string duration = (Convert.ToInt64(job["duration"])).ToString();//音乐时间
                string singer = job["artist"].ToString();//歌手
                string singerid = job["artistid"].ToString();//歌手

                //string mp3url = network.JSON(url);

                string songmid = songid.Replace("MUSIC_", "");
                string mp3url = "http://antiserver.kuwo.cn/anti.s?type=convert_url&rid=" + songmid + "&format=mp3&response=url";
                //歌词
                string lrcurl = "http://m.kuwo.cn/newh5/singles/songinfoandlrc?musicId=" + songmid;
                //专辑图片
                //string pturl = "https://v1.itooi.cn/kuwo/pic?id=" + musicid;
                bool pic = job.ToString().Contains("albumpic");
                string pturl = "";
                if (pic) pturl = job["albumpic"].ToString().Replace("img1.kuwo.cn/", "img1.kwcdn.kuwo.cn/");
                //else { pturl = job["pic"].ToString(); }

                //MV地址
                string mvid = job["rid"].ToString();

                bool hasLossless = (bool)job["hasLossless"];//是否有无损
                //bool hasLossless = false;//是否有无损
                bool fee = (bool)job["isListenFee"];//是否支付
                bool hasmv = false;//是否有MV
                if ((int)job["hasmv"] == 1) hasmv = true;

                MusicInfo musicInfo = new MusicInfo()
                {
                    SongId = songmid,
                    SongName = songname,
                    //subname = subname,
                    Singer = singer,
                    SingerId = singerid,
                    AlbumName = albumname,
                    AlbumId = albumid,
                    Duration = duration,
                    MVId = mvid,
                    MP3Url = mp3url,
                    PTUrl = pturl,
                    LrcUrl = lrcurl,

                    HasLossless = hasLossless,
                    HasMV = hasmv,
                    Fee = false,
                    From = "酷我音乐",
                };
                main.musicSearchList.Add(musicInfo);
                main.LoadToListView(main.srPage.SearchList, musicInfo);
            }
        }

        public string BangJson(string u)//获取URL地址JSON编码
        {
            string t = null;

            // string str = string.Empty;//存储信息
            //实例化一个WebRequest对象
            WebRequest webrequest = WebRequest.Create(u);
            //webrequest.Timeout = 3000;
            webrequest.Method = "GET";
            SetHeaderValue(webrequest.Headers, "csrf", "VS5XKYGJ4J");
            //SetHeaderValue(webrequest.Headers, "Host", "www.kuwo.cn");
            SetHeaderValue(webrequest.Headers, "Referer", "http://www.kuwo.cn/rankList");
            SetHeaderValue(webrequest.Headers, "Cookie", "Hm_lvt_cdb524f42f0ce19b169a8071123a4797=1570943967,1570943977,1571746672,1572274327; Hm_lpvt_cdb524f42f0ce19b169a8071123a4797=1572274384; kw_token=VS5XKYGJ4J");

            ////设置用于对Internet资源请求进行身份验证的网络凭据
            webrequest.Credentials = CredentialCache.DefaultCredentials;
            WebResponse webresponse = webrequest.GetResponse();
            Stream stream = webresponse.GetResponseStream();
            //使用创建的Stream对象创建一个StreamReader流读取对象
            StreamReader sreader = new StreamReader(stream);
            //读取流中的内容，并显示在RichTestBox控件中
            t = sreader.ReadToEnd();
            sreader.Close();
            stream.Close();
            webresponse.Close();

            return t;
        }

        public async void KWMusicBang(int bangid)
        {
            if (!network.IsConnectedInternet()) { return; }//检查联网
            main = MainWindow.Instance;
            try
            {
                //列表清空
                main.HotList.Items.Clear();
                string url = "http://www.kuwo.cn/api/www/bang/bang/musicList?bangId=" + bangid + "&pn=0&rn=100";
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = BangJson(url);//.Replace("{\"result\": ","");//存储信息
                });
                main.LoadShow.IsRunning = false;
                KWMusicBangJson(str);
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
                KWMusicBang(bangid);
            }
        }

        public void KWMusicBangJson(string str1)
        {
            var main = MainWindow.Instance;
            JObject o = (JObject)JsonConvert.DeserializeObject(str1);
            //歌曲列表
            JArray jar = JArray.Parse(o["data"]["musicList"].ToString());

            string UpdateTime = o["data"]["pub"].ToString();
            //MainWindow.Instance.KWTurnoverTime.Text = "排行榜更新时间：" + UpdateTime;
            int mn = 0;
            if (jar.Count < SongNum) mn = jar.Count;
            else mn = SongNum;
            main.musicTopList.Clear();
            for (int i = 0; i < mn; i++)
            {
                //JArray jar = JArray.Parse(o["songlist"].ToString());
                JObject job = JObject.Parse(jar[i].ToString());
                string songname = job["name"].ToString().Replace("/", "-").Replace("|", " ");//歌名
                string albumname = job["album"].ToString();//专辑
                string albummid = job["albumid"].ToString();
                string duration = (Convert.ToInt64(job["duration"])).ToString();//音乐时间
                string singer = job["artist"].ToString();//歌手
                string singerid = job["artistid"].ToString();//歌手

                string songid = job["musicrid"].ToString().Replace("MUSIC_", "");
                string mp3url = "http://antiserver.kuwo.cn/anti.s?type=convert_url&rid=" + songid + "&format=mp3&response=url";
                //歌词
                string lrcurl = "http://m.kuwo.cn/newh5/singles/songinfoandlrc?musicId=" + songid;
                //专辑图片
                //string pturl = "https://v1.itooi.cn/kuwo/pic?id=" + musicid;
                bool pic = job.ToString().Contains("albumpic");
                bool pic2 = job.ToString().Contains("pic");
                string pturl = "";
                if (pic) pturl = job["albumpic"].ToString();
                else if (pic2) pturl = job["pic"].ToString();
                else pturl = "";
                pturl.Replace("120", "500").Replace("300", "500").Replace("img1.kuwo.cn/", "img1.kwcdn.kuwo.cn/");
                //MV地址
                string mvid = job["rid"].ToString();

                bool hasLossless = (bool)job["hasLossless"];//是否有无损
                bool fee = (bool)job["isListenFee"];//是否支付
                bool hasmv = false;//是否有MV
                if ((int)job["hasmv"] == 1) hasmv = true;
                else hasmv = false;

                MusicInfo musicInfo = new MusicInfo()
                {
                    SongId = songid,
                    SongName = songname,
                    Singer = singer,
                    SingerId = singerid,
                    AlbumName = albumname,
                    AlbumId = albummid,
                    Duration = duration,
                    MVId = mvid,
                    MP3Url = mp3url,
                    PTUrl = pturl,
                    LrcUrl = lrcurl,
                    HasLossless = hasLossless,
                    HasMV = hasmv,
                    Fee = false,
                    From = "酷我音乐",
                };
                main.musicTopList.Add(musicInfo);
                main.LoadToListView(main.HotList, musicInfo);
            }
        }

        string kwlrcurltext = "";//获取歌词的信息文本
        public async void KWGetText(string url)
        {
            kwlrcurltext = "";
            main.SongLoadShow.IsRunning = true;
            await Task.Run(() =>
            {
                kwlrcurltext = network.JSON(url);
            });
            main.SongLoadShow.IsRunning = false;
        }

        public string GetLrc(string id)
        {
            if (!network.IsConnectedInternet()) { return ""; }//检查联网
            string url = "http://m.kuwo.cn/newh5/singles/songinfoandlrc?musicId=" + id.Replace("MUSIC_", "");

            string text = SearchJson(url);
            string lrctext = "";
            try
            {
                JObject o = (JObject)JsonConvert.DeserializeObject(text);
                JArray jar = null;
                string ll = o["data"]["lrclist"].ToString();
                if (ll != "")
                    jar = JArray.Parse(o["data"]["lrclist"].ToString());
                else return "";
                int count = jar.Count;
                for (int i = 0; i < count; i++)
                {
                    double t1 = (double)jar[i]["time"];
                    string t2 = jar[i]["lineLyric"].ToString();
                    TimeSpan timeSpan = TimeSpan.FromSeconds(t1);
                    string t3 = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString() + "." + timeSpan.Milliseconds.ToString("f2");
                    lrctext = lrctext + "[" + t3 + "]" + t2 + "\n";
                }
            }
            catch (Exception)
            {
                return "";
            }

            return lrctext;
        }

        public string GetSingersJson(string u)//获取URL地址JSON编码
        {
            string t = string.Empty;

            //实例化一个WebRequest对象
            WebRequest webrequest = WebRequest.Create(u);
            //webrequest.Timeout = 3000;
            webrequest.Method = "GET";
            SetHeaderValue(webrequest.Headers, "csrf", "ZYRMRV30P3");
            SetHeaderValue(webrequest.Headers, "Referer", "http://www.kuwo.cn/");
            SetHeaderValue(webrequest.Headers, "Cookie",
                "Hm_lvt_cdb524f42f0ce19b169a8071123a4797=1570943967,1570943977,1571746672,1572274327; Hm_lpvt_cdb524f42f0ce19b169a8071123a4797=1572288167; kw_token=ZYRMRV30P3");

            ////设置用于对Internet资源请求进行身份验证的网络凭据
            webrequest.Credentials = CredentialCache.DefaultCredentials;
            WebResponse webresponse = webrequest.GetResponse();
            Stream stream = webresponse.GetResponseStream();
            //使用创建的Stream对象创建一个StreamReader流读取对象
            StreamReader sreader = new StreamReader(stream);
            //读取流中的内容，并显示在RichTestBox控件中
            t = sreader.ReadToEnd();
            sreader.Close();
            stream.Close();
            webresponse.Close();

            return t;
        }

        public int SingerNum = 40;//歌手数量
        public async void GetSingers()
        {
            try
            {
                if (!network.IsConnectedInternet()) { return; }//检查联网
                main = MainWindow.Instance;
                //string singerPath = main.SingerPicpath;//歌手图片缓存位置;

                string url = "http://www.kuwo.cn/api/www/artist/artistInfo?category=0&pn=1&rn=" + SingerNum;
                string text = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    text = GetSingersJson(url);//.Replace("{\"result\": ","");//存储信息
                });
                main.LoadShow.IsRunning = false;

                JObject o = (JObject)JsonConvert.DeserializeObject(text);
                JArray jar = JArray.Parse(o["data"]["artistList"].ToString());
                int count = jar.Count;
                main.SingerList.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string singer = jar[i]["name"].ToString();
                    string singerID = jar[i]["id"].ToString();
                    string artistFans = jar[i]["artistFans"].ToString();
                    string albumNum = jar[i]["albumNum"].ToString();
                    string musicNum = jar[i]["musicNum"].ToString();
                    string picUrl = jar[i]["pic"].ToString().Replace("img1.kuwo.cn/", "img1.kwcdn.kuwo.cn/");

                    main.singerInfoSave[i, 0] = singer;
                    main.singerInfoSave[i, 1] = singerID;
                    main.singerInfoSave[i, 2] = artistFans;
                    main.singerInfoSave[i, 3] = albumNum;
                    main.singerInfoSave[i, 4] = musicNum;
                    main.singerInfoSave[i, 5] = picUrl;

                    main.SingerList.ItemTemplate = main.FindResource("MusicRecommendDataTemplate") as DataTemplate;
                    ScrollViewer.SetHorizontalScrollBarVisibility(main.SingerList, ScrollBarVisibility.Disabled);
                    main.SingerList.Items.Add(new SingerInfo
                    {
                        ID = main.SingerList.Items.Count + 1,
                        SingerPic = picUrl,
                        SingerName = singer
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
                GetSingers();
                //main.message.ShowMessage("酷我加载歌手出错" ); 
            }
        }

        public async void HotSearchKey()
        {
            try
            {
                if (!network.IsConnectedInternet()) { return; }//检查联网

                main = MainWindow.Instance;
                string search = main.SearchBox.Text;
                if (search == "") return;
                string url = "http://www.kuwo.cn/api/www/search/searchKey?key=" + search;
                string text = "";
                await Task.Run(() =>
                {
                    text = SearchJson(url);
                });
                if (text == null) return;
                JObject o = (JObject)JsonConvert.DeserializeObject(text);
                JArray jar = JArray.Parse(o["data"].ToString());
                string data = jar.ToString();

                main.hotText.Text = "猜你在搜";
                main.SearchHistoryList.Items.Clear();
                foreach (var item in Regex.Matches(data, "RELWORD=.+SNUM"))
                {
                    string t = item.ToString().Replace("RELWORD=", "").Replace("\\r\\nSNUM", "");
                    main.SearchHistoryList.Items.Add(t);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteException(ex);
            }
        }

        public string SearchJson(string u)//获取URL地址JSON编码
        {
            string t = string.Empty;

            // string str = string.Empty;//存储信息
            //实例化一个WebRequest对象
            WebRequest webrequest = WebRequest.Create(u);
            //webrequest.Timeout = 3000;
            webrequest.Method = "GET";
            SetHeaderValue(webrequest.Headers, "csrf", "YHGRC8T7LZR");
            SetHeaderValue(webrequest.Headers, "Referer", "http://www.kuwo.cn/");
            SetHeaderValue(webrequest.Headers, "Cookie", "_ga=GA1.2.281119717.1609679409; Hm_lvt_cdb524f42f0ce19b169a8071123a4797=1614521252,1616906823; Hm_lpvt_cdb524f42f0ce19b169a8071123a4797=1616906823; _gid=GA1.2.1066888924.1616906823; _gat=1; kw_token=YHGRC8T7LZR");

            ////设置用于对Internet资源请求进行身份验证的网络凭据
            webrequest.Credentials = CredentialCache.DefaultCredentials;
            WebResponse webresponse = webrequest.GetResponse();
            Stream stream = webresponse.GetResponseStream();
            //使用创建的Stream对象创建一个StreamReader流读取对象
            StreamReader sreader = new StreamReader(stream);
            //读取流中的内容，并显示在RichTestBox控件中
            t = sreader.ReadToEnd();
            sreader.Close();
            stream.Close();
            webresponse.Close();

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

        public List<FMListInfo> SongListInfo { get; }
        //歌单
        public async void SongList(string N)
        {
            main = MainWindow.Instance;
            try
            {
                if (!network.IsConnectedInternet()) { return; }//检查联网
                int count = 30;
                string url = "http://www.kuwo.cn/api/www/classify/playlist/getRcmPlayList?pn=1&rn=" + count + "&order=" + N;
                string text = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    text = network.JSON(url);
                });

                JObject o = (JObject)JsonConvert.DeserializeObject(text);
                JArray jar = JArray.Parse(o["data"]["data"].ToString());

                main.SongList.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string name = jar[i]["name"].ToString();
                    string id = jar[i]["id"].ToString();
                    string img = jar[i]["img"].ToString();
                    string total = jar[i]["total"].ToString();
                    string listencnt = jar[i]["listencnt"].ToString();

                    main.songListSave[i, 0] = name;
                    main.songListSave[i, 1] = id;
                    main.songListSave[i, 2] = img;
                    main.songListSave[i, 3] = total;
                    main.songListSave[i, 4] = listencnt;

                    int listen = Convert.ToInt32(listencnt);
                    if (listen > 99999) listencnt = (listen / 10000).ToString() + "万";

                    main.SongList.ItemTemplate = main.FindResource("SongListDataTemplate") as DataTemplate;
                    main.SongList.Items.Add(new FMListInfo
                    {
                        ID = main.SongList.Items.Count + 1,
                        Pic = img,
                        Name = name,
                        Listen = " " + listencnt,
                        Total = total + "首",
                    });
                }
                main.LoadShow.IsRunning = false;
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
                SongList(N);
            }
        }


        public async void SearchSongList()
        {
            try
            {
                main = MainWindow.Instance;
                main.hotlist.IsChecked = false;
                main.newlist.IsChecked = false;
                if (!network.IsConnectedInternet()) { main.message.ShowMessage("无法访问网络，请检查网络是否连接"); return; }

                string key = main.SongListSearchBox.Text;
                if (key == "") return;
                int count = 30;
                string url = "http://www.kuwo.cn/api/www/search/searchPlayListBykeyWord?pn=1&rn=" + count + "&key=" + key;
                string text = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    text = SearchJson(url);
                });
                main.LoadShow.IsRunning = false;
                JObject o = (JObject)JsonConvert.DeserializeObject(text);
                JArray jar = JArray.Parse(o["data"]["list"].ToString());

                int co = jar.Count;
                if (co > count) co = count;
                main.SongList.Items.Clear();
                for (int i = 0; i < co; i++)
                {
                    string name = jar[i]["name"].ToString();
                    string id = jar[i]["id"].ToString();
                    string img = jar[i]["img"].ToString();
                    string total = jar[i]["total"].ToString();
                    string listencnt = jar[i]["listencnt"].ToString();

                    main.songListSave[i, 0] = name;
                    main.songListSave[i, 1] = id;
                    main.songListSave[i, 2] = img;
                    main.songListSave[i, 3] = total;
                    main.songListSave[i, 4] = listencnt;

                    int listen = Convert.ToInt32(listencnt);
                    if (listen > 99999) listencnt = (listen / 10000).ToString() + "万";

                    main.SongList.ItemTemplate = main.FindResource("SongListDataTemplate") as DataTemplate;
                    main.SongList.Items.Add(new FMListInfo
                    {
                        ID = main.SongList.Items.Count + 1,
                        Pic = img,
                        Name = name,
                        Listen = " " + listencnt,
                        Total = total + "首",
                    });
                }
                main.SongList.ScrollIntoView(main.SongList.Items[0]);
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
                SearchSongList();
            }
        }

        public async void SongListToList(int select)
        {
            if (!network.IsConnectedInternet() || select == -1) return;//检查联网
            string name = main.songListSave[select, 0];
            string id = main.songListSave[select, 1];
            string count = main.songListSave[select, 3];
            main = MainWindow.Instance;
            {
                //列表清空
                main.slRPage.SongList.Items.Clear();
                string url = "http://www.kuwo.cn/api/www/playlist/playListInfo?pid=" + id + "&pn=1&rn=" + count;
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = SongListJson(url);
                });
                main.LoadShow.IsRunning = false;
                KWSongListJson(str, select);
            }
            main.slRPage.play_btn.Content = " " + count + "首";
            MyMusicCard myMusicCard = new MyMusicCard();
            if (myMusicCard.isCardExistence(name))
                main.slRPage.Favorites_btn.Content = "已收藏";
            else main.slRPage.Favorites_btn.Content = "收藏歌单";
        }

        public string SongListJson(string u)//获取URL地址JSON编码
        {
            string t = string.Empty;
            //实例化一个WebRequest对象
            WebRequest webrequest = WebRequest.Create(u);
            //webrequest.Timeout = 3000;
            webrequest.Method = "GET";
            SetHeaderValue(webrequest.Headers, "csrf", "ZEDB14W8KI");
            //SetHeaderValue(webrequest.Headers, "Host", "www.kuwo.cn");
            SetHeaderValue(webrequest.Headers, "Referer", "http://www.kuwo.cn/rankList");
            SetHeaderValue(webrequest.Headers, "Cookie", "kw_token=ZEDB14W8KI");

            ////设置用于对Internet资源请求进行身份验证的网络凭据
            webrequest.Credentials = CredentialCache.DefaultCredentials;
            WebResponse webresponse = webrequest.GetResponse();
            Stream stream = webresponse.GetResponseStream();
            //使用创建的Stream对象创建一个StreamReader流读取对象
            StreamReader sreader = new StreamReader(stream);
            //读取流中的内容，并显示在RichTestBox控件中
            t = sreader.ReadToEnd();
            Console.WriteLine("内容：" + t);
            sreader.Close();
            stream.Close();
            webresponse.Close();

            return t;
        }

        int retry = 0;
        public void KWSongListJson(string str1, int select)
        {
            try
            {
                main = MainWindow.Instance;
                if (str1 == null || str1 == "")
                {
                    main.message.ShowMessage("打开出错,请重试");
                    return;
                    if (retry == 3)
                    {
                        main.message.ShowMessage("打开出错,请重试");
                        return;
                    }
                    KWSongListJson(str1, select);
                    Console.WriteLine("重试次数：" + retry);
                    retry++;
                    return;
                }
                retry = 0;
                JObject o = (JObject)JsonConvert.DeserializeObject(str1);
                //歌曲列表
                JArray jar = JArray.Parse(o["data"]["musicList"].ToString());

                main.slRPage.PLImg.Source = new BitmapImage(new Uri(main.songListSave[select, 2], UriKind.RelativeOrAbsolute));
                main.slRPage.PLName.Text = main.songListSave[select, 0];
                main.slRPage.PLInt.Text = o["data"]["info"].ToString().Replace("\n", "");
                //string UpdateTime = o["data"]["musicList"].ToString();
                //ainWindow.Instance.KWTurnoverTime.Text = "排行榜更新时间：" + UpdateTime;
                int mn = jar.Count;
                main.musicSearchList.Clear();
                for (int i = 0; i < mn; i++)
                {
                    JObject job = JObject.Parse(jar[i].ToString());
                    string songname = job["name"].ToString().Replace("/", "-").Replace("|", " ");//歌名
                    string songid = job["musicrid"].ToString();
                    string albumname = job["album"].ToString();//专辑
                    string albumid = job["albumid"].ToString();
                    string duration = (Convert.ToInt64(job["duration"])).ToString();//音乐时间
                    string singer = job["artist"].ToString();//歌手
                    string singerid = job["artistid"].ToString();//歌手
                    //string mp3url = network.JSON(url);

                    string musicid = songid.Replace("MUSIC_", "");
                    string mp3url = "http://antiserver.kuwo.cn/anti.s?type=convert_url&rid=" + musicid + "&format=mp3&response=url";
                    //歌词
                    string lrcurl = "http://m.kuwo.cn/newh5/singles/songinfoandlrc?musicId=" + musicid;
                    //专辑图片
                    //string pturl = "https://v1.itooi.cn/kuwo/pic?id=" + musicid;
                    bool pic = job.ToString().Contains("albumpic");
                    bool pic2 = job.ToString().Contains("pic");
                    string pturl = "";
                    if (pic) pturl = job["albumpic"].ToString();
                    else if (pic2) pturl = job["pic"].ToString();
                    else pturl = "";
                    pturl.Replace("120", "500").Replace("300", "500");
                    //MV地址

                    string mvid = job["rid"].ToString();

                    bool hasLossless = (bool)job["hasLossless"];//是否有无损
                    bool fee = (bool)job["isListenFee"];//是否支付
                    bool hasmv = false;//是否有MV
                    if ((int)job["hasmv"] == 1) hasmv = true;
                    else hasmv = false;

                    MusicInfo musicInfo = new MusicInfo()
                    {
                        SongId = songid,
                        SongName = songname,
                        Singer = singer,
                        SingerId = singerid,
                        AlbumName = albumname,
                        AlbumId = albumid,
                        Duration = duration,
                        MVId = mvid,
                        MP3Url = mp3url,
                        PTUrl = pturl,
                        LrcUrl = lrcurl,
                        HasLossless = hasLossless,
                        HasMV = hasmv,
                        Fee = false,
                        From = "酷我音乐",
                    };
                    main.musicSearchList.Add(musicInfo);
                    main.LoadToListView(main.slRPage.SongList, musicInfo);
                }
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e);
            }
        }


    }
}

using Morin.App.Common.Code;
using Morin.App.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Message = Morin.App.Model.Message;

namespace Morin.App.Common.OtherAPI
{
    public class NTMusicAPI
    {
        TransformType transformType = new TransformType();//实例化一个转换类
        MainWindow main = MainWindow.Instance;
        Network network = new Network();
        int SongNum = 100;

        public async void NTMusicSoso()
        {

            main = MainWindow.Instance;
            //列表清空
            main.srPage.SearchList.Items.Clear();
            string txt = main.SearchBox.Text;
            string SoStr = "http://music.163.com/api/search/get/web?s=" + txt + "&limit=100&type=1&offset=0";
            Console.WriteLine(SoStr);
            string str = "";
            main.LoadShow.IsRunning = true;
            await Task.Run(() =>
            {
                str = network.NTCookieJson(SoStr);//.Replace("{\"result\": ","");//存储信息
            });
            NTJson(str);
            main.LoadShow.IsRunning = false;
        }

        public int ntj = 0;//判断是一键解析还是搜索
        public void NTJson(string str1)
        {
            JObject o = (JObject)JsonConvert.DeserializeObject(str1);
            //歌曲列表
            JArray jar;
            if (ntj == 0)
                jar = JArray.Parse(o["result"]["songs"].ToString());
            else
                jar = JArray.Parse(o["songs"].ToString());
            ntj = 0;
            int mn = 0;
            if (jar.Count < SongNum) mn = jar.Count;
            else mn = SongNum;
            main.musicSearchList.Clear();
            for (int i = 0; i < mn; i++)
            {
                JObject job = JObject.Parse(jar[i].ToString());
                string songname = job["name"].ToString().Replace("/", "-");//歌名
                string songid = job["id"].ToString();
                string mp3url = @"http://music.163.com/song/media/outer/url?id=" + songid + ".mp3";
                string albumname = job["album"]["name"].ToString();//专辑
                string albumid = job["album"]["id"].ToString();//专辑
                string duration = (Convert.ToInt64(job["duration"]) / 1000).ToString();//音乐时间
                string lrcurl = "http://music.163.com/api/song/lyric?os=pc&id=" + songid + "&lv=-1&kv=-1&tv=-1";
                string pturl = "";
                //MV地址
                string mvid = job["mvid"].ToString();
                string copyrightId = job["copyrightId"].ToString();

                bool hasmv = false;
                if (mvid != "0") hasmv = true;
                bool hasLossless = false;
                string feestr = job["fee"].ToString();
                bool fee;
                if (feestr == "0" || feestr == "8") fee = false;
                else fee = true;
                if (copyrightId == "1007") fee = true;
                JArray jar2 = JArray.Parse(job["artists"].ToString());
                string singer = "";//歌手
                string singerid = "";
                for (int i2 = 0; i2 < jar2.Count; i2++)
                {
                    JObject job2 = JObject.Parse(jar2[i2].ToString());
                    string f = " & ";
                    if (i2 == jar2.Count - 1) f = "";
                    singer += job2["name"].ToString().Replace("/", "-") + f;
                    if (singerid == "") singerid = job2["id"].ToString();
                }

                MusicInfo musicInfo = new MusicInfo()
                {
                    SongId = songid,
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
                    Fee = fee,
                    From = "网易云音乐",
                };
                main.musicSearchList.Add(musicInfo);
                main.LoadToListView(main.srPage.SearchList, musicInfo);
            }
        }

        public async void NTSingleResol(string url)//网易云单曲解析，官方接口-
        {
            try
            {
                main = MainWindow.Instance;
                //根据链接取出ID
                Match Duration = Regex.Match(url, "id=\\d+");
                string ID = Duration.Groups[0].Value.Replace("id=", "");
                //string[] ID = url.Split(new string[] { "?id=", "&userid" }, StringSplitOptions.RemoveEmptyEntries);
                string InfoUrl = "http://music.163.com/api/song/detail/?id=" + ID + "&ids=%5B" + ID + "%5D&csrf_token=";

                string str = "";
                //main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.JSON(InfoUrl);//存储信息
                });
                main.LoadShow.IsRunning = false;
                //NTJson(str);
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                //歌曲列表
                JArray jar = JArray.Parse(o["songs"].ToString());
                ntj = 0;
                int mn = 0;
                if (jar.Count < SongNum) mn = jar.Count;
                else mn = SongNum;
                for (int i = 0; i < mn; i++)
                {
                    //JArray jar = JArray.Parse(o["songlist"].ToString());
                    JObject job = JObject.Parse(jar[i].ToString());
                    string songname = job["name"].ToString().Replace("/", "-");//歌名
                                                                               //string songmid = job["songmid"].ToString();
                    string songid = job["id"].ToString();
                    string mp3url = @"http://music.163.com/song/media/outer/url?id=" + songid + ".mp3";
                    //string subname = job["lyric"].ToString();//副名

                    string albumname = job["album"]["name"].ToString();//专辑
                                                                       //string albummid = job["albummid"].ToString();
                    long albumid = (long)job["album"]["id"];
                    string time = (Convert.ToInt64(job["duration"]) / 1000).ToString();//音乐时间

                    string lrcurl = "http://music.163.com/api/song/lyric?os=pc&id=" + songid + "&lv=-1&kv=-1&tv=-1";
                    //专辑图片
                    string pturl = job["album"]["picUrl"].ToString();
                    //MV地址
                    string mvid = job["mvid"].ToString();
                    string fee = job["fee"].ToString();//歌名
                    string copyrightId = job["copyrightId"].ToString();//歌名

                    if (fee == "0" || fee == "8") fee = "0";
                    else fee = "1";
                    if (copyrightId == "1007") fee = "1";
                    JArray jar2 = JArray.Parse(job["artists"].ToString());
                    string singer = "";//歌手
                    for (int i2 = 0; i2 < jar2.Count; i2++)
                    {
                        JObject job2 = JObject.Parse(jar2[i2].ToString());
                        string f = " & ";
                        if (i2 == jar2.Count - 1) f = "";
                        singer += job2["name"].ToString().Replace("/", "-") + f;
                    }
                    main.musicParsSave[0] = songid;
                    main.musicParsSave[1] = songname;
                    //mian.MusicParsSave[i, 2] = subname;
                    main.musicParsSave[3] = singer;
                    //main.MusicParsSave[i, 4] = singermid;
                    main.musicParsSave[5] = albumname;
                    //main.MusicParsSave[i, 6] = albummid;
                    main.musicParsSave[7] = time;
                    main.musicParsSave[8] = albumid.ToString();
                    main.musicParsSave[9] = mp3url;
                    main.musicParsSave[10] = pturl;
                    main.musicParsSave[11] = songid;
                    main.musicParsSave[12] = lrcurl;
                    main.musicParsSave[13] = mvid;
                    main.musicParsSave[17] = fee;
                    main.musicParsSave[18] = "网易云音乐";
                    //解析添加到下载列表
                    network.SingleDownToList();
                }
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e); main.message.ShowPopup("网易云单曲解析失败！", false);
            }
        }

        public string NTGetLrc(string url)
        {
            string lrc = "";
            string str = NTLrcJson(url);
            JObject o = (JObject)JsonConvert.DeserializeObject(str);
            //歌曲列表
            lrc = o["lrc"]["lyric"].ToString();//歌名
            if (lrc == null || lrc == "")
            {
                return "";
            }

            return lrc;
        }
        public string NTLrcJson(string u)//获取URL地址JSON编码
        {
            string t = null;
            try
            {
                // string str = string.Empty;//存储信息
                //实例化一个WebRequest对象
                WebRequest webrequest = WebRequest.Create(u);
                //设置用于对Internet资源请求进行身份验证的网络凭据
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
            catch { }
            return t;
        }

        int call = 0;
        public async Task<MusicCardInfo> getSonglist(string id)
        {
            try
            {
                MainWindow main = MainWindow.Instance;
                main.LoadShow.IsRunning = true;
                List<MusicInfo> musicInfos = new List<MusicInfo>();
                //根据链接取出ID

                string url = "http://music.163.com/api/playlist/detail?id=" + id;
                //string cookie = "os=pc; osver=Microsoft-Windows-10-Professional-build-10586-64bit; appver=2.0.2; channel=netease; __remember_me=true";
                string str = "";
                //main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.NTCookieJson(url);//存储信息
                });
                //main.LoadShow.IsRunning = false;
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                //歌曲列表
                JArray jar = JArray.Parse(o["result"]["tracks"].ToString());
                string name = o["result"]["name"].ToString();
                string info = o["result"]["description"].ToString();
                string coverImgUrl = o["result"]["coverImgUrl"].ToString();
                int Count = jar.Count;
                for (int i = 0; i < Count; i++)
                {
                    JObject job = JObject.Parse(jar[i].ToString());

                    string songname = job["name"].ToString().Replace("/", "-");//歌名
                    string songid = job["id"].ToString();
                    string mp3url = @"http://music.163.com/song/media/outer/url?id=" + songid + ".mp3";

                    string albumname = job["album"]["name"].ToString();//专辑
                    string albumid = job["album"]["id"].ToString();
                    string duration = (Convert.ToInt64(job["duration"]) / 1000).ToString();//音乐时间

                    string lrcurl = "http://music.163.com/api/song/lyric?os=pc&id=" + songid + "&lv=-1&kv=-1&tv=-1";
                    //专辑图片
                    string pturl = job["album"]["picUrl"].ToString();
                    //MV地址
                    string mvid = job["mvid"].ToString();
                    bool hasmv = (mvid == "0") ? false : true;
                    //收费
                    string fee = job["fee"].ToString();//歌名
                    bool feeble = true;
                    string copyrightId = job["copyrightId"].ToString();
                    if (fee == "0" || fee == "8") feeble = false;
                    else fee = "1";
                    if (copyrightId == "1007") feeble = true;

                    JArray jar2 = JArray.Parse(job["artists"].ToString());
                    string singerid = "";
                    string singer = "";//歌手
                    for (int i2 = 0; i2 < jar2.Count; i2++)
                    {
                        JObject job2 = JObject.Parse(jar2[i2].ToString());
                        string f = " & ";
                        if (i2 == jar2.Count - 1) f = "";
                        singer += job2["name"].ToString().Replace("/", "-") + f;
                        if (singerid == "") singerid = job2["id"].ToString();
                    }
                    bool hasLossless = false;

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
                        Fee = feeble,
                        From = "网易云音乐",
                    };
                    musicInfos.Add(musicInfo);
                }
                MusicCardInfo musicCardInfo = new MusicCardInfo
                {
                    Musics = musicInfos,
                    Name = name,
                    Info = info,
                    Pic = coverImgUrl
                };
                main.LoadShow.IsRunning = false;
                return musicCardInfo;
            }
            catch (Exception e)
            {
                call++;
                if (call < 40)
                    await getSonglist(id);
                else
                {
                    new Message().DebugOut(call.ToString()); call = 0;
                    new Message().ShowPopup("解析失败...有时需要多尝试几次", false);
                    main.LoadShow.IsRunning = false; return new MusicCardInfo();
                }
            }
            main.LoadShow.IsRunning = false;
            return new MusicCardInfo();
        }

        public string encrypted_id(string id)
        {
            var strToBytes1 = Encoding.Default.GetBytes("3go8&$8*3*3h0k(2)2");
            var strToBytes2 = Encoding.Default.GetBytes(id);
            var byte1_len = strToBytes1.Length;
            for (int i = 0; i < strToBytes2.Length; i++)
            {
                strToBytes2[i] = Convert.ToByte(strToBytes2[i] ^ strToBytes1[i % byte1_len]);
            }

            // 创建MD5类的默认实例：MD5CryptoServiceProvider 
            MD5 md5 = MD5.Create();
            byte[] hs = md5.ComputeHash(strToBytes2);
            StringBuilder stb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化 
                stb.Append(b.ToString("x2"));
            }

            return stb.ToString();
        }
    }
}

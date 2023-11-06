using Morin.App.Common.Code;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Common.OtherAPI
{
    public class KGMusicAPI
    {
        MainWindow main = MainWindow.Instance;
        int SongNum = 100, ntj = 0;

        //酷狗音乐
        public async void KGMusicSoso()
        {
            main = MainWindow.Instance;
            Network network = new Network();
            try
            {
                //列表清空
                main.srPage.SearchList.Items.Clear();
                string txt = main.SearchBox.Text;
                string SoStr = "http://mobilecdn.kugou.com/api/v3/search/song?keyword=" + txt + "&page=1&pagesize=100";
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.JSON(SoStr);//.Replace("{\"result\": ","");//存储信息
                });
                KGJson(str);
                main.LoadShow.IsRunning = false;
            }
            catch (Exception e) { Logger.Instance.WriteException(e); main.message.ShowMessage("酷狗音乐搜索出错或无结果，请重试"); }
        }

        public void KGJson(string str1)
        {
            JObject o = (JObject)JsonConvert.DeserializeObject(str1);
            //歌曲列表
            JArray jar = JArray.Parse(o["data"]["info"].ToString());
            ntj = 0;
            int mn = 0;
            if (jar.Count < SongNum) mn = jar.Count;
            else mn = SongNum;
            main.musicSearchList.Clear();
            for (int i = 0; i < mn; i++)
            {
                JObject job = JObject.Parse(jar[i].ToString());
                string songname = job["songname"].ToString().Replace("/", "-");//歌名
                string albumname = job["album_name"].ToString();//专辑
                string albumid = job["album_audio_id"].ToString();//专辑
                string duration = job["duration"].ToString();//音乐时间
                string mvhash = job["mvhash"].ToString();
                string singer = job["singername"].ToString().Replace("、", " & ");
                string Hash = job["hash"].ToString();//普通Hash
                string HQHash = job["320hash"].ToString();//高质量Hash
                string SQHash = job["sqhash"].ToString();//无损Hash

                string lrcurl = "";// KGUrlGet(Hash, 3);
                string mp3url = Hash; //KGUrlGet(Hash, 1);
                //专辑图片
                string pturl = "";// KGUrlGet(Hash, 2);

                bool hasmv = false;
                if (mvhash != "") hasmv = true;
                bool fee = false;
                string privilege = job["privilege"].ToString();//价格
                if (privilege == "0" || privilege == "8") fee = false;//可听
                else fee = true;//无
                bool hasLossless = false;
                if (SQHash != "") hasLossless = true;

                MusicInfo musicInfo = new MusicInfo()
                {
                    songid = Hash,
                    songname = songname,
                    //subname = subname,
                    singer = singer,
                    //singerid = singerid,
                    albumname = albumname,
                    albumid = albumid,
                    duration = duration,
                    mvid = mvhash,
                    mp3url = mp3url,
                    pturl = pturl,
                    lrcurl = lrcurl,

                    hasLossless = hasLossless,
                    hasmv = hasmv,
                    fee = fee,
                    from = "酷狗音乐",
                };
                main.musicSearchList.Add(musicInfo);
                main.LoadToListView(main.srPage.SearchList, musicInfo);
            }
        }


        public void KGUrlGet(int s)
        {
            main = MainWindow.Instance;
            string hash = main.musicPlayingList[s].songid;
            string u = "http://www.kugou.com/yy/index.php?r=play/getdata&hash=" + hash;
            string str = "";
            str = KGGetJson(u);
            JObject o = (JObject)JsonConvert.DeserializeObject(str);
            string url = o["data"]["play_url"].ToString();
            string pturl = o["data"]["img"].ToString();
            string lrc = o["data"]["lyrics"].ToString();

            MusicInfo musicInfo = main.musicPlayingList[s];
            musicInfo.mp3url = url;
            musicInfo.pturl = pturl;
            musicInfo.lrcurl = lrc;
            main.musicPlayingList[s] = musicInfo;
        }
        public MusicInfo KGSoSoUrlGet(int s)
        {
            main = MainWindow.Instance;
            string hash = main.musicSearchList[s].songid;
            string u = "http://www.kugou.com/yy/index.php?r=play/getdata&hash=" + hash;

            string str = KGGetJson(u);
            JObject o = (JObject)JsonConvert.DeserializeObject(str);

            string url = o["data"]["play_url"].ToString();
            string pturl = o["data"]["img"].ToString();
            string lrc = o["data"]["lyrics"].ToString();

            //if (main.MusicPlayingList.Count < s) return;
            MusicInfo musicInfo = main.musicSearchList[s];
            musicInfo.mp3url = url;
            musicInfo.pturl = pturl;
            musicInfo.lrcurl = lrc;
            return musicInfo;
            //main.MusicPlayingList[s] = musicInfo;
        }
        public string KGGetJson(string u)//获取URL地址JSON编码
        {
            string t = null;
            try
            {
                // string str = string.Empty;//存储信息
                //实例化一个WebRequest对象
                WebRequest webrequest = WebRequest.Create(u);
                webrequest.Method = "GET";
                //SetHeaderValue(webrequest.Headers, "Referer", "https://y.qq.com/portal/1player.html");
                //SetHeaderValue(webrequest.Headers, "Cookie", "kg_mid=5e5bbec984e01ee36f27c46460b47d1d");
                SetHeaderValue(webrequest.Headers, "Cookie", "kg_mid=7603fe960207a0ce0bef34e99398a90c");

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
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
    }
}

using Morin.App.Common.Code;
using Morin.App.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Common.OtherAPI
{
    public class RingMusicAPI
    {
        MainWindow main = MainWindow.Instance;
        Network network = new Network();
        int SongNum = 100;

        public async void RingMusicBang()
        {
            try
            {
                main = MainWindow.Instance;
                //列表清空
                MainWindow.Instance.HotList.Items.Clear();
                string url = "http://api.ring.kugou.com/ring/all_search?q=抖音&t=1&subtype=1&p=1&pn=" + SongNum;

                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.JSON(url);//.Replace("{\"result\": ","");//存储信息
                });
                RingMusicBangJson(str);
                main.HotList.ScrollIntoView(main.HotList.Items[0]);//滚动条置顶
                main.LoadShow.IsRunning = false;
            }
            catch (Exception e) { Logger.Instance.WriteException(e); main.message.ShowMessage("酷我铃声请求错误"); }
        }
        public void RingMusicBangJson(string str1)
        {
            var main = MainWindow.Instance;
            JObject o = (JObject)JsonConvert.DeserializeObject(str1);
            //歌曲列表
            JArray jar = JArray.Parse(o["response"]["musicInfo"].ToString());
            int mn;
            if (jar.Count < SongNum) mn = jar.Count;
            else mn = SongNum;
            main.HotList.Items.Clear();//列表清空
            main.musicTopList.Clear();
            for (int i = 0; i < mn; i++)
            {
                JObject job = JObject.Parse(jar[i].ToString());
                string songname = job["ringName"].ToString().Replace("/", "-").Replace("|", " ");//歌名
                string songid = "";// job["musicrid"].ToString();
                string albumname = "";// job["ringDesc"].ToString();//专辑
                long albumid = 0;// (long)job["albumid"];
                string duration = (Convert.ToInt64(job["duration"])).ToString();//音乐时间
                string singer = job["singerName"].ToString();//歌手

                string musicid = "";// songid.Replace("MUSIC_", "");
                string mp3url = job["url"].ToString();
                //歌词
                string lrcurl = "";// "https://v1.itooi.cn/kuwo/lrc?id=" + musicid;
                //专辑图片
                bool pic = job["image"].ToString().Contains("hd");
                string pturl = "";
                if (pic) pturl = job["image"]["hd"].ToString();
                //MV地址
                string mvid = "";// job["rid"].ToString();
                bool hasmv = false;
                bool hasLossless = false;
                bool fee = false;

                MusicInfo musicInfo = new MusicInfo()
                {
                    SongId = songid,
                    SongName = songname,
                    //subname = subname,
                    Singer = singer,
                    //singerid = singerid,
                    AlbumName = albumname,
                    //albumid = albummid,
                    Duration = duration,
                    MVId = mvid,
                    MP3Url = mp3url,
                    PTUrl = pturl,
                    LrcUrl = lrcurl,

                    HasLossless = hasLossless,
                    HasMV = hasmv,
                    Fee = fee,
                    From = "酷狗铃声",
                };
                main.musicTopList.Add(musicInfo);
                main.LoadToListView(main.HotList, musicInfo);
            }
        }
    }
}

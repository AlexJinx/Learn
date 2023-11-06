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
    public class QQMusicAPI
    {

        MainWindow main = MainWindow.Instance;


        //string key = ""; int NPlaying = 0;
        //获取QQ MP3链接
        public string QQMusicUrlGet(string songmid)
        {
            try
            {
                Network network = new Network();
                main = MainWindow.Instance;
                //string quality = "M500" + songmid + ".mp3";
                //string url1 = "https://c.y.qq.com/base/fcgi-bin/fcg_music_express_mobile3.fcg?g_tk=1278911659&hostUin=0&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&cid=205361747&uin=0&songmid=" + songmid + "&filename=" + quality + "&guid=3655047200";
                string url2 = "https://u.y.qq.com/cgi-bin/musicu.fcg?format=json&data=%7B%22req_0%22%3A%7B%22module%22%3A%22vkey.GetVkeyServer%22%2C%22method%22%3A%22CgiGetVkey%22%2C%22param%22%3A%7B%22guid%22%3A%22358840384%22%2C%22songmid%22%3A%5B%22" +
                    songmid + "%22%5D%2C%22songtype%22%3A%5B0%5D%2C%22uin%22%3A%221443481947%22%2C%22loginflag%22%3A1%2C%22platform%22%3A%2220%22%7D%7D%2C%22comm%22%3A%7B%22uin%22%3A%2218585073516%22%2C%22format%22%3A%22json%22%2C%22ct%22%3A24%2C%22cv%22%3A0%7D%7D";
                string str = network.QQJSON(url2);
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                JArray jar = JArray.Parse(o["req_0"]["data"]["midurlinfo"].ToString());
                JObject job = JObject.Parse(jar[0].ToString());
                string purl = job["purl"].ToString();
                JArray jar2 = JArray.Parse(o["req_0"]["data"]["sip"].ToString());
                string sip = jar2[0].ToString();
                string Mp3Url = sip + purl;
                return Mp3Url;
            }
            catch (Exception e) { Logger.Instance.WriteException(e); return ""; }
        }

        //QQ音乐搜索
        public void QQMusicSoso()//【QQ音乐】官方
        {
            main = MainWindow.Instance;
            try
            {
                QQJsonParse(MainWindow.Instance.SearchBox.Text);
            }
            catch (Exception e)
            {
                Logger.Instance.WriteException(e); main.message.ShowMessage("QQ音乐搜索出错无结果，请重试");
            }
        }
        public async void QQJsonParse(string text)
        {
            try
            {
                Network network = new Network();
                main = MainWindow.Instance;
                int p = 1;//页数
                //SongNum = 60;//获取的歌曲总数, 一页最多60首
                string url = "https://c.y.qq.com/soso/fcgi-bin/client_search_cp?g_tk=5381&p=" + p + "&n=" + 60 + "&w=" + text + "&format=json&inUin=0&hostUin=0&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&remoteplace=txt.yqq.song&t=0&aggr=1&cr=1&catZhida=1&flag_qc=0";
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.JSON(url);
                });
                main.LoadShow.IsRunning = false;
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                //歌曲列表
                JArray jar = JArray.Parse(o["data"]["song"]["list"].ToString());
                main.musicSearchList.Clear();
                for (int i = 0; i < jar.Count; i++)
                {
                    JObject job = JObject.Parse(jar[i].ToString());
                    string songname = job["songname"].ToString().Replace("/", "-");//歌名
                    string songid = job["songmid"].ToString();
                    string subname = job["lyric"].ToString();//副名

                    string albumname = job["albumname"].ToString();//专辑
                    string albummid = job["albummid"].ToString();
                    long albumid = (long)job["albumid"];
                    string duration = job["interval"].ToString();//音乐时间
                    string mp3url = "";// QQMusicGet(songmid);//MP3链接
                    //string songid = job["songid"].ToString();
                    string mv = job["vid"].ToString();
                    string singerid = "";

                    //专辑图片http://imgcache.qq.com/music/photo/album_300/36300_albumpic_9218636_0.jpg
                    string pturl = "http://y.gtimg.cn/music/photo_new/T002R500x500M000" + albummid + ".jpg";
                    //string pturl = "http://imgcache.qq.com/music/photo/album_300/" + (albumid % 100) + "/300_albumpic_" + albumid + "_0.jpg";
                    string lrcurl = "https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?g_tk=753738303&songmid=" + songid;
                    JArray jar2 = JArray.Parse(job["singer"].ToString());
                    string singer = "";//歌手
                    for (int i2 = 0; i2 < jar2.Count; i2++)
                    {
                        JObject job2 = JObject.Parse(jar2[i2].ToString());
                        string f = " & ";
                        if (i2 == jar2.Count - 1) f = "";
                        singer += job2["name"].ToString().Replace("/", "-") + f;
                        if (singerid == "") singerid = job2["mid"].ToString();
                    }
                    string mvid = job["vid"].ToString();
                    bool hasmv = false;
                    //if (mvid != null) hasmv = "1";
                    bool hasLossless = false;
                    string pay = job["pay"]["payplay"].ToString();//是否支付
                    bool fee = false;
                    if (pay == "1") fee = true;
                    else fee = false;

                    MusicInfo musicInfo = new MusicInfo()
                    {
                        SongId = songid,
                        SongName = songname,
                        SubName = subname,
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
                        Fee = fee,
                        From = "QQ音乐",
                    };
                    main.musicSearchList.Add(musicInfo);
                    main.LoadToListView(main.srPage.SearchList, musicInfo);
                }
            }
            catch (Exception e) { Logger.Instance.WriteException(e); _ = main.message.ShowMessage("QQ音乐加载出错！请重试"); }
        }

        public string QQGetLrc(string url)
        {
            Network network = new Network();
            string lrc = "";
            string str = network.QQLrcJson(url);
            try
            {
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                lrc = o["lyric"].ToString();//歌名
                if (lrc == null || lrc == "")
                {
                    return "";
                }
            }
            catch { }
            byte[] bytes = Convert.FromBase64String(lrc);
            return Encoding.UTF8.GetString(bytes);
        }

        public async void QQSingleResol(string url)
        {
            main = MainWindow.Instance;
            try
            {
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = main.network.JSON(url);
                });
                main.LoadShow.IsRunning = false;

                Match Duration1 = Regex.Match(str, "g_SongData = {.+?};");
                string SongData = Duration1.Groups[0].Value.Replace("g_SongData = ", "").Replace(";", "");
                if (SongData == "") return;
                JObject job = JObject.Parse(SongData);
                string songname = job["songname"].ToString().Replace("/", "-");//歌名
                string songmid = job["songmid"].ToString();
                //string subname = job["lyric"].ToString();//副名

                string albumname = job["albumname"].ToString();//专辑
                string albummid = job["albummid"].ToString();
                long albumid = (long)job["albumid"];
                string time = job["interval"].ToString();//音乐时间
                string mp3url = QQMusicUrlGet(songmid);//MP3链接
                                                       //专辑图片
                                                       //int yu= Convert.ToInt32(albumid) % 100;
                string pturl = "http://imgcache.qq.com/music/photo/album_300/" + (albumid % 100) + "/300_albumpic_" + albumid + "_0.jpg";
                JArray jar2 = JArray.Parse(job["singer"].ToString());
                string pay = job["pay"]["payplay"].ToString();//是否支付
                string singer = "";//歌手
                for (int i2 = 0; i2 < jar2.Count; i2++)
                {
                    JObject job2 = JObject.Parse(jar2[i2].ToString());
                    string f = " ";
                    if (i2 == jar2.Count - 1) f = "";
                    singer += job2["name"].ToString().Replace("/", "-") + f;
                }

                main.musicParsSave[0] = songmid;
                main.musicParsSave[1] = songname;
                //main.MusicParsSave[2] = subname;
                main.musicParsSave[3] = singer;
                //main.MusicParsSave[4] = singermid;
                main.musicParsSave[5] = albumname;
                main.musicParsSave[6] = albummid;
                main.musicParsSave[7] = time;
                main.musicParsSave[8] = albumid.ToString();
                main.musicParsSave[9] = mp3url;
                main.musicParsSave[10] = pturl;
                main.musicParsSave[11] = "";
                main.musicParsSave[12] = "";
                main.musicParsSave[13] = "";
                main.musicParsSave[17] = pay;
                main.musicParsSave[18] = "QQ音乐";
                //解析添加到下载列表
                main.network.SingleDownToList();
            }
            catch { main.message.ShowMessage("QQ音乐智能解析出错，请重试"); }
        }


        public async Task<MusicCardInfo> getSonglist(string id)
        {
            try
            {
                Network network = new Network();
                MainWindow main = MainWindow.Instance;
                List<MusicInfo> musicInfos = new List<MusicInfo>();
                //根据链接取出ID
                string url = "https://c.y.qq.com/qzone/fcg-bin/fcg_ucc_getcdinfo_byids_cp.fcg?type=1&json=1&utf8=1&onlysong=0&format=jsonp&g_tk=5381&jsonpCallback=playlistinfoCallback&loginUin=0&hostUin=0&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&disstid=" + id;
                //string cookie = "os=pc; osver=Microsoft-Windows-10-Professional-build-10586-64bit; appver=2.0.2; channel=netease; __remember_me=true";
                string str = "";
                //main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.QQLrcJson(url);//存储信息
                });
                //main.LoadShow.IsRunning = false;
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                //歌曲列表
                JArray jar = JArray.Parse(o["cdlist"].ToString());
                JObject job00 = JObject.Parse(jar[0].ToString());
                JArray jarlist = JArray.Parse(job00["songlist"].ToString());
                string name = job00["dissname"].ToString();
                string info = job00["desc"].ToString();
                string coverImgUrl = job00["logo"].ToString();
                int Count = jarlist.Count;
                for (int i = 0; i < Count; i++)
                {
                    JObject job = JObject.Parse(jarlist[i].ToString());

                    string songname = job["songname"].ToString().Replace("/", "-");//歌名
                    string songid = job["songmid"].ToString();
                    //string subname = job["lyric"].ToString();//副名

                    string albumname = job["albumname"].ToString();//专辑
                    string albummid = job["albummid"].ToString();
                    string albumid = job["albumid"].ToString();
                    string duration = job["interval"].ToString();//音乐时间
                    string mp3url = "";// QQMusicGet(songmid);//MP3链接
                    //string songid = job["songid"].ToString();
                    string mv = job["vid"].ToString();
                    string singerid = "";

                    //专辑图片http://imgcache.qq.com/music/photo/album_300/36300_albumpic_9218636_0.jpg
                    string pturl = "http://y.gtimg.cn/music/photo_new/T002R500x500M000" + albummid + ".jpg";
                    //string pturl = "http://imgcache.qq.com/music/photo/album_300/" + (albumid % 100) + "/300_albumpic_" + albumid + "_0.jpg";
                    string lrcurl = "https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?g_tk=753738303&songmid=" + songid;
                    JArray jar2 = JArray.Parse(job["singer"].ToString());
                    string singer = "";//歌手
                    for (int i2 = 0; i2 < jar2.Count; i2++)
                    {
                        JObject job2 = JObject.Parse(jar2[i2].ToString());
                        string f = " & ";
                        if (i2 == jar2.Count - 1) f = "";
                        singer += job2["name"].ToString().Replace("/", "-") + f;
                        if (singerid == "") singerid = job2["mid"].ToString();
                    }
                    string mvid = job["vid"].ToString();
                    bool hasmv = false;
                    //if (mvid != null) hasmv = "1";
                    bool hasLossless = false;
                    string pay = job["pay"]["payplay"].ToString();//是否支付
                    bool fee = false;
                    if (pay == "1") fee = true;
                    else fee = false;

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
                        Fee = fee,
                        From = "QQ音乐",
                    };
                    musicInfos.Add(musicInfo);
                }
                MusicCardInfo musicCardInfo = new MusicCardInfo();
                musicCardInfo.Musics = musicInfos;
                musicCardInfo.Name = name;
                musicCardInfo.Info = info;
                musicCardInfo.Pic = coverImgUrl;

                return musicCardInfo;
            }
            catch
            {
                main.message.ShowMessage("QQ音乐歌单解析出错，请重试");
            }
            return new MusicCardInfo();
        }
    }
}

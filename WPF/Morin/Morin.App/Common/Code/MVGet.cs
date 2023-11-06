using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Morin.App.Common.Code
{
    public class MVGet
    {
        Network network = new Network();
        TransformType TransformType = new TransformType();
        MainWindow main = MainWindow.Instance;


        public void SearchMvAdd()
        {
            MainWindow main = MainWindow.Instance;
            int sel, i = 0; string mvid;
            if (main.Interface() == 1)
            {
                sel = main.搜索引擎.SelectedIndex;
                i = main.srPage.SearchList.SelectedIndex;
            }
            else
            {
                sel = 2;
                i = main.slRPage.SongList.SelectedIndex;
            }

            mvid = main.musicSearchList[i].MVId;
            switch (sel)
            {
                case 0:
                    QQMvGet(mvid);
                    break;
                case 1:
                    NTMvGet(mvid);
                    break;
                case 2:
                    KWMvGet(mvid);
                    break;
                case 3:
                    KGMvGet(mvid);
                    break;
            }

        }
        public string pturl = "", name = "", singer = "", time = "", publishTime = "", B240 = "", B480 = "", B720 = "", B1080 = "";


        public async void QQMvGet(string mvid)
        {
            return;
            string url = "https://v1.itooi.cn/tencent/mv?id=" + mvid;
            main = MainWindow.Instance;
            string mvstr = "";
            main.LoadShow.IsRunning = true;
            await Task.Run(() =>
            {
                mvstr = network.JSON(url).Replace("\"", "");
            });
            main.LoadShow.IsRunning = false;

            Match Duration = Regex.Match(mvstr, "name:.+?,");
            name = Duration.Groups[0].Value.Replace("name:", "").Replace(",", "");

            Match Duration1 = Regex.Match(mvstr, "signer:.+?,");
            singer = Duration1.Groups[0].Value.Replace("signer:", "").Replace(",", "");

            Match Duration2 = Regex.Match(mvstr, "pic:.+?,");
            pturl = Duration2.Groups[0].Value.Replace("pic:", "").Replace(",", "");

            Match Duration3 = Regex.Match(mvstr, "time:.+?,");
            time = Duration3.Groups[0].Value.Replace("time:", "").Replace(",", "") + "000";

            publishTime = "";

            string brt = "https://v1.itooi.cn/tencent/mvUrl?id=" + mvid + "&quality=";
            B240 = network.GetRealUrl(brt + "240");
            B480 = network.GetRealUrl(brt + "480");
            B720 = network.GetRealUrl(brt + "720");
            B1080 = network.GetRealUrl(brt + "1080");

            main.mvBit();
        }

        public async void NTMvGet(string mvid)
        {
            string url = "http://music.163.com/api/mv/detail?id=" + mvid + "&type=mp4";
            main = MainWindow.Instance;
            string mvstr = "";
            main.LoadShow.IsRunning = true;
            await Task.Run(() =>
            {
                mvstr = network.JSON(url).Replace("\"", "");
            });
            main.LoadShow.IsRunning = false;

            Match Duration = Regex.Match(mvstr, "name:.+?,");
            name = Duration.Groups[0].Value.Replace("name:", "").Replace(",", "");

            Match Duration1 = Regex.Match(mvstr, "artistName:.+?,");
            singer = Duration1.Groups[0].Value.Replace("artistName:", "").Replace(",", "");

            Match Duration2 = Regex.Match(mvstr, "cover:.+?,");
            pturl = Duration2.Groups[0].Value.Replace("cover:", "").Replace(",", "");

            Match Duration3 = Regex.Match(mvstr, "duration:.+?,");
            time = Duration3.Groups[0].Value.Replace("duration:", "").Replace(",", "");

            Match Duration4 = Regex.Match(mvstr, "publishTime:.+?,");
            publishTime = Duration4.Groups[0].Value.Replace("publishTime:", "").Replace(",", "");

            Match Duration5 = Regex.Match(mvstr, "240:.+?,");
            B240 = Duration5.Groups[0].Value.Replace("240:", "").Replace(",", "").Replace("}", "");
            Match Duration6 = Regex.Match(mvstr, "480:.+?,");
            B480 = Duration6.Groups[0].Value.Replace("480:", "").Replace(",", "").Replace("}", "");
            Match Duration7 = Regex.Match(mvstr, "720:.+?,");
            B720 = Duration7.Groups[0].Value.Replace("720:", "").Replace(",", "").Replace("}", "");
            Match Duration8 = Regex.Match(mvstr, "1080:.+?,");
            B1080 = Duration8.Groups[0].Value.Replace("1080:", "").Replace(",", "").Replace("}", "");

            main.mvBit();
        }

        public async void KWMvGet(string mvid)
        {
            main = MainWindow.Instance;
            string mvurl = "";
            main.LoadShow.IsRunning = true;
            await Task.Run(() =>
            {
                mvurl = network.JSON("http://antiserver.kuwo.cn/anti.s?response=url&format=mp4%7Cmkv&type=convert_url&rid=" + mvid);
            });

            pturl = ""; name = ""; singer = ""; time = ""; publishTime = "";
            B240 = mvurl;
            B480 = mvurl;
            B720 = mvurl;

            main.mvBit();
            main.LoadShow.IsRunning = false;
        }

        public async void KGMvGet(string mvid)
        {
            main = MainWindow.Instance;
            try
            {
                string md5 = TransformType.MD5Encoding(mvid, "kugoumvcloud");
                string url = "http://trackermv.kugou.com/interface/index/cmd=100&hash=" + mvid + "&key=" + md5 + "&ext=mp4";

                string mvstr = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    mvstr = network.JSON(url);
                });
                main.LoadShow.IsRunning = false;

                JObject mvobj = (JObject)JsonConvert.DeserializeObject(mvstr);
                pturl = "";
                name = mvobj["songname"].ToString();
                singer = mvobj["singer"].ToString();
                time = mvobj["mvdata"]["sd"]["timelength"].ToString();
                publishTime = "";

                B240 = mvobj["mvdata"]["sd"]["downurl"].ToString();
                B480 = mvobj["mvdata"]["hd"]["downurl"].ToString();
                B720 = mvobj["mvdata"]["sq"]["downurl"].ToString();
                B1080 = mvobj["mvdata"]["rq"]["downurl"].ToString();

                main.mvBit();
            }
            catch { main.message.ShowMessage("播放失败，请重试..."); }
        }
    }
}

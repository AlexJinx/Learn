using Morin.App.Common.Code;
using Morin.App.Model;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Morin.App.Common.OtherAPI
{
    public class FmGet
    {
        MainWindow main = MainWindow.Instance;
        Network network = new Network();
        FmAPI fmAPI = new FmAPI();

        //获取电台列表
        public async void getFmList()
        {
            main = MainWindow.Instance;
            try
            {
                //列表清空
                //main.SRPage.SearchList.Items.Clear();
                string url = fmAPI.getFmList_Url();
                string str = "";
                main.LoadShow.IsRunning = true;
                await Task.Run(() =>
                {
                    str = network.JSON(url);//.Replace("{\"result\": ","");//存储信息
                });
                FmListJson(str);
                main.LoadShow.IsRunning = false;
            }
            catch (Exception e) { main.message.ShowMessage("获取电台列表出错，请重试"); }
        }

        //fm列表
        public void FmListJson(string str1)
        {
            JObject o = (JObject)JsonConvert.DeserializeObject(str1);
            JArray jar = JArray.Parse(o["data"].ToString());
            main = MainWindow.Instance;
            main.FmList.Items.Clear();
            int mn = jar.Count;
            for (int i = 0; i < mn; i++)
            {
                JObject job = JObject.Parse(jar[i].ToString());
                string id = job["id"].ToString();
                string title = job["title"].ToString().Replace("/", "-");
                string cover = job["cover"].ToString().Replace("!120x120", "");
                string url = job["url"].ToString();
                string speak = job["speak"].ToString();
                string favnum = job["favnum"].ToString();
                string viewnum = job["viewnum"].ToString();

                FmDetailInfo fmDetail = new FmDetailInfo
                {
                    Id = id,
                    Title = title,
                    Cover = cover,
                    Url = url,
                    Speak = speak,
                    Favnum = favnum,
                    Viewnum = viewnum
                };
                main.fmListInfo.Add(fmDetail);

                main.FmList.ItemTemplate = main.FindResource("FmListDataTemplate") as DataTemplate;
                main.FmList.Items.Add(new FMListInfo
                {
                    ID = main.FmList.Items.Count + 1,
                    Pic = cover,
                    Name = title,
                    Speak = "" + speak,
                    Favnum = favnum,
                    Listen = " " + viewnum,
                    Total = " ",
                });

            }
        }
    }
}

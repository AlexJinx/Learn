using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Common.OtherAPI
{
    public class FmAPI
    {
        string key = "046b6a2a43dc6ff6e770255f57328f89";

        //获取电台列表
        public string getFmList_Url()
        {
            string url = "http://yiapi.xinli001.com/fm/newfm-list.json?offset=0&limit=20&key=" + key;
            return url;
        }
        //获取电台详情
        public string getFmDetail_Url(string id)
        {
            string url = "http://yiapi.xinli001.com/fm/broadcast-detail.json?id=" + id;
            return url;
        }

        //获取主播列表
        public string getSpeakerList_Url()
        {
            string url = "http://yiapi.xinli001.com/fm/diantai-new-list.json?offset=0&limit=20&key=" + key;
            return url;
        }
        //获取主播详情
        public string getSpeakerDetail_Url(string id)
        {
            string url = "http://yiapi.xinli001.com/fm/diantai-detai.json?id=" + id;
            return url;
        }
    }
}

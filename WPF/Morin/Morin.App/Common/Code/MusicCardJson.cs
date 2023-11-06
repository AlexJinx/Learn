using Morin.App.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Morin.App.Common.Code
{
    class MusicCardJson
    {
        //写
        public void WriteJsonFile(string path, List<MusicCardInfo> list)
        {
            string str = JsonListToString(list);
            FileWrite(path, str);
        }

        //读
        public List<MusicCardInfo> ReadJsonFile(string path)
        {
            string str = FileRead(path);
            return StringToJsonList(str);
        }


        //3. 将List<T>数据转为Json数据
        //JSON序列化,将List<T>转换为String
        private string JsonListToString(List<MusicCardInfo> list)
        {
            JavaScriptSerializer Serializerx = new JavaScriptSerializer();
            string changestr = Serializerx.Serialize(list);
            return changestr;
        }


        //4. 将Json数据写入文件系统
        //写入文件
        private void FileWrite(string filepath, string writestr)
        {
            FileStream fs = new FileStream(filepath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(writestr);
            sw.Close();
            fs.Close();
        }


        //5.  从文件中读取Json数据
        // string str = FileRead("test.txt");
        //读取文件
        private string FileRead(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return str;
        }

        //6. 将Json数据转换为List<T>数据
        //JSON反序列化,将List<T>转换为String
        private List<MusicCardInfo> StringToJsonList(string str)
        {
            List<MusicCardInfo> face;
            try
            {
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                face = Serializer.Deserialize<List<MusicCardInfo>>(str);
            }
            catch
            {
                return null;
            }
            return face;
        }
    }
}

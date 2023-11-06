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
    public class JsonRWInfo
    {

        //写
        public void WriteJsonFile(string path, List<MusicInfo> list)
        {
            string str = JsonListToString(list);
            FileWrite(path, str);
        }

        //读
        public List<MusicInfo> ReadJsonFile(string path)
        {
            MainWindow main = MainWindow.Instance;
            string str = FileRead(path);
            return StringToJsonList(str);
        }


        //3. 将List<T>数据转为Json数据
        //JSON序列化,将List<T>转换为String
        private string JsonListToString(List<MusicInfo> list)
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
        private List<MusicInfo> StringToJsonList(string str)
        {
            List<MusicInfo> face = new List<MusicInfo>();
            try
            {
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                face = Serializer.Deserialize<List<MusicInfo>>(str);
            }
            catch
            {
                File.Delete(MainWindow.Instance.MyLikeSongPath);
                return null;
            }
            return face;
        }
    }
}

using System;
using System.Text;

namespace Morin.App.Common.Code
{
    public class IniHelper
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string SPath = string.Empty;
        public IniHelper(string path)
        {
            SPath = AppDomain.CurrentDomain.BaseDirectory + path;
        }

        public void WriteValue(string section, string key, string value)
        {
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(section, key, value, SPath);
        }

        public string ReadValue(string section, string key)
        {
            // 每次从ini中读取多少字节
            StringBuilder temp = new System.Text.StringBuilder(2550);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 2550, SPath);
            return temp.ToString();
        }
    }
}

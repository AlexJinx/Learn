using Morin.App.Common.Code;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.DB
{
    public class UserStatistics
    {
        MySQL mySQL = new MySQL();

        //用户信息
        public void StartRegister()
        {
            ThreadPool.QueueUserWorkItem((object state) =>
            {
                string IP = string.Empty;
                int Day = -1;
                int Count = 1;
                string Address = GetCity(out IP);
                string User = Environment.UserName;
                string LastDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");//"yyyy/MM/dd hh:mm"
                string StartDate = LastDate;
                string OSVersion = GetOSName();
                string Version = Application.ResourceAssembly.GetName().Version.ToString();
                string MachineCode = getMNum();//获取机器码

                //检查是否存在
                bool SelectExists = mySQL.SelectExists(String.Format("select * from UserInfo where MachineCode = '{0}';", MachineCode));
                string Content;
                if (!SelectExists)
                {
                    Content = String.Format("insert into UserInfo values('{0}',{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                        IP, Count, Day, Address, User, StartDate, LastDate, OSVersion, Version, MachineCode);
                }
                else
                {
                    Count = mySQL.SelectCount(String.Format("select * from UserInfo where MachineCode = '{0}';", MachineCode));

                    if (Count == -1)
                    {
                        Content = String.Format("update UserInfo set IP='{0}',Day='{2}' ," +
                            "Address='{3}' ,User='{4}'  ,LastDate='{6}' ,OSVersion='{7}',Version='{8}' " +
                            " where MachineCode='{9}'",
                        IP, Count, Day, Address, User, StartDate, LastDate, OSVersion, Version, MachineCode);
                    }
                    else
                    {
                        Count++;
                        Content = String.Format("update UserInfo set IP='{0}',Count='{1}' ,Day='{2}' ," +
                            "Address='{3}' ,LastDate='{6}' ,OSVersion='{7}',Version='{8}' " +
                            " where MachineCode='{9}'",
                        IP, Count, Day, Address, User, StartDate, LastDate, OSVersion, Version, MachineCode);
                    }
                }
                mySQL.Add_Delete_Revise(Content);

            }, null);

        }




        public string GetCity(out string ip)
        {
            try
            {
                Network network = new Network();
                //string sURL = "http://pv.sohu.com/cityjson/"+ strIP;
                string sURL = "http://pv.sohu.com/cityjson?ie=utf-8";
                string str = "";
                str = network.JSON(sURL).Replace("var returnCitySN = ", "").Replace(";", "");
                JObject o = (JObject)JsonConvert.DeserializeObject(str);
                ip = o["cip"].ToString();
                return o["cname"].ToString();
            }
            catch
            {
                ip = "";
                return "未知";
            }
        }

        /// <summary>
        /// 得到当前正在运行的操作系统的名称。 比如： 
        /// "Microsoft Windows 7 Enterprise".
        /// </summary>
        /// <returns></returns>
        public string GetOSName()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Caption"] as string;
                }
            }
            catch { return ""; }
            return "";
        }

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <returns></returns>
        public static string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            return strMNum;
        }
        public int[] intCode = new int[127];//存储密钥
        public int[] intNumber = new int[25];//存机器码的Ascii值
        public char[] Charcode = new char[25];//存储机器码字
        public void setIntCode()//给数组赋值小于10的数
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }
        /// <summary>
        /// 取得设备硬盘的卷标号
        /// </summary>
        /// <returns></returns>
        public static string GetDiskVolumeSerialNumber()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                disk.Get();
                return disk.GetPropertyValue("VolumeSerialNumber").ToString();
            }
            catch { return ""; }
        }
        /// <summary>
        /// 获得CPU的序列号
        /// </summary>
        /// <returns></returns>
        public static string getCpu()
        {
            try
            {
                string strCpu = null;
                ManagementClass myCpu = new ManagementClass("win32_Processor");
                ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
                foreach (ManagementObject myObject in myCpuConnection)
                {
                    strCpu = myObject.Properties["Processorid"].Value.ToString();
                    break;
                }
                return strCpu;
            }
            catch { return ""; }
        }
    }
}

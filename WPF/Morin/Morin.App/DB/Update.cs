using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.DB
{
    public class Update
    {
        MainWindow main = MainWindow.Instance;

        UserStatistics UserStatistics = new UserStatistics();
        MySQL mySQL = new MySQL();
        Message message = new Message();

        public string NewVersion = null;
        public async void UpdateVersion()
        {
            try
            {
                main = MainWindow.Instance;
                ButtonHelper.SetIsWaiting(main.update, true);
                UpdateInfo updateInfo = mySQL.SelectUpdateInfo(String.Format("select * from UpdateInfo where New = '1';"));
                ButtonHelper.SetIsWaiting(main.update, false);


                //读取更新信息
                string NowVersion = Application.ResourceAssembly.GetName().Version.ToString();
                NewVersion = updateInfo.Version;
                string DownUrl = updateInfo.DownUrl;
                MainWindow.Instance.DownUrl = DownUrl;
                string Info = updateInfo.Info;
                if (!CompareVersion(NewVersion, NowVersion))
                { return; }
                bool r;
                if (updateInfo.Necessary)
                {
                    r = message.ShowMessage("检测到新版本：" + NewVersion + " 是否更新？\n\n" + Info, "更新", "退出");
                    if (!r) Environment.Exit(0);
                }
                else
                {
                    r = message.ShowMessage("检测到新版本：" + NewVersion + " 是否更新？\n\n" + Info, "更新", "取消");
                    if (!r) return;
                }
                //打开更新程序
                Page.Updatexaml updatexaml = new Page.Updatexaml();
                updatexaml.ShowDialog();

            }
            catch { }
        }

        /// <summary>
        /// 版本号比较：new_version>=old_version
        /// </summary>
        /// <param name="new_version">8.4.0</param>
        /// <param name="old_version">8.3.10</param>
        /// <returns></returns>
        public static bool CompareVersion(string new_version, string old_version)
        {
            try
            {
                if (string.IsNullOrEmpty(new_version) || string.IsNullOrEmpty(old_version))
                    return false;

                Version v_new = new Version(new_version);
                Version v_old = new Version(old_version);
                if (v_new > v_old)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

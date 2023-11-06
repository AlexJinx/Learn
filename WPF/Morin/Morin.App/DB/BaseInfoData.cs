using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.DB
{
    public class BaseInfoData
    {



        /// <summary>
        /// 版本号比较
        /// </summary>
        public static bool CompareVersion(string new_version, string old_version)
        {
            if (string.IsNullOrEmpty(new_version) || string.IsNullOrEmpty(old_version))
            {
                return false;
            }

            Version v_new = new Version(new_version);
            Version v_old = new Version(old_version);
            if (v_new > v_old)
            {
                return true;
            }

            return false;
        }
    }
}

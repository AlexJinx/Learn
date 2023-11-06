using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Model
{
    public class UpdateInfo
    {
        public string Version { get; set; }

        public bool Necessary { get; set; }

        public string DownUrl { get; set; }

        public string Info { get; set; }

        public string Data { get; set; }
    }
}
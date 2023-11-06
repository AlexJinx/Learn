using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Model
{
    public class MusicCardInfo
    {
        public string Name { get; set; }

        public string Pic { get; set; }

        public string Info { get; set; }

        public List<MusicInfo> Musics { get; set; }
    }
}

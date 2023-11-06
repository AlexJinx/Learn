using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.App.Model
{
    public class MusicInfo
    {
        public string SongId { get; set; }

        public string SongName { get; set; }

        public string SubName { get; set; }

        public string Singer { get; set; }

        public string SingerId { get; set; }

        public string AlbumName { get; set; }

        public string AlbumId { get; set; }

        public string Duration { get; set; }

        public string MVId { get; set; }

        public string From { get; set; }

        public string MP3Url { get; set; }

        public string PTUrl { get; set; }

        public string LrcUrl { get; set; }

        public bool HasMV { get; set; }

        public bool HasLossless { get; set; }

        public bool Fee { get; set; }

        //收藏
        public string LikeDate { get; set; }

        //本地
        public string Ext { get; set; }
        public string Data { get; set; }
        public string Path { get; set; }

        //下载
        public string SongPath { get; set; }

        public string PicPath { get; set; }

        public string Suffix { get; set; }

        public string State { get; set; }

        public string Quality { get; set; }

        public int Progress { get; set; }

    }
}

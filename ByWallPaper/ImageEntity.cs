using System;
using System.Drawing;

namespace ByWallPaper
{
    public class ImageEntity
    {
        public string Url { get; set; }

        public string Descr { get; set; }

        public DateTime Time { get; set; }

        public Image Image { get; set; }
    }
}

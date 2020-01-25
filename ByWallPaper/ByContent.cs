using System.Drawing;

namespace ByWallPaper
{
    /// <summary>
    /// 必应壁纸解析内容
    /// </summary>
    public class ByContent
    {
        /// <summary>
        /// 图片描述
        /// </summary>
        public string Descr { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 图片路径地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public Image Image { get; set; }
    }
}

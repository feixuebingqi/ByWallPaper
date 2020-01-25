using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.InteropServices;

namespace ByWallPaper
{
    public class ImageApi
    {
        public async static Task<ByContent> GetContentImage(string url, Encoding encoding)
        {
            ByContent content = new ByContent();

            var xmlData =await GetByFile(url, encoding);
            var root = XDocument.Parse(xmlData).Descendants();

            content.Date = root.Elements("enddate").FirstOrDefault().Value;
            content.Descr = root.Elements("copyright").FirstOrDefault().Value;
            content.ImageUrl = "http://www.bing.com" + root.Elements("url").FirstOrDefault().Value;
            content.Image = GetImageFromUrl(content.ImageUrl);

            return content;
        }

        /// <summary>
        /// 获取必应地址下内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal async static Task<string> GetByFile(string url, Encoding encoding)
        {
            string result = "";

            await Task.Run(() =>
            {
                using (var stream = GetHtmlStream(url))
                {
                    using (StreamReader sr = new StreamReader(stream, encoding))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            });
           
            return result;
        }


        /// <summary>
        /// 从图片地址中获取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static Image GetImageFromUrl(string url)
        {
            Image image = null;
            using (var stream = GetHtmlStream(url))
            {
                image = Image.FromStream(stream);
            }
            return image;
        }


        /// <summary>
        /// 获取网页数据流
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>数据流</returns>
        internal static Stream GetHtmlStream(string url)
        {
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            return resp.GetResponseStream();
        }


        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
            int uAction,
            int uParam,
            string lovParam,
            int fulWinIni
            );


        /// <summary>
        /// 设置背景图片
        /// </summary>
        /// <param name="strSavePath">图片文件绝对路径</param>
        public static void SetWallPaper(string strSavePath)
        {
            if (File.Exists(strSavePath))
            {
                SystemParametersInfo(20, 1, strSavePath, 1);
            }
        }
    }
}

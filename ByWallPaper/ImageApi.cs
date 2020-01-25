using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace ByWallPaper
{
    public class ImageApi
    {

        public const string RegexStr= "<Url>(?<Url>.*?)</Url>";

        public async static Task<ByContent> GetContentImage(string url, Encoding encoding, string dpi)
        {
            ByContent entity = new ByContent();

            Regex regex = new Regex(RegexStr, RegexOptions.IgnoreCase);

            await Task.Run(() =>
            {
                entity.Content = GetByFile(url, encoding);
                MatchCollection collection = regex.Matches(entity.Content);

                entity.Images = new ImageEntity[collection.Count];

                for (int i = 0; i < collection.Count; i++)
                {
                    string path = "http://www.bing.com" + collection[0].Groups["Url"].Value;
                    entity.Images[i] = new ImageEntity();
                    entity.Images[i].Url = path;
                    entity.Images[i].Image = GetImageFromUrl(path);
                }

            });

            return entity;
        }

        /// <summary>
        /// 获取必应地址下内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal static string GetByFile(string url, Encoding encoding)
        {
            string result = "";
            using (var stream = GetHtmlStream(url)) 
            {
                using (StreamReader sr = new StreamReader(stream, encoding)) 
                {
                    result = sr.ReadToEnd();
                }
            }

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
    }
}

using System.IO;
using System.Runtime.InteropServices;

namespace ByWallPaper
{
    public class WallPaperHelper
    {

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

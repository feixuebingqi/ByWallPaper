using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ByWallPaper;
using System.Drawing;

namespace WallPaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 必应壁纸的URL地址
        const string ByUrl = "https://cn.bing.com/HPImageArchive.aspx?idx=4&n=1";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbDpi.SelectedIndex = 0;
        }

        private async void getImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取壁纸内容
                var imgs = await ImageApi.GetContentImage(ByUrl, Encoding.UTF8, cbDpi.Text.Trim());

                if (imgs.Content.Length > 0)
                {
                    BitmapImage img = new BitmapImage();

                    img.BeginInit();
                    img.UriSource = new Uri(imgs.Images[0].Url);
                    img.EndInit();

                    byImage.Source = img;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setWallPaper_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}

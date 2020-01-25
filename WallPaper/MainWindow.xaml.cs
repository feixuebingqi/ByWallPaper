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
using System.IO;
using System.Configuration;

namespace WallPaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 必应壁纸的URL地址
        const string ByUrl = "https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1";
        ByContent content = new ByContent();
        readonly string Dir = ConfigurationManager.AppSettings["dir_path"];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void getImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取壁纸内容
                content = await ImageApi.GetContentImage(ByUrl, Encoding.UTF8);

                byImage.Source = Converter.Image2ImageSource(content.Image);

                lblDescr.Content = content.Descr;
                lblDate.Content = "日期：" + content.Date;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setWallPaper_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }
                string file = System.IO.Path.Combine(Dir, content.Date + ".jpg");

                if (content.Image != null)
                {
                    // 1. Save image
                    content.Image.Save(file);
                    // 2. Set wall paper
                    ImageApi.SetWallPaper(file);

                    MessageBox.Show("Wall paper set successfully", "Wall paper", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please load the picture first!", "Load", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       
    }
}

# 必应壁纸

## 最近想换下电脑桌面的壁纸，发现必应壁纸非常漂亮、质量非常高，而且必应官网每天都会更新一张图片，于是想写一个小程序直接获取图片，并且可以设置桌面壁纸，觉得挺好玩，给大家分享下......

## 1. 界面

![](C:\Users\Du\Desktop\image\批注 2020-01-25 221304.jpg)

## 2. 必应壁纸地址

[必应壁纸地址 <https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1>](https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1)

+ 该地址为必应提供的XML文件地址，访问该地址可以获取必应壁纸的内容

  ```xml
  <images>
  <image>
  <startdate>20200124</startdate>
  <fullstartdate>202001240800</fullstartdate>
  <enddate>20200125</enddate>
  <url>
  /th?id=OHR.Lunarnewyear2020_ZH-CN1554492287_1920x1080.jpg&rf=LaDigue_1920x1080.jpg&pid=hp
  </url>
  <urlBase>/th?id=OHR.Lunarnewyear2020_ZH-CN1554492287</urlBase>
  <copyright>【今日春节】 (© bingdian/iStock/Getty Images Plus)</copyright>
  <copyrightlink>
  https://www.bing.com/search?q=%E6%98%A5%E8%8A%82&form=hpcapt&mkt=zh-cn
  </copyrightlink>
  <headline/>
  <drk>1</drk>
  <top>1</top>
  <bot>1</bot>
  <hotspots/>
  </image>
  <tooltips>
  <loadMessage>
  <message>正在加载...</message>
  </loadMessage>
  <previousImage>
  <text>上一个图像</text>
  </previousImage>
  <nextImage>
  <text>下一个图像</text>
  </nextImage>
  <play>
  <text>播放视频</text>
  </play>
  <pause>
  <text>暂停视频</text>
  </pause>
  </tooltips>
  </images>
  ```

+ idx后面的参数可以替换，范围由0到7，0获取当天的必应图片，1获取昨天图片，2获取

前一天的图片，以此类推。

## 3. 动态链接库

### 1. 模型类：承载解析的内容

```C#
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
```

### 2. 关键函数：提供解析功能

+ 获取Html内容

  ```C#
  /// <summary>
  /// 获取Html内容
  /// </summary>
  /// <param name="url">地址</param>
  /// <param name="encoding">编码</param>
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
  ```

+ 解析XML文件

  ```c#
  /// <summary>
  /// 解析必应地址内容
  /// </summary>
  /// <param name="url"></param>
  /// <param name="encoding"></param>
  /// <returns></returns>
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
  ```

+ 设置桌面壁纸

  ```c#
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
  ```

## 4. 主程序

### 1. 获取必应壁纸

```C#
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
```

### 2.设置桌面壁纸

```C#
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
```


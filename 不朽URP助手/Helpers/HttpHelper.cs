using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace 不朽URP助手.Helpers
{
    public class HttpHelper
    {
        public static async Task<string> GetPicBase64Async(string url)
        {
            try
            {
                //HTTP下载图片
                WebClient wc = new WebClient();
                using (var ms = new MemoryStream(await wc.DownloadDataTaskAsync(new Uri(url))))
                {
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    string base64 = Convert.ToBase64String(arr);
                    return base64;
                }
            }
            catch
            {
                return null;
            }
        }
        public static async Task<byte[]> GetPicBytesAsync(string url)
        {
            try
            {
                //HTTP下载图片
                WebClient wc = new WebClient();
                using (var ms = new MemoryStream(await wc.DownloadDataTaskAsync(new Uri(url))))
                {
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    return arr;
                }
            }
            catch
            {
                return null;
            }
        }
        public static async Task<BitmapImage> GetPicAsync(string url)
        {
            try
            {
                //HTTP下载图片
                WebClient wc = new WebClient();
                using (var ms = new MemoryStream(await wc.DownloadDataTaskAsync(new Uri(url))))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            catch
            {
                return null;
            }
        }
        public static Task<IRestResponse> GetAsync(string url)
        {
            return Task.Run(() =>
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept-language", "zh-CN,zh;q=0.8");
                request.AddHeader("accept-encoding", "gzip, deflate, sdch");
                request.AddHeader("accept", "text/plain");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36");
                request.AddHeader("upgrade-insecure-requests", "1");
                return client.Execute(request);
            });
        }
        public static Task<IRestResponse> UrpGet(string url, string token)
        {
            return Task.Run(() =>
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept-language", "zh-CN,zh;q=0.8");
                request.AddHeader("accept-encoding", "gzip, deflate, sdch");
                request.AddHeader("referer", "http://202.203.209.96/v5/");
                request.AddHeader("dnt", "1");
                request.AddHeader("authorization", $"Bearer {token}");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");
                request.AddHeader("accept", "application/json, text/plain, */*");
                return client.Execute(request);
            });
        }
    }
}

using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using 不朽URP助手.Entities;

namespace 不朽URP助手.Helpers
{
    public class LoginHelper
    {
        public static string TempGuid;
        public static string ImgGuid;
        public static async Task<HttpMessage> GetGuids()
        {
            var resp = await doGetGuids();
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                message.data = JsonHelper.Deserialize<Guids>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        private static Task<IRestResponse> doGetGuids()
        {
            return Task.Run(() =>
            {
                var client = new RestClient("http://202.203.209.96/v5api/api/GetLoginCaptchaInfo/null");
                var request = new RestRequest(Method.GET);
                return client.Execute(request);
            });
        }
        public static string GetLoginValiPicUrl(string imgGuid = null)
        {
            return $"http://202.203.209.96/vimgs/{imgGuid}.png";
        }
        public static async Task<HttpMessage> Login(string username, string password, string valicode, string tempGuid)
        {
            var resp = await doLogin(username.Trim(), password.Trim(), valicode.Trim(), tempGuid.Trim());
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<LoginData>(resp.Content);
            }
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                message.data = JsonHelper.Deserialize<ErrorLoginMessage>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        private static Task<IRestResponse> doLogin(string username, string password, string valicode, string tempGuid)
        {
            return Task.Run(() =>
            {
                var client = new RestClient("http://202.203.209.96/v5api/OAuth/Token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept-language", "zh-CN,zh;q=0.8");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("referer", "http://202.203.209.96/v5/");
                request.AddHeader("dnt", "1");
                request.AddHeader("user-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
                request.AddHeader("x-devtools-emulate-network-conditions-client-id", "08a3a305-2c90-4715-a5c7-c33a97f32491");
                request.AddHeader("origin", "http://202.203.209.96");
                request.AddHeader("accept", "application/json, text/plain, */*");

                var content = $"grant_type=password&username={Mono.Web.HttpUtility.UrlEncode(username)}&password={Mono.Web.HttpUtility.UrlEncode(password + "|" + valicode + "*" + tempGuid)}&client_id=ynumisSite";
                request.AddParameter("application/x-www-form-urlencoded", content, ParameterType.RequestBody);
                return client.Execute(request);
            });
        }
    }
}

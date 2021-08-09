using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using 不朽URP助手.Entities;

namespace 不朽URP助手.Helpers
{
    public class SelectCourseHelper
    {
        public static async Task<HttpMessage> RemoveCourse(string TeachClassId, string token)
        {
            var resp = await doRemoveCourse(TeachClassId, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = resp.Content;
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        private static Task<IRestResponse> doRemoveCourse(string TeachClassId, string token)
        {
            return Task.Run(() =>
            {
                var client = new RestClient("http://202.203.209.96/v5api/api/xk/remove");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept-language", "zh-CN,zh;q=0.8");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("referer", "http://202.203.209.96/v5/");
                request.AddHeader("content-type", "application/json;charset=UTF-8");
                request.AddHeader("authorization", $"Bearer {token}");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36");
                request.AddHeader("origin", "http://202.203.209.96");
                request.AddHeader("accept", "application/json, text/plain, */*");
                request.AddParameter("application/json;charset=UTF-8", "{\"id\":\"" + TeachClassId + "\"}", ParameterType.RequestBody);
                return client.Execute(request);
            });
        }
        public static async Task<HttpMessage> ListSheet(string token)
        {
            var url = "http://202.203.209.96/v5api/api/xk/sheet";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<List<TeachClassModel>>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        public static async Task<HttpMessage> AddCourse(string TeachClassId, string Captcha, string token)
        {
            var resp = await doAddCourse(TeachClassId, Captcha, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = resp.Content;
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        private static Task<IRestResponse> doAddCourse(string TeachClassId, string Captcha, string token)
        {
            return Task.Run(() =>
            {
                var client = new RestClient("http://202.203.209.96/v5api/api/xk/add");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "377ceae1-13f3-5ff3-0adc-0d24b61bfcb8");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept-language", "zh-CN,zh;q=0.8");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("referer", "http://202.203.209.96/v5/");
                request.AddHeader("dnt", "1");
                request.AddHeader("content-type", "application/json;charset=UTF-8");
                request.AddHeader("authorization", $"Bearer {token}");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");
                request.AddHeader("origin", "http://202.203.209.96");
                request.AddHeader("accept", "application/json, text/plain, */*");
                request.AddParameter("application/json;charset=UTF-8", "{\"id\":\"" + TeachClassId + "\",\"captcha\":\"" + Captcha + "\"}", ParameterType.RequestBody);
                return client.Execute(request);
            });
        }
        public static string GetValiPicUrl(string guid)
        {
            return $"http://202.203.209.96/vimgs/{guid}.png";
        }

        public static async Task<HttpMessage> GetValiGuid(string token)
        {
            var url = "http://202.203.209.96/v5api/api/xk/Captcha";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = resp.Content.Replace("\"", "");
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        public static async Task<HttpMessage> ListTeachClass(
            string majorCode, string gradeCode, string courseNatureCode, string token)
        {
            var url = $"http://202.203.209.96/v5api/api/xk/teachClass/{majorCode}/{gradeCode}/{courseNatureCode}";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<List<TeachClassModel>>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        public static async Task<HttpMessage> ListCourseNature(
            string majorCode, string gradeCode, string token)
        {
            var url = $"http://202.203.209.96/v5api/api/xk/courseNature/{majorCode}/{gradeCode}";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<List<NameCode>>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        public static async Task<HttpMessage> ListGrade(string majorCode, string token)
        {
            var url = $"http://202.203.209.96/v5api/api/xk/grade/{majorCode}";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<List<NameCode>>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        public static async Task<HttpMessage> ListMajor(string academyCode, string token)
        {
            var url = $"http://202.203.209.96/v5api/api/xk/major/{academyCode}";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<List<NameCode>>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        public static async Task<HttpMessage> ListAcadem(string token)
        {
            var url = "http://202.203.209.96/v5api/api/xk/academy";
            var resp = await HttpHelper.UrpGet(url, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<List<NameCode>>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
    }
}

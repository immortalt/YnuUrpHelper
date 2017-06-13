using RestSharp;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using 不朽URP助手.Entities;

namespace 不朽URP助手.Helpers
{
    public class TeachEvaluationHelper
    {
        public static async Task<HttpMessage> TeachEvaluation(EvaluationResult er, string token)
        {
            var json = JsonHelper.Serialize<EvaluationResult>(er);
            var resp = await doTeachEvaluation(json, token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                message.data = resp.Content;
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        private static Task<IRestResponse> doTeachEvaluation(string json, string token)
        {
            return Task.Run(() =>
            {
                var client = new RestClient("http://202.203.209.96/v5api/api/TeachEvaluation/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept-language", "zh-CN,zh;q=0.8");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("referer", "http://202.203.209.96/v5/");
                request.AddHeader("dnt", "1");
                request.AddHeader("content-type", "application/json;charset=UTF-8");
                request.AddHeader("authorization", $"Bearer {token}");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");
                request.AddHeader("x-devtools-emulate-network-conditions-client-id", "a0f67eae-7a28-4218-8682-766e6c711a1c");
                request.AddHeader("origin", "http://202.203.209.96");
                request.AddHeader("accept", "application/json, text/plain, */*");
                request.AddParameter("application/json;charset=UTF-8", json, ParameterType.RequestBody);
                return client.Execute(request);
            });
        }
        public static async Task<HttpMessage> ListTeachEvaluation(string token)
        {
            var resp = await doListTeachEvaluation(token);
            var message = new HttpMessage { statusCode = resp.StatusCode };
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine(resp.Content);
                message.data = JsonHelper.Deserialize<TeachEvaluation>(resp.Content);
            }
            else
            {
                message.data = resp.Content;
            }
            return message;
        }
        private static Task<IRestResponse> doListTeachEvaluation(string token)
        {
            return Task.Run(() =>
            {
                var client = new RestClient("http://202.203.209.96/v5api/api/TeachEvaluation");
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

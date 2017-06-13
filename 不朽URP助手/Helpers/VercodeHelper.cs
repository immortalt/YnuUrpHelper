using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using 不朽URP助手.Entities;

namespace 不朽URP助手.Helpers
{
    public class VercodeHelper
    {
        public static async Task<VercodeResult> Vercode(byte[] pic)
        {
            try
            {
                var resp = await doVercode2(pic);
                Debug.WriteLine(resp);
                VercodeResult result = JsonHelper.Deserialize<VercodeResult>(resp);
                return result;
            }
            catch
            {
                return null;
            }
        }
        private static Task<string> doVercode2(byte[] pfile)
        {
            Dictionary<string, string> nvc = new Dictionary<string, string>();
            //申请的key
            nvc.Add("key", "7072d94dd56066a495abde0ab6e67554");
            //验证码类型代码
            nvc.Add("codeType", "1005");
            //验证码接口地址
            string ApiUrl = "http://op.juhe.cn/vercode/index";
            //验证码图片路径
            string ver_img = @"G:\img\verifyhandler.jpg";
            string contentType = "image/jpeg";
            string paramName = "image";
            string file = ver_img;
            return Task.Run(() =>
            {
                string result = string.Empty;
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(ApiUrl);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                rs.Write(pfile, 0, pfile.Length);

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;
                try
                {
                    wresp = wr.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);

                    result = reader2.ReadToEnd();
                }
                catch (Exception ex)
                {
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }
                return result;
            });
        }
    }
}

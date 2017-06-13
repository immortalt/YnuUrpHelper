using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 不朽URP助手.Helpers
{
    public class StringValues
    {
        public static string[] Comments = new string[] {
            "诲人不倦", "兢兢业业", "尽心尽力", "一丝不苟", "德才兼备", "春风化雨", "润物无声", "无私敬业", "和蔼可亲", "长的最好看的老师！", "最可爱的人", "教的好", "棒极了！", "这样的老师教我三生有幸"
        };
        public static Task<string[]> getValidNames()
        {
            return Task.Run(() =>
            {
                try
                {
                    HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(
                    "http://urp.immortalt.com/ValidNames.txt");
                    HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
                    Stream stream = webreponse.GetResponseStream();
                    byte[] rsByte = new Byte[webreponse.ContentLength];  //save data in the stream
                    stream.Read(rsByte, 0, (int)webreponse.ContentLength);
                    return Encoding.Default.GetString(rsByte, 0, rsByte.Length).ToString().Split(',');
                }
                catch (Exception exp)
                {
                    return new string[] { "田启航" };
                }
            });
        }
    }
}

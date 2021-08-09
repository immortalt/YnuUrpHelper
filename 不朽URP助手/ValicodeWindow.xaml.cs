using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 不朽URP助手.Entities;
using 不朽URP助手.Helpers;

namespace 不朽URP助手
{
    /// <summary>
    /// ValicodeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ValicodeWindow : Window
    {
        string token;
        public static string Valicode;
        public ValicodeWindow(string access_token, string title = "请输入验证码")
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/immortalt.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            this.Title = title;
            this.token = access_token;
            RefreshGuid();
            tbx_valicode.Focus();
        }
        private async Task RefreshGuid()
        {
            HttpMessage resp = await SelectCourseHelper.GetValiGuid(token);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    string guid = resp.data as string;
                    if (guid != null)
                    {
                        Debug.WriteLine(guid);
                        tbx_tip.Text = "正在加载验证码";
                        string url_login_vali = SelectCourseHelper.GetValiPicUrl(guid);
                        BitmapImage image = await HttpHelper.GetPicAsync(url_login_vali);
                        if (image != null)
                        {
                            tbx_tip.Visibility = Visibility.Hidden;
                            img_vali.Source = image;
                        }
                        else
                        {
                            tbx_tip.Text = "验证码加载失败，点击刷新";
                        }
                    }
                    break;
                default:
                    MessageBox.Show(resp.data.ToString(), "异常错误！");
                    break;
            }
        }
        private void btu_submit_Click(object sender, RoutedEventArgs e)
        {
            Valicode = tbx_valicode.Text.Trim();
            Debug.WriteLine(Valicode);
            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btu_submit_Click(sender, null);
            }
        }

        private void tbx_tip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshGuid();
        }

        private void img_vali_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshGuid();
        }
    }
}

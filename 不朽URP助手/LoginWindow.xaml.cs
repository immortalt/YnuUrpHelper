using System;
using System.Collections.Generic;
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
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Guids guids;
        public LoginData loginData;
        public UserData userData;
        public string url_login_vali;
        public bool LoginSuccess = false;
        public static UrpSettings urpSettings;

        public LoginWindow()
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/immortalt.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGuids();
            urpSettings = FileHelper.ReadSettings();
            grid_login.DataContext = urpSettings;
            tbx_password.Password = urpSettings.password;
            if (tbx_password.Password != null && tbx_password.Password != "")
            {
                tbx_valicode.Focus();
            }
            else
            {
                tbx_username.Focus();
            }
        }
        private async void btu_login_Click(object sender, RoutedEventArgs e)
        {
            tbx_tip.Text = "开始初始化";
            await HandleLogin();
        }
        private async Task RefreshGuids()
        {
            tbx_tip.Text = "正在初始化验证码";
            HttpMessage resp = await LoginHelper.GetGuids();
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    Guids guids = resp.data as Guids;
                    if (guids != null)
                    {
                        tbx_tip.Text = "正在加载验证码";
                        this.guids = guids;
                        this.url_login_vali = LoginHelper.GetLoginValiPicUrl(guids.ImgGuid);
                        var image = await HttpHelper.GetPicAsync(this.url_login_vali);
                        if (image != null)
                        {
                            img_vali.Source = image;
                            tbx_tip.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            tbx_tip.Text = "验证码加载失败，点击刷新";
                        }
                    }
                    break;
                default:
                    tbx_tip.Text = "异常错误，点击刷新";
                    Debug.WriteLine(resp.data.ToString(), "异常错误！");
                    break;
            }
        }

        private async Task HandleLogin()
        {
            HttpMessage resp = await LoginHelper.Login(
                tbx_username.Text, tbx_password.Password, tbx_valicode.Text, guids.TempGuid);
            switch (resp.statusCode)
            {
                case HttpStatusCode.OK:
                    this.loginData = resp.data as LoginData;
                    this.userData = JsonHelper.Deserialize<UserData>(loginData.userData);
                    LoginSuccess = true;
                    //var names = await StringValues.getValidNames();
                    //if (names.Contains(userData.Name))
                    //{
                    //    MessageBox.Show(userData.Name + ",欢迎你！", "欢迎");
                    //}
                    //else
                    //{
                    //    MessageBox.Show(userData.Name + ",请联系不朽购买软件使用权限" +
                    //        Environment.NewLine + "QQ:623408596", "警告");
                    //    Environment.Exit(0);
                    //}
                    urpSettings.username = tbx_username.Text;
                    if (urpSettings.savepwd)
                    {
                        urpSettings.password = tbx_password.Password;
                    }
                    else
                    {
                        urpSettings.password = "";
                    }
                    FileHelper.WriteSettings(urpSettings);
                    MainWindow mainWindow = new MainWindow(loginData, userData);
                    mainWindow.Show();
                    this.Close();
                    break;
                case HttpStatusCode.BadRequest:
                    ErrorLoginMessage err = resp.data as ErrorLoginMessage;
                    if (err.error_description == "验证码错误!")
                    {
                        tbx_valicode.Text = "";
                    }
                    MessageBox.Show(err.error_description, "错误");
                    await RefreshGuids();
                    break;
                default:
                    MessageBox.Show(resp.data.ToString(), "异常错误！");
                    break;
            }
        }

        private void img_vali_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshGuids();
        }

        private void tbx_valicode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btu_login_Click(sender, null);
            }
        }

        private void tbx_tip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshGuids();
        }
    }
}

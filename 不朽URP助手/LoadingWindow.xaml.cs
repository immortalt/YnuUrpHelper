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
    /// LoadingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public string StatusText
        {
            get
            {
                return tbx_statusText.Text;
            }
            set
            {
                tbx_statusText.Text = value;
            }
        }
        public LoadingWindow(string statusText)
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/immortalt.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            tbx_statusText.Text = statusText;
        }
    }
}

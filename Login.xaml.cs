using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace Final
{
    /// <summary>
    /// web.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Window window;

        public static string WBAppKey = "4276674222";
        public static string WBAppSecret = "610f8119d39ea76957b5fc665d59eb56";
        public static string WBRedirectURI = "http://baidu.com/";
        public static string urlStr = "https://api.weibo.com/oauth2/authorize?client_id=" + WBAppKey + "&redirect_uri=" + WBRedirectURI;

        private static string code;
        public static string Code { get => code; set => code = value; }

        public Login()
        {
            window = this;
            InitializeComponent();
            wb.Navigating += wb_Navigating;

        }

        private void wb_Loaded(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(urlStr);
            wb.Navigate(uri);  
        }
        void wb_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Uri uri = e.Uri;
            if (uri.AbsoluteUri.StartsWith("http://baidu.com/"))
            {
                //string code = uri.AbsoluteUri.Substring(uri.AbsoluteUri.IndexOf("code=") + 5);
                string[] split = uri.AbsoluteUri.Split('=');
                string code = split[1];
                e.Cancel = true;
                if (code == "21330")
                {
                    MessageBox.Show("授权失败");
                }
                else
                {
                    //成功拦截到code，注意把code存起来
                    Code = code;
                    NetWorkManager.Shared().GetAccessToken(Code);
                    //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {tb.Text="Token = "+Token ; }));

                }
            }
        }

    }
}

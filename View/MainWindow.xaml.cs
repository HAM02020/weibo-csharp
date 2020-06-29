using Final.Model;
using Final.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private WBStatusListViewModel listViewModel = new WBStatusListViewModel();


        //是否登录
        bool IsLogin = true;

        public MainWindow()
        {
            InitializeComponent();
            SetupUI();
        }
        /// <summary>
        /// 页面初始化
        /// </summary>
        private void SetupUI()
        {
            IsLogin = IsUserLogon();
            
            if (!IsLogin)
            {
                //登录页
                gd_welcome.Visibility = Visibility.Visible;
                gd_pages.Visibility = Visibility.Hidden;
            }
            else
            {
                //微博页面
                gd_pages.Visibility = Visibility.Visible;
                gd_welcome.Visibility = Visibility.Hidden;

                //设置用户名
                tb_username.Text = UserAccount.Shared().name;
                LoadStatus(false);
                //新微博提醒
                InitRemind();
            }
        }
        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        /// <returns></returns>
        private bool IsUserLogon()
        {
            //从文件加载用户信息
            if (File.Exists("userAccount.json"))
            {
                StreamReader reader = File.OpenText("userAccount.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["access_token"] != null)
                {
                    UserAccount.Shared().access_token = (string)jsonObject["access_token"];
                    return true;
                }
                return false;
            }
            else
                return false;
        }
        private string selectedButton = "btn_home";
        /// <summary>
        /// 按钮的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //登录按钮
            if (button.Name.Equals("btn_login"))
            {
                Login loginFrame = new Login();
                loginFrame.Closed += new EventHandler(OnLoginframeClosed);
                loginFrame.ShowDialog();
                return;
            }
            //退出按钮
            if (button.Name.Equals("btn_exit"))
            {
                MessageBoxResult dr = MessageBox.Show("是否退出?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
                return;
            }



            //4个目录按钮
            if (button.Name.Equals("btn_home") && IsLogin)
            {
                foreach (Grid grid in gd_pages.Children)
                {
                    grid.Visibility = Visibility.Hidden;
                }
                page_home.Visibility = Visibility.Visible;

                if (selectedButton.Equals(button.Name))
                {
                    tb_btn_name.Text = "正在刷新";
                    //滚动到顶部
                    scrollview.ScrollToVerticalOffset(0);
                    //开始刷新
                    LoadStatus(false);

                }
            }
            if (button.Name.Equals("btn_msg") && IsLogin)
            {
                foreach (Grid grid in gd_pages.Children)
                {
                    grid.Visibility = Visibility.Hidden;
                }
                page_msg.Visibility = Visibility.Visible;
            }
            if (button.Name.Equals("btn_discover") && IsLogin)
            {
                foreach (Grid grid in gd_pages.Children)
                {
                    grid.Visibility = Visibility.Hidden;
                }
                page_discover.Visibility = Visibility.Visible;
            }
            if (button.Name.Equals("btn_profile") && IsLogin)
            {
                foreach (Grid grid in gd_pages.Children)
                {
                    grid.Visibility = Visibility.Hidden;
                }
                page_profile.Visibility = Visibility.Visible;
            }
            selectedButton = button.Name;
            //设置设置按钮的样式
            foreach (Button btn in sp_btns.Children)
            {
                //btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6666FF"));
                btn.Background = Brushes.Transparent;
                btn.BorderThickness = new Thickness(0);
                btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#848484"));
            }
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6666FF"));
            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }

        /// <summary>
        /// 登录窗体关闭的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoginframeClosed(object sender,EventArgs e)
        {
                SetupUI();
        }
        /// <summary>
        /// Scrollviewer滚动的事件，主要用来监测滚动到底部，进行刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight && scrollViewer.VerticalOffset > 2 * Window.GetWindow(this).Height)
            {
                var offset = scrollViewer.VerticalOffset;
                scrollview.ScrollChanged -= OnScrollChanged;
                Console.WriteLine("下拉刷新");
                LoadStatus(true);
            }
        }
        /// <summary>
        /// 刷新微博
        /// </summary>
        /// <param name="isPullup">是否下拉刷新</param>
        private void LoadStatus(bool isPullup)
        {
            
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                //加载微博
                listViewModel.getStatusList(isPullup);
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    sp_status.Children.Clear();
                    foreach (WBStatusViewModel viewModel in listViewModel.statusList)
                    {

                        var cell = (UIElement)WBCellViewModel.CreatWBStatusCell(viewModel);
                        sp_status.Children.Add(cell);
                    }
                    tb_btn_name.Text = "首页";
                });
                scrollview.ScrollChanged += OnScrollChanged;

            });
        }
        /// <summary>
        /// 初始化消息提醒计时器，每30秒请求服务器接口一次
        /// </summary>
        private void InitRemind()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 * 30);
                    String response = NetWorkManager.Shared().TokenRequest("https://rm.api.weibo.com/2/remind/unread_count.json", "uid=" + UserAccount.Shared().uid);
                    JObject j = JObject.Parse(response);
                    int count = j.Value<int>("status");
                    if (count == 0)
                        continue;
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        tb_btn_name.Text = "未读:" + count.ToString();
                    });
                }

            });
        }
    }
}

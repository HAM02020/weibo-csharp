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

        private WBStatusViewModel listViewModel = new WBStatusViewModel();


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
            //从文件加载用户信息
            if (File.Exists("userAccount.json"))
            {
                StreamReader reader = File.OpenText("userAccount.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["access_token"] != null)
                {
                    IsLogin = true;
                    UserAccount.Shared().access_token = (string)jsonObject["access_token"];
                }
                else
                {
                    IsLogin = false;
                }
            }
            else
            {
                IsLogin = false;
            }
            
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


                new Thread((() =>
                {

                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        //设置用户名
                        tb_username.Text = UserAccount.Shared().name;


                        //加载微博

                        string response = NetWorkManager.Shared().TokenRequest("https://api.weibo.com/2/statuses/home_timeline.json");
                        JObject json = JObject.Parse(response);
                        JArray jArray = JArray.FromObject(json["statuses"]);
                        foreach (JObject j in jArray)
                        {
                            //把数据 反序列化成模型
                            Console.WriteLine(j["text"]);
                            WBStatus status = j.ToObject<WBStatus>();

                            Pic_urls pic_Urls = new Pic_urls();
                            JArray jArray1 = JArray.FromObject(j["pic_urls"]);
                            foreach (JObject jj in jArray1)
                            {
                                pic_Urls.thumbnail_pic.Add((string)jj["thumbnail_pic"]);
                            }
                            status.pic_urls = pic_Urls;

                            //添加到ViewModel
                            listViewModel.statusList.Add(status);
                        }

                        //从视图模型加载数据
                        foreach(WBStatus wbStatus in listViewModel.statusList)
                        {
                            setupWBCell(wbStatus);
                        }


                    }));

                })).Start();
                
            }
        }
        private void setupWBCell(WBStatus status)
        {
            WBCell cell = new WBCell();
            //调用NetWorkManager获取微博数据

            //把数据转模型

            //把模型的值付给cell
            cell.username.Text = status.user.screen_name;
            cell.post_time.Text = status.created_at;
            cell.text.Text = status.text;


            //工具栏
            cell.retweeted_count.Text = status.reposts_count.ToString();
            cell.comment_count.Text = status.comments_count.ToString();
            cell.like_count.Text = status.attitudes_count.ToString();

            sp_status.Children.Add(cell);
        }


        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in sp_btns.Children)
            {
                //btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6666FF"));
                btn.Background = Brushes.Transparent;
                btn.BorderThickness = new Thickness(0);
                btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#848484"));
            }
            Button button = (Button)sender;
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6666FF"));
            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));

            if (button.Name.Equals("btn_home") && IsLogin)
            {
                foreach (Grid grid in gd_pages.Children)
                {
                    grid.Visibility = Visibility.Hidden;
                }
                page_home.Visibility = Visibility.Visible;
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
        }

        private void Btn_exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否退出?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }

        }

        private void Btn_login_Click(object sender, RoutedEventArgs e)
        {
            Login loginFrame = new Login();
            loginFrame.Closed += new EventHandler(OnLoginframeClosed);
            loginFrame.ShowDialog();
        }
        /// <summary>
        /// 登录窗体关闭的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoginframeClosed(object sender,EventArgs e)
        {

        }
        
    }
}

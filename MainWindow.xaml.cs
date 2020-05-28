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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsLogin = false;

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


            }
        }

        public Border SetupWB()
        {
            Border bd_main = new Border();
            bd_main.CornerRadius = new CornerRadius(10);
            bd_main.Background = Brushes.Black;
            bd_main.BorderThickness = new Thickness(5, 10, 15, 20);
            return bd_main;
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
    }
}

using Final.ViewModel;
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
    /// RetweetWBCell.xaml 的交互逻辑
    /// </summary>
    public partial class RetweetWBCell : UserControl
    {
        private WBStatusViewModel viewModel;

        public RetweetWBCell(WBStatusViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            InitWithViewModel();
        }

        public void InitWithViewModel()
        {
            //把模型的值付给cell
            username.Text = viewModel.status.user.screen_name;
            post_time.Text = viewModel.status.created_at;
            text.Text = viewModel.status.text;
            tb_originname.Text = "@" + viewModel.status.retweeted_status.user.screen_name + "：";
            tb_otigintext.Text = viewModel.status.retweeted_status.text;



            //工具栏
            retweeted_count.Text = viewModel.status.reposts_count == 0 ? "转发" : viewModel.status.reposts_count.ToString();
            comment_count.Text = viewModel.status.comments_count == 0 ? "评论" : viewModel.status.comments_count.ToString();
            like_count.Text = viewModel.status.attitudes_count == 0 ? "点赞" : viewModel.status.attitudes_count.ToString();

            //头像
            
            Task.Run(() =>
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    avatar.Source = BitmapFrame.Create(new Uri(viewModel.status.user.avatar_large), BitmapCreateOptions.None, BitmapCacheOption.Default);
                    setupPic();
                });
            });
        }
        public void setupPic()
        {
            //设置图片
            List<string> pic_urls = viewModel.wap360s;

            if (pic_urls.Count != 0)
            {
                int count = pic_urls.Count;
                int index = 0;
                if (count != 1)
                {
                    foreach (Image img in gd_imgs.Children)
                    {
                        if (index == count)
                            break;
                        img.Source = BitmapFrame.Create(new Uri(pic_urls[index]), BitmapCreateOptions.None, BitmapCacheOption.Default);
                        img.Stretch = Stretch.UniformToFill;
                        img.Margin = new Thickness(5);
                        index++;
                    }
                    if (count <= 3)
                    {
                        row_pic.Height = new GridLength(100);

                    }
                    else if (count <= 6)
                    {
                        row_pic.Height = new GridLength(200);
                    }
                    else if (count <= 9)
                    {
                        row_pic.Height = new GridLength(300);
                    }
                }
                else if (count == 1)
                {
                    gd_imgs.Visibility = Visibility.Hidden;
                    Image img = new Image();
                    img.Source = BitmapFrame.Create(new Uri(pic_urls[0]), BitmapCreateOptions.None, BitmapCacheOption.Default);
                    img.Stretch = Stretch.Uniform;
                    img.Margin = new Thickness(5);
                    gd_pics.Children.Add(img);
                }



            }
            else
            {
                row_pic.Height = new GridLength(0);
            }
        }
    }
}

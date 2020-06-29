using Final.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.ViewModel
{
    
    public class WBStatusViewModel
    {
        public WBStatus status;
        private Pic_urls pic_urls;
        public string retweetedStr;
        public List<string> wap360s;
        public List<string> larges;
        public WBStatusViewModel(WBStatus status)
        {
            this.status = status;
            //判断是否有转发微博
            bool isRetweetedStatus = status.retweeted_status != null;

            pic_urls = !isRetweetedStatus ? status.pic_urls : status.retweeted_status.pic_urls;
            retweetedStr = !isRetweetedStatus ? "" : status.retweeted_status.text;
            wap360s = new List<string>();
            larges = new List<string>();

            for (int i = 0; i < pic_urls.thumbnail_pic.Count; i++)
            {
                string wap360 = pic_urls.thumbnail_pic[i].Replace("/thumbnail/", "/wap360/");
                string large = pic_urls.thumbnail_pic[i].Replace("/thumbnail/", "/large/");
                wap360s.Add(wap360);
                larges.Add(large);
            }

            
        }
    }
}

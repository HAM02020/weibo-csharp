using Final.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final.ViewModel
{
    class WBStatusListViewModel
    {

        public List<WBStatusViewModel> statusList;

        public WBStatusListViewModel()
        {
            statusList = new List<WBStatusViewModel>();
        }

        public void getStatusList(bool isPullup)
        {
            string param = "";
            if (statusList.Count != 0)
            {
                long since_id = isPullup ? 0 : statusList.First().status.id;
                long max_id = !isPullup ? 0 : statusList.Last().status.id;
                max_id = max_id > 0 ? max_id - 1 : 0;
                param = "since_id=" + since_id + "&max_id=" + max_id;
            }

            //加载微博

            string response = NetWorkManager.Shared().TokenRequest("https://api.weibo.com/2/statuses/home_timeline.json",param);
            JObject json = JObject.Parse(response);
            JArray jArray = JArray.FromObject(json["statuses"]);

            List<WBStatusViewModel> array = new List<WBStatusViewModel>();

            foreach (JObject j in jArray)
            {
                //把数据 反序列化成模型
                WBStatus status = j.ToObject<WBStatus>();

                

                Pic_urls pic_Urls = new Pic_urls();
                JArray jArray1 = JArray.FromObject(j["pic_urls"]);
                foreach (JObject jj in jArray1)
                {
                    pic_Urls.thumbnail_pic.Add((string)jj["thumbnail_pic"]);
                }
                status.pic_urls = pic_Urls;


                //被转发微博
                if (j.ContainsKey("retweeted_status"))
                {
                    Pic_urls retweet_pic_Urls = new Pic_urls();
                    JArray jArray2 = JArray.FromObject(j["retweeted_status"]["pic_urls"]);
                    foreach (JObject jj in jArray2)
                    {
                        retweet_pic_Urls.thumbnail_pic.Add((string)jj["thumbnail_pic"]);
                    }
                    status.retweeted_status.pic_urls = retweet_pic_Urls;
                }

                //创建WBStatusViewModel
                WBStatusViewModel viewModel = new WBStatusViewModel(status);

                //添加到list中
                array.Add(viewModel);
                

            }
            if (isPullup)
            {
                statusList = statusList.Concat(array).ToList();
            }
            else
            {
                //保留重复项 Concat
                statusList = array.Concat(statusList).ToList();
            }
        }
    }
}

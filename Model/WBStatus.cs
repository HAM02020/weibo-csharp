using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Model
{
    public class WBStatus
    {
        [JsonProperty]
        public long id;
        [JsonProperty]
        public string text { get; set; }
        [JsonProperty]
        public string created_at{ get; set; }
        [JsonProperty]
        public DateTime createdDate { get; set; }
        //来源
        [JsonProperty]
        public string source { get; set; }
        //转发评论数
        [JsonProperty]
        public int reposts_count { get; set; }
        [JsonProperty]
        public int comments_count { get; set; }
        [JsonProperty]
        public int attitudes_count { get; set; }
        //用户
        [JsonProperty]
        public WBUser user { get; set; }

        //图片链接
        [JsonIgnore]
        public Pic_urls pic_urls{ get; set; }
        [JsonProperty]
        public WBStatus retweeted_status { get; set; }
    }
}

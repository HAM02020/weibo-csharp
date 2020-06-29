using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Model
{
    public class WBUser
    {
        [JsonProperty]
        public long id { get; set; }
        [JsonProperty]
        public string screen_name { get; set; }
        [JsonProperty]
        public int verified_type { get; set; }
        [JsonProperty]
        public int mbrank { get; set; }
        [JsonProperty]
        public string avatar_large { get; set; }
    }
}

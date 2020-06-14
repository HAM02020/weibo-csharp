using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Final.Model
{
    public class UserAccount
    {
        private static readonly UserAccount instance = new UserAccount();
        public static UserAccount Shared()
        {
            return instance;
        }
        [JsonProperty]
        public string access_token { get; set; }
        [JsonProperty]
        public string uid { get; set; }
        [JsonProperty]
        public string name { get; set; }
        //用户头像地址 180*180
        [JsonProperty]
        public string avatar_large { get; set; }

        private UserAccount()
        {
            //从文件加载用户信息
            if (File.Exists("userAccount.json"))
            {
                StreamReader reader = File.OpenText("userAccount.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                Console.WriteLine(jsonObject);
                this.access_token = (string)jsonObject["access_token"];
                this.uid = (string)jsonObject["uid"];
                this.name = (string)jsonObject["name"];
                this.avatar_large = (string)jsonObject["avatar_large"];
            }
            
        }
        public void SaveAccount() {
            string path = @".\userAccount.json";
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(this,Formatting.Indented);
            File.WriteAllText(path, output);
        }
    }
}

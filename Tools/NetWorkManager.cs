using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Final.Model;
using Newtonsoft.Json;

namespace Final
{
    class NetWorkManager
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        private static NetWorkManager instance = new NetWorkManager();
        public static NetWorkManager Shared()
        {
            return instance;
        }

        UserAccount userAccount =UserAccount.Shared();


        /// <summary>
        /// 根据code获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool GetAccessToken(string code)
        {
            string param = String.Format("client_id={0}&client_secret={1}&grant_type=authorization_code&code={2}&redirect_uri={3}", Login.WBAppKey, Login.WBAppSecret, code, Login.WBRedirectURI);

            string json = PostWebRequest("https://api.weibo.com/oauth2/access_token", param);

            userAccount = JsonConvert.DeserializeObject<UserAccount>(json);
            LoadUserInfo();
            userAccount.SaveAccount();
            return userAccount.access_token != null;

        }
        /// <summary>
        /// 带token的GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string TokenRequest(string url,string param = "")
        {
            string token = userAccount.access_token;
            if (token == null)
            {
                return "没有token";
            }
            url = url + "?" + param + "&access_token=" + token;
            return GetWebRequest(url);
        }

        /// <summary>
        /// 获取用户名和头像
        /// </summary>
        private void LoadUserInfo()
        {
            string uid = userAccount.uid;
            if (uid == null)
            {
                return;
            }
            string param = "uid=" + uid;
            string jsonStr = TokenRequest("https://api.weibo.com/2/users/show.json", param);
            UserAccount temp = JsonConvert.DeserializeObject<UserAccount>(jsonStr);
            userAccount.name = temp.name;
            userAccount.avatar_large = temp.avatar_large;

        }






        #region 公共方法
        /// <summary>
        /// Get数据接口
        /// </summary>
        /// <param name="getUrl">接口地址</param>
        /// <returns></returns>
        private string GetWebRequest(string getUrl)
        {

            string responseContent = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
            request.ContentType = "text/html;charset=utf-8";
            request.Method = "GET";


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //在这里对接收到的页面内容进行处理
            using (Stream resStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                {
                    responseContent = reader.ReadToEnd().ToString();
                }
            }
            return responseContent;
        }
        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式(Encoding.UTF8)</param>
        /// <returns></returns>
        private string PostWebRequest(string postUrl, string paramData)
        {
            string responseContent = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                using (Stream reqStream = webReq.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);//写入参数
                    //reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseContent = sr.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return responseContent;
        }

        #endregion
    }

}

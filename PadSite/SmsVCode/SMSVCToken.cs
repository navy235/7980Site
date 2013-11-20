using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Threading;

namespace SmsVCode
{
    /// <summary>
    /// 获取信任码类
    /// </summary>
    public class SMSVCToken_API : API
    {
        private string _timestamp;
        public SMSVCToken_API(string app_id,string app_secret, string access_token)
        {
            Access_Token = access_token;
            APP_ID = app_id;
            APP_SECRET = app_secret;
            _timestamp = Utility.GetCurrentDate();
            Method = RequestType.get;
            PrepareParam();
        }

        public override IDictionary<string, string> RequestParams
        {
            get { return _requestParams; }
        }

        public override string URL
        {
            get { return "http://api.189.cn/v2/dm/randcode/token"; }
        }

        public override string GetResult
        {
            get {
                return this.HttpGetRequest(_requestParams); }
        }

        public override string PostResult
        {
            get { return NotSupportMessage; }
        }

        

        private string sign
        {
            get {
               // string plaintext = "access_token=" + base_access_token + "&app_id=" + app_id + "&timestamp=" + timestamp;
              //  return Utility.DoSignature(plaintext, app_secret);
                SortedDictionary<string,string> paramlist = new SortedDictionary<string,string>();
                paramlist.Add("access_token", Access_Token);
                paramlist.Add("app_id", APP_ID);
                paramlist.Add("timestamp", _timestamp);
                return Utility.DoSignature(paramlist, APP_SECRET);
            }
        }

        protected override void PrepareParam()
        {
            _requestParams = new Dictionary<string, string>();
            _requestParams.Add("access_token", Access_Token);
            _requestParams.Add("app_id", APP_ID);
            _requestParams.Add("timestamp", _timestamp);
            _requestParams.Add("sign",sign);
        }
    }
}

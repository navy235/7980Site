using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using PadSite.ViewModels;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Filters;
using PadSite.Utils;
using PadSite.Service.Interface;
using PadSite.Setting;
using SmsVCode;
namespace PadSite.Controllers
{
    public class SmsCodeController : Controller
    {
        //
        // GET: /SmsCode/

        public ActionResult Index()
        {
            //应用的APPID 
            string app_id = ConfigSetting.SmsAppID;
            //应用的APPKEY 
            string app_secret = ConfigSetting.SmsAppSecret;

            string my_url = string.Format("http://www.{0}/smscode/index", ConfigSetting.DomainUrl);
            //Step1：获取Authorization Code 
            //session_start(); 
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回 
                Session["state"] = Guid.NewGuid();//md5(uniqid(rand(), TRUE));  
                //拼接URL      
                string dialog_url = "https://oauth.api.189.cn/emp/oauth2/v2/authorize?response_type=code&app_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + Session["state"];
                return Content("<script>window.top.location.href='" + dialog_url + "'</script>");
                //string response = HttpHelper.WebPagePostGet(dialog_url, data, System.Text.Encoding.UTF8);
                //NameValueCollection res = ParseJson(response);

            }
            if (Request["state"].ToString().Equals(Session["state"].ToString()))
            {
                Session["state"] = null;
                //拼接URL    
                string token_url = "https://oauth.api.189.cn/emp/oauth2/v2/access_token";
                string data = "grant_type=authorization_code&app_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&app_secret=" + app_secret + "&code=" + code;
                string response = HttpHelper.WebPagePostGet(token_url, data, System.Text.Encoding.UTF8);

                NameValueCollection res = ParseJson(response);
                if (!string.IsNullOrEmpty(res["error"]))
                {
                    return View(new OpenSmsCodeViewModel()
                    {
                        Success = false,
                        res_message = res["res_message"].ToString()
                    });
                }
                //OpenLoginViewModel OpenUser = new OpenLoginViewModel()
                //{
                //    Success = true,
                //    OpenType = (int)OpenLoginType.Taobao,
                //    Uid = res["taobao_user_id"].ToString(),
                //    OpenId = res["access_token"].ToString(),
                //    NickName = res["taobao_user_nick"].ToString()

                //};
                //if (MemberService.OpenUserLogin(OpenUser, OpenLoginType.Taobao))
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    Session["registerAuto"] = OpenUser;
                //    return RedirectToAction("RegAuto", "Reg");
                //}
                return View(new OpenSmsCodeViewModel()
                {
                    Success = res["res_code"] == "0",
                    res_message = res["res_message"].ToString(),
                    access_token = res["access_token"].ToString()

                });
            }
            else
            {
                return View(new OpenSmsCodeViewModel()
                {
                    Success = false,
                    res_message = "The state does not match. You may be a victim of CSRF"

                });

            }
        }

        public ActionResult SendCode()
        {
            string appid = ConfigSetting.SmsAppID;
            string appsecret = ConfigSetting.SmsAppSecret;
            string access_token = "f98fa5c4a5a85136de2c5fbbeaf74ceb1383643813166";
            string mobileNum = "18321841288"; //替换为发往的手机号码
            int? exptime = 6; //设置验证码过期时间
            SendSMSVC_API sendsmsvc = new SendSMSVC_API(appid, appsecret, access_token, mobileNum, exptime);
            return Json(sendsmsvc.Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCode(string rand_code, string identifier)
        {
            Session["SmsVCode"] = rand_code;
            return Json(new { res_code = 0 }, JsonRequestBehavior.AllowGet);
        }

        NameValueCollection ParseJson(string json_code)
        {
            NameValueCollection mc = new NameValueCollection();
            Regex regex = new Regex(@"(\s*\""?([^""]*)\""?\s*\:\s*\""?([^""]*)\""?\,?)");
            json_code = json_code.Trim();
            if (json_code.StartsWith("{"))
            {
                json_code = json_code.Substring(1, json_code.Length - 2);
            }
            foreach (Match m in regex.Matches(json_code))
            {
                mc.Add(m.Groups[2].Value, m.Groups[3].Value);
                //Response.Write(m.Groups[2].Value + "=" + m.Groups[3].Value + "<br/>"); 
            }
            return mc;
        }
        NameValueCollection ParseUrlParameters(string str_params)
        {
            NameValueCollection nc = new NameValueCollection();
            foreach (string p in str_params.Split('&'))
            {
                string[] p_s = p.Split('=');
                nc.Add(p_s[0], p_s[1]);
            }
            return nc;
        }
    }
}

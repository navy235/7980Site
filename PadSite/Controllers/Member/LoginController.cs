﻿using System;
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
namespace PadSite.Controllers
{
    public class LoginController : Controller
    {


        private IMemberService MemberService;
        public LoginController(
            IMemberService MemberService)
        {

            this.MemberService = MemberService;
        }

        #region base

        public ActionResult Index(string username = null)
        {
            var model = new LoginViewModel();
            if (!string.IsNullOrEmpty(username))
            {
                model.Email = username;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string ReturnUrl = null, bool Remember = false)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string Md5Password = CheckHelper.StrToMd5(model.Password);
                    if (MemberService.Login(model.Email, Md5Password))
                    {
                        ViewBag.Message = null;

                        if (!string.IsNullOrEmpty(ReturnUrl))
                            return Redirect(ReturnUrl);
                        else
                            return RedirectToAction("index", "home");
                    }
                    else
                    {
                        ViewBag.Message = "您的用户名和密码不匹配";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + model.Email + "登录失败!", ex);
                    ViewBag.Message = "服务器错误，请刷新页面重新登录";
                    return View(model);
                }
            }
            else
            {
                ViewBag.Message = "您的输入有误请确认后提交";
                return View(model);
            }
        }


        public ActionResult PopLogin(string username = null)
        {
            var model = new LoginViewModel();
            if (!string.IsNullOrEmpty(username))
            {
                model.Email = username;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PopLogin(LoginViewModel model, bool Remember = false)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string Md5Password = CheckHelper.StrToMd5(model.Password);
                    if (MemberService.Login(model.Email, Md5Password))
                    {
                        ViewBag.Message = null;
                        return Content("<script>window.top.location.reload();</script>");
                    }
                    else
                    {
                        ViewBag.Message = "您的用户名和密码不匹配";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + model.Email + "登录失败!", ex);
                    ViewBag.Message = "服务器错误，请刷新页面重新登录";
                    return View(model);
                }
            }
            else
            {
                ViewBag.Message = "您的输入有误请确认后提交";
                return View(model);
            }
        }

        public ActionResult LogOut(string returnUrl = null)
        {
            CookieHelper.ClearCookie();
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        #endregion


        #region login_returnUrl
        public ActionResult LoginReturnUrl(string url)
        {
            if (CookieHelper.IsLogin)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                CookieHelper.ClearCookie();
                MemberService.Login(member.Email, CheckHelper.StrToMd5(member.Password));
                return Redirect(url);
            }
            else
            {
                return RedirectToAction("index", new { ReturnUrl = url });
            }
        }

        #endregion

        public ActionResult QQ()
        {
            //应用的APPID
            string app_id = ConfigSetting.QQAppID;
            //应用的APPKEY
            string app_secret = ConfigSetting.QQKey;
            //成功授权后的回调地址
            string my_url = string.Format("http://www.{0}/login/qq", ConfigSetting.DomainUrl);

            //Step1：获取Authorization Code
            //session_start();
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回
                Session["state"] = Guid.NewGuid();//md5(uniqid(rand(), TRUE)); 
                //拼接URL     
                string dialog_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + Session["state"];
                return Content("<script>window.top.location.href='" + dialog_url + "'</script>");
            }

            //Step2：通过Authorization Code获取Access Token
            if (Request["state"].ToString().Equals(Session["state"].ToString()))
            {
                //拼接URL   
                string token_url = "https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&"
                + "client_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&client_secret=" + app_secret + "&code=" + code;

                string response = HttpHelper.WebPageContentGet(token_url, System.Text.Encoding.UTF8);
                NameValueCollection msg;
                if (response.IndexOf("callback") != -1)
                {
                    int lpos = response.IndexOf("(");
                    int rpos = response.IndexOf(")");
                    response = response.Substring(lpos + 1, rpos - lpos - 1);
                    msg = ParseJson(response);

                    if (!string.IsNullOrEmpty(msg["error"]))
                    {

                        return View(new OpenLoginViewModel()
                        {
                            Success = false,
                            Error = msg["error"].ToString(),
                            Message = msg["error_description"]
                        });
                    }
                }
                NameValueCollection ps = ParseUrlParameters(response);
                string graph_url = "https://graph.qq.com/oauth2.0/me?access_token=" + ps["access_token"];
                string str = HttpHelper.WebPageContentGet(graph_url, System.Text.Encoding.Default);
                if (str.IndexOf("callback") != -1)
                {
                    int lpos = str.IndexOf("(");
                    int rpos = str.IndexOf(")");
                    str = str.Substring(lpos + 1, rpos - lpos - 1);
                }
                NameValueCollection user = ParseJson(str);
                if (!string.IsNullOrEmpty(user["error"]))
                {
                    return View(new OpenLoginViewModel()
                    {
                        Success = false,
                        Error = user["error"].ToString(),
                        Message = user["error_description"]
                    });
                }
                OpenLoginViewModel OpenUser = new OpenLoginViewModel()
                {
                    Success = true,
                    OpenType = (int)OpenLoginType.QQ,
                    OpenId = user["openid"].ToString()
                };
                if (MemberService.OpenUserLogin(OpenUser, OpenLoginType.QQ))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string user_profile_url = "https://graph.qq.com/user/get_user_info?access_token="
                        + ps["access_token"] +
                        "&oauth_consumer_key=" + app_id +
                        "&openid=" + OpenUser.OpenId;
                    string response_profile = HttpHelper.WebPageContentGet(user_profile_url, System.Text.Encoding.UTF8);
                    NameValueCollection userProfile = ParseJson(response_profile);
                    OpenUser.NickName = userProfile["nickname"].ToString();
                    Session["registerAuto"] = OpenUser;
                    return RedirectToAction("RegAuto", "Reg");
                }

            }
            else
            {
                return View(new OpenLoginViewModel()
                {
                    Success = false,
                    Error = "The state does not match. You may be a victim of CSRF.",
                    Message = "request=" + Request["state"] + ",session=" + Session["state"]
                });
            }

        }

        public ActionResult Sina()
        {
            //应用的APPID 
            string app_id = ConfigSetting.SinaAppID;
            //应用的APPKEY 
            string app_secret = ConfigSetting.SinaKey;
            //成功授权后的回调地址 

            string my_url = string.Format("http://www.{0}/login/sina", ConfigSetting.DomainUrl);

            //Step1：获取Authorization Code 
            //session_start(); 
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回 
                Session["state"] = Guid.NewGuid();//md5(uniqid(rand(), TRUE));  
                //拼接URL      
                string dialog_url = "https://api.weibo.com/oauth2/authorize?response_type=code&client_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + Session["state"];
                return Content("<script> window.top.location.href='" + dialog_url + "'</script>");
            }
            if (Request["state"].ToString().Equals(Session["state"].ToString()))
            {
                Session["state"] = null;
                //拼接URL    
                string token_url = "https://api.weibo.com/oauth2/access_token";
                string data = "grant_type=authorization_code&client_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&client_secret=" + app_secret + "&code=" + code;
                string response = HttpHelper.WebPagePostGet(token_url, data, System.Text.Encoding.UTF8);

                NameValueCollection user = ParseJson(response);
                if (!string.IsNullOrEmpty(user["error"]))
                {
                    return View(new OpenLoginViewModel()
                    {
                        Success = false,
                        Error = user["error"].ToString(),
                        Message = user["error_description"].ToString()
                    });
                }
                OpenLoginViewModel OpenUser = new OpenLoginViewModel()
                {
                    Success = true,
                    OpenType = (int)OpenLoginType.Sina,
                    Uid = user["uid"].ToString(),
                    OpenId = user["access_token"].ToString()
                };
                if (MemberService.OpenUserLogin(OpenUser, OpenLoginType.Sina))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string user_profile_url = "https://api.weibo.com/2/users/show.json?access_token=" + OpenUser.OpenId + "&uid=" + OpenUser.Uid;
                    string response_profile = HttpHelper.WebPageContentGet(user_profile_url, System.Text.Encoding.UTF8);
                    NameValueCollection userProfile = ParseJson(response_profile);
                    OpenUser.NickName = userProfile["screen_name"].ToString();
                    Session["registerAuto"] = OpenUser;
                    return RedirectToAction("RegAuto", "Reg");
                }
            }
            else
            {
                return View(new OpenLoginViewModel()
                {
                    Success = false,
                    Error = "The state does not match. You may be a victim of CSRF",
                    Message = "request=" + Request["state"] + ",session=" + Session["state"]
                });

            }
        }

        public ActionResult Taobao()
        {
            //应用的APPID 
            string app_id = ConfigSetting.TaboBaoAppID;
            //应用的APPKEY 
            string app_secret = ConfigSetting.TaboBaoKey;

            string my_url = string.Format("http://www.{0}/login/taobao", ConfigSetting.DomainUrl);
            //Step1：获取Authorization Code 
            //session_start(); 
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回 
                Session["state"] = Guid.NewGuid();//md5(uniqid(rand(), TRUE));  
                //拼接URL      
                string dialog_url = "https://oauth.taobao.com/authorize?response_type=code&client_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + Session["state"];
                return Content("<script>window.top.location.href='" + dialog_url + "'</script>");
            }
            if (Request["state"].ToString().Equals(Session["state"].ToString()))
            {
                Session["state"] = null;
                //拼接URL    
                string token_url = "https://oauth.taobao.com/token";
                string data = "grant_type=authorization_code&client_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&client_secret=" + app_secret + "&code=" + code;
                string response = HttpHelper.WebPagePostGet(token_url, data, System.Text.Encoding.UTF8);

                NameValueCollection user = ParseJson(response);
                if (!string.IsNullOrEmpty(user["error"]))
                {
                    return View(new OpenLoginViewModel()
                    {
                        Success = false,
                        Error = user["error"].ToString(),
                        Message = user["error_description"].ToString()
                    });
                }
                OpenLoginViewModel OpenUser = new OpenLoginViewModel()
                {
                    Success = true,
                    OpenType = (int)OpenLoginType.Taobao,
                    Uid = user["taobao_user_id"].ToString(),
                    OpenId = user["access_token"].ToString(),
                    NickName = user["taobao_user_nick"].ToString()

                };
                if (MemberService.OpenUserLogin(OpenUser, OpenLoginType.Taobao))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["registerAuto"] = OpenUser;
                    return RedirectToAction("RegAuto", "Reg");
                }
            }
            else
            {
                return View(new OpenLoginViewModel()
                {
                    Success = false,
                    Error = "The state does not match. You may be a victim of CSRF",
                    Message = "request=" + Request["state"] + ",session=" + Session["state"]
                });

            }
        }
        public ActionResult Douban()
        {
            //应用的APPID 
            string app_id = ConfigSetting.DouBanKey;
            //应用的APPKEY 
            string app_secret = ConfigSetting.DouBanSecret;
            //成功授权后的回调地址 

            string my_url = string.Format("http://www.{0}/login/douban", ConfigSetting.DomainUrl);

            //Step1：获取Authorization Code 
            //session_start(); 
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回 
                Session["state"] = Guid.NewGuid();//md5(uniqid(rand(), TRUE));  
                //拼接URL      
                string dialog_url = "https://www.douban.com/service/auth2/auth?response_type=code&client_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + Session["state"];
                return Content("<script>window.top.location.href='" + dialog_url + "'</script>");
            }
            if (Request["state"].ToString().Equals(Session["state"].ToString()))
            {
                Session["state"] = null;
                //拼接URL    
                string token_url = "https://www.douban.com/service/auth2/token";
                string data = "grant_type=authorization_code&client_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&client_secret=" + app_secret + "&code=" + code;
                string response = HttpHelper.WebPagePostGet(token_url, data, System.Text.Encoding.UTF8);
                NameValueCollection user = ParseJson(response);
                if (!string.IsNullOrEmpty(user["error"]))
                {
                    return View(new OpenLoginViewModel()
                    {
                        Success = false,
                        Error = user["error"].ToString(),
                        Message = user["error_description"].ToString()
                    });
                }
                OpenLoginViewModel OpenUser = new OpenLoginViewModel()
                {
                    Success = true,
                    OpenType = (int)OpenLoginType.Douban,
                    Uid = user["douban_user_id"].ToString(),
                    OpenId = user["access_token"].ToString()
                };
                if (MemberService.OpenUserLogin(OpenUser, OpenLoginType.Douban))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string user_profile_url = "https://api.douban.com/v2/user/" + OpenUser.Uid;
                    string response_profile = HttpHelper.WebPageContentGet(user_profile_url, System.Text.Encoding.UTF8);
                    NameValueCollection userProfile = ParseJson(response_profile);
                    OpenUser.NickName = userProfile["name"].ToString();
                    Session["registerAuto"] = OpenUser;
                    return RedirectToAction("RegAuto", "Reg");
                }
            }
            else
            {
                return View(new OpenLoginViewModel()
                {
                    Success = false,
                    Error = "The state does not match. You may be a victim of CSRF",
                    Message = "request=" + Request["state"] + ",session=" + Session["state"]
                });

            }
        }

        public ActionResult Renren()
        {
            //应用的APPID 
            string app_id = ConfigSetting.RenRenAPPKey;
            //应用的APPKEY 
            string app_secret = ConfigSetting.RenRenSecretKey;
            //成功授权后的回调地址 
            string my_url = string.Format("http://www.{0}/login/renren", ConfigSetting.DomainUrl);

            //Step1：获取Authorization Code 
            //session_start(); 
            string code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                //state参数用于防止CSRF攻击，成功授权后回调时会原样带回 
                Session["state"] = Guid.NewGuid();//md5(uniqid(rand(), TRUE));  
                //拼接URL      
                string dialog_url = "https://graph.renren.com/oauth/authorize?response_type=code&client_id="
                   + app_id + "&redirect_uri=" + Server.UrlEncode(my_url) + "&state="
                   + Session["state"];
                return Content("<script>window.top.location.href='" + dialog_url + "'</script>");
            }
            if (Request["state"].ToString().Equals(Session["state"].ToString()))
            {
                Session["state"] = null;
                //拼接URL    
                string token_url = "https://graph.renren.com/oauth/token?grant_type=authorization_code&client_id=" + app_id + "&redirect_uri=" + Server.UrlEncode(my_url)
                + "&client_secret=" + app_secret + "&code=" + code;
                string response = HttpHelper.WebPageContentGet(token_url, System.Text.Encoding.UTF8);
                NameValueCollection user = ParseJson(response);
                if (!string.IsNullOrEmpty(user["error"]))
                {
                    return View(new OpenLoginViewModel()
                    {
                        Success = false,
                        Error = user["error"].ToString(),
                        Message = user["error_description"].ToString()
                    });
                }
                OpenLoginViewModel OpenUser = new OpenLoginViewModel()
                {
                    Success = true,
                    OpenType = (int)OpenLoginType.Renren,
                    NickName = user["name"].ToString(),
                    OpenId = user["access_token"].ToString()
                };
                if (MemberService.OpenUserLogin(OpenUser, OpenLoginType.Renren))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["registerAuto"] = OpenUser;
                    return RedirectToAction("RegAuto", "Reg");
                }
            }
            else
            {
                return View(new OpenLoginViewModel()
                {
                    Success = false,
                    Error = "The state does not match. You may be a victim of CSRF",
                    Message = "request=" + Request["state"] + ",session=" + Session["state"]
                });

            }
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

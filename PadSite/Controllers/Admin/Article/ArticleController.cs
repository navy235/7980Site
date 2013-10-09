using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Maitonn.Core;
using PadSite.Service.Interface;
using PadSite.Models;
using PadSite.ViewModels;
using PadSite.Utils;

namespace PadSite.Controllers
{

    public class ArticleController : Controller
    {
        //
        // GET: /Article/
        private IArticleService ArticleService;
        private IArticleCateService ArticleCateService;
        public ArticleController(
             IArticleService _ArticleService,
             IArticleCateService _ArticleCateService
          )
        {
            ArticleService = _ArticleService;
            ArticleCateService = _ArticleCateService;
        }

        #region KendoGrid Action

        public ActionResult Index()
        {
            ViewBag.ArticleCode = GetForeignData();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            var Articles = ArticleService.GetKendoALL();
            return Json(Articles.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Article> Articles)
        {
            var results = new List<Article>();

            if (Articles != null && ModelState.IsValid)
            {
                foreach (var Article in Articles)
                {
                    ArticleService.Create(Article);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Article> Articles)
        {
            if (Articles.Any())
            {
                foreach (var Article in Articles)
                {
                    ArticleService.Delete(Article);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion


        #region Create Edit
        public ActionResult Create()
        {

            ViewBag.Data_ArticleCode = GetForeignData();
            return View(new ArticleViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleViewModel model)
        {
            ViewBag.Data_ArticleCode = GetForeignData();
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Article article = new Article()
                    {
                        AddTime = DateTime.Now,
                        ArticleCodeValue = model.ArticleCode,
                        Content = model.Content,
                        Name = model.Name,
                        LastTime = DateTime.Now,
                        ArticleCode = Convert.ToInt32(model.ArticleCode.Split(',').Last())
                    };

                    ArticleService.Create(article);
                    result.Message = "添加文章信息成功！";
                    LogHelper.WriteLog("添加文章信息成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加文章信息错误", ex);
                    return View(model);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {


            Article article = ArticleService.Find(id);
            List<int> ids = article.ArticleCodeValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            ViewBag.Data_ArticleCode = GetForeignData(ids);
            ArticleViewModel model = new ArticleViewModel()
            {
                Name = article.Name,
                ID = article.ID,
                ArticleCode = article.ArticleCodeValue,
                Content = article.Content
            };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleViewModel model)
        {
            List<int> ids = model.ArticleCode.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            ViewBag.Data_ArticleCode = GetForeignData(ids);
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Article entity = new Article()
                    {
                        ID = model.ID,
                        Name = model.Name,
                        Content = model.Content,
                        ArticleCodeValue = model.ArticleCode,
                        ArticleCode = Convert.ToInt32(model.ArticleCode.Split(',').Last()),
                        LastTime = DateTime.Now
                    };
                    ArticleService.Update(entity);
                    result.Message = "编辑文章信息成功！";
                    LogHelper.WriteLog("编辑文章信息成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("编辑文章信息错误", ex);
                    return View(model);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");
                return View(model);
            }

        }
        #endregion


        public List<SelectListItem> GetForeignData(List<int> selectIdList)
        {
            List<SelectListItem> data = new List<SelectListItem>();
            data = ArticleCateService.GetALL().ToList().Select(x => new SelectListItem
            {
                Text = x.CateName,
                Value = x.ID.ToString(),
                Selected = selectIdList.Contains(x.ID)
            }).ToList();
            return data;
        }

        public List<SelectListItem> GetForeignData()
        {
            return GetForeignData(new List<int>());
        }
    }
}

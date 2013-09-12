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
    public class IndustryCateController : Controller
    {
        //
        // GET: /Area/
        private IIndustryCateService IndustryCateService;
        public IndustryCateController(
             IIndustryCateService _IndustryCateService
          )
        {
            IndustryCateService = _IndustryCateService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = IndustryCateService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            return View(new IndustryCateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IndustryCateViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    IndustryCate entity = new IndustryCate();
                    entity.CateName = model.CateName;

                    IndustryCateService.Create(entity);
                    result.Message = "添加投放行业分类成功！";
                    LogHelper.WriteLog("添加投放行业分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加投放行业分类错误", ex);
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



        public ActionResult Edit(int ID)
        {

            IndustryCateViewModel model = new IndustryCateViewModel();
            var entity = IndustryCateService.Find(ID);
            model.CateName = entity.CateName;
            model.ID = entity.ID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IndustryCateViewModel model)
        {

            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    IndustryCate entity = new IndustryCate();
                    entity.ID = model.ID;
                    entity.CateName = model.CateName;

                    IndustryCateService.Update(entity);
                    result.Message = "编辑投放行业分类成功！";
                    LogHelper.WriteLog("编辑投放行业分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加投放行业分类错误", ex);
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

        [HttpPost]
        public ActionResult Delete(string ids)
        {
            ServiceResult result = new ServiceResult();
            var IdArr = ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            try
            {
                for (var i = 0; i < IdArr.Count; i++)
                {
                    var model = IndustryCateService.Find(IdArr[i]);
                    IndustryCateService.Delete(model);
                }
                LogHelper.WriteLog("删除投放行业分类成功");
                result.Message = "删除投放行业分类成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除投放行业分类错误！";
                result.AddServiceError("删除投放行业分类错误!");
                LogHelper.WriteLog("删除投放行业分类错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region private Method

        private List<SelectListItem> GetSelectList(int value = 0)
        {
            var list = Utilities.GetSelectListData(
                    IndustryCateService.GetALL().ToList()
                    , item => item.ID
                    , item => item.CateName, true).ToList();

            list.Single(x => x.Value == value.ToString()).Selected = true;

            return list;
        }
        #endregion
    }
}


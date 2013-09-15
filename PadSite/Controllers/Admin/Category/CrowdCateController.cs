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
    public class CrowdCateController : Controller
    {
        //
        // GET: /Area/
        private ICrowdCateService CrowdCateService;
        public CrowdCateController(
             ICrowdCateService _CrowdCateService
          )
        {
            CrowdCateService = _CrowdCateService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = CrowdCateService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            return View(new CrowdCateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrowdCateViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    CrowdCate entity = new CrowdCate();
                    entity.CateName = model.CateName;

                    CrowdCateService.Create(entity);
                    result.Message = "添加人群分类成功！";
                    LogHelper.WriteLog("添加人群分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加人群分类错误", ex);
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

            CrowdCateViewModel model = new CrowdCateViewModel();
            var entity = CrowdCateService.Find(ID);
            model.CateName = entity.CateName;
            model.ID = entity.ID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrowdCateViewModel model)
        {

            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    CrowdCate entity = new CrowdCate();
                    entity.ID = model.ID;
                    entity.CateName = model.CateName;

                    CrowdCateService.Update(entity);
                    result.Message = "编辑人群分类成功！";
                    LogHelper.WriteLog("编辑人群分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加人群分类错误", ex);
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
                    var model = CrowdCateService.Find(IdArr[i]);
                    CrowdCateService.Delete(model);
                }
                LogHelper.WriteLog("删除人群分类成功");
                result.Message = "删除人群分类成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除人群分类错误！";
                result.AddServiceError("删除人群分类错误!");
                LogHelper.WriteLog("删除人群分类错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region private Method

        private List<SelectListItem> GetSelectList(int value = 0)
        {
            var list = Utilities.GetSelectListData(
                    CrowdCateService.GetALL().ToList()
                    , item => item.ID
                    , item => item.CateName, true).ToList();

            if (value != 0)
            {
                list.Single(x => x.Value == value.ToString()).Selected = true;
            }

            return list;
        }
        #endregion
    }
}


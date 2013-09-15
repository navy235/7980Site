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
    public class AreaCateController : Controller
    {
        //
        // GET: /Area/
        private IAreaCateService AreaCateService;
        public AreaCateController(
             IAreaCateService _AreaCateService
          )
        {
            AreaCateService = _AreaCateService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = AreaCateService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            return View(new AreaCateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaCateViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    AreaCate entity = new AreaCate();
                    entity.CateName = model.CateName;

                    AreaCateService.Create(entity);
                    result.Message = "添加商业区分类成功！";
                    LogHelper.WriteLog("添加商业区分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加商业区分类错误", ex);
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

            AreaCateViewModel model = new AreaCateViewModel();
            var entity = AreaCateService.Find(ID);
            model.CateName = entity.CateName;
            model.ID = entity.ID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaCateViewModel model)
        {

            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    AreaCate entity = new AreaCate();
                    entity.ID = model.ID;
                    entity.CateName = model.CateName;

                    AreaCateService.Update(entity);
                    result.Message = "编辑商业区分类成功！";
                    LogHelper.WriteLog("编辑商业区分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加商业区分类错误", ex);
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
                    var model = AreaCateService.Find(IdArr[i]);
                    AreaCateService.Delete(model);
                }
                LogHelper.WriteLog("删除商业区分类成功");
                result.Message = "删除商业区分类成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除商业区分类错误！";
                result.AddServiceError("删除商业区分类错误!");
                LogHelper.WriteLog("删除商业区分类错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region private Method

        private List<SelectListItem> GetSelectList(int value = 0)
        {
            var list = Utilities.GetSelectListData(
                    AreaCateService.GetALL().ToList()
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


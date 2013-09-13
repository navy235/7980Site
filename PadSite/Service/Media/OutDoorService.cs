﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PadSite.Models;
using PadSite.Service.Interface;
using PadSite.ViewModels;
using PadSite.Utils;
using Maitonn.Core;
using Kendo.Mvc.Extensions;
namespace PadSite.Service
{
    public class OutDoorService : IOutDoorService
    {
        private readonly IUnitOfWork db;
        private readonly IIndustryCateService IndustryCateService;
        private readonly ICrowdCateService CrowdCateService;
        private readonly IAreaCateService AreaCateService;
        private readonly IPurposeCateService PurposeCateService;
        public OutDoorService(IUnitOfWork db
            , IIndustryCateService IndustryCateService
            , ICrowdCateService CrowdCateService
            , IAreaCateService AreaCateService
            , IPurposeCateService PurposeCateService
            )
        {
            this.db = db;
            this.IndustryCateService = IndustryCateService;
            this.CrowdCateService = CrowdCateService;
            this.AreaCateService = AreaCateService;
            this.PurposeCateService = PurposeCateService;
        }

        public IQueryable<OutDoor> GetALL()
        {
            return db.Set<OutDoor>();
        }

        public IQueryable<OutDoor> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<OutDoor>();
        }

        public void Create(OutDoor model)
        {
            db.Add<OutDoor>(model);
            db.Commit();
        }

        public void Update(OutDoor model)
        {
            var target = Find(model.ID);
            db.Attach<OutDoor>(target);
            target.Name = model.Name;
            db.Commit();
        }

        public void Delete(OutDoor model)
        {
            var target = Find(model.ID);
            db.Remove<OutDoor>(target);
            db.Commit();
        }

        public OutDoor Find(int ID)
        {
            return db.Set<OutDoor>().Single(x => x.ID == ID);
        }


        public OutDoor Create(OutDoorViewModel model)
        {

            OutDoor od = new OutDoor();
            od.AddIP = HttpHelper.IP;
            od.AddTime = DateTime.Now;
            od.CityCodeValue = model.CityCode;
            od.CityCode = Utilities.GetCascadingId(model.CityCode);
            od.Description = model.Description;
            od.FormatCode = model.FormatCode;
            od.HasLight = model.HasLight;
            od.LastIP = HttpHelper.IP;
            od.LastTime = DateTime.Now;
            od.Lat = Convert.ToDouble(model.Position.Split('|')[0]);
            od.Lng = Convert.ToDouble(model.Position.Split('|')[1]);
            od.Location = model.Location;
            od.MemberID = CookieHelper.MemberID;
            od.MediaCodeValue = model.MediaCode;
            od.MediaCode = Utilities.GetCascadingId(model.MediaCode);
            od.Name = model.Name;
            od.PeriodCode = model.PeriodCode;
            od.Price = model.Price;
            od.PriceExten = model.PriceExten;

            od.TrafficAuto = model.TrafficAuto;
            od.TrafficPerson = model.TrafficPerson;
            od.VideoUrl = model.VideoUrl;
            od.Unapprovedlog = string.Empty;


            //MediaArea参数设置
            if (!string.IsNullOrEmpty(model.MediaArea))
            {
                var areaParams = model.MediaArea.Split('|');
                var IsRegular = Convert.ToBoolean(areaParams[0]);
                if (IsRegular)
                {
                    od.IsRegular = true;
                    od.Wdith = Convert.ToDecimal(areaParams[1]);
                    od.Height = Convert.ToDecimal(areaParams[2]);
                    od.TotalFaces = Convert.ToInt32(areaParams[3]);
                    od.TotalArea = od.Wdith * od.Height * od.TotalFaces;
                }
                else
                {
                    od.IsRegular = false;
                    od.IrRegularArea = model.MediaArea;
                    od.TotalArea = 0;
                    for (var i = 1; i < areaParams.Length; i += 2)
                    {
                        od.TotalArea += (Convert.ToDecimal(areaParams[i]) * Convert.ToDecimal(areaParams[i + 1]));
                    }
                    od.TotalFaces = areaParams.Length / 2;
                }
            }
            //Light Time 设置
            if (model.HasLight)
            {
                if (!string.IsNullOrEmpty(model.LightTime) && model.LightTime.Split('|').Length == 2)
                {
                    var LightTime = model.LightTime.Split('|');
                    od.LightStrat = LightTime[0];
                    od.LightEnd = LightTime[1];
                }
            }


            //MediaImg 设置
            od.MediaImg = model.MediaImg;
            od.MediaFoucsImg = UIHelper.GetImgUrl(model.MediaImg.Split(',')[0], ImgUrlType.Img120);


            //补充信息设置
            var AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var AreaCateList = AreaCateService.GetALL().Where(x => AreaCateArray.Contains(x.ID));
            od.AreaCate.AddRange(AreaCateList);

            if (!string.IsNullOrEmpty(model.IndustryCate))
            {
                var IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var IndustryCateList = IndustryCateService.GetALL().Where(x => IndustryCateArray.Contains(x.ID));
                od.IndustryCate.AddRange(IndustryCateList);
            }

            if (!string.IsNullOrEmpty(model.CrowdCate))
            {
                var CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var CrowdCateList = CrowdCateService.GetALL().Where(x => CrowdCateArray.Contains(x.ID));
                od.CrowdCate.AddRange(CrowdCateList);
            }

            if (!string.IsNullOrEmpty(model.PurposeCate))
            {
                var PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var PurposeCateList = PurposeCateService.GetALL().Where(x => PurposeCateArray.Contains(x.ID));
                od.PurposeCate.AddRange(PurposeCateList);
            }

            //认证信息设置
            od.OwnerCode = model.OwnerCode;
            if (!string.IsNullOrEmpty(model.CredentialsImg))
            {
                od.CredentialsImg = model.CredentialsImg;
            }

            od.OwnerCode = model.OwnerCode;
            od.Deadline = model.Deadline;

            //set OutDoor Status 待审核状态
            od.Status = (int)OutDoorStatus.PreVerify;

            db.Add<OutDoor>(od);
            db.Commit();
            return od;
        }


        public void ChangeStatus(string ids, OutDoorStatus Status)
        {
            var IdsArray = Utilities.GetIdList(ids);
            db.Set<OutDoor>().Where(x => IdsArray.Contains(x.ID)).ToList().ForEach(x => x.Status = (int)Status);
            db.Commit();
        }


        public OutDoor Update(OutDoorViewModel model)
        {
            OutDoor od = GetALL()
                      .Include(x => x.AreaCate)
                      .Include(x => x.CrowdCate)
                      .Include(x => x.IndustryCate)
                      .Include(x => x.PurposeCate)
                      .Single(x => x.ID == model.ID);
            db.Attach<OutDoor>(od);
            od.CityCodeValue = model.CityCode;
            od.CityCode = Utilities.GetCascadingId(model.CityCode);
            od.PeriodCode = model.PeriodCode;
            od.MediaCodeValue = model.MediaCode;
            od.MediaCode = Utilities.GetCascadingId(model.MediaCode);
            od.FormatCode = model.FormatCode;
            od.Description = model.Description;
            od.HasLight = model.HasLight;
            od.LastIP = HttpHelper.IP;
            od.LastTime = DateTime.Now;
            od.Lat = Convert.ToDouble(model.Position.Split('|')[0]);
            od.Lng = Convert.ToDouble(model.Position.Split('|')[1]);
            od.Location = model.Location;
            od.Name = model.Name;
            od.Price = model.Price;
            od.PriceExten = model.PriceExten;
            od.TrafficAuto = model.TrafficAuto;
            od.TrafficPerson = model.TrafficPerson;
            od.VideoUrl = model.VideoUrl;
            od.Unapprovedlog = string.Empty;


            //MediaArea参数设置
            if (!string.IsNullOrEmpty(model.MediaArea))
            {
                var areaParams = model.MediaArea.Split('|');
                var IsRegular = Convert.ToBoolean(areaParams[0]);
                if (IsRegular)
                {
                    od.IsRegular = true;
                    od.Wdith = Convert.ToDecimal(areaParams[1]);
                    od.Height = Convert.ToDecimal(areaParams[2]);
                    od.TotalFaces = Convert.ToInt32(areaParams[3]);
                    od.TotalArea = od.Wdith * od.Height * od.TotalFaces;
                }
                else
                {
                    od.IsRegular = false;
                    od.IrRegularArea = model.MediaArea;
                    od.TotalArea = 0;
                    for (var i = 1; i < areaParams.Length; i += 2)
                    {
                        od.TotalArea += (Convert.ToDecimal(areaParams[i]) * Convert.ToDecimal(areaParams[i + 1]));
                    }
                    od.TotalFaces = areaParams.Length / 2;
                }
            }
            //Light Time 设置
            if (model.HasLight)
            {
                if (!string.IsNullOrEmpty(model.LightTime) && model.LightTime.Split('|').Length == 2)
                {
                    var LightTime = model.LightTime.Split('|');
                    od.LightStrat = LightTime[0];
                    od.LightEnd = LightTime[1];
                }
            }


            //MediaImg 设置
            od.MediaImg = model.MediaImg;
            od.MediaFoucsImg = UIHelper.GetImgUrl(model.MediaImg.Split(',')[0], ImgUrlType.Img120);

            var AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            if (AreaCateArray.Count == 0)
            {
                od.AreaCate = new List<AreaCate>();
            }
            else
            {
                var AreaCateList = AreaCateService.GetALL().Where(x => AreaCateArray.Contains(x.ID));
                var currentAreaCateArray = od.AreaCate.Select(x => x.ID).ToList();

                foreach (AreaCate ac in AreaCateService.GetALL())
                {
                    if (AreaCateArray.Contains(ac.ID))
                    {
                        if (!currentAreaCateArray.Contains(ac.ID))
                        {
                            od.AreaCate.Add(ac);
                        }
                    }
                    else
                    {
                        if (currentAreaCateArray.Contains(ac.ID))
                        {
                            od.AreaCate.Remove(ac);
                        }
                    }
                }
            }


            var CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            if (CrowdCateArray.Count == 0)
            {
                od.CrowdCate = new List<CrowdCate>();
            }
            else
            {
                var CrowdCateList = CrowdCateService.GetALL().Where(x => CrowdCateArray.Contains(x.ID));
                var currentCrowdCateArray = od.CrowdCate.Select(x => x.ID).ToList();

                foreach (CrowdCate ac in CrowdCateService.GetALL())
                {
                    if (CrowdCateArray.Contains(ac.ID))
                    {
                        if (!currentCrowdCateArray.Contains(ac.ID))
                        {
                            od.CrowdCate.Add(ac);
                        }
                    }
                    else
                    {
                        if (currentCrowdCateArray.Contains(ac.ID))
                        {
                            od.CrowdCate.Remove(ac);
                        }
                    }
                }
            }

            var IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            if (IndustryCateArray.Count == 0)
            {
                od.IndustryCate = new List<IndustryCate>();
            }
            else
            {
                var IndustryCateList = IndustryCateService.GetALL().Where(x => IndustryCateArray.Contains(x.ID));
                var currentIndustryCateArray = od.IndustryCate.Select(x => x.ID).ToList();

                foreach (IndustryCate ac in IndustryCateService.GetALL())
                {
                    if (IndustryCateArray.Contains(ac.ID))
                    {
                        if (!currentIndustryCateArray.Contains(ac.ID))
                        {
                            od.IndustryCate.Add(ac);
                        }
                    }
                    else
                    {
                        if (currentIndustryCateArray.Contains(ac.ID))
                        {
                            od.IndustryCate.Remove(ac);
                        }
                    }
                }
            }

            var PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            if (PurposeCateArray.Count == 0)
            {
                od.PurposeCate = new List<PurposeCate>();
            }
            else
            {
                var PurposeCateList = PurposeCateService.GetALL().Where(x => PurposeCateArray.Contains(x.ID));
                var currentPurposeCateArray = od.PurposeCate.Select(x => x.ID).ToList();

                foreach (PurposeCate ac in PurposeCateService.GetALL())
                {
                    if (PurposeCateArray.Contains(ac.ID))
                    {
                        if (!currentPurposeCateArray.Contains(ac.ID))
                        {
                            od.PurposeCate.Add(ac);
                        }
                    }
                    else
                    {
                        if (currentPurposeCateArray.Contains(ac.ID))
                        {
                            od.PurposeCate.Remove(ac);
                        }
                    }
                }
            }

            //set OutDoor Status 待审核状态
            od.Status = (int)OutDoorStatus.PreVerify;

            db.Commit();

            return od;
        }
    }
}
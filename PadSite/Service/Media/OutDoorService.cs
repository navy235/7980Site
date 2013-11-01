using System;
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
        private readonly IOutDoorLuceneService OutDoorLuceneService;
        private readonly IMemberService MemberService;
        private readonly IMessageService MessageService;
        public OutDoorService(IUnitOfWork db
            , IIndustryCateService IndustryCateService
            , ICrowdCateService CrowdCateService
            , IAreaCateService AreaCateService
            , IPurposeCateService PurposeCateService
            , IOutDoorLuceneService OutDoorLuceneService
            , IMemberService MemberService
            , IMessageService MessageService
            )
        {
            this.db = db;
            this.IndustryCateService = IndustryCateService;
            this.CrowdCateService = CrowdCateService;
            this.AreaCateService = AreaCateService;
            this.PurposeCateService = PurposeCateService;
            this.OutDoorLuceneService = OutDoorLuceneService;
            this.MemberService = MemberService;
            this.MessageService = MessageService;
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
            if (string.IsNullOrEmpty(model.Description))
            {
                od.Description = string.Empty;
            }
            else
            {
                od.Description = model.Description;
            }
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
            od.RealPrice = model.RealPrice;
            //od.PriceExten = model.PriceExten;

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
                    od.LightStart = Convert.ToInt32(LightTime[0]);
                    od.LightEnd = Convert.ToInt32(LightTime[1]);
                }
            }


            //MediaImg 设置
            od.MediaImg = model.MediaImg;
            od.MediaFoucsImg = UIHelper.GetImgUrl(model.MediaImg.Split(',')[0], ImgUrlType.Img120);


            //补充信息设置
            //if (!string.IsNullOrEmpty(model.AreaCate))
            //{
            //    var AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var AreaCateList = AreaCateService.GetALL().Where(x => AreaCateArray.Contains(x.ID));
            //    od.AreaCate.AddRange(AreaCateList);
            //}

            //if (!string.IsNullOrEmpty(model.IndustryCate))
            //{
            //    var IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var IndustryCateList = IndustryCateService.GetALL().Where(x => IndustryCateArray.Contains(x.ID));
            //    od.IndustryCate.AddRange(IndustryCateList);
            //}

            //if (!string.IsNullOrEmpty(model.CrowdCate))
            //{
            //    var CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var CrowdCateList = CrowdCateService.GetALL().Where(x => CrowdCateArray.Contains(x.ID));
            //    od.CrowdCate.AddRange(CrowdCateList);
            //}

            //if (!string.IsNullOrEmpty(model.PurposeCate))
            //{
            //    var PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var PurposeCateList = PurposeCateService.GetALL().Where(x => PurposeCateArray.Contains(x.ID));
            //    od.PurposeCate.AddRange(PurposeCateList);
            //}

            //认证信息设置
            //od.OwnerCode = model.OwnerCode;
            //if (!string.IsNullOrEmpty(model.CredentialsImg))
            //{
            //    od.CredentialsImg = model.CredentialsImg;
            //}

            //od.OwnerCode = model.OwnerCode;
            od.Deadline = model.Deadline;

            //set OutDoor Status 待审核状态
            od.Status = (int)OutDoorStatus.PreVerify;

            db.Add<OutDoor>(od);
            db.Commit();
            return od;
        }

        public void ChangeStatus(string ids, OutDoorStatus Status, string reason = null)
        {
            var IdsArray = Utilities.GetIdList(ids);

            var dbStatus = Status;
            if (Status == OutDoorStatus.Verified)
            {
                dbStatus = OutDoorStatus.ShowOnline;
            }
            var info = string.Empty;
            if (Status == OutDoorStatus.VerifyFailed)
            {
                if (!string.IsNullOrEmpty(reason))
                {
                    info = reason;
                }
                else
                {
                    info = "请检查信息是否含有非法字符，或者表单是否填写完整";
                }
            }
            db.Set<OutDoor>().Where(x => IdsArray.Contains(x.ID)).ToList()
                .ForEach(x =>
                {
                    x.Status = (int)dbStatus;
                    x.Unapprovedlog = reason;
                });
            db.Commit();
            if (Status == OutDoorStatus.Verified)
            {
                OutDoorLuceneService.CreateIndex(ids);
                SendAuthedMessage(IdsArray, reason);
            }
            else
            {
                OutDoorLuceneService.UpdateIndex(ids);
                if (Status == OutDoorStatus.VerifyFailed)
                {
                    SendAuthFieldedMessage(IdsArray, reason);
                }
            }
        }

        private void SendAuthedMessage(IEnumerable<int> IdsArray, string reason = null)
        {
            foreach (var id in IdsArray)
            {
                var outdoor = Find(id);
                var memberID = outdoor.MemberID;
                var info = "您好！您发布媒体信息:(" + outdoor.Name + ")通过审核";
                if (!string.IsNullOrEmpty(reason))
                {
                    info = reason;
                }
                //var member = MemberService.Find(memberID);
                var message = new Message()
                {
                    AddTime = DateTime.Now,
                    SenderID = 0,
                    RecipientID = memberID,
                    MessageType = (int)MessageType.System,
                    RecipienterStatus = (int)MessageStatus.Show,
                    Title = info,
                    Content = info
                };
                MessageService.Create(message);
            }
        }

        private void SendAuthFieldedMessage(IEnumerable<int> IdsArray, string reason = null)
        {
            foreach (var id in IdsArray)
            {
                var outdoor = Find(id);
                var memberID = outdoor.MemberID;
                var info = "您好！您发布媒体信息:(" + outdoor.Name + ")未通过审核";
                var infoContent = "【请检查信息是否含有非法字符，或者表单是否填写完整】";
                if (!string.IsNullOrEmpty(reason))
                {
                    infoContent = reason;
                }
                var message = new Message()
                {
                    AddTime = DateTime.Now,
                    SenderID = 0,
                    RecipientID = memberID,
                    MessageType = (int)MessageType.System,
                    RecipienterStatus = (int)MessageStatus.Show,
                    Title = info,
                    Content = infoContent
                };
                MessageService.Create(message);
            }
        }

        public OutDoor Update(OutDoorViewModel model)
        {
            OutDoor od = GetALL()
                      //.Include(x => x.AreaCate)
                      //.Include(x => x.CrowdCate)
                      //.Include(x => x.IndustryCate)
                      //.Include(x => x.PurposeCate)
                      .Single(x => x.ID == model.ID);
            db.Attach<OutDoor>(od);
            od.CityCodeValue = model.CityCode;
            od.CityCode = Utilities.GetCascadingId(model.CityCode);
            od.PeriodCode = model.PeriodCode;
            od.MediaCodeValue = model.MediaCode;
            od.MediaCode = Utilities.GetCascadingId(model.MediaCode);
            od.FormatCode = model.FormatCode;
            if (string.IsNullOrEmpty(model.Description))
            {
                od.Description = string.Empty;
            }
            else
            {
                od.Description = model.Description;
            }
            od.HasLight = model.HasLight;
            od.LastIP = HttpHelper.IP;
            od.LastTime = DateTime.Now;
            od.Lat = Convert.ToDouble(model.Position.Split('|')[0]);
            od.Lng = Convert.ToDouble(model.Position.Split('|')[1]);
            od.Location = model.Location;
            od.Name = model.Name;
            od.Price = model.Price;
            od.RealPrice = model.RealPrice;
            //od.PriceExten = model.PriceExten;
            od.TrafficAuto = model.TrafficAuto;
            od.TrafficPerson = model.TrafficPerson;
            od.VideoUrl = model.VideoUrl;
            od.Unapprovedlog = string.Empty;
            od.Deadline = model.Deadline;
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
                    od.LightStart = Convert.ToInt32(LightTime[0]);
                    od.LightEnd = Convert.ToInt32(LightTime[1]);
                }
            }


            //MediaImg 设置
            od.MediaImg = model.MediaImg;
            od.MediaFoucsImg = UIHelper.GetImgUrl(model.MediaImg.Split(',')[0], ImgUrlType.Img120);

            //od.CredentialsImg = model.CredentialsImg;

            //var AreaCateArray = new List<int>();
            //if (string.IsNullOrEmpty(model.AreaCate))
            //{
            //    od.AreaCate = new List<AreaCate>();
            //}
            //else
            //{
            //    AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var AreaCateList = AreaCateService.GetALL().Where(x => AreaCateArray.Contains(x.ID));
            //    var currentAreaCateArray = od.AreaCate.Select(x => x.ID).ToList();

            //    foreach (AreaCate ac in AreaCateService.GetALL())
            //    {
            //        if (AreaCateArray.Contains(ac.ID))
            //        {
            //            if (!currentAreaCateArray.Contains(ac.ID))
            //            {
            //                od.AreaCate.Add(ac);
            //            }
            //        }
            //        else
            //        {
            //            if (currentAreaCateArray.Contains(ac.ID))
            //            {
            //                od.AreaCate.Remove(ac);
            //            }
            //        }
            //    }
            //}

            //var CrowdCateArray = new List<int>();
            //if (string.IsNullOrEmpty(model.CrowdCate))
            //{
            //    od.CrowdCate = new List<CrowdCate>();
            //}
            //else
            //{
            //    CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var CrowdCateList = CrowdCateService.GetALL().Where(x => CrowdCateArray.Contains(x.ID));
            //    var currentCrowdCateArray = od.CrowdCate.Select(x => x.ID).ToList();

            //    foreach (CrowdCate ac in CrowdCateService.GetALL())
            //    {
            //        if (CrowdCateArray.Contains(ac.ID))
            //        {
            //            if (!currentCrowdCateArray.Contains(ac.ID))
            //            {
            //                od.CrowdCate.Add(ac);
            //            }
            //        }
            //        else
            //        {
            //            if (currentCrowdCateArray.Contains(ac.ID))
            //            {
            //                od.CrowdCate.Remove(ac);
            //            }
            //        }
            //    }
            //}

            //var IndustryCateArray = new List<int>();
            //if (string.IsNullOrEmpty(model.IndustryCate))
            //{
            //    od.IndustryCate = new List<IndustryCate>();
            //}
            //else
            //{
            //    IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var IndustryCateList = IndustryCateService.GetALL().Where(x => IndustryCateArray.Contains(x.ID));
            //    var currentIndustryCateArray = od.IndustryCate.Select(x => x.ID).ToList();

            //    foreach (IndustryCate ac in IndustryCateService.GetALL())
            //    {
            //        if (IndustryCateArray.Contains(ac.ID))
            //        {
            //            if (!currentIndustryCateArray.Contains(ac.ID))
            //            {
            //                od.IndustryCate.Add(ac);
            //            }
            //        }
            //        else
            //        {
            //            if (currentIndustryCateArray.Contains(ac.ID))
            //            {
            //                od.IndustryCate.Remove(ac);
            //            }
            //        }
            //    }
            //}

            //var PurposeCateArray = new List<int>();
            //if (string.IsNullOrEmpty(model.PurposeCate))
            //{
            //    od.PurposeCate = new List<PurposeCate>();
            //}
            //else
            //{
            //    PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //    var PurposeCateList = PurposeCateService.GetALL().Where(x => PurposeCateArray.Contains(x.ID));
            //    var currentPurposeCateArray = od.PurposeCate.Select(x => x.ID).ToList();

            //    foreach (PurposeCate ac in PurposeCateService.GetALL())
            //    {
            //        if (PurposeCateArray.Contains(ac.ID))
            //        {
            //            if (!currentPurposeCateArray.Contains(ac.ID))
            //            {
            //                od.PurposeCate.Add(ac);
            //            }
            //        }
            //        else
            //        {
            //            if (currentPurposeCateArray.Contains(ac.ID))
            //            {
            //                od.PurposeCate.Remove(ac);
            //            }
            //        }
            //    }
            //}

            //set OutDoor Status 待审核状态
            od.Status = (int)OutDoorStatus.PreVerify;
            db.Commit();
            OutDoorLuceneService.UpdateIndex(model.ID.ToString());
            return od;
        }


        public void ChangeSuggestStatus(string ids, int SuggestStatus)
        {
            var IdsArray = Utilities.GetIdList(ids);
            db.Set<OutDoor>().Where(x => IdsArray.Contains(x.ID)).ToList()
                .ForEach(x =>
                {
                    x.SuggestStatus = SuggestStatus;
                });
            db.Commit();
            OutDoorLuceneService.UpdateIndex(ids);

        }
    }
}
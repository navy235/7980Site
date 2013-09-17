﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data.Entity;
using PadSite.Models;
using PadSite.Service.Interface;
using PadSite.ViewModels;
using PadSite.Utils;
using Maitonn.Core;
using Lucene.Net.Util;
using Kendo.Mvc.Extensions;
using PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Tokenattributes;
using Directory = System.IO.Directory;


namespace PadSite.Service
{
    public class OutDoorLuceneService : IOutDoorLuceneService
    {
        private static readonly object IndexWriterLock = new object();

        private static IndexWriter _indexWriter;

        private readonly IUnitOfWork db;
        private readonly IMediaCateService MediaCateService;
        private readonly ICityCateService CityCateService;
        private readonly IIndustryCateService IndustryCateService;
        private readonly ICrowdCateService CrowdCateService;
        private readonly IOwnerCateService OwnerCateService;
        private readonly IAreaCateService AreaCateService;
        private readonly IPurposeCateService PurposeCateService;
        private readonly IFormatCateService FormatCateService;
        private readonly IPeriodCateService PeriodCateService;
        public OutDoorLuceneService(
            IUnitOfWork db,
            IIndustryCateService IndustryCateService,
            ICrowdCateService CrowdCateService,
            IOwnerCateService OwnerCateService,
            IAreaCateService AreaCateService,
            IPurposeCateService PurposeCateService,
            IFormatCateService FormatCateService,
            IPeriodCateService PeriodCateService,
            ICityCateService CityCateService,
            IMediaCateService MediaCateService)
        {
            this.db = db;
            this.IndustryCateService = IndustryCateService;
            this.CrowdCateService = CrowdCateService;
            this.OwnerCateService = OwnerCateService;
            this.AreaCateService = AreaCateService;
            this.PurposeCateService = PurposeCateService;
            this.FormatCateService = FormatCateService;
            this.PeriodCateService = PeriodCateService;
            this.CityCateService = CityCateService;
            this.MediaCateService = MediaCateService;
        }

        public void CreateIndex(string ids)
        {
            try
            {
                EnsureIndexWriter(true);
                var Ids = Utilities.GetIdList(ids);
                var data = GetOutDoors(Ids);
                var IdsQuery = from ID in data.Select(p => p.ID).Distinct()
                               select new Term(OutDoorIndexFields.ID, ID.ToString(CultureInfo.InvariantCulture));
                _indexWriter.DeleteDocuments(IdsQuery.ToArray());
                data.ForEach(x => WriteIndex(x));
                _indexWriter.Commit();
                _indexWriter.Optimize();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Lucene添加媒体索引出错", ex);
            }
        }
        public void ChangeStatus(string ids, OutDoorStatus Status)
        {
            try
            {
                EnsureIndexWriter(false);
                if (!Directory.Exists(LuceneCommon.IndexOutDoorDirectory))
                {
                    LogHelper.WriteLog("Lucene更新媒体索引状态出错,不存在索引目录");
                }
                var Ids = Utilities.GetIdList(ids);
                var data = GetOutDoors(Ids);
                for (var i = 0; i < Ids.Count; i++)
                {
                    var id = Ids[i];
                    var outDoorIndexEntity = data.Single(x => x.ID == id);
                    var doc = CreateDocument(outDoorIndexEntity);
                    ChangeStatus(id, doc);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Lucene更新媒体索引状态出错", ex);
            }
        }

        public void ChangeStatus(int id, Document doc)
        {
            var term = new Term(OutDoorIndexFields.ID, id.ToString());
            _indexWriter.UpdateDocument(term, doc);
            _indexWriter.Commit();

        }

        private static void EnsureIndexWriter(bool creatingIndex)
        {
            if (_indexWriter == null)
            {
                lock (IndexWriterLock)
                {
                    if (_indexWriter == null)
                    {
                        EnsureIndexWriterCore(creatingIndex);
                    }
                }
            }
        }

        private static void EnsureIndexWriterCore(bool creatingIndex)
        {
            if (!Directory.Exists(LuceneCommon.IndexOutDoorDirectory))
            {
                Directory.CreateDirectory(LuceneCommon.IndexOutDoorDirectory);
            }

            var analyzer = new PanGuAnalyzer();
            var directoryInfo = new DirectoryInfo(LuceneCommon.IndexOutDoorDirectory);
            var directory = new SimpleFSDirectory(directoryInfo);
            _indexWriter = new IndexWriter(directory, analyzer, create: creatingIndex, mfl: IndexWriter.MaxFieldLength.UNLIMITED);
        }

        private List<OutDoorIndexEntity> GetOutDoors(List<int> Ids)
        {
            List<OutDoorIndexEntity> result = new List<OutDoorIndexEntity>();

            var query = db.Set<OutDoor>()
                .Include(x => x.CityCate)
                .Include(x => x.MediaCate)
                .Include(x => x.AreaCate)
                .Include(x => x.CrowdCate)
                .Include(x => x.IndustryCate)
                .Include(x => x.PurposeCate)
                .Include(x => x.PeriodCate)
                .Include(x => x.FormatCate)
                .Include(x => x.Member)
                .Include(x => x.Member.Company)
                .Where(x => Ids.Contains(x.ID));

            foreach (var entity in query.ToList())
            {
                OutDoorIndexEntity item = new OutDoorIndexEntity();
                item.ID = entity.ID;
                item.MemberID = entity.MemberID;



                var cityIds = entity.CityCodeValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var cityValues = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
                item.CityCode = entity.CityCode;
                item.CityCateCode = entity.CityCate.Code;
                item.CityCateValue = entity.CityCodeValue;
                item.CityCateName = String.Join(",", cityValues);



                var meidaIds = entity.MediaCodeValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var meidaValues = MediaCateService.GetALL().Where(x => meidaIds.Contains(x.ID)).Select(x => x.CateName).ToList();
                item.MediaCode = entity.MediaCode;
                item.MediaCateCode = entity.MediaCate.Code;
                item.MediaCateValue = entity.MediaCodeValue;
                item.MediaCateName = String.Join(",", meidaValues);


                item.FormatCode = entity.FormatCode;
                item.FormatName = entity.FormatCate.CateName;
                item.PeriodCode = entity.PeriodCode;
                item.PeriodName = entity.PeriodCate.CateName;
                item.Price = entity.Price;
                item.OwnerCode = entity.OwnerCode;
                item.OwnerName = entity.OwnerCate.CateName;
                item.Status = entity.Status;
                item.AuthStatus = entity.AuthStatus;
                item.Title = entity.Name;
                item.Description = entity.Description;
                item.ImgUrl = entity.MediaImg;
                item.FocusImgUrl = entity.MediaFoucsImg;
                item.VideoUrl = entity.VideoUrl;
                item.AreaCate = string.Join(",", entity.AreaCate.Select(x => x.CateName).ToList());
                item.CrowdCate = string.Join(",", entity.CrowdCate.Select(x => x.CateName).ToList());
                item.IndustryCate = string.Join(",", entity.IndustryCate.Select(x => x.CateName).ToList());
                item.PurposeCate = string.Join(",", entity.PurposeCate.Select(x => x.CateName).ToList());
                item.IsRegular = entity.IsRegular ? 1 : 0;
                if (entity.IsRegular)
                {
                    item.Width = entity.Wdith;
                    item.Height = entity.Height;
                    item.TotalFaces = entity.TotalFaces;
                    item.TotalArea = entity.Wdith * entity.Height * entity.TotalFaces;
                }
                else
                {
                    item.IrRegularArea = entity.IrRegularArea;

                    var areaParams = entity.IrRegularArea.Split('|').ToList();
                    var totalArea = 0.0m;
                    for (var i = 1; i < areaParams.Count; i += 2)
                    {
                        totalArea += Convert.ToDecimal(areaParams[i]) * Convert.ToDecimal(areaParams[i + 1]);
                    }
                    item.TotalArea = totalArea;

                }
                item.Lat = entity.Lat;
                item.Lng = entity.Lng;
                item.CompanyName = entity.Member.Company.Name;
                item.DeadLine = entity.Deadline;
                item.Published = entity.LastTime;
                item.MemberStatus = entity.Member.Status;
                result.Add(item);
            }
            return result;
        }

        private void WriteIndex(OutDoorIndexEntity OutDoor)
        {
            var document = CreateDocument(OutDoor);
            _indexWriter.AddDocument(document);
        }


        private Document CreateDocument(OutDoorIndexEntity OutDoor)
        {
            var document = new Document();

            var field = new Field(OutDoorIndexFields.Title, OutDoor.Title, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 2.5f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.Description, OutDoor.Description, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 2.1f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.CompanyName, OutDoor.CompanyName, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 2.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.AreaCate, OutDoor.AreaCate, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 1.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.CrowdCate, OutDoor.CrowdCate, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 1.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.IndustryCate, OutDoor.IndustryCate, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 1.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.PurposeCate, OutDoor.PurposeCate, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 1.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.CityCateName, OutDoor.CityCateName, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 1.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.MediaCateName, OutDoor.MediaCateName, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 1.0f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.FormatName, OutDoor.FormatName, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 0.2f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.PeriodName, OutDoor.PeriodName, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 0.2f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.OwnerName, OutDoor.OwnerName, Field.Store.YES, Field.Index.ANALYZED);
            field.Boost = 0.2f;
            document.Add(field);

            field = new Field(OutDoorIndexFields.ImgUrl, OutDoor.ImgUrl, Field.Store.YES, Field.Index.NOT_ANALYZED);
            document.Add(field);

            field = new Field(OutDoorIndexFields.FocusImgUrl, OutDoor.FocusImgUrl, Field.Store.YES, Field.Index.NOT_ANALYZED);
            document.Add(field);

            field = new Field(OutDoorIndexFields.VideoUrl, string.IsNullOrEmpty(OutDoor.VideoUrl) ? string.Empty : OutDoor.VideoUrl, Field.Store.YES, Field.Index.NOT_ANALYZED);
            document.Add(field);

            field = new Field(OutDoorIndexFields.CityCateValue, OutDoor.CityCateValue, Field.Store.YES, Field.Index.NOT_ANALYZED);
            document.Add(field);

            field = new Field(OutDoorIndexFields.MediaCateValue, OutDoor.MediaCateValue, Field.Store.YES, Field.Index.NOT_ANALYZED);
            document.Add(field);


            field = new Field(OutDoorIndexFields.ID, OutDoor.ID.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED);
            document.Add(field);

            document.Add(new NumericField(OutDoorIndexFields.Price, Field.Store.YES, true).SetDoubleValue(Convert.ToDouble(OutDoor.Price)));

            document.Add(new NumericField(OutDoorIndexFields.IsRegular, Field.Store.YES, true).SetIntValue(OutDoor.IsRegular));

            if (OutDoor.IsRegular == 1)
            {
                document.Add(new NumericField(OutDoorIndexFields.Width, Field.Store.YES, true).SetDoubleValue(Convert.ToDouble(OutDoor.Width)));
                document.Add(new NumericField(OutDoorIndexFields.Height, Field.Store.YES, true).SetDoubleValue(Convert.ToDouble(OutDoor.Height)));
                document.Add(new NumericField(OutDoorIndexFields.TotalFaces, Field.Store.YES, true).SetIntValue(OutDoor.TotalFaces));
            }
            else
            {
                field = new Field(OutDoorIndexFields.IrRegularArea, OutDoor.IrRegularArea, Field.Store.YES, Field.Index.NOT_ANALYZED);
                document.Add(field);
            }

            document.Add(new NumericField(OutDoorIndexFields.TotalArea, Field.Store.YES, true).SetDoubleValue(Convert.ToDouble(OutDoor.TotalArea)));
            document.Add(new NumericField(OutDoorIndexFields.CityCode, Field.Store.YES, true).SetIntValue(OutDoor.CityCode));
            document.Add(new NumericField(OutDoorIndexFields.CityCateCode, Field.Store.YES, true).SetLongValue(OutDoor.CityCateCode));

            document.Add(new NumericField(OutDoorIndexFields.MediaCode, Field.Store.YES, true).SetIntValue(OutDoor.MediaCode));
            document.Add(new NumericField(OutDoorIndexFields.MediaCateCode, Field.Store.YES, true).SetLongValue(OutDoor.MediaCateCode));
            document.Add(new NumericField(OutDoorIndexFields.FormatCode, Field.Store.YES, true).SetIntValue(OutDoor.FormatCode));
            document.Add(new NumericField(OutDoorIndexFields.PeriodCode, Field.Store.YES, true).SetIntValue(OutDoor.PeriodCode));
            document.Add(new NumericField(OutDoorIndexFields.OwnerCode, Field.Store.YES, true).SetIntValue(OutDoor.OwnerCode));
            document.Add(new NumericField(OutDoorIndexFields.Status, Field.Store.YES, true).SetIntValue(OutDoor.Status));
            document.Add(new NumericField(OutDoorIndexFields.AuthStatus, Field.Store.YES, true).SetIntValue(OutDoor.AuthStatus));

            document.Add(new NumericField(OutDoorIndexFields.Hit, Field.Store.YES, true).SetIntValue(OutDoor.Hit));
            document.Add(new NumericField(OutDoorIndexFields.Published, Field.Store.YES, true).SetLongValue(OutDoor.Published.Ticks));
            document.Add(new NumericField(OutDoorIndexFields.DeadLine, Field.Store.YES, true).SetLongValue(OutDoor.DeadLine.Ticks));
            document.Add(new NumericField(OutDoorIndexFields.MemberStatus, Field.Store.YES, true).SetIntValue(OutDoor.MemberStatus));
            document.Add(new NumericField(OutDoorIndexFields.Lat, Field.Store.YES, true).SetDoubleValue(OutDoor.Lat));
            document.Add(new NumericField(OutDoorIndexFields.Lng, Field.Store.YES, true).SetDoubleValue(OutDoor.Lng));
            document.Add(new NumericField(OutDoorIndexFields.MemberID, Field.Store.YES, true).SetIntValue(OutDoor.MemberID));
            return document;
        }

        public List<LinkItem> Search(QueryTerm queryTerm, SearchFilter searchFilter, out int totalHits)
        {
            if (!Directory.Exists(LuceneCommon.IndexOutDoorDirectory))
            {
                totalHits = 0;
                return new List<LinkItem>();
            }
            SortField sortField = GetSortField(searchFilter);

            int numRecords = searchFilter.Skip + searchFilter.Take;

            using (var directory = new SimpleFSDirectory(new DirectoryInfo(LuceneCommon.IndexOutDoorDirectory)))
            {
                var searcher = new IndexSearcher(directory, readOnly: true);

                var query = ParseQuery(queryTerm, searchFilter);

                //var termQuery = new TermQuery(new Term(OutDoorIndexFields.ID, "1"));

                var results = searcher.Search(query, filter: null, n: numRecords, sort: new Sort(sortField));

                var keys = results.ScoreDocs.Skip(searchFilter.Skip)
                    .Select(c => GetMediaItem(searcher.Doc(c.Doc)))
                    .ToList();

                totalHits = results.TotalHits;

                searcher.Dispose();

                return keys;
            }
        }

        private static LinkItem GetMediaItem(Document doc)
        {
            int MemberStatus = Int32.Parse(doc.Get(OutDoorIndexFields.MemberStatus), CultureInfo.InvariantCulture);
            int MemberID = Int32.Parse(doc.Get(OutDoorIndexFields.MemberID), CultureInfo.InvariantCulture);
            int Hit = Int32.Parse(doc.Get(OutDoorIndexFields.Hit), CultureInfo.InvariantCulture);
            int ID = Int32.Parse(doc.Get(OutDoorIndexFields.ID), CultureInfo.InvariantCulture);
            int CityCode = Int32.Parse(doc.Get(OutDoorIndexFields.CityCode), CultureInfo.InvariantCulture);
            int FormatCode = Int32.Parse(doc.Get(OutDoorIndexFields.FormatCode), CultureInfo.InvariantCulture);
            int AuthStatus = Int32.Parse(doc.Get(OutDoorIndexFields.AuthStatus), CultureInfo.InvariantCulture);
            int MediaCode = Int32.Parse(doc.Get(OutDoorIndexFields.MediaCode), CultureInfo.InvariantCulture);
            decimal Price = Decimal.Parse(doc.Get(OutDoorIndexFields.Price), CultureInfo.InvariantCulture);

            double Lng = double.Parse(doc.Get(OutDoorIndexFields.Lng), CultureInfo.InvariantCulture);
            double Lat = double.Parse(doc.Get(OutDoorIndexFields.Lat), CultureInfo.InvariantCulture);

            DateTime Published = new DateTime(Int64.Parse(doc.Get(OutDoorIndexFields.Published), CultureInfo.InvariantCulture));
            DateTime DeadLine = new DateTime(Int64.Parse(doc.Get(OutDoorIndexFields.DeadLine), CultureInfo.InvariantCulture));

            int IsRegularValue = Int32.Parse(doc.Get(OutDoorIndexFields.IsRegular), CultureInfo.InvariantCulture);


            int CityCateCode = Int32.Parse(doc.Get(OutDoorIndexFields.CityCateCode), CultureInfo.InvariantCulture);

            int MediaCateCode = Int32.Parse(doc.Get(OutDoorIndexFields.MediaCateCode), CultureInfo.InvariantCulture);

            int Status = Int32.Parse(doc.Get(OutDoorIndexFields.Status), CultureInfo.InvariantCulture);

            bool IsRegular = IsRegularValue == 1;



            LinkItem item = new LinkItem();
            item.ID = ID;
            item.MemberID = MemberID;
            item.Status = Status;
            item.FocusImgUrl = doc.Get(OutDoorIndexFields.FocusImgUrl);
            item.CityCode = CityCode;
            item.CityCateName = doc.Get(OutDoorIndexFields.CityCateName);
            item.CityCateValue = doc.Get(OutDoorIndexFields.CityCateValue);
            item.CityCateCode = CityCateCode;
            item.MediaCode = MediaCode;
            item.MediaCateName = doc.Get(OutDoorIndexFields.MediaCateName);
            item.MediaCateValue = doc.Get(OutDoorIndexFields.MediaCateValue);
            item.MediaCateCode = MediaCateCode;
            item.Name = doc.Get(OutDoorIndexFields.Title);
            item.Description = doc.Get(OutDoorIndexFields.Description);
            item.Price = Price;
            item.PeriodName = doc.Get(OutDoorIndexFields.PeriodName);
            item.FormatName = doc.Get(OutDoorIndexFields.FormatName);
            item.OwnerName = doc.Get(OutDoorIndexFields.OwnerName);
            item.AuthStatus = AuthStatus;
            item.MemberStatus = MemberStatus;
            item.CompanyName = doc.Get(OutDoorIndexFields.CompanyName);
            decimal TotalArea = Decimal.Parse(doc.Get(OutDoorIndexFields.TotalArea), CultureInfo.InvariantCulture);
            if (IsRegular)
            {
                decimal Width = Decimal.Parse(doc.Get(OutDoorIndexFields.Width), CultureInfo.InvariantCulture);
                decimal Height = Decimal.Parse(doc.Get(OutDoorIndexFields.Height), CultureInfo.InvariantCulture);
                int TotalFaces = Int32.Parse(doc.Get(OutDoorIndexFields.TotalFaces), CultureInfo.InvariantCulture);
                item.IsRegular = IsRegular;
                item.Width = Width;
                item.Height = Height;
                item.TotalFaces = TotalFaces;
                item.TotalArea = TotalArea;
            }
            else
            {
                item.IsRegular = IsRegular;
                item.IrRegularArea = doc.Get(OutDoorIndexFields.IrRegularArea);
                item.TotalArea = TotalArea;
            }
            item.Lat = Lat;
            item.Lng = Lng;
            item.VideoUrl = doc.Get(OutDoorIndexFields.VideoUrl);
            return item;
        }

        private static SortField GetSortField(SearchFilter searchFilter)
        {
            switch (searchFilter.SortProperty)
            {
                case SortProperty.Hit:
                    return new SortField(OutDoorIndexFields.Hit, SortField.INT, reverse: searchFilter.SortDirection.Equals(SortDirection.Descending));
                case SortProperty.Price:
                    return new SortField(OutDoorIndexFields.Price, SortField.DOUBLE, reverse: searchFilter.SortDirection.Equals(SortDirection.Descending));
                case SortProperty.Published:
                    return new SortField(OutDoorIndexFields.Published, SortField.LONG, reverse: searchFilter.SortDirection.Equals(SortDirection.Descending));
            }
            return SortField.FIELD_SCORE;
        }

        private static Query ParseQuery(QueryTerm queryTerm, SearchFilter searchFilter)
        {
            var combineQuery = new BooleanQuery();

            #region 关键字查询构建
            if (!String.IsNullOrWhiteSpace(searchFilter.SearchTerm))
            {
                var fields = new[] { 
                    OutDoorIndexFields.Title, 
                    OutDoorIndexFields.Description,
                    OutDoorIndexFields.AreaCate,
                    OutDoorIndexFields.IndustryCate,
                    OutDoorIndexFields.CrowdCate,
                    OutDoorIndexFields.PurposeCate,
                    OutDoorIndexFields.MediaCateName,
                    OutDoorIndexFields.CityCateName,
                    OutDoorIndexFields.FormatName,
                    OutDoorIndexFields.PeriodName,
                    OutDoorIndexFields.OwnerName
                };
                var analyzer = new PanGuAnalyzer();
                //var analyzer = new StandardAnalyzer(LuceneCommon.LuceneVersion);
                var queryParser = new MultiFieldQueryParser(LuceneCommon.LuceneVersion, fields, analyzer);

                var query = queryParser.Parse(searchFilter.SearchTerm);

                //conjuction 一起选择
                var conjuctionQuery = new BooleanQuery();
                conjuctionQuery.Boost = 2.0f;

                //disjunction 分离
                var disjunctionQuery = new BooleanQuery();
                disjunctionQuery.Boost = 0.1f;

                //wildCard 通配符
                var wildCardQuery = new BooleanQuery();
                wildCardQuery.Boost = 0.5f;

                var escapedSearchTerm = Escape(searchFilter.SearchTerm);

                var exactIdQuery = new TermQuery(new Term(OutDoorIndexFields.Title, escapedSearchTerm));

                exactIdQuery.Boost = 2.5f;

                var wildCardIdQuery = new WildcardQuery(new Term(OutDoorIndexFields.Title, "*" + escapedSearchTerm + "*"));

                foreach (var term in GetSearchTerms(searchFilter.SearchTerm))
                {
                    var termQuery = queryParser.Parse(term);
                    conjuctionQuery.Add(termQuery, Occur.MUST);
                    disjunctionQuery.Add(termQuery, Occur.SHOULD);

                    foreach (var field in fields)
                    {
                        var wildCardTermQuery = new WildcardQuery(new Term(field, term + "*"));
                        wildCardTermQuery.Boost = 0.7f;
                        wildCardQuery.Add(wildCardTermQuery, Occur.SHOULD);
                    }
                }
                //关键查询
                var keywordsQuery =
                    conjuctionQuery.Combine(new Query[] { exactIdQuery, wildCardIdQuery, conjuctionQuery, disjunctionQuery, wildCardQuery });

                combineQuery.Add(keywordsQuery, Occur.MUST);
            }
            #endregion

            #region 用户状态
            var memberStatusQuery = NumericRangeQuery.NewIntRange(OutDoorIndexFields.MemberStatus, (int)MemberStatus.CompanyAuth, 99, true, true);
            combineQuery.Add(memberStatusQuery, Occur.MUST);
            #endregion

            #region 审核状态查询构建
            var verifyStatus = NumericRangeQuery.NewIntRange(OutDoorIndexFields.Status, (int)OutDoorStatus.ShowOnline, 99, true, true);
            combineQuery.Add(verifyStatus, Occur.MUST);
            #endregion


            #region 城市查询
            if (queryTerm.City != 0)
            {
                var cityQuery = NumericRangeQuery.NewLongRange(OutDoorIndexFields.CityCateCode, queryTerm.CityCateCode, queryTerm.CityMaxCode, true, true);
                combineQuery.Add(cityQuery, Occur.MUST);
            }
            #endregion

            #region 认证状态
            if (queryTerm.AuthStatus != 0)
            {
                var authStatusQuery = NumericRangeQuery.NewIntRange(OutDoorIndexFields.AuthStatus, queryTerm.AuthStatus, queryTerm.AuthStatus, true, true);
                combineQuery.Add(authStatusQuery, Occur.MUST);
            }
            #endregion



            #region 经纬度搜索
            if (queryTerm.MinX != 0)
            {
                var latQuery = NumericRangeQuery.NewDoubleRange(OutDoorIndexFields.Lat, queryTerm.MinX, queryTerm.MaxX, true, true);
                var lngQuery = NumericRangeQuery.NewDoubleRange(OutDoorIndexFields.Lng, queryTerm.MinY, queryTerm.MaxY, true, true);
                combineQuery.Add(latQuery, Occur.MUST);
                combineQuery.Add(lngQuery, Occur.MUST);
            }
            #endregion


            #region 媒体类别查询
            if (queryTerm.MediaCode != 0)
            {
                var mediaCodeQuery = NumericRangeQuery.NewLongRange(OutDoorIndexFields.MediaCateCode,
                    queryTerm.MediaCateCode, queryTerm.MediaMaxCode, true, true);
                combineQuery.Add(mediaCodeQuery, Occur.MUST);
            }
            #endregion

            #region 媒体表现形式查询
            if (queryTerm.FormatCode != 0)
            {
                var FormatCodeCodeQuery = NumericRangeQuery.NewIntRange(OutDoorIndexFields.FormatCode,
                    queryTerm.FormatCode, queryTerm.FormatCode, true, true);
                combineQuery.Add(FormatCodeCodeQuery, Occur.MUST);
            }
            #endregion

            #region 媒体所有权查询
            if (queryTerm.OwnerCode != 0)
            {
                var OwnerCodeCodeQuery = NumericRangeQuery.NewIntRange(OutDoorIndexFields.OwnerCode,
                    queryTerm.OwnerCode, queryTerm.OwnerCode, true, true);
                combineQuery.Add(OwnerCodeCodeQuery, Occur.MUST);
            }
            #endregion

            #region 媒体购买周期查询
            if (queryTerm.PeriodCode != 0)
            {
                var PeriodCodeCodeQuery = NumericRangeQuery.NewIntRange(OutDoorIndexFields.PeriodCode,
                    queryTerm.PeriodCode, queryTerm.PeriodCode, true, true);
                combineQuery.Add(PeriodCodeCodeQuery, Occur.MUST);
            }
            #endregion

            #region 媒体价格区间查询
            if (queryTerm.Price != 0)
            {
                var rangeValue = EnumHelper.GetPriceValue(queryTerm.Price);
                var PriceQuery = NumericRangeQuery.NewDoubleRange(OutDoorIndexFields.Price,
                    Convert.ToDouble(rangeValue.Min), Convert.ToDouble(rangeValue.Max), true, true);
                combineQuery.Add(PriceQuery, Occur.MUST);
            }
            #endregion

            #region 媒体档期查询
            if (queryTerm.DeadLine != 0)
            {
                var minValue = (DateTime.Now.AddYears(-10)).Ticks;
                var maxValue = (new DateTime(DateTime.Now.Year, queryTerm.DeadLine, 1)).Ticks;
                var DeadLineQuery = NumericRangeQuery.NewLongRange(OutDoorIndexFields.DeadLine,
                    Convert.ToInt64(minValue), Convert.ToInt64(maxValue), true, true);
                combineQuery.Add(DeadLineQuery, Occur.MUST);
            }
            #endregion

            return combineQuery;
        }


        private static string Escape(string term)
        {
            return QueryParser.Escape(term).ToLowerInvariant();
        }

        private static IEnumerable<string> GetSearchTerms(string searchTerm)
        {
            List<string> result = new List<string>();
            var analyzer = new PanGuAnalyzer();
            StringReader sr = new StringReader(searchTerm);
            TokenStream stream = analyzer.TokenStream(null, sr);
            bool hasnext = stream.IncrementToken();
            System.DateTime start = System.DateTime.Now;
            ITermAttribute ita;
            while (hasnext)
            {
                ita = stream.GetAttribute<ITermAttribute>();
                result.Add(ita.Term);
                hasnext = stream.IncrementToken();
            }
            stream.CloneAttributes();
            sr.Close();
            analyzer.Dispose();

            var resultString = string.Join(" ", result);

            return resultString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Concat(new[] { searchTerm })
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(Escape);

        }


        public List<LinkItem> Search(out int totalHits)
        {
            List<LinkItem> result = new List<LinkItem>();
            totalHits = 0;
            if (!Directory.Exists(LuceneCommon.IndexOutDoorDirectory))
            {
                totalHits = 0;
                return result;
            }
            using (var directory = new SimpleFSDirectory(new DirectoryInfo(LuceneCommon.IndexOutDoorDirectory)))
            {
                var searcher = new IndexSearcher(directory, readOnly: false);
                var reader = IndexReader.Open(directory, false);
                var docs = new List<Document>();
                var term = reader.TermDocs();
                while (term.Next())
                {
                    totalHits++;
                    docs.Add(searcher.Doc(term.Doc));
                    result.Add(GetMediaItem(searcher.Doc(term.Doc)));
                }
                reader.Dispose();
                searcher.Dispose();
            }
            return result;
        }
    }
}
using Contestant.ICommon.Enums;
using Contestant.ICommon.Helper;
using Contestant.ICommon.Interface;
using Contestant.ICommon.Results;
using Contestant.IDataRepository.BaseRepository;
using Contestant.Models;
using Contestant.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contestant.General
{
    public class DistrictBL : IDistrictBL
    {
        private readonly IBaseRepository _repo;
        private readonly IHelperTool _helper;
        public DistrictBL(IBaseRepository repo, IHelperTool helper)
        {
            _repo = repo;
            _helper = helper;
        }

        Expression<Func<District, bool>> predicate(string pq_filter, DisplayStatus displayStatus)
        {
            var searchCondition = PredicateBuilder.True<District>();
            List<Filter> filters = new List<Filter>();
            if (pq_filter != null)
            {
                FilterObj filterObj = JsonConvert.DeserializeObject<FilterObj>(pq_filter);
                filters = filterObj.data;
            }
            else {
                searchCondition = searchCondition.And(p => 0 == 0);
            }

            switch (displayStatus)
            {
                case DisplayStatus.All:
                    searchCondition = searchCondition.And(a => a.Status != RowStatusOption.Deleted);
                    break;
                case DisplayStatus.Approved:
                    searchCondition = searchCondition.And(a => a.Status == RowStatusOption.Approved || a.Status == RowStatusOption.NonApproval);
                    break;
                case DisplayStatus.Unapproved:
                    searchCondition = searchCondition.And(a => (a.Status == RowStatusOption.New || a.Status == RowStatusOption.Modified || a.Status == RowStatusOption.ApprovedModified));
                    break;
                default:
                    searchCondition = searchCondition.And(a => 0 == 0);
                    break;
            }

            foreach (Filter filter in filters)
            {
                int iValue;
                switch (filter.dataIndx)
                {
                    case ("districtname"):
                        searchCondition = searchCondition.And(p => p.DistrictName.ToLower().Contains(filter.value.ToLower()));
                        break;

                    case ("status"):
                        bool success = int.TryParse(filter.value, out iValue);
                        if (success == true && iValue >= 0 && iValue <= 5)
                        {
                            RowStatusOption enumvalue = (RowStatusOption)iValue;
                            searchCondition = searchCondition.And(p => p.Status.Equals(enumvalue));
                        }
                        break;

                    default:
                        searchCondition = searchCondition.And(a => 0 == 0);
                        break;
                }
            }
            return searchCondition;
        }

        async Task<int> GetTotalCount(DisplayStatus displayStatus, string pq_filter)
        {
            return await _repo.GetModelList<District>().Where(predicate(pq_filter, displayStatus)).AsNoTracking().CountAsync();
        }

        public async Task<GridIndexData> GetIndexData(DisplayStatus disp, string pq_filter, int pq_curPage, int pq_rPP)
        {
            GridIndexData data = new GridIndexData
            {
                totalRecords = await GetTotalCount(disp, pq_filter),
                dataRow = null,
                curPage = pq_curPage
            };
            int skip = _helper.GetPageSkip(pq_curPage, pq_rPP, data.totalRecords);
            data.dataRow = await (from e in _repo.GetModelList<District>().Where(predicate(pq_filter, disp)).AsNoTracking().OrderByDescending(o => o.Id).Skip(skip).Take(pq_rPP)
                                  join u in _repo.GetModelList<ApplicationUser>().AsNoTracking() on e.StatusChgUserID equals u.Id
                                  select new
                                  {
                                      Id = e.Id,
                                      DistrictName = e.DistrictName,
                                      DistrictNameLocLang = e.DistrictNameLocLang,
                                      Status = e.Status,
                                      //ChgDate = oprDate == OprDateType.Nepali ? _helper.GetDateInBS(e.StatusChgDate) : e.StatusChgDate.ToString("yyyy/MM/dd"),
                                      StatusChgUserId = e.StatusChgUserID,
                                      StatusChgUserName = u.UserName,
                                  }).ToListAsync();
            return data;
        }

        public async Task<DistrictViewModel> GetData(int Id)
        {
            var data = await (from e in _repo.GetModelList<District>().Where(x => x.Id.Equals(Id)).AsNoTracking()
                              join u in _repo.GetModelList<ApplicationUser/*SyncApplicationUser*/>().AsNoTracking() on e.StatusChgUserID equals u.Id
                              select new DistrictViewModel
                              {
                                  Id = e.Id,
                                  StateId = e.StateId,
                                  StateName = e.State.StateName,
                                  DistrictName = e.DistrictName,
                                  DistrictNameLocLang = e.DistrictNameLocLang,
                                  Status = e.Status,
                                  StatusChgDate = e.StatusChgDate.ToString("yyyy/MM/dd"),
                                  StatusChgUserID = e.StatusChgUserID,
                                  StatusChgUserName = u.UserName
                              }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<DataResult> Create(DistrictViewModel data, string controllerName)
        {
            DateTime curDate = _helper.GetAppEngDate();
            var nameExists = _repo.GetModelList<District>().Where(a => a.DistrictName == data.DistrictName).AsNoTracking().FirstOrDefault();
            if (nameExists != null)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Duplicate District Name found..  " };
            }
            if (data.DistrictNameLocLang != null && data.DistrictNameLocLang != string.Empty)
            {
                var locallangexists = _repo.GetModelList<District>().Where(a => a.DistrictNameLocLang == data.DistrictNameLocLang).AsNoTracking().FirstOrDefault();
                if (locallangexists != null)
                {
                    return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Duplicate District Name Local Language found..  " };
                }
            }
            var model = new District
            {
                DistrictName = data.DistrictName,
                DistrictNameLocLang = data.DistrictNameLocLang,
                StateId = data.StateId ?? 0,
                Status = (_helper.IsApprovalRequired(controllerName) == true) ? RowStatusOption.New : RowStatusOption.NonApproval,
                EntryDate = curDate,
                EntryUserId = data.StatusChgUserID,
                StatusChgDate = DateTime.Now,
                StatusChgUserID = data.StatusChgUserID
            };
            return await _repo.Create<District>(model, await _repo.ActivityPrepare<District>(model, controllerName, string.Format("{0} ,District created.", model.DistrictName)));
        }

        public async Task<DataResult> Update(DistrictViewModel data, ActionPermissionViewModel permission, string controllerName)
        {
            DateTime curDate = _helper.GetAppEngDate();
            District model = _repo.GetModelList<District>().Where(a => a.Id == data.Id).AsTracking().FirstOrDefault();
            if (model == null)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Update request data not found." };
            }
            if ((model.Status == RowStatusOption.Approved || model.Status == RowStatusOption.ApprovedModified) && permission.ApproveModify == false)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Insufficient priviledge to update." };
            }

            var nameExists = _repo.GetModelList<District>().Where(a => a.Id != data.Id && a.DistrictName == data.DistrictName).AsNoTracking().FirstOrDefault();
            if (nameExists != null)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -!Duplicate District Name found..  " };
            }
            if (data.DistrictNameLocLang != null && data.DistrictNameLocLang != string.Empty)
            {
                var locallangexists = _repo.GetModelList<District>().Where(a => a.Id != data.Id && a.DistrictNameLocLang == data.DistrictNameLocLang).AsNoTracking().FirstOrDefault();
                if (locallangexists != null)
                {
                    return new DataResult { ResultType = ResultType.Failed, Message = "Warning -!Duplicate District Name Local Language found..  " };
                }
            }

            model.DistrictName = data.DistrictName;
            model.DistrictNameLocLang = data.DistrictNameLocLang;
            model.StateId = data.StateId ?? 0;
            model.Status = (_helper.IsApprovalRequired(controllerName) == true) ? ((model.Status == RowStatusOption.Approved || model.Status == RowStatusOption.ApprovedModified) ? RowStatusOption.ApprovedModified : RowStatusOption.Modified) : RowStatusOption.NonApproval;
            model.StatusChgDate = curDate;
            model.StatusChgUserID = data.StatusChgUserID;
            return await _repo.Update<District>(model, await _repo.ActivityPrepare<District>(model, controllerName, string.Format("{0} Data Updated.", model.DistrictName)));
        }

        public async Task<DataResult> Delete(int Id, int userId, ActionPermissionViewModel permission, string controllerName)
        {
            DateTime curDate = _helper.GetAppEngDate();
            if (permission == null)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Permission not defined." };
            }
            District model = _repo.GetModelList<District>().Where(a => a.Id == Id).AsNoTracking().FirstOrDefault();
            if (model == null)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Delete requested data not found." };
            }

            if ((model.Status == RowStatusOption.Approved || model.Status == RowStatusOption.ApprovedModified) && permission.ApproveDelete == false)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Insufficient priviledge to delete." };
            }
            model.Status = RowStatusOption.Deleted;
            model.StatusChgDate = curDate;
            model.StatusChgUserID = userId;
            return await _repo.Delete<District>(model, await _repo.ActivityPrepare<District>(model, controllerName, string.Format("{0} Data Deleted.", model.DistrictName)));
        }

        public async Task<DataResult> Approve(int Id, int userId, string controllerName)
        {
            DateTime curDate = _helper.GetAppEngDate();
            District model = _repo.GetModelList<District>()
                                        .Where(a => a.Id == Id && (a.Status == RowStatusOption.New || a.Status == RowStatusOption.Modified || a.Status == RowStatusOption.ApprovedModified))
                                        .AsTracking()
                                        .FirstOrDefault();
            if (model == null)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = "Warning -! Approve request data not found." };
            }
            model.Status = RowStatusOption.Approved;
            model.StatusChgDate = curDate;
            model.StatusChgUserID = userId;
            return await _repo.Approve<District>(model, await _repo.ActivityPrepare<District>(model, controllerName, string.Format("{0} District Approved.", model.DistrictName)));
        }

        public async Task<GridIndexData> GetSearchData(string pq_filter, int pq_curPage, int pq_rPP)
        {
            GridIndexData data = new GridIndexData
            {
                totalRecords = await GetTotalCount(DisplayStatus.Approved, pq_filter),
                curPage = pq_curPage,
                dataRow = null
            };
            int skip = _helper.GetPageSkip(pq_curPage, pq_rPP, data.totalRecords);
            data.dataRow = await (from au in _repo.GetModelList<District>()
                                    .Where(predicate(pq_filter, DisplayStatus.Approved))
                                    .AsNoTracking()
                                    .OrderByDescending(o => o.Id)
                                    .Skip(skip).Take(pq_rPP)
                                  select new DistrictViewModel
                                  {
                                      Id = au.Id,
                                      DistrictName = au.DistrictName,
                                      DistrictNameLocLang = au.DistrictNameLocLang,
                                      StateId = au.StateId
                                  }).ToListAsync();
            return data;
        }
    }
}

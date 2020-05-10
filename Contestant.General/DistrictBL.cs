using Contestant.ICommon.Enums;
using Contestant.ICommon.Helper;
using Contestant.ICommon.Interface;
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
        public DistrictBL(IBaseRepository repo)
        {
            _repo = repo;
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

            //switch (displayStatus)
            //{
            //    case DisplayStatus.All:
            //        searchCondition = searchCondition.And(a => a.Status != RowStatusOption.Deleted);
            //        break;
            //    case DisplayStatus.Approved:
            //        searchCondition = searchCondition.And(a => a.Status == RowStatusOption.Approved || a.Status == RowStatusOption.NonApproval);
            //        break;
            //    case DisplayStatus.Unapproved:
            //        searchCondition = searchCondition.And(a => (a.Status == RowStatusOption.New || a.Status == RowStatusOption.Modified || a.Status == RowStatusOption.ApprovedModified));
            //        break;
            //    default:
            //        searchCondition = searchCondition.And(a => 0 == 0);
            //        break;
            //}

            foreach (Filter filter in filters)
            {
                int iValue;
                switch (filter.dataIndx)
                {
                    case ("name"):
                        searchCondition = searchCondition.And(p => p.Name.ToLower().Contains(filter.value.ToLower()));
                        break;

                    //case ("status"):
                    //    bool success = int.TryParse(filter.value, out iValue);
                    //    if (success == true && iValue >= 0 && iValue <= 5)
                    //    {
                    //        RowStatusOption enumvalue = (RowStatusOption)iValue;
                    //        searchCondition = searchCondition.And(p => p.Status.Equals(enumvalue));
                    //    }
                    //    break;

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
        }
    }
}

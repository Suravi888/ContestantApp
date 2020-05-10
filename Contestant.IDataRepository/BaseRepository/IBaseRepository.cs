using Contestant.ICommon.Results;
using Contestant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestant.IDataRepository.BaseRepository
{
    public interface IBaseRepository
    {
        Task<ActivityLogViewModel> ActivityPrepare<T1>(T1 model, string controllerName, string remarks) where T1 : class;
        IQueryable<T1> GetModelList<T1>() where T1 : class;
        void SetConnectionString(string conn);
        string GetConnectionString();

        Task<DataResult> Create<T1>(T1 model) where T1 : class;
        Task<DataResult> Create<T1>(T1 model, ActivityLogViewModel activity) where T1 : class;

        Task<DataResult> Create<T1>(T1[] model) where T1 : class;
        //Task<DataResult> Create<T1, T2>(T1 model1, T2 model2, ActivityLogViewModel activity) where T1 : class where T2 : class;

        Task<DataResult> Update<T1>(T1 model) where T1 : class;
        Task<DataResult> Update<T1>(T1[] model) where T1 : class;
        Task<DataResult> Update<T1>(T1 model, ActivityLogViewModel activity) where T1 : class;
        Task<DataResult> Update<T1, T2>(T1 model, T2[] delete1, ActivityLogViewModel activity) where T1 : class where T2 : class;
        //Task<DataResult> Update<T1, T2, T3>(T1 model, T2[] delete1, T3[] delete2, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class;
        Task<DataResult> Update<T1, T2, T3, T4>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class;
        //Task<DataResult> Update<T1, T2, T3, T4, T5>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, T5[] delete4, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class;
        //Task<DataResult> Update<T1, T2, T3>(T1 model1, T2 model2, T3[] delete1, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class;
        Task<DataResult> Update<T1, T2, T3, T4, T5, T6>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, T5[] delete4, T6[] delete5, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class;

        Task<DataResult> Delete<T1>(T1 model, ActivityLogViewModel activity) where T1 : class;
        Task<DataResult> Delete<T1, T2>(T1 model, T2[] delete1, ActivityLogViewModel activity) where T1 : class where T2 : class;
        Task<DataResult> Delete<T1, T2, T3>(T1 model, T2[] delete1, T3[] delete2, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class;
        //Task<DataResult> Delete<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, T5[] delete4, T6[] delete5, T7[] delete6, T8[] delete7, T9[] delete8, T10[] delete9, T11[] delete10, T12[] delete11, T13[] delete12, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class;
        Task<DataResult> Approve<T1>(T1 model, ActivityLogViewModel activity) where T1 : class;
    }
}

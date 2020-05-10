using Contestant.ICommon.Enums;
using Contestant.ICommon.Interface;
using Contestant.ICommon.Results;
using Contestant.IDataRepository.BaseRepository;
using Contestant.Models;
using Contestant.Repository.ContextManager;
using Contestant.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contestant.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly RootEfContext context;
        private readonly IErrorLogger exceLog;
        private readonly CodeSequenceGenerator csg;

        public BaseRepository(RootEfContext _context, IErrorLogger _exceLog)
        {
            context = _context;
            exceLog = _exceLog;
            csg = new CodeSequenceGenerator();
        }

        async Task<bool> ActivityUpdate(ActivityLogViewModel activity, int relatedId)
        {
            int logId = 0;

            #region Activity
            ActivityLog activityLog = await context.ActivityLogs
                                            .Where(w => w.MenuId == activity.MenuId && w.RelatedId == relatedId)
                                            .AsTracking()
                                            .FirstOrDefaultAsync();

            if (activityLog == null)
            {
                activityLog = new ActivityLog
                {
                    MenuId = activity.MenuId,
                    RelatedId = relatedId,
                    Status = activity.Status,
                    StatusChgDate = activity.StatusChgDate,
                    StatusChgUserID = activity.StatusChgUserID,
                };
                context.Add(activityLog);
            }
            else
            {
                activityLog.Status = activity.Status;
                activityLog.StatusChgDate = activity.StatusChgDate;
                activityLog.StatusChgUserID = activity.StatusChgUserID;
                context.Update(activityLog);
            }
            await context.SaveChangesAsync();
            logId = activityLog.Id;
            #endregion
            #region ActivityHist
            ApplicationMenu menuData = await context.ApplicationMenu
                                            .Where(w => w.Id == activity.MenuId && w.IsGroup == false && w.RequiredActivityLog == true)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
            if (menuData != null)
            {
                if (string.IsNullOrWhiteSpace(activity.DataJson))
                {
                    throw new Exception("Warning -! Activity Log Hist data not found.");
                }
                ActivityLogHist hist = new ActivityLogHist
                {
                    ActivityLogId = logId,
                    Status = activity.Status,
                    StatusChgDate = activity.StatusChgDate,
                    StatusChgUserID = activity.StatusChgUserID,
                    Remarks = activity.Remarks,
                    DataJson = activity.DataJson,

                };
                context.ActivityLogHist.Add(hist);
                await context.SaveChangesAsync();
            }
            #endregion
            return true;
        }
        async Task<int> GetMenuId(string controllerName)
        {
            var menu = await context.ApplicationMenu
                            .Where(w => w.Controller.ToLower() == controllerName.ToLower())
                            .AsNoTracking()
                            .Select(s => new { Id = s.Id })
                            .FirstOrDefaultAsync();
            return menu.Id;
        }
        public string GetConnectionString()
        {
            string conStr = context.Database.GetDbConnection().ConnectionString;
            return conStr;
        }

        DataResult HandleExceptionsMessage(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as SqlException;
            if (sqlException == null)
            {
                throw ex;
            }
            if (sqlException.Errors.Count > 0)
            {
                switch (sqlException.Errors[0].Number)
                {
                    case 547: // Foreign Key violation
                        return new DataResult { ResultType = ResultType.Failed, Message = "Data could not be deleted, because it is in use." };
                    case 2601: //duplicate key
                        return new DataResult { ResultType = ResultType.Failed, Message = "Cannot insert duplicate key row with unique." };
                    default:
                        return new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            throw ex;
        }
        void SetCustomGeneratedCodeValue<T1>(ref T1 model) where T1 : class
        {
            PropertyInfo[] props = typeof(T1).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                CustomGeneratedCodeAttribute attrs = prop.GetCustomAttribute<CustomGeneratedCodeAttribute>(true);
                if (attrs != null)
                {
                    prop.SetValue(model, csg.GetCustomCode<T1>(context), null);
                    break;
                }
                ////code for reference
                //foreach (dynamic attr in attrs)
                //{
                //    if (attr.GetType().Name.ToString() == "CustomGeneratedCodeAttribute")
                //    {
                //        prop.SetValue(model, "mak", null);
                //        return;
                //    }
                //}
            }
        }

        //activity prepare
        public async Task<ActivityLogViewModel> ActivityPrepare<T1>(T1 model, string controllerName, string remarks) where T1 : class
        {
            Type t = model.GetType();
            return new ActivityLogViewModel
            {
                MenuId = await GetMenuId(controllerName),
                RelatedId = (int)t.GetProperty("Id")?.GetValue(model, null),
                Status = (RowStatusOption)t.GetProperty("Status")?.GetValue(model, null),
                StatusChgDate = (DateTime)t.GetProperty("StatusChgDate")?.GetValue(model, null),
                StatusChgUserID = (int)t.GetProperty("StatusChgUserID")?.GetValue(model, null),
                DataJson = JsonConvert.SerializeObject(model),
                Remarks = remarks
            };
        }

        // get model
        public virtual IQueryable<T1> GetModelList<T1>() where T1 : class
        {
            return context.Set<T1>();
        }

        //create
        public virtual async Task<DataResult> Create<T1>(T1 model) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //Auto generated code
                    SetCustomGeneratedCodeValue<T1>(ref model);

                    //model add
                    context.Set<T1>().Add(model);
                    await context.SaveChangesAsync();

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Created." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Create", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Create", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Create<T1>(T1[] model) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model add
                    context.Set<T1>().AddRange(model);
                    await context.SaveChangesAsync();

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Created." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Create", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Create", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Create<T1>(T1 model, ActivityLogViewModel activity) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //Auto generated code
                    SetCustomGeneratedCodeValue<T1>(ref model);

                    //model add
                    context.Set<T1>().Add(model);
                    await context.SaveChangesAsync();

                    //activity update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Created." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Create", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Create", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }


        //update
        public virtual async Task<DataResult> Update<T1>(T1 model) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1>(T1[] model) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model update
                    context.Set<T1>().UpdateRange(model);
                    await context.SaveChangesAsync();

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1>(T1 model, ActivityLogViewModel activity) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        //newadded
        public virtual async Task<DataResult> Update<T1, T2>(T1 model, T2 model1, ActivityLogViewModel activity) where T1 : class where T2 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model update
                    context.Set<T1>().Update(model);
                    context.Set<T2>().Update(model1);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1, T2>(T1 model, T2[] delete1, ActivityLogViewModel activity) where T1 : class where T2 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //detail delete
                    context.Set<T2>().RemoveRange(delete1);
                    await context.SaveChangesAsync();

                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1, T2, T3>(T1 model, T2[] delete1, T3[] delete2, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //details delete
                    context.Set<T2>().RemoveRange(delete1);
                    await context.SaveChangesAsync();
                    context.Set<T3>().RemoveRange(delete2);
                    await context.SaveChangesAsync();

                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1, T2, T3, T4>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //details delete
                    context.Set<T2>().RemoveRange(delete1);
                    await context.SaveChangesAsync();
                    context.Set<T3>().RemoveRange(delete2);
                    await context.SaveChangesAsync();
                    context.Set<T4>().RemoveRange(delete3);
                    await context.SaveChangesAsync();

                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1, T2, T3, T4, T5>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, T5[] delete4, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //details delete
                    context.Set<T2>().RemoveRange(delete1);
                    // await context.SaveChangesAsync();
                    context.Set<T3>().RemoveRange(delete2);
                    // await context.SaveChangesAsync();
                    context.Set<T4>().RemoveRange(delete3);
                    // await context.SaveChangesAsync();
                    context.Set<T5>().RemoveRange(delete4);
                    //  await context.SaveChangesAsync();                   

                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public virtual async Task<DataResult> Update<T1, T2, T3, T4, T5, T6>(T1 model, T2[] delete1, T3[] delete2, T4[] delete3, T5[] delete4, T6[] delete5, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //details delete
                    context.Set<T2>().RemoveRange(delete1);
                    // await context.SaveChangesAsync();
                    context.Set<T3>().RemoveRange(delete2);
                    // await context.SaveChangesAsync();
                    context.Set<T4>().RemoveRange(delete3);
                    // await context.SaveChangesAsync();
                    context.Set<T5>().RemoveRange(delete4);
                    //  await context.SaveChangesAsync();      
                    context.Set<T6>().RemoveRange(delete5);
                    //  await context.SaveChangesAsync();    

                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Updated." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Updated", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }

        //delete
        public async Task<DataResult> Delete<T1>(T1 model, ActivityLogViewModel activity) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model delete
                    context.Set<T1>().Remove(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Deleted." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Delete", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Delete", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }
        public async Task<DataResult> Delete<T1, T2>(T1 model, T2[] delete1, ActivityLogViewModel activity) where T1 : class where T2 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //detail delete
                    context.Set<T2>().RemoveRange(delete1);
                    await context.SaveChangesAsync();

                    //model delete
                    context.Set<T1>().Remove(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Deleted." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Delete", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Delete", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }

        public async Task<DataResult> Delete<T1, T2, T3>(T1 model, T2[] delete1, T3[] delete2, ActivityLogViewModel activity) where T1 : class where T2 : class where T3 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //details delete
                    context.Set<T2>().RemoveRange(delete1);
                    await context.SaveChangesAsync();
                    context.Set<T3>().RemoveRange(delete2);
                    await context.SaveChangesAsync();

                    //model delete
                    context.Set<T1>().Remove(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Deleted." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Delete", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Delete", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }

        public async Task<DataResult> Approve<T1>(T1 model, ActivityLogViewModel activity) where T1 : class
        {
            DataResult result = new DataResult();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    //model update
                    context.Set<T1>().Update(model);
                    await context.SaveChangesAsync();

                    //ActivityLog update
                    await ActivityUpdate(activity, (int)model.GetType().GetProperty("Id")?.GetValue(model, null));

                    dbContextTransaction.Commit();
                    result = new DataResult { ResultType = ResultType.Success, Message = "Successfully Approve." };
                }
                catch (DbUpdateException ex)
                {
                    dbContextTransaction.Rollback();
                    result = HandleExceptionsMessage(ex);
                    await exceLog.LogException(this.GetType().Name, "Approve", ex);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await exceLog.LogException(this.GetType().Name, "Approve", ex);
                    result = new DataResult { ResultType = ResultType.Exception, Message = ex.Message.ToString() };
                }
            }
            return result;
        }

        public void SetConnectionString(string conn)
        {
            context.Database.GetDbConnection().ConnectionString = conn;
        }
    }
}

using Contestant.ICommon.Interface;
using Contestant.Models;
using Contestant.Repository.ContextManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestant.Repository
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly RootEfContext context;

        public ErrorLogger(RootEfContext _context)
        {
            context = _context;
        }

        public async Task<bool> LogException(string moduleName, string actionName, Exception ex) 
        {
            ExceptionLog log = new ExceptionLog
            {
                DateTime = DateTime.Now,
                Message = ex.Message,
                InnerMessage = ex.InnerException == null ? string.Empty : ex.InnerException.Message,
                Source = ex.Source,
                ModuleName = moduleName,
                ActionName = actionName,
                StackTrace = ex.StackTrace == null ? string.Empty : ex.StackTrace.ToString()
            };
            try {
                //detach the entity if any with errors
                var changedEntriesCopy = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted).ToList();
                foreach (var entity in changedEntriesCopy)
                {
                    context.Entry(entity.Entity).State = EntityState.Detached;
                }
                context.Set<ExceptionLog>().Add(log);
                await context.SaveChangesAsync();
            }
            catch (Exception _ex)
            {
                WriteTextLog(moduleName, actionName, ex.Message.ToString());
                WriteTextLog(moduleName, actionName, _ex.Message.ToString());
            }
            return true;
        }

        public async Task<bool> LogException(string moduleName, string actionName, string errorMessage)
        {
            ExceptionLog log = new ExceptionLog
            {
                DateTime = DateTime.Now,
                Message = errorMessage,
                InnerMessage = string.Empty,
                Source = string.Empty,
                ModuleName = moduleName,
                ActionName = actionName,
                StackTrace = string.Empty
            };
            try
            {
                //detach the entity if any with errors
                var changedEntriesCopy = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted).ToList();
                foreach (var entity in changedEntriesCopy)
                {
                    context.Entry(entity.Entity).State = EntityState.Detached;
                }
                context.Set<ExceptionLog>().Add(log);
                await context.SaveChangesAsync();
            }
            catch (Exception _ex)
            {
                WriteTextLog(moduleName, actionName, errorMessage);
                WriteTextLog(moduleName, actionName, _ex.Message.ToString());
            }
            return true;
        }

        void WriteTextLog(string moduleName, string actionName, string errorMessage)
        {
            string message = string.Concat(moduleName, " - ", actionName, " - ", errorMessage);
            string logPath = @"c:\Log";
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                string path = string.Concat(logPath.ToString(), @"\", DateTime.Now.Year, @"-", DateTime.Now.Month.ToString().PadLeft(2, '0'), @"-", DateTime.Now.Day.ToString().PadLeft(2, '0'), @".log");
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(string.Concat(DateTime.Now.TimeOfDay, " - ", message));
                }
            }
            catch (Exception ex)
            { 
              //do nothing
            }
        }
    }
}

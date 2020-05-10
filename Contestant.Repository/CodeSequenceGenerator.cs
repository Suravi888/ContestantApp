using Contestant.Models;
using Contestant.Repository.ContextManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contestant.Repository
{
    public class CodeSequenceGenerator
    {
        public string GetCustomCode<T>(RootEfContext context) where T : class
        {
            string key = typeof(T).Name.ToString();
            switch (key)
            {
                case "Employee":
                    return EmployeeMaster<T>(context);

                case "Branch":
                    return BranchCode<T>(context);

                default:
                    return Default<T>(context);
            }
        }

        string EmployeeMaster<T>(RootEfContext context)
        {
            string key = typeof(T).Name.ToString();
            string changeSequenceKey = "";
            var model = context.CodeSequences
                            .Where(w => w.SequenceKey == key && w.ChangeSequenceKey == changeSequenceKey)
                            .AsNoTracking()
                            .FirstOrDefault();
            if (model == null)
            {
                model = new CodeSequence
                {
                    SequenceKey = key,
                    ChangeSequenceKey = changeSequenceKey,
                    SequenceValue = 1
                };
            }
            else
            {
                model.SequenceValue = model.SequenceValue + 1;
            }
            context.Update(model);
            context.SaveChanges();
            return string.Format("EMP{0}", model.SequenceValue.ToString("00000"));
        }

        string BranchCode<T>(RootEfContext context)
        {
            string key = typeof(T).Name.ToString();
            string changeSequenceKey = "";
            var model = context.CodeSequences
                            .Where(w => w.SequenceKey == key && w.ChangeSequenceKey == changeSequenceKey)
                            .AsNoTracking()
                            .FirstOrDefault();
            if (model == null)
            {
                model = new CodeSequence
                {
                    SequenceKey = key,
                    ChangeSequenceKey = changeSequenceKey,
                    SequenceValue = 1
                };
            }
            else
            {
                model.SequenceValue = model.SequenceValue + 1;
            }
            context.Update(model);
            context.SaveChanges();
            return string.Format("{0}", model.SequenceValue.ToString("000"));
        }


        string Default<T>(RootEfContext context)
        {
            string key = typeof(T).Name.ToString();
            string changeSequenceKey = "";
            CodeSequence model = context.CodeSequences
                                    .Where(w => w.SequenceKey == key && w.ChangeSequenceKey == changeSequenceKey)
                                    .AsNoTracking()
                                    .FirstOrDefault();
            if (model == null)
            {
                model = new CodeSequence
                {
                    SequenceKey = key,
                    ChangeSequenceKey = changeSequenceKey,
                    SequenceValue = 1
                };
            }
            else
            {
                model.SequenceValue = model.SequenceValue + 1;
            }
            context.Update(model);
            context.SaveChanges();
            return string.Format("{0}", model.SequenceValue.ToString("00000"));
        }
    }
}

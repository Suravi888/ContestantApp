using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contestant.ICommon.Interface
{
    public interface IErrorLogger
    {
        Task<bool> LogException(string moduleName, string actionName, Exception ex);
        Task<bool> LogException(string moduleName, string actionName, string errorMessage);
    }
}

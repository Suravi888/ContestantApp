using System;
using System.Collections.Generic;
using System.Text;

namespace Contestant.ICommon.Interface
{
    public interface IHelperTool
    {
        int GetPageSkip(int pq_curPage, int pq_rPP, int totalRecords);
        DateTime GetAppEngDate();
        bool IsApprovalRequired(string ControllerName);
    }
}

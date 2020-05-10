using System;
using System.Collections.Generic;
using System.Text;

namespace Contestant.ViewModels
{
    public class GridIndexData
    {
        public int totalRecords { get; set; }

        public int curPage { get; set; }

        public dynamic dataRow { get; set; }
    }
}

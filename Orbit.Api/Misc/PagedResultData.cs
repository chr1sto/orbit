using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Misc
{
    public class PagedResultData<T>
    {
        public PagedResultData(in T content, int recordCount, int currentIndex, int currentCountPerPage)
        {
            Content = content;
            RecordCount = recordCount;
            CurrentIndex = currentIndex;
            CurrentCountPerPage = currentCountPerPage;
            PageCount = (int)Math.Ceiling((double)recordCount / (double)currentCountPerPage);
        }

        public T Content { get; private set; }
        public int RecordCount { get; private set; }
        public int CurrentIndex { get; private set; }
        public int CurrentCountPerPage { get; private set; }
        public int PageCount { get; private set; }
    }
}

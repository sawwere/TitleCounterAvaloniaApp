using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tc.Dto;

namespace tc.Service
{
    public interface ISearchableService
    {
        public IEnumerable<ISearchable> SearchByTitle(string title);
    }
}

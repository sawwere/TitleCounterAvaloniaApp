using System.Collections.Generic;
using System.Linq;
using tc.Models;

namespace tc.Service
{
    public interface IFilterContentStrategy
    {
        IEnumerable<Entry> Filter(List<Entry> contents, string arg);
    }

    public class FilterByYear : IFilterContentStrategy
    {
        public IEnumerable<Entry> Filter(List<Entry> contents, string arg)
        {
            return contents.Where(x => x.Content.DateRelease.GetValueOrDefault().Year.ToString() == arg);
        }
    }

    public class FilterByScore : IFilterContentStrategy
    {
        public IEnumerable<Entry> Filter(List<Entry> contents, string arg)
        {
            int value = int.Parse(arg);
            return contents.Where(x => x.Score == value);
        }
    }

    public class FilterByStatus : IFilterContentStrategy
    {
        public IEnumerable<Entry> Filter(List<Entry> contents, string arg)
        {
            arg = arg.ToLower();
            return contents.Where(x => x.Status.ToString() == arg);
        }
    }

    public class FilterByName : IFilterContentStrategy
    {
        public IEnumerable<Entry> Filter(List<Entry> contents, string arg)
        {
            return contents.Where(x => x.CustomTitle.StartsWith(arg));
        }
    }
}

using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using tc.Dto;
using tc.Models;

namespace tc.Service
{
    public interface ISearchableService
    {
        /// <summary>
        /// Perform search by given parameters
        /// </summary>
        /// <param name="title">Title of the content</param>
        /// <returns></returns>
        public IEnumerable<ISearchable> SearchByTitle(string title);

        /// <summary>
        /// Create entry based on content
        /// </summary>
        /// <param name="searchable">Content to be associated with</param>
        /// <returns></returns>
        public Task<Entry?> Create(ISearchable searchable);

        public Task<byte[]> LoadCoverAsync(long id);
    }
}

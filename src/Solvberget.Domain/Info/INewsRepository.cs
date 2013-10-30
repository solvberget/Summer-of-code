using System.Collections.Generic;

namespace Solvberget.Domain.Info
{
    public interface INewsRepository
    {
        List<NewsItem> GetNewsItems(int? limitCount);
    }
}
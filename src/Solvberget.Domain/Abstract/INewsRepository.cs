using System.Collections.Generic;

using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface INewsRepository
    {
        List<NewsItem> GetNewsItems(int? limitCount);
    }
}
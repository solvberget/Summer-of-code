using System;

namespace Solvberget.Domain.Abstract
{
    public interface IListRepositoryStatic : IListRepository
    {
        DateTime? GetTimestampForLatestChange();
    }
}
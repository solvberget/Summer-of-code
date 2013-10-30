using System;

namespace Solvberget.Domain.Lists
{
    public interface IListRepositoryStatic : IListRepository
    {
        DateTime? GetTimestampForLatestChange();
    }
}
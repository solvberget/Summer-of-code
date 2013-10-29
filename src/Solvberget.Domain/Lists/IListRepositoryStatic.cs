using System;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IListRepositoryStatic : IListRepository
    {
        DateTime? GetTimestampForLatestChange();
    }
}
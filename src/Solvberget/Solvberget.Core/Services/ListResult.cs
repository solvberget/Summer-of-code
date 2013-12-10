using System.Collections.Generic;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services
{
    public class ListResult<TDto> : RequestReplyDto
    {
        public List<TDto> Results { get; set; } 
    }
}
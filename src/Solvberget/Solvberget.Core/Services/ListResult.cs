using System.Collections.Generic;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services
{
    public class ListResult<TDto> : RequestReplyDto
    {
		public ListResult()
		{
			Results = new List<TDto>();
		}

        public List<TDto> Results { get; set; } 
    }
}
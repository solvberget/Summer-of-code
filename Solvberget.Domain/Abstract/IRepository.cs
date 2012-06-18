using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain
{
    public interface IRepository
    {

        List<Document> Search(string value);
    }

    
}

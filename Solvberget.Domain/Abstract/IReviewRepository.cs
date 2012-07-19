using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.Abstract
{
    public interface IReviewRepository
    {
        string GetDocumentReview(string id);
    }
}

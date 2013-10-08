using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.Abstract
{
    public interface IImageRepository
    {
        string GetDocumentImage( string id );
        string GetDocumentThumbnailImage(string id, string size);
    }
}

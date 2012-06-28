﻿using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IRepository
    {
        List<Document> Search(string value);
        Document FindDocument(string documentNumber);
    }  
    
}

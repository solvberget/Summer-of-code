﻿using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IRepository
    {
        List<Document> Search(string value);
        Document GetDocument(string documentNumber, bool isLight);
        UserInfo GetUserInformation(string userId, string verification);
    }  
    
}

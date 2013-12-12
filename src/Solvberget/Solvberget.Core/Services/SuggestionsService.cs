﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        private readonly DtoDownloader _downloader;

        public SuggestionsService(DtoDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<List<LibrarylistDto>> GetSuggestionsLists()
        {
            var response = await _downloader.DownloadList<LibrarylistDto>(Resources.ServiceUrl + Resources.ServiceUrl_Lists);

            return response.Results;
        }

        public async Task<LibrarylistDto> GetList(string id)
        {
            return await _downloader.Download<LibrarylistDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_List, id));
        } 
    }
}

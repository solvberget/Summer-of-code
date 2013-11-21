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
        private readonly IStringDownloader _downloader;

        public SuggestionsService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<List<LibrarylistDto>> GetSuggestionsLists()
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Lists);
                return JsonConvert.DeserializeObject<List<LibrarylistDto>>(response);
            }
            catch (Exception)
            {
                return new List<LibrarylistDto>
                {
                    new LibrarylistDto
                    {
                        Name = "Feil ved lasting, kunne desverre ikke finne noen lister. Prøv igjen senere.",
                    }
                };
            }
        }

        public async Task<LibrarylistDto> GetList(string id)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_List, id));
                return JsonConvert.DeserializeObject<LibrarylistDto>(response);
            }
            catch (Exception)
            {
                return new LibrarylistDto
                {

                    Documents = {new DocumentDto
                    {
                        Title = "Feil",
                        SubTitle = "Kunne ikke laste listen"
                    }}
                };
            }
        } 
    }
}

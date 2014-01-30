﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    class SearchService : ISearchService
    {
        private readonly DtoDownloader _dtos;
        private readonly IStringDownloader _rawHttp;

        public SearchService(DtoDownloader stringDownloader, IStringDownloader rawHttp)
        {
            _dtos = stringDownloader;
            _rawHttp = rawHttp;
        }

        public async Task<IEnumerable<DocumentDto>>  Search(string query)
        {
            var result = await _dtos.DownloadList<DocumentDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Search, query));
                        return result.Results;
        }

        public async Task<DocumentDto> Get(string docId)
        {
            try
            {
                var response = await _rawHttp.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Document, docId));
                var doc = JsonConvert.DeserializeObject<DocumentDto>(response);

                switch (doc.Type)
                {
                    case "Cd":
                        return JsonConvert.DeserializeObject<CdDto>(response);
                    case "Film":
                        return JsonConvert.DeserializeObject<FilmDto>(response);
                    case "Book":
                        return JsonConvert.DeserializeObject<BookDto>(response);
                    case "Journal":
                        return JsonConvert.DeserializeObject<JournalDto>(response);
                    case "Game":
                        return JsonConvert.DeserializeObject<GameDto>(response);
                    case "SheetMusic":
                        return JsonConvert.DeserializeObject<SheetMusicDto>(response);
                    case "AudioBook":
                        return JsonConvert.DeserializeObject<AudioBookDto>(response);
                    default:
                        return doc;
                }

            }
            catch (Exception e)
            {
                Mvx.Trace(e.Message);
                return new DocumentDto
                {
                    Success = false,
                    Reply = "Kunen ikke laste dokumentet",
                    Title = "Kunne ikke laste dokumentet"
                };
            }
        }

        public async Task<DocumentRatingDto> GetRating(string docId)
        {
            var response = await _dtos.Download<DocumentRatingDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Rating, docId));
            return response;
        }

        public async Task<DocumentReviewDto> GetReview(string docId)
        {
            return await _dtos.Download<DocumentReviewDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Review, docId));
        }
    }
}

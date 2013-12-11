﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class DtoDownloader
    {
        private readonly IStringDownloader _downloader;

        public DtoDownloader(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

		public virtual async Task<TDto> Download<TDto>(string url, string method = "GET")
            where TDto : RequestReplyDto, new()
        {
            try
            {
                var response = await _downloader.Download(url, method);
                return JsonConvert.DeserializeObject<TDto>(response);
            }
            catch (WebException ex)
            {
				if (ex.Message.Contains("(401) Unauthorized"))
				{
					return new TDto { Success = false, Reply = Replies.RequireLoginReply };
				}

                switch (ex.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        return new TDto { Success = false, Reply = "Får ikke kontakt med bibliotekssystemet." };
                    default:
                        return new TDto { Success = false, Reply = "En ukjent feil oppstod." };
                }
                
            }
            catch(Exception ex)
            {
                return new TDto { Success = false, Reply = ex.Message };
            }
        }

		public virtual async Task<ListResult<TDto>> DownloadList<TDto>(string url, string method = "GET")
        {
            try
            {
                var response = await _downloader.Download(url, method);
                var list = JsonConvert.DeserializeObject<List<TDto>>(response);

                return new ListResult<TDto>{Results = list};
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        return new ListResult<TDto> { Success = false, Reply = "Får ikke kontakt med bibliotekssystemet." };
                    default:
                        return new ListResult<TDto> { Success = false, Reply = "En ukjent feil oppstod." };
                }

            }
            catch (Exception ex)
            {
                return new ListResult<TDto> { Success = false, Reply = ex.Message };
            }
        }
    }
}
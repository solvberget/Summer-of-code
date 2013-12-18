using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services
{
    public class DtoDownloader
    {
        private readonly IStringDownloader _downloader;

        public DtoDownloader(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

		public virtual async Task<TDto> Download<TDto>(string url, string method = "GET", bool ignoreError = false)
            where TDto : RequestReplyDto, new()
        {
            try
            {
                var response = await _downloader.Download(url, method);
                return JsonConvert.DeserializeObject<TDto>(response);
            }
            catch (WebException ex)
			{
				if (ex.Message.Contains("NameResolutionFailure")) // WebExceptionStatus.NameResolutionFailure doesnt exist in mono?
				{
					return new TDto{ Success = false, Reply = "App´en trenger tilgang til internett for å fortsette." };
				}

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

		public virtual async Task<ListResult<TDto>> DownloadList<TDto>(string url, string method = "GET", bool ignoreError = false)
        {
            try
            {
                var response = await _downloader.Download(url, method);
                var list = JsonConvert.DeserializeObject<List<TDto>>(response);

                return new ListResult<TDto>{Results = list};
            }
            catch (WebException ex)
			{
				if (ex.Message.Contains("(401) Unauthorized"))
				{
                    return new ListResult<TDto> { Success = false, Reply = Replies.RequireLoginReply, Results = new List<TDto>() };
				}

				if (ex.Message.Contains("NameResolutionFailure")) // WebExceptionStatus.NameResolutionFailure doesnt exist in mono?
				{
					return new ListResult<TDto>{ Success = false, Reply = "App´en trenger tilgang til internett for å fortsette.", Results = new List<TDto>()};
				}

                switch (ex.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        return new ListResult<TDto> { Success = false, Reply = "Får ikke kontakt med bibliotekssystemet.", Results = new List<TDto>()};
                    default:
                        return new ListResult<TDto> { Success = false, Reply = "En ukjent feil oppstod.", Results = new List<TDto>()};
                }

            }
            catch (Exception ex)
            {
                return new ListResult<TDto> { Success = false, Reply = ex.Message, Results = new List<TDto>()};
            }
        }
    }
}
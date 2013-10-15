using System.Web.Hosting;

using Solvberget.Domain.Abstract;

namespace Solvberget.Domain.DTO
{
    public class RootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return HostingEnvironment.ApplicationPhysicalPath;
        }
    }
}
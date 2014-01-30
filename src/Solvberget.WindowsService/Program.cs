using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Solvberget.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new LuceneService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

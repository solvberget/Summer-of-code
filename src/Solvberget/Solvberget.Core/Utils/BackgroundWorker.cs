using System;
using System.Threading;

namespace Solvberget.Core.Utils
{
    class BackgroundWorker : IBackgroundWorker
    {
        public void Invoke(Action unitOfWork)
        {
            ThreadPool.QueueUserWorkItem(t => unitOfWork());
        }
    }
}

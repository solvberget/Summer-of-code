using System;

namespace Solvberget.Core.Utils
{
    class SynchronousBackgroundWorker : IBackgroundWorker
    {
        public void Invoke(Action unitOfWork)
        {
            unitOfWork();
        }
    }
}

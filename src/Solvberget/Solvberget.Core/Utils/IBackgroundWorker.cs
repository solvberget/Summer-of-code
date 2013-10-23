using System;

namespace Solvberget.Core.Utils
{
    public interface IBackgroundWorker
    {
        void Invoke(Action unitOfWork);
    }
}
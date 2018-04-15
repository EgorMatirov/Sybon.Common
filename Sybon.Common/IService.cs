using System;

namespace Sybon.Common
{
    public interface IService
    {
        string ServiceName { get; }
        void Start(string[] startupArguments, Action serviceStoppedCallback);
        void Stop();
    }
}
using System;

namespace Sybon.Common
{
    public static class StartupHandler<TService> where TService : IService
    {
        public static void Handle(TService myService, string[] args)
        {
            myService.Start(new string[0], () => { });
        }
    }
}
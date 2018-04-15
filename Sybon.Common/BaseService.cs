using System;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Sybon.Common
{
    public abstract class BaseService : IService
    {
        private IWebHost _app;
        private readonly Func<string[], IWebHost> _buildWebHostFunc;

        protected BaseService(Func<string[], IWebHost> buildWebHostFunc)
        {
            _buildWebHostFunc = buildWebHostFunc;
        }
        
        public void Start(string[] startupArguments, Action serviceStoppedCallback)
        {
            // TODO: Workaround https://github.com/NLog/NLog.Web/issues/210
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                _app = _buildWebHostFunc(startupArguments);
                _app.Run();
            }
            catch (Exception e)
            {
                //NLog: catch setup errors
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                serviceStoppedCallback();
            }
        }

        public void Stop()
        {
            _app.StopAsync().Wait();
        }

        public virtual string ServiceName { get; }
    }
}
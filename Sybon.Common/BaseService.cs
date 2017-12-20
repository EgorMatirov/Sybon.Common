﻿using System;
using System.Globalization;
using System.Threading;
using DasMulli.Win32.ServiceUtils;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Sybon.Common
{
    public abstract class BaseService : IService
    {
        protected IWebHost _app;
        protected readonly Func<string[], IWebHost> _buildWebHostFunc;
        
        public BaseService(Func<string[], IWebHost> buildWebHostFunc)
        {
            _buildWebHostFunc = buildWebHostFunc;
        }
        
        public void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback)
        {
            // TODO: Workaround https://github.com/NLog/NLog.Web/issues/210
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                _app = _buildWebHostFunc(startupArguments);
                _app.Start();
            }
            catch (Exception e)
            {
                //NLog: catch setup errors
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
        }

        public void Stop()
        {
            _app.StopAsync().Wait();
        }

        public virtual string ServiceName { get; }
    }
}
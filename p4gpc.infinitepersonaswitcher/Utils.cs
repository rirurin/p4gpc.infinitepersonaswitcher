using p4gpc.infinitepersonaswitcher.Configuration;
using Reloaded.Mod.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace p4gpc.infinitepersonaswitcher
{
    public class Utils
    {
        public Config Configuration;
        private ILogger _logger;
        public Utils(Config configuration, ILogger logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public void LogDebug(string message)
        {
            if (Configuration.DebugEnabled)
                _logger.WriteLine($"[InfinitePersonaSwitcher] {message}");
        }

        public void Log(string message)
        {
            _logger.WriteLine($"[InfinitePersonaSwitcher] {message}");
        }

        public void LogError(string message, Exception e)
        {
            _logger.WriteLine($"[InfinitePersonaSwitcher] {message}: {e.Message}", System.Drawing.Color.Red);
        }
    }
}
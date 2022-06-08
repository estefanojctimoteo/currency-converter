using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Currency_Converter.Infra.Data.Helper
{
    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        { }

        private class MyLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                try
                {
                    string nomeArquivo = string.Format(@"C:\temp\log_Currency_Converter_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                    File.AppendAllText(nomeArquivo, formatter(state, exception));
                }
                catch (Exception)
                { }
                Console.WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}

using System;

namespace MyApp.Business.Services
{
    public interface ILog
    {
        void Debug(string message);
        void Error(string message);
        void Warning(string message);
        void Info(string message);
    }
}

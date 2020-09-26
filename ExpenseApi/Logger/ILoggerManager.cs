using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseApi.Logger
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogModelState(ModelStateDictionary modelState);
    }
}

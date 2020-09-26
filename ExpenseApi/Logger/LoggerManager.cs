using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;

namespace ExpenseApi.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        public void LogError(string message)
        {
            logger.Error(message);
        }
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogModelState(ModelStateDictionary modelState)
        {
            var serializableModelState = new SerializableError(modelState);
            var modelStateSerialized = JsonSerializer.Serialize(serializableModelState);
            logger.Info($"BadRequest : {modelStateSerialized}");
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}

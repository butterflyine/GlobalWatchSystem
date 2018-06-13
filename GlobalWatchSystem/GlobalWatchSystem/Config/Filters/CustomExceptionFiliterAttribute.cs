using System.Web.Mvc;
using NLog;

namespace GlobalWatchSystem.Filters
{
    public class CustomExceptionFiliterAttribute : IExceptionFilter
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext context)
        {
            logger.Error("Error occurred: {0}", context.Exception.Message);
            logger.Error(context.Exception.StackTrace);
        }
    }
}
using System.Web.Mvc;
using NLog;

namespace WebClient.Filters
{
    public class NLogFilter : IActionFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var action = filterContext.ActionDescriptor.ActionName;
            Logger.Info("About to execute action '{0}'...", action);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var action = filterContext.ActionDescriptor.ActionName;
            Logger.Info("Executed action '{0}'...", action);
        }
    }
}
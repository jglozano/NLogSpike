using System;
using System.Web;
using Elmah;

namespace ElmahR.Client
{
    public static class ElmahExtension
    {
        public static void LogToElmah(this Exception ex)
        {
            if (HttpContext.Current != null)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            else
            {
                if (_httpApplication == null)
                {
                    InitNoContext();
                }

                ErrorSignal.Get(_httpApplication).Raise(ex);
            }
        }

        private static HttpApplication _httpApplication;
        private static readonly ErrorFilterConsole errorFilter = new ErrorFilterConsole();
        public static ErrorLogModule ErrorLog = new ErrorLogModule();

        private static void InitNoContext()
        {
            _httpApplication = new HttpApplication();
            errorFilter.Init(_httpApplication);

            (ErrorLog as IHttpModule).Init(_httpApplication);
            errorFilter.HookFiltering(ErrorLog);
        }

        private class ErrorFilterConsole : ErrorFilterModule
        {
            public void HookFiltering(IExceptionFiltering module)
            {
                module.Filtering += base.OnErrorModuleFiltering;
            }
        }
    }
}

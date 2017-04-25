using OntologyTypeAheadApi.App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;

namespace OntologyTypeAheadApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}

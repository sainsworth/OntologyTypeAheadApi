using Microsoft.Practices.Unity;
using OntologyTypeAheadApi.Context.Contract;
using OntologyTypeAheadApi.Context.Implementation;
using OntologyTypeAheadApi.Controllers;
using OntologyTypeAheadApi.Service.Contract;
using OntologyTypeAheadApi.Service.Implementation;
using System.Web.Http;
using Unity.WebApi;

namespace OntologyTypeAheadApi.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterInstance<IDatastoreContext>(new Mock_DatastoreContext());
            container.RegisterInstance<IRdfSourceContext>(new Mock_RdfSourceContext());

            container.RegisterType<IDatastoreService, DatastoreService>();
            container.RegisterType(typeof(LookupController));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}

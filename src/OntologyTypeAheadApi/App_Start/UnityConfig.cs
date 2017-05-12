using Microsoft.Practices.Unity;
using OntologyTypeAheadApi.Context.Contract;
using OntologyTypeAheadApi.Context.Implementation;
using OntologyTypeAheadApi.Controllers;
using OntologyTypeAheadApi.Service.Contract;
using OntologyTypeAheadApi.Service.Implementation;
using System.Web.Http;
using Unity.WebApi;
using System.Configuration;

namespace OntologyTypeAheadApi.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            switch (ConfigurationManager.AppSettings["Datastore"].ToUpper())
            {
                case "MOCK":
                    container.RegisterInstance<IDatastoreContext>(new Mock_DatastoreContext());
                    break;
                case "ELASTIC":
                    var elasticUrl = ConfigurationManager.AppSettings["ElasticUrl"];
                    container.RegisterInstance<IDatastoreContext>(new Elastic_DatastoreContext(elasticUrl));
                    break;
            }
            switch (ConfigurationManager.AppSettings["RdfSource"].ToUpper())
            {
                case "MOCK":
                    container.RegisterInstance<IRdfSourceContext>(new Mock_RdfSourceContext());
                    break;
            }

            container.RegisterType<IDatastoreService, DatastoreService>();
            container.RegisterType(typeof(LookupController));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}

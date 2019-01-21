using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Service;
using Server.Controllers;

namespace Server
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            TechService service = CreateTechService();
            CreateEmployeesAndTasks(service);

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(service));
        }

        private static TechService CreateTechService()
        {
            var config = new TechServiceConfig()
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(60)),
                Tm = TimeSpan.FromSeconds(10),
                Td = TimeSpan.FromSeconds(20)
            };

            return new TechService(config);
        }

        private static void CreateEmployeesAndTasks(TechService service)
        {
            service.CreateDirector("Директор");
            service.CreateManager("Менеджер");
            service.CreateOperator("Оператор №1");
            service.CreateOperator("Оператор №2");

            service.CreateTask("Запрос в службу поддержки №1");
            service.CreateTask("Запрос в службу поддержки №2");
            service.CreateTask("Запрос в службу поддержки №3");
            service.CreateTask("Запрос в службу поддержки №4");
            service.CreateTask("Запрос в службу поддержки №5");
            service.CreateTask("Запрос в службу поддержки №6");
            service.CreateTask("Запрос в службу поддержки №7");
        }
    }

    internal class ControllerFactory : DefaultControllerFactory
    {
        private TechService _service;

        public ControllerFactory(TechService service)
        {
            _service = service;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == typeof(HomeController))
                return (IController)Activator.CreateInstance<HomeController>();

            return (IController)Activator.CreateInstance(controllerType, new[] { _service });
        }
    }
}

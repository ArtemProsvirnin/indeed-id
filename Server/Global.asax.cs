using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Service;

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

            CreateEmployeesAndTasks();
        }

        private static void CreateEmployeesAndTasks()
        {
            var service = TechServiceSingleton.Instance;

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
}

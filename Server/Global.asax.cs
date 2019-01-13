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
            var servise = TechServiceSingleton.Instance;
            var factory = new EmployeeFactory(servise);

            var director = factory.CreateDirector("Директор");
            var manager = factory.CreateManager("Менеджер");
            var operator1 = factory.CreateOperator("Оператор №1");
            var operator2 = factory.CreateOperator("Оператор №2");

            servise.CreateTask("Запрос в службу поддержки №1");
            servise.CreateTask("Запрос в службу поддержки №2");
            servise.CreateTask("Запрос в службу поддержки №3");
            servise.CreateTask("Запрос в службу поддержки №4");
            servise.CreateTask("Запрос в службу поддержки №5");
            servise.CreateTask("Запрос в службу поддержки №6");
            servise.CreateTask("Запрос в службу поддержки №7");
        }
    }
}

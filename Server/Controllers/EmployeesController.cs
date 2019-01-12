using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Server.Models;

namespace Server.Controllers
{
    public class EmployeesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Employee> employees = TechServiceSingleton.Instance.Employees;
            IEnumerable<EmployeeDTO> dto = employees.Select(e => new EmployeeDTO(e));

            return Json(dto);
        }
    }
}
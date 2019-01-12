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

        [HttpPost]
        public ActionResult Add(EmployeeDTO dto)
        {
            var factory = new EmployeeFactory(TechServiceSingleton.Instance);
            Employee employee = factory.CreateByPositionName(dto.Position, dto.Name);

            return Json(new EmployeeDTO(employee));
        }

        [HttpDelete]
        public ActionResult Delete(EmployeeDTO dto)
        {
            TechServiceSingleton.Instance.Employees.Remove(dto.Id);
            return new HttpStatusCodeResult(200);
        }
    }
}
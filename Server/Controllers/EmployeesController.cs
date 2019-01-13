using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class EmployeesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<Employee> employees = TechServiceSingleton.Instance.Employees.ToList();
            IEnumerable<EmployeeDTO> dto = employees.Select(e => new EmployeeDTO(e));

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create(EmployeeDTO dto)
        {
            var factory = new EmployeeFactory(TechServiceSingleton.Instance);
            Employee employee = factory.CreateByPositionName(dto.Position, dto.Name);

            return await Task.Run(() => Json(new EmployeeDTO(employee)));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            TechServiceSingleton.Instance.Employees.Remove(id);
            return new HttpStatusCodeResult(200);
        }
    }
}
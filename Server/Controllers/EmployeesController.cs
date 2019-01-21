using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Service;
using Server.Models;

namespace Server.Controllers
{
    public class EmployeesController : Controller
    {
        private TechService _service;

        public EmployeesController(TechService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Employee> employees = _service.Employees.ToList();
            IEnumerable<EmployeeDTO> dto = employees.Select(e => new EmployeeDTO(e));

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(EmployeeDTO dto)
        {
            Employee employee = _service.CreateEmployee(dto.Position, dto.Name);
            return Json(new EmployeeDTO(employee)); 
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _service.Employees.Remove(id);
            return new HttpStatusCodeResult(200);
        }
    }
}
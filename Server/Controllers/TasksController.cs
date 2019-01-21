using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Service;
using Server.Models;

namespace Server.Controllers
{
    public class TasksController : Controller
    {
        private TechService _service;

        public TasksController(TechService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<TechTask> tasks = _service.Tasks;
            IEnumerable<TaskDTO> dto = tasks.Select(t => new TaskDTO(t));

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(TaskDTO dto)
        {
            TechTask task = _service.CreateTask(dto.Description);
            return Json(new TaskDTO(task));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _service.DeleteTask(id);
            return new HttpStatusCodeResult(200);
        }
    }
}
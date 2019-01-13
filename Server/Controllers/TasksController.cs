using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Service;
using Server.Models;

namespace Server.Controllers
{
    public class TasksController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<TechTask> tasks = TechServiceSingleton.Instance.Tasks.ToList();
            IEnumerable<TaskDTO> dto = tasks.Select(t => new TaskDTO(t));

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(TaskDTO dto)
        {
            TechTask task = TechServiceSingleton.Instance.CreateTask(dto.Description);
            return Json(new TaskDTO(task));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            TechServiceSingleton.Instance.DeleteTask(id);
            return new HttpStatusCodeResult(200);
        }
    }
}
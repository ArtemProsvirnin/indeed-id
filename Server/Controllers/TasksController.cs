using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            IEnumerable<TechTask> tasks = TechServiceSingleton.Instance.Tasks;
            IEnumerable<TaskDTO> dto = tasks.Select(t => new TaskDTO(t));

            return Json(dto);
        }

        [HttpPost]
        public ActionResult Add(string description)
        {
            TechTask task = TechServiceSingleton.Instance.CreateTask(description);
            return Json(new TaskDTO(task));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            TechServiceSingleton.Instance.DeleteTask(id);
            return new HttpStatusCodeResult(200);
        }
    }
}
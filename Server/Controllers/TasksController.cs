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
    }
}
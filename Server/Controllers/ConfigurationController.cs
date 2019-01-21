using System;
using System.Web.Mvc;
using Service;
using Server.Models;

namespace Server.Controllers
{
    public class ConfigurationController : Controller
    {
        private TechService _service;

        public ConfigurationController(TechService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var config = _service.Config;
            return Json(new ConfigurationDTO(config), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(ConfigurationDTO dto)
        {
            //todo Гибкое настраивание временных интервалов (Дни, часы, минуты...)
            var config = _service.Config;

            if (dto.Td != 0)
                config.Td = TimeSpan.FromSeconds(dto.Td);

            if (dto.Tm != 0)
                config.Tm = TimeSpan.FromSeconds(dto.Tm);

            config.TimeRange = new TimeRange(TimeSpan.FromSeconds(dto.RangeMin), TimeSpan.FromSeconds(dto.RangeMax));

            return new HttpStatusCodeResult(200);
        }
    }
}
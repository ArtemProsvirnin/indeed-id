using System;
using System.Web.Mvc;
using Service;
using Server.Models;

namespace Server.Controllers
{
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var config = TechServiceSingleton.Instance.Config;
            return Json(new ConfigurationDTO(config), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(ConfigurationDTO dto)
        {
            //todo Гибкое настраивание временных интервалов (Дни, часы, минуты...)
            var config = TechServiceSingleton.Instance.Config;

            if (dto.Td != 0)
                config.Td = TimeSpan.FromSeconds(dto.Td);

            if (dto.Tm != 0)
                config.Tm = TimeSpan.FromSeconds(dto.Tm);

            config.TimeRange = new TimeRange(TimeSpan.FromSeconds(dto.RangeMin), TimeSpan.FromSeconds(dto.RangeMax));

            return new HttpStatusCodeResult(200);
        }
    }
}
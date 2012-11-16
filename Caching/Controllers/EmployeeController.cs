using System.Threading;
using System.Web.Mvc;
using Caching.Caching;
using Caching.Models;

namespace Caching.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpGet]
        [ETagFromAppVersion]
        public ActionResult Add()
        {
            return View(new EmployeeModel());
        }

        [HttpPost]
        public ActionResult Add(EmployeeModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            InMemoryRepository.Save(model);

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var model = InMemoryRepository.Get<EmployeeModel>(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            InMemoryRepository.Save(model);

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        [ETagFromModelVersion]
        public ActionResult Details(long id)
        {
            var model = InMemoryRepository.Get<EmployeeModel>(id);
            //simulate a long running process
            Thread.Sleep(7500);

            return View(model);
        }
    }
}
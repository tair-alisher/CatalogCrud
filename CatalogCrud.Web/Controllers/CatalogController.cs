using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CatalogCrud.Web.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService CatalogService;

        public CatalogController(ICatalogService catalogService)
        {
            CatalogService = catalogService;
        }

        public ActionResult Index()
        {
            var catalogs = CatalogService.GetAll().ToList();
            var catalogVMList = Mapper.Map<IEnumerable<CatalogVM>>(catalogs);

            return View(catalogVMList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CatalogVM model)
        {
            if (ModelState.IsValid)
            {
                var catalogDTO = Mapper.Map<CatalogDTO>(model);
                CatalogService.Add(catalogDTO);

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
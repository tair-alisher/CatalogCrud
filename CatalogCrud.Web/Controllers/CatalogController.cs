using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Exceptions;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.Web.Models.ViewModels;
using CatalogCrud.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CatalogCrud.Web.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService CatalogService;
        private IFieldService FieldService;

        public CatalogController(ICatalogService catalogService, IFieldService fieldService)
        {
            CatalogService = catalogService;
            FieldService = fieldService;
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

        public ActionResult AttachedFields(Guid? catalogId)
        {
            try
            {
                var catalog = CatalogService.Get(catalogId);
                var catalogVM = Mapper.Map<CatalogVM>(catalog);

                return View(catalogVM);
            }
            catch (ArgumentNullException)
            {
                return RedirectToRoute(new
                {
                    controller = "Message",
                    action = "Error",
                    message = Messages.IdIsNull
                });
            }
            catch (NotFoundException)
            {
                return RedirectToRoute(new
                {
                    controller = "Message",
                    action = "Error",
                    message = Messages.NotFound
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttachField(Guid catalogId, Guid fieldId)
        {
            var fieldVM = Mapper.Map<FieldVM>(FieldService.Get(fieldId));
            var result = CatalogService.AttachField(catalogId, fieldId);

            if (result.Succeeded)
                return PartialView(fieldVM);
            else
                return RedirectToRoute(new
                {
                    controller = "Message",
                    action = "PartialError",
                    message = result.Message
                });
        }
    }
}
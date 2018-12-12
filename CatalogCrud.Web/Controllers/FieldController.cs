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
    [Authorize(Roles = "admin, catalog_admin, app_admin")]
    public class FieldController : Controller
    {
        private IFieldService FieldService;

        public FieldController(IFieldService fieldService)
        {
            FieldService = fieldService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var fields = FieldService.GetAll().ToList();
            var fieldVMList = Mapper.Map<IEnumerable<FieldVM>>(fields);

            return View(fieldVMList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FieldVM model)
        {
            if (ModelState.IsValid)
            {
                var fieldDTO = Mapper.Map<FieldDTO>(model);
                FieldService.Add(fieldDTO);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                var fieldDTO = FieldService.Get(id);
                var fieldVM = Mapper.Map<FieldVM>(fieldDTO);

                return View(fieldVM);
            }
            catch (ArgumentNullException)
            {
                return RedirectToRoute(new { controller = "Message", action = "Error", message = Messages.IdIsNull });
            }
            catch (NotFoundException)
            {
                return RedirectToRoute(new { controller = "Message", action = "Error", message = Messages.NotFound });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FieldVM model)
        {
            if (ModelState.IsValid)
            {
                var fieldDTO = Mapper.Map<FieldDTO>(model);
                FieldService.Update(fieldDTO);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(Guid? id)
        {
            var result = FieldService.Delete(id);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                return RedirectToRoute(new { controller = "Message", action = "Error", message = result.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindFields(string value)
        {
            var foundFields = FieldService.FindFields(value).ToList();
            var foundFieldVMList = Mapper.Map<IEnumerable<FieldVM>>(foundFields);

            return PartialView(foundFieldVMList.ToList());
        }
    }
}
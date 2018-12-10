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
        private IValueService ValueService;

        public CatalogController(ICatalogService catalogService, IFieldService fieldService, IValueService valueService)
        {
            CatalogService = catalogService;
            FieldService = fieldService;
            ValueService = valueService;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DetachField(Guid catalogId, Guid fieldId)
        {
            var result = CatalogService.DetachField(catalogId, fieldId);

            if (result.Succeeded)
                return "success";
            else
                return "fail";
        }

        public ActionResult Values(Guid? catalogId)
        {
            try
            {
                ViewBag.CatalogId = catalogId;
                ViewBag.Fields = Mapper.Map<IEnumerable<FieldVM>>(CatalogService.GetOrderedCatalogFieldList(catalogId).ToList()).ToList();
                ViewBag.Rows = ValueService.GetCatalogValuesByRows(catalogId);

                return View();
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRow(Guid catalogId, int rowNumber)
        {
            ViewBag.RowNumber = rowNumber;
            var fields = CatalogService.GetOrderedCatalogFieldList(catalogId).ToList();
            var fieldVMList = Mapper.Map<IEnumerable<FieldVM>>(fields).ToList();

            return PartialView(fieldVMList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddedRow(IEnumerable<ValueVM> values)
        {
            ValueDTO valueDTO;
            foreach (var value in values)
            {
                valueDTO = Mapper.Map<ValueDTO>(value);
                ValueService.Add(valueDTO);
            }

            int row = values.First().Row;
            Guid catalogId = values.First().CatalogId;
            ViewBag.RowNumber = row;
            ViewBag.Values = ValueService.GetCatalogValuesByRow(catalogId, row);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRow(Guid catalogId, int rowNumber)
        {
            try
            {
                ViewBag.RowNumber = rowNumber;
                ViewBag.Row = ValueService.GetCatalogValuesByRow(catalogId, rowNumber);

                return PartialView();
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new
                {
                    controller = "Message",
                    action = "PartialError",
                    mesasge = ex.Message
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditedRow(IEnumerable<ValueVM> values)
        {
            return PartialView();
        }
    }
}
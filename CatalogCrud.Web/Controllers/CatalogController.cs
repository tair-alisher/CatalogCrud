using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Exceptions;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.Web.Models.ViewModels;
using CatalogCrud.Web.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CatalogCrud.Web.Controllers
{
    //[Authorize(Roles = "admin, catalog_admin, app_admin")]
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

        [AllowAnonymous]
        public ActionResult Index(int ? page)
        {
            var ItemsPerPage = 10;
            var catalogs = CatalogService.GetAll().ToList();
            var catalogVMList = Mapper.Map<IEnumerable<CatalogVM>>(catalogs);

            return View(catalogVMList.ToPagedList(page ?? 1, ItemsPerPage));
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

        public ActionResult Edit(Guid? id)
        {
            try
            {
                var catalogDTO = CatalogService.Get(id);
                var catalogVM = Mapper.Map<CatalogVM>(catalogDTO);

                return View(catalogVM);
            }
            catch (ArgumentNullException)
            {
                return RedirectToRoute(new { controller = "Message", action = "Error", message = Messages.IdIsNull });
            }
            catch (NotFoundException)
            {
                return RedirectToRoute(new { controller = "Message", Action = "Error", message = Messages.NotFound });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CatalogVM model)
        {
            if (ModelState.IsValid)
            {
                var catalogDTO = Mapper.Map<CatalogDTO>(model);
                CatalogService.Update(catalogDTO);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(Guid? id)
        {
            var result = CatalogService.Delete(id);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                return RedirectToRoute(new { controller = "Message", action = "Error", message = result.Message });
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
                return RedirectToRoute(new { controller = "Message", action = "Error", message = Messages.IdIsNull });
            }
            catch (NotFoundException)
            {
                return RedirectToRoute(new { controller = "Message", action = "Error", message = Messages.NotFound });
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
                return RedirectToRoute(new { controller = "Message", action = "PartialError", message = result.Message });
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

        public ActionResult Values(Guid? catalogId, int? page)
        {
            try
            {
                int ItemsPerPage = 2;
                ViewBag.CatalogId = catalogId;
                ViewBag.Fields = Mapper.Map<IEnumerable<FieldVM>>(CatalogService.GetOrderedCatalogFieldList(catalogId).ToList()).ToList();

                var valuesByRows = ValueService.GetCatalogValuesByRows(catalogId);
                var rows = ConvertDTOValuesByRowsToVMValuesByRows(valuesByRows);

                return View(rows.ToPagedList(page ?? 1, ItemsPerPage));
            }
            catch (ArgumentNullException)
            {
                return RedirectToRoute(new { controller = "Message", action = "Error", message = Messages.IdIsNull });
            }
        }

        public ActionResult UploadFile(Guid catalogId)
        {
            ViewBag.CatalogId = catalogId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Guid catalogId)
        {
            var file = Request.Files["file"];
            if (file.ContentLength > 0)
            {
                var catalogFields = CatalogService.GetOrderedCatalogFieldList(catalogId);
                string validFileType = ".csv";
                string fileExt = Path.GetExtension(file.FileName).ToLower();

                if (fileExt == validFileType)
                {
                    StreamReader csvreader = new StreamReader(file.InputStream);
                    //while (!csvreader.EndOfStream)
                    //{
                    //    var line = csvreader.ReadLine();
                    //    var value = line.Split(';');
                    //}
                    var csvlines = csvreader.ReadToEnd().Split('\n');
                    List<string> csvFields = csvlines[0].Split(';').Select(f => f.ToLower()).ToList();
                    for (int line = 1; line < csvlines.Length; line++)
                    {
                        var values = csvlines[line].Split(';');
                        for (int value = 0; value < values.Length; value++)
                        {
                            var valueField = catalogFields.Where(cf => csvFields.Contains(cf.Name.ToLower())).FirstOrDefault();
                            if (valueField != null)
                            {
                                var valueDTO = new ValueDTO
                                {
                                    Title = values[value], // need to encode in utf8
                                    Row = line,
                                    FieldId = valueField.Id,
                                    CatalogId = catalogId
                                };
                                ValueService.Add(valueDTO);
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("file", "Файл должен быть формата <b>.csv</b>.");
            }
            else
                ModelState.AddModelError("excelFile", "Выберите файл.");

            return View("UploadFile");
        }

        private IEnumerable<RowVM> ConvertDTOValuesByRowsToVMValuesByRows(IEnumerable<IOrderedEnumerable<ValueDTO>> valuesByRows)
        {
            List<RowVM> rows = new List<RowVM>();

            foreach (var valuesByRow in valuesByRows)
            {
                var row = new RowVM
                {
                    Number = valuesByRow.First().Row
                };
                foreach (var valueDTO in valuesByRow)
                    row.Values.Add(Mapper.Map<ValueVM>(valueDTO));
                rows.Add(row);
            }

            return rows;
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
            ViewBag.CatalogId = catalogId;
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
                return RedirectToRoute(new { controller = "Message", action = "PartialError", mesasge = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditedRow(IEnumerable<ValueVM> values)
        {
            ValueDTO valueDTO;
            foreach (var value in values)
            {
                valueDTO = Mapper.Map<ValueDTO>(value);
                ValueService.Update(valueDTO);
            }

            int row = values.First().Row;
            Guid catalogId = values.First().CatalogId;
            ViewBag.CatalogId = catalogId;
            ViewBag.RowNumber = row;
            ViewBag.Values = ValueService.GetCatalogValuesByRow(catalogId, row);

            return PartialView();
        }

        public ActionResult DeleteRow(Guid catalogId, int rowNumber)
        {
            var result = ValueService.DeleteRowAndDecrementAllFollowing(catalogId, rowNumber);
            if (result.Succeeded)
                return RedirectToAction("Values", new { catalogId });
            else
                return RedirectToRoute(new { controller = "Message", action = "Error", message = result.Message });
        }
    }
}
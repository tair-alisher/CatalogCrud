using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CatalogCrud.Web.Controllers
{
    public class FieldController : Controller
    {
        private IFieldService FieldService;

        public FieldController(IFieldService fieldService)
        {
            FieldService = fieldService;
        }

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
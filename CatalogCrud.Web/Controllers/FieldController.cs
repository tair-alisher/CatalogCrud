using AutoMapper;
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
    }
}
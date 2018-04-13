using InventoryKeeper.Data;
using InventoryKeeper.Data.Models;
using InventoryKeeper.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace InventoryKeeper.Controllers
{
    public class TypeController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        public JsonResult SearchItems()
        {
            return Json(
              GridData.Make<ListItem>(Request,
              (searchCriteria, displayStart, recordsPerPage, orderField, sortDir) => IKData.ListItemDao.FindBy(searchCriteria, displayStart, recordsPerPage, orderField, sortDir),
              li => new string[] { li.Id.ToString(), li.TypeName, li.ListItemName, li.ListItemDescr }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id, bool? success)
        {
            int parsedId = 0;
            int.TryParse(id, out parsedId);

            ListItemEditModel result = new ListItemEditModel
            {
                ShowSuccessMessage = success.HasValue ? success.Value : false,
                EditItem = parsedId == 0 ? new ListItem() : IKData.ListItemDao.GetById(parsedId)
            };

            LoadViewData();

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ListItemEditModel value)
        {
            if (ModelState.IsValid)
            {
                Response Result = IKData.ListItemDao.Save(value.EditItem);
                if (Result.Success)
                    return RedirectToAction("Edit", new { id = Result.Id, Result.Success });
                else
                    ModelState.AddModelError("", Result.ResultMessage);
            }

            value.ShowSuccessMessage = ModelState.IsValid;

            LoadViewData();

            return View(value);
        }

        private void LoadViewData()
        {
            List<ListItem> types = IKData.ListItemDao.GetAll();

            ViewData["Types"] = new SelectList(types, "Id", "ListItemDescr");
        }

        public JsonResult AddType(string Type)
        {
            ListItem NewType = new ListItem
            {
                ListItemName = Type,
                ListItemDescr = Type,
                IsEnabled = true
            };
            return Json(IKData.ListItemDao.Save(NewType).Id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int Id)
        {
            return Json(IKData.ListItemDao.Delete(Id).ResultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}
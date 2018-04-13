using InventoryKeeper.Data;
using InventoryKeeper.Data.Models;
using InventoryKeeper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace InventoryKeeper.Controllers
{
    public class ItemController : Controller
    {
        public ActionResult Search()
        {
            LoadViewData();
            return View();
        }

        public JsonResult SearchItems(string typeId)
        {
            int TypeId;
            int.TryParse(typeId, out TypeId);

            return Json(
              GridData.Make<InventoryItem>(Request,
              (searchCriteria, displayStart, recordsPerPage, orderField, sortDir) => IKData.InventoryItemDao.FindBy(searchCriteria, displayStart, recordsPerPage, orderField, sortDir, TypeId),
              li => new string[] { li.Id.ToString(), li.TypeName, li.ItemDescr, li.SN,  "data:image/jpg;base64," + Convert.ToBase64String(li.Photo) }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id, bool? success)
        {
            int parsedId = 0;
            int.TryParse(id, out parsedId);

            InventoryItemEditModel result = new InventoryItemEditModel
            {
                ShowSuccessMessage = success.HasValue ? success.Value : false,
                EditItem = parsedId == 0 ? new InventoryItem() : IKData.InventoryItemDao.GetById(parsedId)
            };

            LoadViewData();

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InventoryItemEditModel value)
        {
            if (ModelState.IsValid)
            {
                if (Request != null && Request.Files[0].InputStream != null && Request.Files[0].InputStream.Length > 0)
                {
                    var validTypes = new[] { "image/jpeg", "image/pjpeg", "image/png", "image/gif" };
                    if (validTypes.Contains(Request.Files[0].ContentType))
                    {
                        using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                        {
                            value.EditItem.Photo = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                Response Result = IKData.InventoryItemDao.Save(value.EditItem);
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
            List<ListItem> itemTypes = IKData.ListItemDao.GetAll();

            ViewData["ItemTypes"] = new SelectList(itemTypes, "Id", "ListItemDescr");
        }

        public JsonResult Delete(int Id)
        {
            return Json(IKData.InventoryItemDao.Delete(Id).ResultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}
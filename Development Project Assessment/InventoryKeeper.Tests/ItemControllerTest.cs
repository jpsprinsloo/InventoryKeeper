using InventoryKeeper.Controllers;
using InventoryKeeper.Data;
using InventoryKeeper.Data.Models;
using InventoryKeeper.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace InventoryKeeper.Tests
{
    [TestClass]
    public class ItemControllerTest
    {
        private static int TestId;

        [TestMethod]
        public void Item_Search()
        {
            // Arrange
            ItemController controller = new ItemController();

            // Act
            ViewResult result = controller.Search() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Item_Edit_Redirect()
        {
            // Arrange
            ItemController controller = new ItemController();
            InventoryItemEditModel item = GetDemo(0);

            var result = controller.Edit(item) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual(true, result.RouteValues["success"]);
            Assert.AreNotEqual(0, result.RouteValues["id"]);

            TestId = (int)result.RouteValues["id"];

        }

        [TestMethod]
        public void Item_Edit()
        {
            // Arrange
            ItemController controller = new ItemController();
            InventoryItemEditModel newItem = GetDemo(TestId);
            var saveresult = controller.Edit(newItem) as ViewResult;

            if (saveresult != null)
            {
                var savedItem = (InventoryItemEditModel)saveresult.ViewData.Model;
                Assert.AreEqual(true, saveresult.ViewData.ModelState.IsValid, "ModelState");
                Assert.AreEqual("Unit Test Item Desription", savedItem.EditItem.ItemDescr);
            }

            var result = controller.Edit(TestId.ToString(), false) as ViewResult;
            InventoryItemEditModel editItem = (InventoryItemEditModel)result.ViewData.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(TestId, editItem.EditItem.Id);
            Assert.AreEqual("Unit Test Item Desription (amended)", editItem.EditItem.ItemDescr);
        }

        [TestMethod]
        public void Item_Delete()
        {
            ItemController controller = new ItemController();
            var result = controller.Delete(TestId) as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Item deleted successfully", result.Data);
        }

        private InventoryItemEditModel GetDemo(int id)
        {
            if (id == 0)
            {
                InventoryItem model = new InventoryItem
                {
                    Id = id,
                    ItemDescr = "Unit Test Item Desription",
                    SN = "00000000-0000-0000-0000-000000000000",
                    TypeId = IKData.ListItemDao.GetByName("UT_LIN").Id
                };

                return new InventoryItemEditModel { EditItem = model };
            }
            else
            {
                InventoryItem model = IKData.InventoryItemDao.GetById(id);
                model.ItemDescr = "Unit Test Item Desription (amended)";
                return new InventoryItemEditModel { EditItem = model };
            }
        }
    }
}

using InventoryKeeper.Controllers;
using InventoryKeeper.Data;
using InventoryKeeper.Data.Models;
using InventoryKeeper.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace InventoryKeeper.Tests
{
    [TestClass]
    public class TypeControllerTest
    {
        private static int TestId;

        [TestMethod]
        public void Type_Search()
        {
            // Arrange
            TypeController controller = new TypeController();

            // Act
            ViewResult result = controller.Search() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Type_Edit_Redirect()
        {
            // Arrange
            TypeController controller = new TypeController();
            ListItemEditModel item = GetDemo(0);

            var result = controller.Edit(item) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual(true, result.RouteValues["success"]);
            Assert.AreNotEqual(0, result.RouteValues["id"]);

            TestId = (int)result.RouteValues["id"];

        }

        [TestMethod]
        public void Type_Edit()
        {
            // Arrange
            TypeController controller = new TypeController();
            ListItemEditModel newItem = GetDemo(TestId);
            var saveresult = controller.Edit(newItem) as ViewResult;

            if (saveresult != null)
            {
                var savedItem = (ListItemEditModel)saveresult.ViewData.Model;
                Assert.AreEqual(true, saveresult.ViewData.ModelState.IsValid, "ModelState");
                Assert.AreEqual("UT_LIN", savedItem.EditItem.ListItemName);
            }

            var result = controller.Edit(TestId.ToString(), false) as ViewResult;
            ListItemEditModel editItem = (ListItemEditModel)result.ViewData.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(TestId, editItem.EditItem.Id);
            Assert.AreEqual("UT_LIN", editItem.EditItem.ListItemName);
            Assert.AreEqual("Unit Test List Item Description (amended)", editItem.EditItem.ListItemDescr);
        }

        [TestMethod]
        public void Type_Delete()
        {
            TypeController controller = new TypeController();
            var result = controller.Delete(TestId) as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("ListItem deleted successfully", result.Data);
        }

        private ListItemEditModel GetDemo(int id)
        {
            if (id == 0)
            {
                ListItem model = new ListItem
                {
                    Id = id,
                    ListItemName = "UT_LIN",
                    ListItemDescr = "Unit Test List Item Description",
                    ListItemValue = "Unit Test List Item Value",
                    IsEnabled = true
                };

                return new ListItemEditModel { EditItem = model };
            }
            else
            {
                ListItem model = IKData.ListItemDao.GetById(id);
                model.ListItemDescr = "Unit Test List Item Description (amended)";
                return new ListItemEditModel { EditItem = model };
            }
        }
    }
}

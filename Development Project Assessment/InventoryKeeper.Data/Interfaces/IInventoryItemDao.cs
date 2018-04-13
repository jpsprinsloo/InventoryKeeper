using InventoryKeeper.Data.Models;

namespace InventoryKeeper.Data.Interfaces
{
    public interface IInventoryItemDao
    {
        SearchResults<InventoryItem> FindBy(string searchCriteria, int displayStart, int recordsPerPage, int orderField, string sortDir, int typeId);
        InventoryItem GetById(int id);
        Response Save(InventoryItem item);
        Response Delete(int Id);
    }
}

using InventoryKeeper.Data.Models;
using System.Collections.Generic;

namespace InventoryKeeper.Data.Interfaces
{
    public interface IListItemDao
    {
        SearchResults<ListItem> FindBy(string searchCriteria, int displayStart, int recordsPerPage, int orderField, string sortDir);
        ListItem GetById(int id);
        ListItem GetByName(string name);
        List<ListItem> GetByTypeName(string typeName);
        List<ListItem> GetAll();
        Response Save(ListItem item);
        Response Delete(int Id);
    }
}

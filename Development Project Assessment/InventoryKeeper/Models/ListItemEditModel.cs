using InventoryKeeper.Data.Models;

namespace InventoryKeeper.Models
{
    public class ListItemEditModel
    {
        public string Name { get; set; }
        public ListItem EditItem { get; set; }
        public bool ShowSuccessMessage { get; set; }
    }
}
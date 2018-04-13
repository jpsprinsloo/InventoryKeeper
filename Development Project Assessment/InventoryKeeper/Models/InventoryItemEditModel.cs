using InventoryKeeper.Data.Models;

namespace InventoryKeeper.Models
{
    public class InventoryItemEditModel
    {
        public InventoryItem EditItem { get; set; }
        public bool ShowSuccessMessage { get; set; }
    }
}
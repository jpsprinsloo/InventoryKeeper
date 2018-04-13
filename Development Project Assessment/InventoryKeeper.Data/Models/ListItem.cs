using System.ComponentModel.DataAnnotations;

namespace InventoryKeeper.Data.Models
{
    public class ListItem : Base
    {
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        [Required, Display(Name = "Name")]
        public string ListItemName { get; set; }
        [Display(Name = "Description")]
        public string ListItemDescr { get; set; }
        [Display(Name = "Value")]
        public string ListItemValue { get; set; }
        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }

        public int TotalCount { get; set; }
    }
}

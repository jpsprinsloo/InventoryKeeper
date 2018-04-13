using System.ComponentModel.DataAnnotations;

namespace InventoryKeeper.Data.Models
{
    public class InventoryItem : Base
    {
        [Required, Display(Name = "Type")]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        [Required, Display(Name = "Description")]
        public string ItemDescr { get; set; }
        [Required, Display(Name = "Serial Number")]
        public string SN { get; set; }
        public byte[] Photo { get; set; }

        public int TotalCount { get; set; }
    }
}

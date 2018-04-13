using InventoryKeeper.Data.Interfaces;

namespace InventoryKeeper.Data
{
    public static class IKData
    {
        private static IListItemDao listitemdao;
        public static IListItemDao ListItemDao
        {
            get
            {
                if (listitemdao == null)
                    listitemdao = new DataAccess.ListItemDao();

                return listitemdao;
            }
        }

        private static IInventoryItemDao inventoryitemdao;
        public static IInventoryItemDao InventoryItemDao
        {
            get
            {
                if (inventoryitemdao == null)
                    inventoryitemdao = new DataAccess.InventoryItemDao();

                return inventoryitemdao;
            }
        }
    }
}

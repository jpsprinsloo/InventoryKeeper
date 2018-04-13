using Dapper;
using InventoryKeeper.Data.Helpers;
using InventoryKeeper.Data.Interfaces;
using InventoryKeeper.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryKeeper.Data.DataAccess
{
    public class InventoryItemDao : IInventoryItemDao
    {
        public SearchResults<InventoryItem> FindBy(string searchCriteria, int displayStart, int recordsPerPage, int orderField, string sortDir, int typeId)
        {
            int recCount = 0, totalCount = 0;

            var inParam = new DynamicParameters();

            inParam.Add("SearchCriteria", searchCriteria);
            inParam.Add("TypeId", typeId);
            inParam.Add("DisplayStart", displayStart);
            inParam.Add("PageSize", recordsPerPage);
            inParam.Add("OrderField", orderField);
            inParam.Add("SortOrder", sortDir);

            List<InventoryItem> result = SqlHelper.QuerySP<InventoryItem>("IK_Search_InventoryItems", inParam).ToList();

            if (result != null && result.Count > 0)
            {
                totalCount = result.FirstOrDefault().TotalCount;
                recCount = result.Count;
            }

            return new SearchResults<InventoryItem>
            {
                List = result,
                TotalRecords = totalCount,
                DisplayRecords = totalCount
            };
        }

        public InventoryItem GetById(int id)
        {
            InventoryItem item = SqlHelper.QuerySP<InventoryItem>("IK_GetInventoryItemById", new { Id = id }).FirstOrDefault();
            if (item == null)
                throw new NullReferenceException("Inventory item '" + id + "' not found.");

            return item;
        }

        public Response Save(InventoryItem item)
        {
            var inParam = new DynamicParameters();

            inParam.Add("TypeId", item.TypeId);
            inParam.Add("ItemDescr", item.ItemDescr);
            inParam.Add("SN", item.SN);
            inParam.Add("Photo", item.Photo ?? new byte[0]);

            if (item.Id == 0)
            {
                try
                {
                    int newId = SqlHelper.QuerySP<int>("IK_Add_InventoryItem", inParam).FirstOrDefault();

                    return new Response { Id = newId, Success = true };
                }
                catch (Exception ex)
                {
                    return new Response { Id = 0, ResultMessage = "Error Adding Item. " + ex.Message, Success = false };
                }
            }
            else
            {
                try
                {
                    inParam.Add("Id", item.Id);
                    inParam.Add("Age", item.Age);

                    int resultId = SqlHelper.QuerySP<int>("IK_Update_InventoryItem", inParam).FirstOrDefault();
                    if (resultId == 0)
                        return new Response { Id = item.Id, ResultMessage = "Item Already Changed", Success = false };
                    else
                        return new Response { Id = item.Id, Success = true };
                }
                catch (Exception ex)
                {
                    return new Response { Id = item.Id, ResultMessage = "Error Editing Item. " + ex.Message, Success = false };
                }
            }
        }

        public Response Delete(int Id)
        {
            try
            {
                var inParam = new DynamicParameters();
                inParam.Add("Id", Id);
                inParam.Add(name: "@RetVal", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                SqlHelper.ExecuteSP("IK_Delete_InventoryItem", inParam);

                int returnValue = inParam.Get<int>("@RetVal");

                switch (returnValue)
                {
                    case 0: return new Response { ResultMessage = "Item deleted successfully", Success = true };
                    case 1: return new Response { ResultMessage = "Item could not be found", Success = false };
                    case 2: return new Response { ResultMessage = "Cannot delete Item", Success = false };
                    default: throw new NotSupportedException();
                }
            }
            catch (Exception ex)
            {
                return new Response { ResultMessage = ex.Message, Success = false };
            }
        }
    }
}

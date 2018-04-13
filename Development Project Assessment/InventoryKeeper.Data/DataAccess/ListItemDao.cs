using Dapper;
using InventoryKeeper.Data.Helpers;
using InventoryKeeper.Data.Interfaces;
using InventoryKeeper.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryKeeper.Data.DataAccess
{
    public class ListItemDao : IListItemDao
    {
        public SearchResults<ListItem> FindBy(string searchCriteria, int displayStart, int recordsPerPage, int orderField, string sortDir)
        {
            int recCount = 0, totalCount = 0;

            var inParam = new DynamicParameters();

            inParam.Add("SearchCriteria", searchCriteria);
            inParam.Add("DisplayStart", displayStart);
            inParam.Add("PageSize", recordsPerPage);
            inParam.Add("OrderField", orderField);
            inParam.Add("SortOrder", sortDir);

            List<ListItem> result = SqlHelper.QuerySP<ListItem>("IK_Search_ListItems", inParam).ToList();

            if (result != null && result.Count > 0)
            {
                totalCount = result.FirstOrDefault().TotalCount;
                recCount = result.Count;
            }

            return new SearchResults<ListItem>
            {
                List = result,
                TotalRecords = totalCount,
                DisplayRecords = totalCount
            };
        }

        public ListItem GetById(int id)
        {
            ListItem listitem = SqlHelper.QuerySP<ListItem>("IK_GetListItemById", new { Id = id }).FirstOrDefault();
            if (listitem == null)
                throw new NullReferenceException("List item '" + id + "' not found.");

            return listitem;
        }

        public ListItem GetByName(string name)
        {
            ListItem listitem = SqlHelper.QuerySP<ListItem>("IK_GetListItemByName", new { Name = name }).FirstOrDefault();
            if (listitem == null)
                throw new NullReferenceException("List item '" + name + "' not found.");

            return listitem;
        }

        public List<ListItem> GetAll()
        {
            return SqlHelper.QuerySP<ListItem>("IK_GetAllListItems").ToList();
        }

        public List<ListItem> GetByTypeName(string typeName)
        {
            return SqlHelper.QuerySP<ListItem>("IK_GetListItemsByTypeName", new { TypeName = typeName }).ToList();
        }

        public Response Save(ListItem item)
        {
            var inParam = new DynamicParameters();

            inParam.Add("TypeId", item.TypeId);
            inParam.Add("ListItemName", item.ListItemName);
            inParam.Add("ListItemDescr", item.ListItemDescr);
            inParam.Add("ListItemValue", item.ListItemValue);
            inParam.Add("IsEnabled", item.IsEnabled);

            if (item.Id == 0)
            {
                try
                {
                    int newId = SqlHelper.QuerySP<int>("IK_Add_ListItem", inParam).FirstOrDefault();

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

                    int resultId = SqlHelper.QuerySP<int>("IK_Update_ListItem", inParam).FirstOrDefault();
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
                SqlHelper.ExecuteSP("IK_Delete_ListItem", inParam);

                int returnValue = inParam.Get<int>("@RetVal");

                switch (returnValue)
                {
                    case 0: return new Response { ResultMessage = "ListItem deleted successfully", Success = true };
                    case 1: return new Response { ResultMessage = "ListItem could not be found", Success = false };
                    case 2: return new Response { ResultMessage = "Cannot delete ListItem", Success = false };
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

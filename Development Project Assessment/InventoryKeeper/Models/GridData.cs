using InventoryKeeper.Data.Models;
using System;
using System.Linq;
using System.Web;

namespace InventoryKeeper.Models
{
    public class GridData
    {
        public string[][] aaData;
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }

        private GridData() { }

        internal static GridData Make<T>(HttpRequestBase Request, Func<string, int, int, int, string, SearchResults<T>> list, Func<T, string[]> arrayBuilder)
        {
            int recordsPerPage = int.Parse(Request.QueryString["iDisplayLength"] ?? Request.Form["iDisplayLength"]);
            int displayStart = int.Parse(Request.QueryString["iDisplayStart"] ?? Request.Form["iDisplayStart"]);
            int sortCol = int.Parse(Request.QueryString["iSortCol_0"] ?? Request.Form["iSortCol_0"]);
            string sortDir = Request.QueryString["sSortDir_0"] ?? Request.Form["sSortDir_0"];

            GridData data = new GridData();
            SearchResults<T> searchResults = list(Request.QueryString["sSearch"], displayStart, recordsPerPage, sortCol, sortDir);
            data.aaData = searchResults.List.Select(r => arrayBuilder(r)).ToArray();
            data.iTotalRecords = searchResults.TotalRecords;
            data.iTotalDisplayRecords = searchResults.DisplayRecords;

            return data;
        }
    }
}
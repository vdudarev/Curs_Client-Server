using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using System.Data;

namespace CursProper.Pages.Databases
{
    public class IndexModel : PageModel
    {
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        private readonly ConversionHelper conversionHelper = new ConversionHelper();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        [BindProperty(SupportsGet = true)]
        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<DatabasesDB> Databases { get; set; }
        public async Task OnGetAsync(string sortOrder,string searchString)
        {
            Count = await _sqlHelper.GetCount("Databases");
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CurrentFilter = searchString;
            DataTable dt = await _sqlHelper.GetData(CurrentPage, PageSize, "Databases", "DatabaseID");
            List<DatabasesDB> databases = await conversionHelper.MakeDatabaseDB(dt);
            if (!string.IsNullOrEmpty(searchString))
            {
                databases = databases.Where(s => s.Name_rus.Contains(searchString)).ToList();
                for(int i = 1; i <= TotalPages; i++)
                {
                    if (databases.Count == 0)
                    {
                        dt = await _sqlHelper.GetData(CurrentPage + i, PageSize, "Databases", "DatabaseID");
                        databases = await conversionHelper.MakeDatabaseDB(dt);
                        databases = databases.Where(s => s.Name_rus.Contains(searchString)).ToList();
                    }
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    databases = databases.OrderByDescending(s => s.Name_rus).ToList();
                    break;
                default:
                    databases = databases.OrderBy(s => s.Name_rus).ToList();
                    break;
            }
            Databases = databases;
        }
    }
}

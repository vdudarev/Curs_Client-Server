using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using System.Data;

namespace CursProper.Pages.LitReferences
{
    public class IndexModel : PageModel
    {
        private readonly ConversionHelper conversionHelper = new ConversionHelper();
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<LitReference> Databases { get; set; }
        public async Task OnGetAsync()
        {
            DataTable data1 = await _sqlHelper.GetData(CurrentPage, PageSize, "LitReferences", "ReferenceID");
            List<LitReference> databases = await conversionHelper.MakeLitReferences(data1);
            foreach(var data in databases)
            {
                DataTable dt = await _sqlHelper.FindByReferences("AuthorsInfo", "AuthorID", "LitReferences_AuthorsInfo", "ReferenceId", data.ReferenceId);
                List<AuthorsInfo> list = await conversionHelper.MakeAuthorsInfos(dt); ;
                data.Authors.AddRange(list);
            }
            Databases = databases;
            Count = await _sqlHelper.GetCount("LitReferences");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using System.Data;

namespace CursProper.Pages.LitReferences
{
    public class DetailsModel : PageModel
    {
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        private readonly ConversionHelper _conversionHelper = new ConversionHelper();
        public LitReference Database { get; set; }
        public List<DatabasesDB> Databases { get; set; }
        public List<AuthorsInfo> Authors { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Database = (await _conversionHelper.MakeLitReferences(await _sqlHelper.FindByIdDb(id, "LitReferences", "ReferenceID"))).First();
            if (Database == null)
            {
                return NotFound();
            }
            DataTable dt = await _sqlHelper.FindByReferences("Databases", "DatabaseID", "DB_LitReferences", "ReferenceID",Database.ReferenceId);
            Databases= await _conversionHelper.MakeDatabaseDB(dt);
            dt = await _sqlHelper.FindByReferences("AuthorsInfo", "AuthorID", "LitReferences_AuthorsInfo", "ReferenceID", Database.ReferenceId);
            Authors = await _conversionHelper.MakeAuthorsInfos(dt);
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
using System.Data;
namespace CursProper.Pages.LitReferences
{
    public class AddAuthorRefModel : DI_BasePageModel
    {
        public AddAuthorRefModel(
   ApplicationDbContext context,
   IAuthorizationService authorizationService,
   UserManager<IdentityUser> userManager)
   : base(context, authorizationService, userManager)
        {
        }

        private readonly ConversionHelper conversionHelper = new ConversionHelper();
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<AuthorsInfo> Authors { get; set; }
        public int Db_id { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, _sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            DataTable dt = await _sqlHelper.GetData(CurrentPage, PageSize, "AuthorsInfo", "AuthorID");
            List<AuthorsInfo>authors = await conversionHelper.MakeAuthorsInfos(dt);
            Authors = authors;
            Count = await _sqlHelper.GetCount("AuthorsInfo");
            Db_id = id;
            return Page();
        }
        public async Task<IActionResult> OnPostSelectItemAsync(int id, int db_id)
        {
            string url = Url.Page("Details", new { id = db_id });
            await _sqlHelper.InsertIntoCluster("LitReferences_AuthorsInfo", db_id, id, "ReferenceID", "AuthorID");
            return Redirect(url);
        }

    }
}

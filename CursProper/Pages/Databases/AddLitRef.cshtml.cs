using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using System.Data;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.Databases
{
    public class AddLitRefModel : DI_BasePageModel
    {
        public AddLitRefModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            :base(context, authorizationService, userManager) { }

        private readonly ConversionHelper conversionHelper = new ConversionHelper();
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<LitReference> Databases { get; set; }
        public int Db_id { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            DataTable data1 = await _sqlHelper.GetData(CurrentPage, PageSize, "LitReferences", "ReferenceID");
            List<LitReference> databases = await conversionHelper.MakeLitReferences(data1);
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, _sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            foreach (var data in databases)
            {
                DataTable dt = await _sqlHelper.FindByReferences("AuthorsInfo", "AuthorID", "LitReferences_AuthorsInfo", "ReferenceId", data.ReferenceId);
                List<AuthorsInfo> list = await conversionHelper.MakeAuthorsInfos(dt); ;
                data.Authors.AddRange(list);
            }
            Databases = databases;
            Count = await _sqlHelper.GetCount("LitReferences");
            Db_id = id;
            return Page();
        }
        public async Task<IActionResult> OnPostSelectItemAsync(int id,int db_id)
        {
            string url = Url.Page("Details",new {id = db_id});
            await _sqlHelper.InsertIntoCluster("DB_LitReferences", db_id, id, "DatabaseID", "ReferenceID");
            return Redirect(url);
        }
    }
}

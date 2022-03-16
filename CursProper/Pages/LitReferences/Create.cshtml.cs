using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.LitReferences
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
           ApplicationDbContext context,
           IAuthorizationService authorizationService,
           UserManager<IdentityUser> userManager)
           : base(context, authorizationService, userManager)
        {
        }
        SqlHelper sqlHelper= new SqlHelper();
        [BindProperty]
        public LitReference LitReference { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string article = string.IsNullOrEmpty(LitReference.Article) ?"":LitReference.Article;
            string source = string.IsNullOrEmpty(LitReference.Source) ? "": LitReference.Source;
            string year =  string.IsNullOrEmpty(LitReference.Year) ? "" : LitReference.Year;
            string volume =  string.IsNullOrEmpty(LitReference.Volume) ? "": LitReference.Volume;
            string number = string.IsNullOrEmpty(LitReference.Number) ? "": LitReference.Number;
            string pages = string.IsNullOrEmpty(LitReference.Pages) ? "" : LitReference.Pages;
            await sqlHelper.InsertLitReference(article, source,year,volume,number,pages);
            return RedirectToPage("./Index");
        }
    }
}

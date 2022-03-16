using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.LitReferences
{
    public class CreateAuthorModel : DI_BasePageModel
    {
        public CreateAuthorModel(
   ApplicationDbContext context,
   IAuthorizationService authorizationService,
   UserManager<IdentityUser> userManager)
   : base(context, authorizationService, userManager)
        {
        }
        SqlHelper sqlHelper = new SqlHelper();

        public async Task<IActionResult> OnGetAsync()
        {
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            return Page();
        }
        [BindProperty]
        public AuthorsInfo Authors { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await sqlHelper.InsertAuthor(Authors.AuthorName);
            return RedirectToPage("./Index");
        }

    }
}

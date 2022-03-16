using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.LitReferences
{
    public class DeleteAuthorModel : DI_BasePageModel
    {
        public DeleteAuthorModel(
          ApplicationDbContext context,
          IAuthorizationService authorizationService,
          UserManager<IdentityUser> userManager)
          : base(context, authorizationService, userManager)
        {
        }
        private readonly SqlHelper sqlHelper = new SqlHelper();
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Delete);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            Id = id;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (await sqlHelper.GetCount("LitReferences_AuthorsInfo", "AuthorID", id) != 0)
            {
                await sqlHelper.DeleteFromDb("LitReferences_AuthorsInfo", "AuthorID", id);
            }
            await sqlHelper.DeleteFromDb("AuthorsInfo", "AuthorID", id);
            return Redirect("./AddAuthorRef");
        }

    }
}

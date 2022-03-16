using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.LitReferences
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(
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
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            Id = id;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (await sqlHelper.GetCount("DB_LitReferences", "ReferenceID", id) != 0)
            {
                await sqlHelper.DeleteFromDb("DB_LitReferences", "ReferenceID", id);
            }
            if (await sqlHelper.GetCount("LitReferences_AuthorsInfo", "ReferenceID", id) != 0)
            {
                await sqlHelper.DeleteFromDb("LitReferences_AuthorsInfo", "ReferenceID", id);
            }
            await sqlHelper.DeleteFromDb("LitReferences", "ReferenceID", id);
            return Redirect("./Index");
        }

    }
}

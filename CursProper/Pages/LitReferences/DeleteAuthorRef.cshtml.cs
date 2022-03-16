using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.LitReferences
{
    public class DeleteAuthorRefModel : DI_BasePageModel
    {
        public DeleteAuthorRefModel(
   ApplicationDbContext context,
   IAuthorizationService authorizationService,
   UserManager<IdentityUser> userManager)
   : base(context, authorizationService, userManager)
        {
        }
        private readonly SqlHelper sqlHelper = new SqlHelper();
        public int Id { get; set; }
        public int Db_id { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int db_id)
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
            Db_id = db_id;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id, int db_id)
        {
            await sqlHelper.DeleteFromDb("LitReferences_AuthorsInfo", "AuthorID", id);
            string url = Url.Page("Details", new { id = db_id });
            return Redirect(url);
        }

    }
}

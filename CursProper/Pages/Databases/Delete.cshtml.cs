using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.Databases
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
            DatabasesDB databases = new DatabasesDB();
            if (id == null)
            {
                return NotFound();
            }
            Id = id;
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Delete);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if(await sqlHelper.GetCount("DB_LitReferences", "DatabaseID", id) != 0)
            {
                await sqlHelper.DeleteFromDb("DB_LitReferences", "DatabaseID", id);
            }
            if (await sqlHelper.GetCount("OrganisationsInfo_Databases", "DatabaseID", id) != 0)
            {
                await sqlHelper.DeleteFromDb("OrganisationsInfo_Databases", "DatabaseID", id);
            }
            await sqlHelper.DeleteFromDb("Databases", "DatabaseID", id);
            return Redirect("./Index");
        }
    }
}

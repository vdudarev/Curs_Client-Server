using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
using CursProper.Data;
using CursProper.Models;
namespace CursProper.Pages.Databases
{
    public class DeleteLitRefModel : DI_BasePageModel
    {
        public DeleteLitRefModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager) { }
        private readonly SqlHelper sqlHelper = new SqlHelper();
        public int Id { get; set; }
        public int Db_id { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int db_id)
        {
            DatabasesDB database = new DatabasesDB();
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }
            Id = id;
            Db_id = db_id;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id,int db_id)
        {
            await sqlHelper.DeleteFromDb("DB_LitReferences", "ReferenceID", id);
            string url = Url.Page("Details", new { id = db_id });
            return Redirect(url);
        }

    }
}

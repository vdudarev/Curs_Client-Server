using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
namespace CursProper.Pages.Databases
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
        SqlHelper sqlHelper = new SqlHelper();
        [BindProperty]
        public DatabasesDB Database { get; set; }
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
            
            string name_rus = string.IsNullOrEmpty(Database.Name_rus)?"":Database.Name_rus;
            string name_eng = string.IsNullOrEmpty(Database.Name_eng)?"": Database.Name_eng;
            string phone = string.IsNullOrEmpty(Database.Phone) ? "" : Database.Phone;
            string fax = string.IsNullOrEmpty(Database.Fax) ?"": Database.Fax;
            string email = string.IsNullOrEmpty(Database.Email) ?"": Database.Email;
            string url = string.IsNullOrEmpty(Database.URL) ? "":Database.URL;
            string comment = string.IsNullOrEmpty(Database.Comment_rus) ? "" : Database.Comment_rus;
            await sqlHelper.InsertDatabase(name_rus, name_eng, phone, fax, email, url, comment);
            return RedirectToPage("./Index");
        }
    }
}

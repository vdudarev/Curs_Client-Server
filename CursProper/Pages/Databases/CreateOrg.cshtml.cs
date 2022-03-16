using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using CursProper.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CursProper.Authorization;
using System.Data;
namespace CursProper.Pages.Databases
{
    public class AddOrgModel : DI_BasePageModel
    {
        public AddOrgModel(
   ApplicationDbContext context,
   IAuthorizationService authorizationService,
   UserManager<IdentityUser> userManager)
   : base(context, authorizationService, userManager)
        {
        } 
        SqlHelper sqlHelper = new SqlHelper();
        ConversionHelper conversionHelper = new ConversionHelper();
        [BindProperty]
        public OrganisationsInfo Org { get; set; }
        public int DB_id { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var IsAuthorized = await AuthorizationService.AuthorizeAsync(User, sqlHelper, Operations.Create);
            if (!IsAuthorized.Succeeded)
            {
                return Forbid();
            }
            DB_id = id;
            DataTable dt = new DataTable();
            dt = await sqlHelper.GetData("CountriesInfo");
            List<CountriesInfo> list = new List<CountriesInfo>();
            list = await conversionHelper.MakeCountryList(dt);
            Counties = list.Select(
                p=> new SelectListItem
                {
                    Value =  p.CountryId.ToString(),
                    Text = p.CountryNameRus
                }
                ).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int db_id)
        {
            string name_rus = string.IsNullOrEmpty(Org.NameRus) ? "" : Org.NameRus;
            string name_eng = string.IsNullOrEmpty(Org.NameEng) ? "" : Org.NameEng;
            string abbr_rus = string.IsNullOrEmpty(Org.AbbreviationRus) ? "" : Org.AbbreviationRus;
            string abbr_eng = string.IsNullOrEmpty(Org.AbbreviationEng) ? "" : Org.AbbreviationEng;
            string adr_rus = string.IsNullOrEmpty(Org.AdressRus) ? "" : Org.AdressRus;
            string adr_eng = string.IsNullOrEmpty(Org.AdressEng) ? "" : Org.AdressEng;
            int country_id = Convert.ToInt32(Request.Form["Countries"]);
            await sqlHelper.InsertOrg(name_rus, name_eng, abbr_rus,abbr_eng,adr_rus,adr_eng, db_id,country_id);
            string url = Url.Page("Details", new { id = db_id });
            return Redirect(url);
        }
    }
}

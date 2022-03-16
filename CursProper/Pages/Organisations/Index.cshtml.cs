using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using CursProper.Models;
namespace CursProper.Pages.Organisations
{
    public class IndexModel : PageModel
    {
        private readonly ConversionHelper conversionHelper = new ConversionHelper();
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<OrganisationsInfo> Databases { get; set; }
        public async Task OnGetAsync()
        {
            DataTable dt = await _sqlHelper.GetData(CurrentPage, PageSize, "OrganisationsInfo", "OrganisationID");
            List<OrganisationsInfo> database = await conversionHelper.MakeOrgInfo(dt);
            foreach (var item in database)
            {
                if (item.CountryId != null)
                {
                    DataTable table = await _sqlHelper.FindByIdDb(item.CountryId, "CountriesInfo", "CountryID");
                    CountriesInfo country = await conversionHelper.MakeCountry(table);
                    item.Country = country;
                }
                else
                {
                    item.Country = new CountriesInfo { CountryId = 0, CountryNameEng = "", CountryNameRus = "" };
                }
            }
            Databases = database;
            Count = await _sqlHelper.GetCount("OrganisationsInfo");
        }
    }
}

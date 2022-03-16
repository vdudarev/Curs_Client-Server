using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CursProper.Models;
using System.Data;

namespace CursProper.Pages.Databases
{
    public class DetailsModel : PageModel
    {
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        private readonly ConversionHelper _conversionHelper = new ConversionHelper();
        public DatabasesDB Database { get; set; }
        public List<LitReference> References { get; set; }
        public List<OrganisationsInfo> Organisations { get; set;}
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Database = (await _conversionHelper.MakeDatabaseDB(await _sqlHelper.FindByIdDb(id, "Databases", "DatabaseID"))).First();
            if (Database == null)
            {
                return NotFound();
            }
            DataTable dataTable = await _sqlHelper.FindByReferences("LitReferences", "ReferenceID", "DB_LitReferences", "DatabaseID",Database.Id);
            List<LitReference> reference = await _conversionHelper.MakeLitReferences(dataTable);
            foreach (var referenceItem in reference)
            {
                DataTable dt = await _sqlHelper.FindByReferences("AuthorsInfo", "AuthorID", "LitReferences_AuthorsInfo", "ReferenceId", referenceItem.ReferenceId);
                List<AuthorsInfo> list = await _conversionHelper.MakeAuthorsInfos(dt); ;
                referenceItem.Authors.AddRange(list);
            }
            References = reference;
            dataTable = await _sqlHelper.FindByReferences("OrganisationsInfo", "OrganisationID", "OrganisationsInfo_Databases", "DatabaseID", Database.Id);
            List<OrganisationsInfo> orgs= await _conversionHelper.MakeOrgInfo(dataTable);
            foreach(var orgsItem in orgs)
            {
                DataTable table = await _sqlHelper.FindByIdDb(orgsItem.CountryId, "CountriesInfo", "CountryID");
                CountriesInfo country = await _conversionHelper.MakeCountry(table);
                orgsItem.Country = country;
                table = await _sqlHelper.FindByIdDb(Database.Id, "OrganisationsInfo_Databases", "DatabaseID");
                foreach(DataRow dr in table.Rows)
                {
                    orgsItem.AdressRus = dr[2].ToString();
                    orgsItem.AdressEng = dr[3].ToString();
                }
            }
            Organisations = orgs;
            return Page();
        }
    }
}

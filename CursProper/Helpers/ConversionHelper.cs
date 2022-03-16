using CursProper.Models;
using System.Data;

namespace CursProper
{
    public class ConversionHelper
    {
        public async Task<List<DatabasesDB>> MakeDatabaseDB(DataTable dt)
        {
            List<DatabasesDB> databases = new List<DatabasesDB>();
            foreach (DataRow dr in dt.Rows)
            {
                databases.Add(new DatabasesDB
                {
                    Name_rus = dr[1].ToString(),
                    Phone = dr[6].ToString(),
                    Email = dr[7].ToString(),
                    Fax = dr[7].ToString(),
                    Comment_rus = dr[9].ToString(),
                    URL = dr[8].ToString(),
                    Id = Convert.ToInt32(dr[0]),
                });
            }
            return databases;
        }
        public async Task<List<LitReference>> MakeLitReferences(DataTable dt)
        {
            List<LitReference> databases = new List<LitReference>();
            foreach (DataRow dr in dt.Rows)
            {
                databases.Add(new LitReference
                {
                    ReferenceId = Convert.ToInt32(dr[0]),
                    Article = dr[1].ToString(),
                    Source = dr[2].ToString(),
                    Year = dr[3].ToString(),
                    Volume = dr[4].ToString(),
                    Number = dr[5].ToString(),
                    Pages = dr[6].ToString(),
                    Authors = new List<AuthorsInfo>()
                }) ;
            }
            return databases;
        }
        public async Task<List<AuthorsInfo>> MakeAuthorsInfos(DataTable dt)
        {
            List<AuthorsInfo> authors = new List<AuthorsInfo>();
            foreach(DataRow dr in dt.Rows)
            {
                authors.Add(new AuthorsInfo
                {
                    AuthorId = Convert.ToInt32(dr[0]),
                    AuthorName = dr[1].ToString()
                });
            }
            return authors;
        }
        public async Task<List<OrganisationsInfo>> MakeOrgInfo(DataTable dt)
        {
            List<OrganisationsInfo> orgs = new List<OrganisationsInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1] is DBNull)
                {
                    orgs.Add(new OrganisationsInfo
                    {
                        OrganisationId = Convert.ToInt32(dr[0]),
                        CountryId = null,
                        NameRus = dr[2].ToString(),
                        NameEng = dr[3].ToString(),
                        AbbreviationRus = dr[4].ToString(),
                        AbbreviationEng = dr[5].ToString()
                    });
                }
                else
                {
                    orgs.Add(new OrganisationsInfo
                    {                     
                        OrganisationId = Convert.ToInt32(dr[0]),
                        CountryId = Convert.ToInt32(dr[1]),
                        NameRus = dr[2].ToString(),
                        NameEng = dr[3].ToString(),
                        AbbreviationRus = dr[4].ToString(),
                        AbbreviationEng = dr[5].ToString()
                    }); ;
                }
              /*  orgs.Add(new OrganisationsInfo
                {
                    OrganisationId = Convert.ToInt32(dr[0]),
                    CountryId = Convert.ToInt32(dr[1]),
                    NameRus = dr[2].ToString(),
                    NameEng = dr[3].ToString(),
                    AbbreviationRus = dr[4].ToString(),
                    AbbreviationEng = dr[5].ToString()
                }) ;*/
            }
            return orgs;
        }
        public async Task<CountriesInfo> MakeCountry(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            CountriesInfo country = new CountriesInfo
            {
                CountryId = Convert.ToInt32(dr[0]),
                CountryNameRus = dr[1].ToString(),
                CountryNameEng = dr[2].ToString()
            };
            return country;
        }
        public async Task<List<CountriesInfo>> MakeCountryList(DataTable dt)
        {
            List<CountriesInfo> countries = new List<CountriesInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                countries.Add(new CountriesInfo
                {
                    CountryId = Convert.ToInt32(dr[0]),
                    CountryNameRus = dr[1].ToString(),
                    CountryNameEng = dr[2].ToString()
                });
            }
            return countries;
        }
    }

}

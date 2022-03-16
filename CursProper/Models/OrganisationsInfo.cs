namespace CursProper.Models
{
    public class OrganisationsInfo
    {
        public int OrganisationId { get; set; }
        public int? CountryId { get; set; }
        public string NameRus { get; set; } = null!;
        public string NameEng { get; set; } = null!;
        public string AbbreviationRus { get; set; } = null!;
        public string AbbreviationEng { get; set; } = null!;
        public CountriesInfo Country { get; set; }
        public string AdressRus { get; set; }
        public string AdressEng { get; set; }
    }
}

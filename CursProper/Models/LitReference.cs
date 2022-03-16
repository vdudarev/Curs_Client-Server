namespace CursProper.Models
{
    public class LitReference
    {
        public int ReferenceId { get; set; }
        public string Article { get; set; } = null!;
        public string Source { get; set; } = null!;
        public string Year { get; set; }
        public string Volume { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string Pages { get; set; } = null!;
        public List<AuthorsInfo> Authors { get; set; }
    }
}

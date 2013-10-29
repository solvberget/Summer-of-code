namespace Solvberget.Domain.DTO
{
    public sealed class BokElskereBook
    {
        // Same property names as XML-doc
        public string Id { get; set; }
        public string gjennomsnittelig_terningkast { get; set; }
        public string antall_sitater { get; set; }
        public string favorittprosent { get; set; }
        public string antall_favoritt { get; set; }
        public string link { get; set; }
        public string antall_diskusjoner { get; set; }
        public string antall_eiere { get; set; }
        public string antall_lesere { get; set; }
        public string antall_lister { get; set; }
        public string antall_terningkast { get; set; }
    }
}

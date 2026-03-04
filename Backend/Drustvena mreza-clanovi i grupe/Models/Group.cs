namespace Drustvena_mreza_clanovi_i_grupe.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime DatumOsnivanja { get; set; }
        public List<User> Korisnici { get; set; } = new List<User>();

        public Group(int id, string ime, DateTime datumOsnivanja)
        {
            Id = id;
            Ime = ime;
            DatumOsnivanja = datumOsnivanja;
        }

        public Group() { }
    }
}

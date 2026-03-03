namespace Drustvena_mreza_clanovi_i_grupe.Models
{
    public class User
    {

        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }


        public User(string korisnickoIme, string ime, string prezime, DateTime datumRodjenja)
        {
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
        }

        public User() { }

    }
}

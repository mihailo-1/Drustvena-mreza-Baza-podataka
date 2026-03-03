using Drustvena_mreza_clanovi_i_grupe.Models;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class UserRepository
    {
        private const string putanja = "data/korisnici.csv";
        public static Dictionary<int, User> Data;

        public UserRepository()
        {
            if(Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, User>();
            string[] lines = File.ReadAllLines(putanja);
            foreach(string line in lines)
            {
                string[] atributi = line.Split(',');
                int id = int.Parse(atributi[0]);
                string korisnickoIme = atributi[1];
                string ime = atributi[2];
                string prezime = atributi[3];
                DateTime datumRodjenja = DateTime.ParseExact(atributi[4].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                User user = new User(korisnickoIme, ime, prezime, datumRodjenja);
                user.Id = id;

                Data[id] = user;
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach(User u in  Data.Values)
            {
                string datumRodjenja = u.DatumRodjenja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                lines.Add($"{u.Id},{u.KorisnickoIme},{u.Ime},{u.Prezime},{datumRodjenja}");
            }

            File.WriteAllLines(putanja, lines);
        }
    }
}

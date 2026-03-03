using Drustvena_mreza_clanovi_i_grupe.Models;
using System.Text.RegularExpressions;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class GroupUsersRepository
    {
        private const string putanja = "data/clanstva.csv";
        public static Dictionary<int, List<int>> Data;

        public GroupUsersRepository()
        {
            if(Data == null)
            {
                Load();
            }
        }

        public static void Load()
        {
            Data = new Dictionary<int, List<int>>();

            string[] lines = File.ReadAllLines(putanja);

            foreach (string line in lines)
            {
                string[] atributi = line.Split(',');

                int userId = int.Parse(atributi[0]);
                int groupId = int.Parse (atributi[1]);

                if (!Data.ContainsKey(groupId))
                {
                    Data[groupId] = new List<int>();
                }
                Data[groupId].Add(userId);
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Drustvena_mreza_clanovi_i_grupe.Models.Group group in GroupRepository.Data.Values)
            {
                foreach(User user in group.Korisnici)
                {
                    lines.Add($"{user.Id},{group.Id}");
                }
            }
            File.WriteAllLines(putanja, lines);
        }

    }
}

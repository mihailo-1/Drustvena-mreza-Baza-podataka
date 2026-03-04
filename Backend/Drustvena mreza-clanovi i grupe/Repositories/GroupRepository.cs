using Drustvena_mreza_clanovi_i_grupe.Models;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class GroupRepository
    {
        private const string filePath = "data/grupe.csv";
        public static Dictionary<int, Group> Data;

        public GroupRepository()
        {
            if (Data == null)
            {

                _ = new UserRepository();           
                _ = new GroupUsersRepository();     
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Group>();
            if (!File.Exists(filePath)) return;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string ime = attributes[1];
                DateTime datum = DateTime.Parse(attributes[2]);
                Data[id] = new Group(id, ime, datum);

                Group group = new Group(id, ime, datum);
                Data[id] = group;

                if (GroupUsersRepository.Data.ContainsKey(id))
                {
                    List<int> userIds = GroupUsersRepository.Data[id];

                    foreach(int userId in userIds)
                    {
                        User user = UserRepository.Data[userId];
                        group.Korisnici.Add(user);
                    }
                }
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Group g in Data.Values)
            {
                lines.Add($"{g.Id},{g.Ime},{g.DatumOsnivanja:yyyy-MM-dd}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}

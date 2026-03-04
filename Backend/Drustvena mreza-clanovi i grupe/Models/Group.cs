namespace Drustvena_mreza_clanovi_i_grupe.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<User> Korisnici { get; set; } = new List<User>();

        public Group(int id, string name, DateTime creationDate)
        {
            Id = id;
            Name = name;
            CreationDate = creationDate;
        }

        public Group() { }
    }
}

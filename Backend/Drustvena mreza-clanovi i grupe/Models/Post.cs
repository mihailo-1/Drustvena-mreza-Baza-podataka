namespace Drustvena_mreza_clanovi_i_grupe.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

        public User? Author { get; set; }
    }
}

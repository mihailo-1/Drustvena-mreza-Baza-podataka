using Microsoft.Data.Sqlite;
using Drustvena_mreza_clanovi_i_grupe.Models;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class PostDbRepository
    {
        private readonly string connectionString = "Data Source=data/mydatabase.db";

        public List<Post> GetAll()
        {
            var posts = new List<Post>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    SELECT 
                        p.Id      AS PostId, 
                        p.Content AS PostContent, 
                        p.Date    AS PostDate, 
                        p.UserId  AS AuthorId,
                        u.Username AS KorisnickoIme,
                        u.Name     AS Ime,
                        u.Surname  AS Prezime
                    FROM Posts p
                    INNER JOIN Users u ON p.UserId = u.Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var post = new Post
                    {
                        Id = Convert.ToInt32(reader["PostId"]),
                        Content = reader["PostContent"].ToString(),
                        Date = reader["PostDate"].ToString(),
                        UserId = Convert.ToInt32(reader["AuthorId"]),

                        Author = new User //autor u objavu
                        {
                            Id = Convert.ToInt32(reader["AuthorId"]),
                            KorisnickoIme = reader["KorisnickoIme"].ToString(),
                            Ime = reader["Ime"].ToString(),
                            Prezime = reader["Prezime"].ToString()
                        }
                    };

                    posts.Add(post);
                }

                return posts;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Baza greška (Posts): {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Opšta greška (Posts): {ex.Message}");
                throw;
            }
        }

        public void Create(Post post)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = "INSERT INTO Posts (UserId, Content, Date) VALUES (@userId, @content, @date)";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@userId", post.UserId);
            command.Parameters.AddWithValue("@content", post.Content);
            command.Parameters.AddWithValue("@date", post.Date);

            command.ExecuteNonQuery();
        }
    }
}

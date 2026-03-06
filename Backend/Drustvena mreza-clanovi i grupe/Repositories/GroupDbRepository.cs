using Drustvena_mreza_clanovi_i_grupe.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class GroupDbRepository
    {
        private readonly string connectionString;
        //= "Data Source=data/mydatabase.db";

        public GroupDbRepository(IConfiguration configuration)
        {
            
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Group> GetAll(int page, int pageSize)
        {
            List<Group> groups = new List<Group>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups LIMIT @Limit OFFSET @Offset";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Limit", pageSize);
                command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    groups.Add(new Group
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Ime = reader["Name"].ToString(),
                        DatumOsnivanja = DateTime.Parse(reader["CreationDate"].ToString())
                    });
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri povezivanju sa bazom ili izvršavanju SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u formatu podataka: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Greška jer konekcija nije ili je više puta otvorena: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
            return groups;
        }


        public Group GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "SELECT Id, Name, CreationDate FROM Groups WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Group
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Ime = reader["Name"].ToString(),
                        DatumOsnivanja = DateTime.Parse(reader["CreationDate"].ToString())
                    };
                }
                return null;
            }
            
            catch (SqliteException ex) 
            {
                Console.WriteLine($"Baza greška: {ex.Message}");
                throw; 
            }
            catch (InvalidOperationException ex) {
                Console.WriteLine($"Konekcija greška: {ex.Message}");
                throw; 
            }
            catch (Exception ex) {
                Console.WriteLine($"Opšta greška: {ex.Message}");
                throw; 
            }
            
        }

        public int Add(Group group)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Groups (Name, CreationDate) VALUES (@Name, @Date); SELECT last_insert_rowid();";
                command.Parameters.AddWithValue("@Name", group.Ime);
                command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int noviId = Convert.ToInt32(command.ExecuteScalar());
                return noviId;
            }
            catch (SqliteException ex) {
                Console.WriteLine($"SQL Insert greška: {ex.Message}");
                throw; 
            }
            catch (Exception ex) {
                Console.WriteLine($"Neočekivana greška pri dodavanju: {ex.Message}");
                throw; 
            }
        }

        public void Update(Group group)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Groups SET Name = @Name WHERE Id = @Id";
                command.Parameters.AddWithValue("@Name", group.Ime);
                command.Parameters.AddWithValue("@Id", group.Id);
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQL Update greška: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška pri dodavanju: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Groups WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQL Delete greška: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška pri dodavanju: {ex.Message}");
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "SELECT COUNT(*) FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

                return Convert.ToInt32(command.ExecuteScalar()); 
            }
            catch (Exception) { throw; }
        }
    }
}
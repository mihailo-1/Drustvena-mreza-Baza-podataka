using Microsoft.Data.Sqlite;
using Drustvena_mreza_clanovi_i_grupe.Models;
using System;
using System.Collections.Generic;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class GroupDbRepository
    {
        private readonly string connectionString = "Data Source=data/mydatabase.db";

        public List<Group> GetAll()
        {
            List<Group> groups = new List<Group>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "SELECT Id, Name, CreationDate FROM Groups";

                using SqliteCommand command = new SqliteCommand(query, connection);
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
            catch (Exception ex)
            {
                Console.WriteLine($"Greška u GetAll: {ex.Message}");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška u GetById: {ex.Message}");
            }
            return null;
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
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri dodavanju: " + ex.Message); 
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
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri izmeni: " + ex.Message); 
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
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri brisanju: " + ex.Message);
                throw;
            }
        }
    }
}
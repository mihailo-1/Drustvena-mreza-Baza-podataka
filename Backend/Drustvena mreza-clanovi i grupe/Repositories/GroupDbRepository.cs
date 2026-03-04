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
    }
}
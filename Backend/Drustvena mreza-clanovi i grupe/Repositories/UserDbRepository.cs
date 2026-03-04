using Drustvena_mreza_clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class UserDbRepository
    {
        public List<User> GetAllFromDataBase()
        {
            List<User> result = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=data/mydatabase.db");
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";

                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        KorisnickoIme = reader["Username"].ToString(),
                        Ime = reader["Name"].ToString(),
                        Prezime = reader["Surname"].ToString(),
                        DatumRodjenja = DateTime.ParseExact(
                            reader["Birthday"].ToString(),
                            "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture)
                    };
                    result.Add(user);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška sa bazom: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u formatu datuma: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Problem sa konekcijom: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return result;
        }

        public User GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=data/mydatabase.db");
                connection.Open();

                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users WHERE Id = @id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        KorisnickoIme = reader["Username"].ToString(),
                        Ime = reader["Name"].ToString(),
                        Prezime = reader["Surname"].ToString(),
                        DatumRodjenja = DateTime.ParseExact(
                                            reader["Birthday"].ToString(),
                                            "yyyy-MM-dd",
                                            System.Globalization.CultureInfo.InvariantCulture)
                    };

                    return user;
                }

                return null;  
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška sa bazom: {ex.Message}");
                return null;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u formatu datuma: {ex.Message}");
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Problem sa konekcijom: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return null;
            }
        }
    }
}

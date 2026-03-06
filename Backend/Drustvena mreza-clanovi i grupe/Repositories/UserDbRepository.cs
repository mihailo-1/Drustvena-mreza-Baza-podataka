using Drustvena_mreza_clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class UserDbRepository
    {

        public readonly string connectionString;

        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<User> GetAllFromDataBase()
        {
            List<User> result = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
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
                throw new Exception($"Greska sa bazom: {ex.Message}");
            }
            catch (FormatException ex)
            {
                throw new Exception($"Greska u formatu datuma: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Problem sa konekcijom: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Neocekivana greška: {ex.Message}");
            }

            return result;
        }

        public User GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
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
                throw new Exception($"Greska sa bazom: {ex.Message}");
            }
            catch (FormatException ex)
            {
                throw new Exception($"Greska u formatu datuma: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Problem sa konekcijom: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Neocekivana greska: {ex.Message}");
            }
        }

        public User Create(User newUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    INSERT INTO Users (Username, Name, Surname, Birthday)
                    VALUES (@Username, @Name, @Surname, @Birthday);
                    SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", newUser.KorisnickoIme);
                command.Parameters.AddWithValue("@Name", newUser.Ime);
                command.Parameters.AddWithValue("@Surname", newUser.Prezime);
                command.Parameters.AddWithValue("@Birthday", newUser.DatumRodjenja.ToString("yyyy-MM-dd"));

                int newId = Convert.ToInt32(command.ExecuteScalar());

                newUser.Id = newId;
                return newUser;
            }
            catch (SqliteException ex)
            {
                throw new Exception("Greska pri dodavanju korisnika u bazu", ex);
            }
            catch (FormatException ex)
            {
                throw new Exception("Neispravan format datuma rođenja", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Neocekivana greška pri kreiranju korisnika", ex);
            }
        }

        public void Update(User updatedUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    UPDATE Users 
                    SET Username = @Username, 
                        Name     = @Name, 
                        Surname  = @Surname, 
                        Birthday = @Birthday 
                     WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", updatedUser.KorisnickoIme);
                command.Parameters.AddWithValue("@Name", updatedUser.Ime);
                command.Parameters.AddWithValue("@Surname", updatedUser.Prezime);
                command.Parameters.AddWithValue("@Birthday", updatedUser.DatumRodjenja.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Id", updatedUser.Id);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    throw new Exception($"Korisnik sa Id = {updatedUser.Id} nije pronađen");
                }
            }
            catch (SqliteException ex)
            {
                throw new Exception("Greska pri azuriranju korisnika u bazi", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Neocekivana greška pri azuriranju korisnika", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    throw new Exception($"Korisnik sa Id = {id} nije pronadjen");
                }
            }
            catch (SqliteException ex)
            {
                throw new Exception("Greska pri brisanju korisnika iz baze", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Neocekivana greska pri brisanju korisnika", ex);
            }
        }
    }
}

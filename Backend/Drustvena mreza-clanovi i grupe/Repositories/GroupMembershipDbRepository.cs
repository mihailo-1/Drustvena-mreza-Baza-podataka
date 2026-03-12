using Drustvena_mreza_clanovi_i_grupe.Models;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace Drustvena_mreza_clanovi_i_grupe.Repositories
{
    public class GroupMembershipDbRepository
    {
        private readonly string connectionString;

        public GroupMembershipDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public bool AddUserToGroup(int userId, int groupId)
        {
            try
            {

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    INSERT INTO GroupMemberships (UserId, GroupId)
                    VALUES (@UserId, @GroupId);";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch (SqliteException ex)
            {
                throw new Exception($"Greska sa bazom: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Problem sa konekcijom: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Neocekivana greška: {ex.Message}");
            }
        }

        public bool RemoveUserFromGroup(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    DELETE FROM GroupMemberships 
                    WHERE UserId = @UserId AND GroupId = @GroupId;";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri uklanjanju korisnika iz grupe: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Opsta greska: {ex.Message}");
                return false;
            }
        }
    }
}

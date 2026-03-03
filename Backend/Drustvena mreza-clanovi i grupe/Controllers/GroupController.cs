using Drustvena_mreza_clanovi_i_grupe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private string connectionString = "Data Source=data/mydatabase.db";

        [HttpGet]
        public ActionResult<List<Group>> GetAll()
        {
            try
            {
                List<Group> groups = GetAllFromDatabase();
                return Ok(groups);
            }
            catch (SqliteException ex)
            {
                return StatusCode(500, $"Greška baze: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        private List<Group> GetAllFromDatabase()
        {
            List<Group> groups = new List<Group>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Ime, DatumOsnivanja FROM Groups";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            groups.Add(new Group
                            {
                                Id = reader.GetInt32(0),
                                Ime = reader.GetString(1),
                                DatumOsnivanja = DateTime.Parse(reader.GetString(2))
                            });
                        }
                    }
                }
            }
            return groups;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Groups WHERE Id = @id";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0) return NotFound();
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
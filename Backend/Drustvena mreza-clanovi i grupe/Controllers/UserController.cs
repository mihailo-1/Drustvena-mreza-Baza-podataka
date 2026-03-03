using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            try
            {
                var users = GetAllFromDataBase();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Greška prilikom čitanja korisnika: " + ex.Message);
            }
        }

        //[HttpGet("{id}")]
        //public ActionResult<User> GetById(int id)
        //{
        //    if (!UserRepository.Data.ContainsKey(id))
        //    {
        //        return NotFound();
        //    }
        //    return Ok(UserRepository.Data[id]);
        //}

        //[HttpPost]
        //public ActionResult<User> Create([FromBody] User newUser)
        //{
        //    if (string.IsNullOrWhiteSpace(newUser.KorisnickoIme) ||
        //        string.IsNullOrWhiteSpace(newUser.Ime) ||
        //        string.IsNullOrWhiteSpace(newUser.Prezime))
        //    {
        //        return BadRequest();
        //    }

        //    newUser.Id = SracunajNoviId(UserRepository.Data.Keys.ToList());

        //    UserRepository.Data[newUser.Id] = newUser;

        //    repo.Save();
        //    return Ok(newUser);
        //}

        //[HttpPut("{id}")]
        //public ActionResult<User> Update(int id, [FromBody] User uUser)
        //{
        //    if (string.IsNullOrWhiteSpace(uUser.KorisnickoIme) ||
        //        string.IsNullOrWhiteSpace(uUser.Ime) ||
        //        string.IsNullOrWhiteSpace(uUser.Prezime))
        //    {
        //        return BadRequest();
        //    }

        //    if (!UserRepository.Data.ContainsKey(id))
        //    {
        //        return NotFound();
        //    }

        //    User user = UserRepository.Data[id];
        //    user.KorisnickoIme = uUser.KorisnickoIme;
        //    user.Ime = uUser.Ime;
        //    user.Prezime = uUser.Prezime;
        //    user.DatumRodjenja = uUser.DatumRodjenja;

        //    repo.Save();

        //    return Ok(user);
        //}

        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
        //    if(!UserRepository.Data.ContainsKey(id))
        //    {
        //        return NotFound();
        //    }

        //    UserRepository.Data.Remove(id);
        //    repo.Save();

        //    return NoContent();
        //}


        //private int SracunajNoviId(List<int> identifikatori)
        //{
        //    int maxId = 0;
        //    foreach (int id in identifikatori)
        //    {
        //        if (id > maxId)
        //        {
        //            maxId = id;
        //        }
        //    }
        //    return maxId + 1;
        //}

        private List<User> GetAllFromDataBase()
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
    }
}

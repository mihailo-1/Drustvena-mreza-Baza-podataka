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
            UserDbRepository repo = new UserDbRepository();
            try
            {
                var users = repo.GetAllFromDataBase();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Greška prilikom čitanja korisnika: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            UserDbRepository repo = new UserDbRepository();
            User? user = repo.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.KorisnickoIme) ||
                string.IsNullOrWhiteSpace(newUser.Ime) ||
                string.IsNullOrWhiteSpace(newUser.Prezime))
            {
                return BadRequest();
            }

            try
            {
                var repo = new UserDbRepository();
                var kreirani = repo.Create(newUser);

                return Ok(kreirani);
            }
            catch (Exception ex)
            {
                return BadRequest("Neuspesno dodavanje korisnika: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            if (string.IsNullOrWhiteSpace(uUser.KorisnickoIme) ||
                string.IsNullOrWhiteSpace(uUser.Ime) ||
                string.IsNullOrWhiteSpace(uUser.Prezime))
            {
                return BadRequest();
            }

            try
            {
                UserDbRepository repo = new UserDbRepository();

                uUser.Id = id;
                repo.Update(uUser);

                return Ok(uUser);
            }
            catch(Exception ex)
            {
                return BadRequest("Greska pri azuriranju korisnika" + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                UserDbRepository repo = new UserDbRepository();
                repo.Delete(id);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest("Greska pri brisanju korisnika" + ex.Message);
            }
        }
    }
}

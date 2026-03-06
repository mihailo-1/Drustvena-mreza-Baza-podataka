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
        private UserDbRepository userDbRepository;

        public UserController(IConfiguration configuration)
        {
            userDbRepository = new UserDbRepository(configuration);
        }


        [HttpGet]
        public ActionResult GetPaged([FromQuery]int page = 1,[FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page i pageSize moraju biti veci od 0.");
            }

            try
            {
                List<User> users = userDbRepository.GetPaged(page, pageSize);
                int totalCount = userDbRepository.CountAll();
                Object result = new
                {
                    Data = users,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Greška prilikom čitanja korisnika: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            User? user = userDbRepository.GetById(id);

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
                var kreirani = userDbRepository.Create(newUser);

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
                uUser.Id = id;
                userDbRepository.Update(uUser);

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
                userDbRepository.Delete(id);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest("Greska pri brisanju korisnika" + ex.Message);
            }
        }
    }
}

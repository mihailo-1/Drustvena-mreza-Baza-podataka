using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepository repo = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            List<User> users = UserRepository.Data.Values.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(UserRepository.Data[id]);
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

            newUser.Id = SracunajNoviId(UserRepository.Data.Keys.ToList());

            UserRepository.Data[newUser.Id] = newUser;

            repo.Save();
            return Ok(newUser);
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

            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            User user = UserRepository.Data[id];
            user.KorisnickoIme = uUser.KorisnickoIme;
            user.Ime = uUser.Ime;
            user.Prezime = uUser.Prezime;
            user.DatumRodjenja = uUser.DatumRodjenja;

            repo.Save();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if(!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            UserRepository.Data.Remove(id);
            repo.Save();

            return NoContent();
        }


        private int SracunajNoviId(List<int> identifikatori)
        {
            int maxId = 0;
            foreach (int id in identifikatori)
            {
                if (id > maxId)
                {
                    maxId = id;
                }
            }
            return maxId + 1;
        }
    }
}

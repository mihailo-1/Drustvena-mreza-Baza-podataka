using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupDbRepository _repository;

        public GroupController()
        {
            _repository = new GroupDbRepository();
        }

        [HttpGet]
        public ActionResult<List<Group>> GetAll()
        {
            try
            {
                List<Group> groups = _repository.GetAll();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Group> GetById(int id)
        {
            try
            {
                Group group = _repository.GetById(id);
                if (group == null)
                {
                    return NotFound(); 
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Group group)
        {
            if (group == null) return BadRequest();
            int noviId = _repository.Add(group);
            group.Id = noviId;
            return CreatedAtAction(nameof(GetById), new { id = noviId }, group);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Group group)
        {
            var postojeca = _repository.GetById(id);
            if (postojeca == null) return NotFound(); 

            group.Id = id;
            _repository.Update(group);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var postojeca = _repository.GetById(id);
            if (postojeca == null) return NotFound();

            _repository.Delete(id);
            return NoContent();
        }

    }
}
using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupDbRepository _repository;

        public GroupController(IConfiguration configuration)
        {
            _repository = new GroupDbRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var groups = _repository.GetAll(page, pageSize);
                var totalCount = _repository.CountAll();

                return Ok(new
                {
                    Data = groups,
                    TotalCount = totalCount
                });
            }
            catch (Exception)
            {
                return Problem("Greška pri dohvatanju stranice sa grupama.");
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
            try {
                int noviId = _repository.Add(group);
                group.Id = noviId;
                return CreatedAtAction(nameof(GetById), new { id = noviId }, group);
            }

            catch (Exception ex) {
                return Problem("Greška pri kreiranju grupe."); 
            }

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Group group)
        {
            try
            {
                var postojeca = _repository.GetById(id);
                if (postojeca == null) return NotFound();

                group.Id = id;
                _repository.Update(group);
                return NoContent();
            }
            catch (Exception)
            {
                return Problem("Greška pri ažuriranju grupe."); 
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var postojeca = _repository.GetById(id);
                if (postojeca == null) return NotFound();

                _repository.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return Problem("Greška pri brisanju grupe."); 
            }
        }

    }
}
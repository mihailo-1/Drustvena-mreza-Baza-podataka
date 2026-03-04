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

    }
}
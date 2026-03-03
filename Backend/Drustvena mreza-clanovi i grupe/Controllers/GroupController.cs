using Microsoft.AspNetCore.Mvc;
using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private GroupRepository groupRepository = new GroupRepository();

        [HttpGet]
        public ActionResult<List<Group>> GetAll()
        {
            return Ok(GroupRepository.Data.Values.ToList());
        }

        [HttpPost]
        public ActionResult<Group> Create([FromBody] Group newGroup)
        {
            if (string.IsNullOrWhiteSpace(newGroup.Ime))
            {
                return BadRequest("Ime grupe je obavezno.");
            }

            int newId = GroupRepository.Data.Keys.Count > 0 ? GroupRepository.Data.Keys.Max() + 1 : 1;
            newGroup.Id = newId;
            newGroup.DatumOsnivanja = DateTime.Now;

            GroupRepository.Data[newId] = newGroup;
            groupRepository.Save();

            return Ok(newGroup);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!GroupRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            GroupRepository.Data.Remove(id);
            groupRepository.Save();

            return NoContent();
        }
    }
}
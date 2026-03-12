using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupMembershipController : ControllerBase
    {
        private readonly GroupMembershipDbRepository _membershipRepo;

        public GroupMembershipController(GroupMembershipDbRepository membershipRepo)
        {
            _membershipRepo = membershipRepo;
        }

        [HttpPut("{userId}")]
        public ActionResult AddUserToGroup(int groupId, int userId)
        {
            try
            {
                bool uspesno = _membershipRepo.AddUserToGroup(userId, groupId);

                if (uspesno)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Neuspesno dodavanje korisnika u grupu.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska: {ex.Message}");
            }
        }

        [HttpDelete("{userId}")]
        public ActionResult RemoveUserFromGroup(int groupId, int userId)
        {
            try
            {
                bool uspešno = _membershipRepo.RemoveUserFromGroup(userId, groupId);

                if (uspešno)
                {
                    return NoContent();  
                }
                else
                {
                    return NotFound("Korisnik nije clan ove grupe ili nije uspesno uklonjen.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greska pri uklanjanju: {ex.Message}");
            }
        }
    }
}

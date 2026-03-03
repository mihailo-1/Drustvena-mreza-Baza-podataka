using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupUsersController : ControllerBase
    {
        private UserRepository userRepository = new UserRepository();
        private GroupRepository groupRepository = new GroupRepository();
        private GroupUsersRepository groupUsersRepository = new GroupUsersRepository();

        [HttpGet]
        public ActionResult<List<User>> GetUsersByGroup(int groupId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound();
            }

            Group group = GroupRepository.Data[groupId];
            return Ok(group.Korisnici);
        }

        // B.2
        [HttpPut("{userId}")]
        public ActionResult AddUserToGroup(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId) || !UserRepository.Data.ContainsKey(userId))
            {
                return NotFound("Grupa ili korisnik ne postoje.");
            }

            var group = GroupRepository.Data[groupId];
            var user = UserRepository.Data[userId];

            if (!group.Korisnici.Any(u => u.Id == userId))
            {
                group.Korisnici.Add(user);

                if (!GroupUsersRepository.Data.ContainsKey(groupId))
                    GroupUsersRepository.Data[groupId] = new List<int>();

                GroupUsersRepository.Data[groupId].Add(userId);

                groupUsersRepository.Save();
            }

            return Ok();
        }

        // B.2
        [HttpDelete("{userId}")]
        public ActionResult RemoveUserFromGroup(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound("Grupa ne postoji.");
            }

            var group = GroupRepository.Data[groupId];
            var userForRemoval = group.Korisnici.FirstOrDefault(u => u.Id == userId);

            if (userForRemoval != null)
            {
                group.Korisnici.Remove(userForRemoval);

                if (GroupUsersRepository.Data.ContainsKey(groupId))
                {
                    GroupUsersRepository.Data[groupId].Remove(userId);
                }

                groupUsersRepository.Save();
                return NoContent();
            }

            return NotFound("Korisnik nije član ove grupe.");
        }
    }
}

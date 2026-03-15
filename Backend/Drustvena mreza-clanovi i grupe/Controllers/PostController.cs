using Microsoft.AspNetCore.Mvc;
using Drustvena_mreza_clanovi_i_grupe.Models;
using Drustvena_mreza_clanovi_i_grupe.Repositories;

namespace Drustvena_mreza_clanovi_i_grupe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostDbRepository _postRepository;

        public PostController()
        {
            _postRepository = new PostDbRepository();
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAll()
        {
            var posts = _postRepository.GetAll();
            return Ok(posts);
        }
    }
}
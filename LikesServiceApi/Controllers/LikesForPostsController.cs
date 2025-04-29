using CommentServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LikesServiceApi.Controllers
{
    [Authorize]
    [Route("api/likes/posts")]
    [ApiController]
    public class LikesForPostsController : ControllerBase
    {
        private readonly ILikesForPostsRepository _likesRep;
        public LikesForPostsController(ILikesForPostsRepository likesRep)
        {
            _likesRep = likesRep;
        }

        //Метод для того, чтобы поставить лайк на пост
        [HttpPost("{postId}")]
        public async Task<IActionResult> Like(string postId)
        {
            try
            {
                await _likesRep.AddLikeAsync(postId);
                return Ok("Лайк поставлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы убрать лайк с поста
        [HttpDelete("{postId}")]
        public async Task<IActionResult> Unlike(string postId)
        {
            try
            {
                await _likesRep.UnlikeAsync(postId);
                return Ok("Лайк убран.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы получить данные о лайке, поставленном на пост
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetPostLikeByIdAsync(string id)
        {
            return await _likesRep.GetPostLikeByIdAsync(id);
        }

        //Метод для того, чтобы получить данные о лайке, поставленном на пост
        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetUserLikesForPostsAsync(string userId)
        {
            var likes = await _likesRep.GetAllUserLikesForPostsAsync(userId);
            return Ok(likes);
        }
    }
}

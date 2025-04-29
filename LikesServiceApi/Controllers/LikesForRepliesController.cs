using LikesServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LikesServiceApi.Controllers
{
    [Authorize]
    [Route("api/likes/replies")]
    [ApiController]
    public class LikesForRepliesController : ControllerBase
    {
        private readonly ILikesForRepliesRepository _likesRep;
        public LikesForRepliesController(ILikesForRepliesRepository likesRep)
        {
            _likesRep = likesRep;
        }

        //Метод для того, чтобы поставить лайк на пост
        [HttpPost("{replyId}")]
        public async Task<IActionResult> Like(string replyId)
        {
            try
            {
                await _likesRep.AddLikeAsync(replyId);
                return Ok("Лайк поставлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы убрать лайк с поста
        [HttpDelete("{replyId}")]
        public async Task<IActionResult> Unlike(string replyId)
        {
            try
            {
                await _likesRep.UnlikeAsync(replyId);
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
            return await _likesRep.GetReplyLikeByIdAsync(id);
        }

        //Метод для того, чтобы получить данные о лайке, поставленном на пост
        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetUserLikesForPostsAsync(string userId)
        {
            var likes = await _likesRep.GetAllUserLikesForRepliesAsync(userId);
            return Ok(likes);
        }
    }
}

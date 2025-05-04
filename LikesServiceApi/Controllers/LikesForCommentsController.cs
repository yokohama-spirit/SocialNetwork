using LikesServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LikesServiceApi.Controllers
{
    [Authorize]
    [Route("api/likes/comments")]
    [ApiController]
    public class LikesForCommentsController : ControllerBase
    {
        private readonly ILikesForCommentsRepository _likesRep;
        public LikesForCommentsController(ILikesForCommentsRepository likesRep)
        {
            _likesRep = likesRep;
        }

        //Метод для того, чтобы поставить лайк на комментарий
        [HttpPost("{commentId}")]
        public async Task<IActionResult> Like(string commentId)
        {
            try
            {
                await _likesRep.AddLikeAsync(commentId);
                return Ok("Лайк поставлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы убрать лайк с комментария
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Unlike(string commentId)
        {
            try
            {
                await _likesRep.UnlikeAsync(commentId);
                return Ok("Лайк убран.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы получить данные о лайке, поставленном на комментарий
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetCommentLikeByIdAsync(string id)
        {
            return await _likesRep.GetCommentLikeByIdAsync(id);
        }

        //Метод для того, чтобы получить все лайки пользователя, поставленные на комментарий
        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetUserLikesForCommentsAsync(string userId)
        {
            var likes = await _likesRep.GetAllUserLikesForCommentsAsync(userId);
            return Ok(likes);
        }
    }
}

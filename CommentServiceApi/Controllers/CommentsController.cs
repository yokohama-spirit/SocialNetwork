using CommentServiceLibrary.Application.Requests;
using CommentServiceLibrary.Domain.Entities;
using CommentServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentServiceApi.Controllers
{
    [Authorize]
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commRep;

        public CommentsController(ICommentRepository commRep)
        {
            _commRep = commRep;
        }

        //Метод для получения комментария по идентификатору
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(string id)
        {
            try
            {
                var result = await _commRep.GetCommentByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для создания комментария
        [HttpPost("{postId}")]
        public async Task<IActionResult> CreateComment
            (string postId, [FromBody] CreateCommentDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _commRep.CreateCommentAsync(postId, command);
                return Ok("Комментарий оставлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для изменения комментария
        [HttpPut("{postId}/{commentId}")]
        public async Task<IActionResult> UpdateComment
            (string postId, string commentId, [FromBody] UpdateCommentDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _commRep.UpdateCommentAsync(postId, commentId, command);
                return Ok("Комментарий исправлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для удаления комментария
        [HttpDelete("{postId}/{commentId}")]
        public async Task<IActionResult> DeleteComment
            (string postId, string commentId)
        {
            try
            {
                await _commRep.DeleteCommentAsync(postId, commentId);
                return Ok("Комментарий удален.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения всех постов определенного человека
        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserComments
            (string userId)
        {
            try
            {
                var result = await _commRep.GetAllUserCommentsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

    }
}

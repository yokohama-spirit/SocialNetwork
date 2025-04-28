using CommentServiceLibrary.Application.Requests.Replies;
using CommentServiceLibrary.Domain.Entities;
using CommentServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentServiceApi.Controllers
{
    [Authorize]
    [Route("api/replies")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly IRepliesRepository _repliesRep;
        public RepliesController(IRepliesRepository repliesRep)
        {
            _repliesRep = repliesRep;
        }

        //Метод для добавления ответа на комментарий
        [HttpPost("{postId}/{commentId}")]
        public async Task<IActionResult> CreateReply
            (string postId, string commentId, [FromBody] CreateReplyDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _repliesRep.CreateReplyAsync(postId, commentId, command);
                return Ok("Ответ на комментарий оставлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для изменения ответа на комментарий
        [HttpPut("{replyId}")]
        public async Task<IActionResult> UpdateReply
            (string replyId, [FromBody] UpdateReplyDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _repliesRep.UpdateReplyAsync(replyId, command);
                return Ok("Ответ на комментарий изменен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для удаления ответа на комментарий
        [HttpDelete("{replyId}")]
        public async Task<IActionResult> DeleteReply
            (string replyId)
        {
            try
            {
                await _repliesRep.DeleteReplyAsync(replyId);
                return Ok("Ответ на комментарий удален!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения ответа по его идентификатору
        [HttpGet("{replyId}")]
        public async Task<Reply> GetReplyByIdAsync
            (string replyId)
        {
            return await _repliesRep.GetReplyByIdAsync(replyId);
        }

        //Метод для получения всех ответов пользователя
        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<Reply>> GetUserRepliesAsync
            (string userId)
        {
            return await _repliesRep.GetAllUserRepliesAsync(userId);
        }
    }
}

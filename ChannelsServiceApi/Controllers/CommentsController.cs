using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsServiceApi.Controllers
{
    [Authorize]
    [Route("api/channels/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IChannelCommentsRepository _commRep;
        public CommentsController(IChannelCommentsRepository commRep)
        {
            _commRep = commRep;
        }

        //Метод для создания комментария
        [HttpPost("{channelId}/{postId}")]
        public async Task<IActionResult> AddComment
            (string channelId, string postId, [FromBody] ChannelComment command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _commRep.AddCommentAsync(channelId, postId, command);
                return Ok("Комментарий успешно опубликован!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для изменения содержимого комментария
        [HttpPut("{postId}/{commentId}")]
        public async Task<IActionResult> UpdateComment
            (string postId, string commentId, [FromBody] ChannelComment command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _commRep.UpdateCommentAsync(postId, commentId, command);
                return Ok("Комментарий успешно изменен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }



        //Метод для удаления комментария
        [HttpDelete("{channelId}/{postId}/{commentId}")]
        public async Task<IActionResult> DeleteComment
            (string channelId, string postId, string commentId)
        {
            try
            {
                await _commRep.DeleteCommentAsync(channelId, postId, commentId);
                return Ok("Комментарий удален.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения комментария поста по идентификатору
        [AllowAnonymous]
        [HttpGet("{postId}/{commentId}")]
        public async Task<IActionResult> GetChannelCommentByIdAsync
            (string postId, string commentId)
        {
            try
            {
                var result = await _commRep.GetChannelCommentByIdAsync(postId, commentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения всех комментариев под постом
        [AllowAnonymous]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetAllComments
            (string postId)
        {
            try
            {
                var result = await _commRep.GetAllCommentsAsync(postId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }
    }
}

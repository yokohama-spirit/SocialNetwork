using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsServiceApi.Controllers
{
    [Authorize]
    [Route("api/channels/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IChannelPostsRepository _postsRep;
        public PostsController(IChannelPostsRepository postsRep)
        {
            _postsRep = postsRep;
        }

        //Метод для создания поста
        [HttpPost("{channelId}")]
        public async Task<IActionResult> AddPost
            (string channelId, [FromBody] CreatePostDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _postsRep.AddPostAsync(channelId, command);
                return Ok("Пост успешно опубликован!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для изменения содержимого поста
        [HttpPut("{channelId}/{postId}")]
        public async Task<IActionResult> UpdatePost
            (string channelId, string postId, [FromBody] ChannelPost command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _postsRep.UpdatePostAsync(channelId, postId, command);
                return Ok("Пост успешно изменен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }



        //Метод для удаления поста
        [HttpDelete("{channelId}/{postId}")]
        public async Task<IActionResult> DeletePost
            (string channelId, string postId)
        {
            try
            {
                await _postsRep.DeletePostAsync(channelId, postId);
                return Ok("Пост удален.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения поста канала по идентификатору
        [AllowAnonymous]
        [HttpGet("{channelId}/{postId}")]
        public async Task<IActionResult> GetChannelPostByIdAsync
            (string channelId, string postId)
        {
            try
            {
                var result = await _postsRep.GetChannelPostByIdAsync(channelId, postId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения всех постов канала
        [AllowAnonymous]
        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetAllPosts
            (string channelId)
        {
            try
            {
                var result = await _postsRep.GetAllPostsAsync(channelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }
    }
}

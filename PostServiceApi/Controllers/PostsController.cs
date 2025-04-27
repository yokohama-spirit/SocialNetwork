using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;

namespace PostServiceApi.Controllers
{
    [Authorize]
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRep;
        private readonly IMediator _mediator;
        public PostsController
            (IPostRepository postRep, 
            IMediator mediator)
        {
            _postRep = postRep;
            _mediator = mediator;
        }


        //Метод для создания поста
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _mediator.Send(command);
                return Ok("Пост успешно создан!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения всех существующих постов
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _postRep.GetAllPostsAsync();
        }

        //Метод для получения поста по идентификатору
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostByIdAsync(string id)
        {
            return await _postRep.GetPostByIdAsync(id);
        }

        //Метод для получения всех постов пользователя
        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IEnumerable<Post>> GetUserPostsAsync(string id)
        {
            return await _postRep.GetAllUserPostAsync(id);
        }


        //Метод для изменения поста
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _postRep.UpdatePostAsync(id, command);
                return Ok("Пост успешно обновлен!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для удаления поста
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostByIdAsync(string id)
        {
            try
            {
                await _postRep.DeletePostAsync(id);
                return Ok("Пост успешно удален!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

    }
}

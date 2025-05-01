using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubscriptionServiceLibrary.Domain.Entities;
using SubscriptionServiceLibrary.Domain.Interfaces;

namespace SubscriptionServiceApi.Controllers
{
    [Authorize]
    [Route("api/subs")]
    [ApiController]
    public class SubsController : ControllerBase
    {
        private readonly ISubscriptionRepository _subsRep;
        private readonly ISubscriptionService _subsService;
        public SubsController
            (ISubscriptionRepository subsRep, ISubscriptionService subsService)
        {
            _subsRep = subsRep;
            _subsService = subsService;
        }

        //Метод для того, чтобы поставить лайк на пост
        [HttpPost("{followingId}")]
        public async Task<IActionResult> Subscribe(string followingId)
        {
            try
            {
                await _subsRep.SubscribeAsync(followingId);
                return Ok("Вы подписались!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы убрать лайк с поста
        [HttpDelete("{followingId}")]
        public async Task<IActionResult> Unsubscribe(string followingId)
        {
            try
            {
                await _subsRep.UnsubscribeAsync(followingId);
                return Ok("Вы отписались.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для того, чтобы получить данные о лайке, поставленном на пост
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetPostLikeByIdAsync(string id)
        {
            return await _subsRep.GetSubscriptionByIdAsync(id);
        }

        //Метод для того, чтобы получить данные о лайке, поставленном на пост
        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetUserLikesForPostsAsync(string userId)
        {
            var likes = await _subsRep.GetAllUserSubscriptionsAsync(userId);
            return Ok(likes);
        }

        //Метод для получения рекомендованых пользователей
        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedUsers()
        {
            var users = await _subsService.GetRecommendedUsers();
            return Ok(users);
        }

    }
}

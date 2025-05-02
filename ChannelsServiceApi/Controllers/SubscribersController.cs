using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsServiceApi.Controllers
{
    [Authorize]
    [Route("api/channel/subs")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        private readonly IChannelSubsRepository _subsRep;
        public SubscribersController(IChannelSubsRepository subsRep)
        {
            _subsRep = subsRep;
        }

        //Метод для добавления подписчика
        [HttpPost("{channelId}")]
        public async Task<IActionResult> AddSubscriber
            (string channelId, [FromBody] ChannelSubscriber command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _subsRep.AddChannelSubAsync(channelId, command);
                return Ok($"Подписчик успешно добавлен!\nЕго ID: {command.UserId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для подписки на канал
        [HttpPost("join/{channelId}")]
        public async Task<IActionResult> JoinChannel
            (string channelId)
        {
            try
            {
                await _subsRep.JoinChannelAsync(channelId);
                return Ok("Вы подписались на канал!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }


        //Метод для удаления подписчика
        [HttpDelete("{channelId}/{subId}")]
        public async Task<IActionResult> DeleteSubscriber
            (string channelId, string subId)
        {
            try
            {
                await _subsRep.DeleteChannelSubAsync(channelId, subId);
                return Ok($"Подписчик удален.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения подписчика канала по идентификатору
        [HttpGet("{channelId}/{subId}")]
        public async Task<IActionResult> GetChannelSubByIdAsync
            (string channelId, string subId)
        {
            try
            {
                var result = await _subsRep.GetChannelSubByIdAsync(channelId, subId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }


        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetAllSubscribers
            (string channelId)
        {
            try
            {
                var result = await _subsRep.GetAllSubsAsync(channelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

    }
}

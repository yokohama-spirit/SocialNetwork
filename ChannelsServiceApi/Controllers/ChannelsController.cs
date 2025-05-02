using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsServiceApi.Controllers
{
    [Authorize]
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IChannelRepository _channelRep;
        public ChannelsController(IMediator mediator, IChannelRepository channelRep)
        {
            _mediator = mediator;
            _channelRep = channelRep;
        }
        //Метод для создания канала
        [HttpPost]
        public async Task<IActionResult> CreateChannel([FromBody] CreateChannelRequest command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _mediator.Send(command);
                return Ok("Канал успешно создан!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для удаления канала
        [HttpDelete("{channelId}")]
        public async Task<IActionResult> DeleteChannel(string channelId)
        {
            try
            {
                await _channelRep.DeleteChannelAsync(channelId);
                return Ok("Канал удален :(");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения канала по его идентификатору
        [HttpGet("{channelId}")]
        public async Task<ActionResult<Channel>> GetChannelByIdAsync(string channelId)
        {
            try
            {
                var result = await _channelRep.GetChannelByIdAsync(channelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения всех каналов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetAllChannelAsync()
        {
            try
            {
                var result = await _channelRep.GetAllChannelsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для изменения названия канала
        [HttpPatch("name/{channelId}")]
        public async Task<IActionResult> EditChannelName
            (string channelId, [FromBody] UpdateChannelNameDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _channelRep.UpdateChannelNameAsync(channelId, command);
                return Ok("Имя канала успешно обновлено!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }


        //Метод для изменения описания канала
        [HttpPatch("desc/{channelId}")]
        public async Task<IActionResult> EditChannelDesc
            (string channelId, [FromBody] UpdateChannelDescriptionDTO command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _channelRep.UpdateChannelDescAsync(channelId, command);
                return Ok("Описание канала успешно обновлено!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }
    }
}

using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsServiceApi.Controllers
{
    [Authorize]
    [Route("api/admins")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IChannelAdminsRepository _adminsRep;

        public AdminsController(IChannelAdminsRepository adminsRep)
        {
            _adminsRep = adminsRep;
        }

        //Метод для добавления админа
        [HttpPost("{channelId}")]
        public async Task<IActionResult> AddAdmin
            (string channelId, [FromBody] ChannelAdmin command)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                return BadRequest($"Некорректно указаны данные! Ошибка: {error}");
            }
            try
            {
                await _adminsRep.AddChannelAdminAsync(channelId, command);
                return Ok($"Админ успешно добавлен!\nЕго ID: {command.UserId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для удаления админа
        [HttpDelete("{channelId}/{userId}")]
        public async Task<IActionResult> RemoveAdmin
                (string channelId, string userId)
        {
            try
            {
                await _adminsRep.DeleteChannelAdminAsync(channelId, userId);
                return Ok("Админ успешно убран.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения всех админов канала
        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetAllChannelAdmins
                (string channelId)
        {
            try
            {
                var result = await _adminsRep.GetAllChannelAdminsAsync(channelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }
    }
}

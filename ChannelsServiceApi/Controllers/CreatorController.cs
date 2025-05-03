using ChannelsServiceLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsServiceApi.Controllers
{
    [Authorize]
    [Route("api/panel")]
    [ApiController]
    public class CreatorController : ControllerBase
    {
        private readonly ICreatorService _service;
        public CreatorController(ICreatorService service)
        {
            _service = service;
        }

        //Метод для получения всех заявок на вступление
        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetMyRequests(string channelId)
        {
            try
            {
                var result = await _service.GetMyRequests(channelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для принятия заявки
        [HttpPost("submit/{requestId}")]
        public async Task<IActionResult> SubmitRequest(string requestId)
        {
            try
            {
                await _service.SubmitRequest(requestId);
                return Ok("Заявка принята.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для отклонения заявки 
        [HttpPost("reject/{requestId}")]
        public async Task<IActionResult> RejectRequest(string requestId)
        {
            try
            {
                await _service.RejectRequest(requestId);
                return Ok("Заявка отклонена.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }
    }
}

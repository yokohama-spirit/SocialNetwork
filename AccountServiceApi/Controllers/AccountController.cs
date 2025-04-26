using AuthServiceLibrary.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserServiceLibrary.Domain.Interfaces;

namespace AccountServiceApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRep;
        public AccountController(IUserRepository userRep)
        {
            _userRep = userRep;
        }

        //Метод для получения всех пользователей
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userRep.GetUsersAsync();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения текущего пользователя
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<User>> GetMeAsync()
        {
            try
            {
                var result = await _userRep.GetCurrentUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }

        //Метод для получения текущего пользователя
        [Authorize]
        [HttpGet("me-id")]
        public async Task<ActionResult<string>> GetMyIdAsync()
        {
            try
            {
                var result = await _userRep.GetCurrentUserId();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }


        //Метод для получения пользователя по идентификатору
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(string id)
        {
            try
            {
                var result = await _userRep.GetUserByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка: {ex}");
            }
        }
    }
}

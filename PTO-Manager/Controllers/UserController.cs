using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInputDto user)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _userService.Login(user);
                response.Data = token;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var id = await _userService.Register(user);
                response.Data = id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }
        }
        
        
        [HttpGet]
        [Route("GetRemainingDays")]
        public async Task<IActionResult> GetRemainingDays()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var value = await _userService.RemainingDaysGet();
                response.Data = value;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }
        }
        
        [HttpPost]
        [Route("GetRemainingDaysByUserid")]
        [Authorize]
        public async Task<IActionResult> GetRemainingDaysByUserid(GetRemainingForUserDto remainingDaysForUserDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var value = await _userService.RemainingDaysGetByUserid(remainingDaysForUserDto);
                response.Data = value;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}

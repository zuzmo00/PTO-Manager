using Microsoft.AspNetCore.Mvc;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController:ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost]
        [Route("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] Guid id, int reszlegId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _adminService.CreateAdmin(id, reszlegId);
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
        [HttpDelete]
        [Route("RemoveDepartment")]
        public async Task<IActionResult> RemoveDepartment([FromBody] Guid id, int reszlegId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _adminService.RemoveDeparment(id, reszlegId);
                response.Data = result;
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

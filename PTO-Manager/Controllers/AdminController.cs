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
    public class AdminController:ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost]
        [Route("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin(CreateAdminInputDTO createAdminInputDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _adminService.CreateAdmin(createAdminInputDto);
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
        [Route("RemovePriviligeByParams")]
        public async Task<IActionResult> RemovePriviligeByParams(RemoveAdminPriviligeInputDto removeDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var result = await _adminService.RemovePriviligeByDeparment(removeDto);
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
        [HttpPost]
        [Route("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] Guid id, int reszlegId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _adminService.AddDepartment(id, reszlegId);
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
        [HttpPut]
        [Route("ChangePermissions")]
        public async Task<IActionResult> ChangePermissions(PermissionUpdateDto permissionUpdateDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                response.Message = await _adminService.ChangePermissions(permissionUpdateDto);
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
        [Route("GetPermissionsForUser")]
        public async Task<IActionResult>  GetPermissionsForUser(GetpermissionInputDto getpermissionInputDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _adminService.GetPermissionsForUser(getpermissionInputDto);
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
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [Route("CreateDepartment")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto departmentName)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                response.Message = await _departmentService.CreateDepartment(departmentName); 
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
        public async Task<IActionResult> RemoveDepartment(DepartmentRemoveDto departmentRemoveDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var removedId = await _departmentService.RemoveDepartment(departmentRemoveDto);
                response.Data = removedId;
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
        
        
        [AllowAnonymous]
        [HttpGet]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var removedId = await _departmentService.GetDepartments();
                response.Data = removedId;
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
        [Route("GetDepartmentsForDecide")]
        public async Task<IActionResult> GetDepartmentsForDecide()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var removedId = await _departmentService.GetDepartmentsForDecide();
                response.Data = removedId;
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

        
        
        
        [AllowAnonymous]
        [HttpGet]
        [Route("GetDepartmentsForManage")]
        public async Task<IActionResult> GetDepartmentsForManage()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var removedId = await _departmentService.GetDepartmentsForManage();
                response.Data = removedId;
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

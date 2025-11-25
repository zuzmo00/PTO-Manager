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
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public DepartmentController(IDepartmentService departmentService, IMapper mapper, AppDbContext appDbContext)
        {
            _departmentService = departmentService;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto departmentName)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var id = await _departmentService.CreateDepartment(departmentName);
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

        [HttpDelete]
        [Route("RemoveDepartment")]
        public async Task<IActionResult> RemoveDepartment([FromBody] int id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var removedId = await _departmentService.RemoveDepartment(id);
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
    }
}

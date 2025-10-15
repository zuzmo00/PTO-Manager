using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialDaysController:ControllerBase
    {
        private readonly ISpecialDaysService _specialDaysService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public SpecialDaysController(ISpecialDaysService specialDaysService, IMapper mapper, AppDbContext appDbContext)
        {
            _specialDaysService = specialDaysService;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        [HttpPost]
        [Route("AddSpecalDay")]
        public async Task<IActionResult> AddSpecialDays([FromBody]SpecialDaysAddDto specialDaysAddDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _specialDaysService.AddSpecialDay(specialDaysAddDto);
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
        [Route("Remove")]
        public async Task<IActionResult> RemoveDay([FromBody] DateOnly date)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _specialDaysService.RemovalSpecialDay(date);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }
        }
        [HttpGet]
        [Route("ListDays")]
        public async Task<IActionResult> ListDays()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data= await _specialDaysService.GetSpecialDays();
                response.Data = data;
                return Ok(response);

            }
            catch
            {
                return BadRequest(response);
            }
        }
    }
}

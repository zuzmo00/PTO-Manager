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
        public SpecialDaysController(ISpecialDaysService specialDaysService)
        {
            _specialDaysService = specialDaysService;
        }
        [HttpPost]
        [Route("AddSpecialDay")]
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
        [Route("RemoveSpecialDay")]
        public async Task<IActionResult> RemoveSpecialDay(SpecialDayRemoveDto specialDayRemoveDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _specialDaysService.RemoveSpecialDay(specialDayRemoveDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }
        }
        
        
        
        [HttpPut]
        [Route("ModifySpecialDay")]
        public async Task<IActionResult> ModifySpecialDay(SpecialDayModifyDto specialDayModifyDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _specialDaysService.ModifySpecialDay(specialDayModifyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }
        }

            
        [HttpGet]
        [Route("ListSpecialDays")]
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

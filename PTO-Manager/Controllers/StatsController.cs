using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController: ControllerBase
    {
        private readonly IStatsService _statsService;
        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }
        
        [HttpPost]
        [Route("GetStats")]
        public async Task<IActionResult> GetStats([FromBody] Stats stats)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var token = await _statsService.GetStatsForDay(stats);
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
        [Route("GetStatsForWeek")]
        public async Task<IActionResult> GetStatsForWeek(Stats stats)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                response.Data = await _statsService.GetStatsForWeek(stats);
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

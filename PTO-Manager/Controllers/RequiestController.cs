using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequiestController: ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RequiestController(IRequestService requestService, AppDbContext appDbContext, IMapper mapper)
        {
            _requestService = requestService;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("CreateRequest")]

        public async Task<IActionResult> CreateRequest([FromBody] RequestAddDto requestAddDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _requestService.CreateRequest(requestAddDto);
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
        [Route("GetAllRequests")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetAllRequests()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var requests = await _requestService.GetAllRequestsAndSpecialDays();
                response.Data = requests;
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

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
    public class RequestController: ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RequestController(IRequestService requestService, AppDbContext appDbContext, IMapper mapper)
        {
            _requestService = requestService;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("CreateRequest")]

        public async Task<IActionResult> CreateRequest([FromBody] RequestAddAsUserDto requestAddDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                await _requestService.CreateRequestAsUser(requestAddDto);
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
        
        
        
        [HttpPost]
        [Route("getPendingRequestByDepartment")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetPendingRequestByDepartment(PendingRequestsInputDto pendingRequestsInputDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var requests = await _requestService.GetPendingRequestByDepartment(pendingRequestsInputDto);
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

        
        
        
        [HttpGet]
        [Route("getPendingRequest")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> GetPendingRequest()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var requests = await _requestService.GetPendingRequest();
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
        
        
        
        [HttpPost]
        [Route("makeDecision")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> MakeDecision(RequestDecisionInputDto requestDecisionInputDto)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var requests = await _requestService.MakeDecision(requestDecisionInputDto);
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
        

        [HttpPost]
        [Route("RevokeWholeRequest")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> RevokeWholeRequest(RevokeRequestInputDto revokeRequestInput)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var requests = await _requestService.RevokeWholeRequest(revokeRequestInput);
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


        [HttpPost]
        [Route("RevokeARequest")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> RevokeARequest(RevokeRequestInputDto revokeRequestInput)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var requests = await _requestService.RevokeARequest(revokeRequestInput);
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

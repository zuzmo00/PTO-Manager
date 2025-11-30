using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;
using PTO_Manager.Services;

namespace PTO_Manager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PreferenceController : ControllerBase
{
 
    private readonly IPrefenceService _prefenceService;

    public PreferenceController( IPrefenceService prefenceService)
    {
        _prefenceService = prefenceService;
    }

    [HttpPost]
    [Route("CreatePreference")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePreference(PreferenceDto preferenceDto)
    {
        ApiResponse response = new ApiResponse();
        try
        {
            response.Message = await _prefenceService.CreatePreference(preferenceDto);
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
    [Route("GetPreference")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPreference(GetPreferenceInputDto getPreferenceInputDto)
    {
        ApiResponse response = new ApiResponse();
        try
        {
            response.Data = await _prefenceService.GetPreference(getPreferenceInputDto);
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
    [Route("ModifyPreference")]
    [AllowAnonymous]
    public async Task<IActionResult> ModifyPreference(ModifyPreferenceInputDto preferenceDto)
    {
        ApiResponse response = new ApiResponse();
        try
        {
            response.Data = await _prefenceService.ModifyPreference(preferenceDto);
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
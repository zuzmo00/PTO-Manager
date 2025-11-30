using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;

namespace PTO_Manager.Services;

public interface IPrefenceService
{
    Task<string> CreatePreference(PreferenceDto preferenceDto);
    Task<PreferenceDto> GetPreference(GetPreferenceInputDto getPreferenceInputDto);
    Task<string> ModifyPreference(ModifyPreferenceInputDto preferenceDto);
}



public class PreferenceService : IPrefenceService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public PreferenceService (AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> CreatePreference(PreferenceDto preferenceDto)
    {
        var temp = _mapper.Map<Preferences>(preferenceDto);
        await _context.Preferences.AddAsync(temp);
        await _context.SaveChangesAsync();
        return "Preference added successfully";
    }
    
    
    public async Task<PreferenceDto> GetPreference(GetPreferenceInputDto getPreferenceInputDto)
    {
        var temp = await _context.Preferences.FirstOrDefaultAsync(c=> c.Name == getPreferenceInputDto.preferenceName ) ?? throw new Exception("Preference not found");
        return _mapper.Map<PreferenceDto>(temp);
    }
    
    public async Task<string> ModifyPreference(ModifyPreferenceInputDto preferenceDto)
    {
        var temp = await _context.Preferences.FirstOrDefaultAsync(c=> c.Name == preferenceDto.Name ) ?? throw new Exception("Preference not found");
        temp.Value = preferenceDto.Value;
        _context.Preferences.Update(temp);
        await _context.SaveChangesAsync();
        
        return "Preference modified successfully";
    }
}
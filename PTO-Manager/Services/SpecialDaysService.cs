using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;

namespace PTO_Manager.Services
{
    public interface ISpecialDaysService
    {
        public Task<string> AddSpecialDay(SpecialDaysAddDto specialDaysAddDto);
        public Task<string> RemovelSpecialDay(DateOnly date);
        public Task<List<SpecialDaysGetDto>> GetSpecialDays();
    }
    public class SpecialDaysService : ISpecialDaysService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SpecialDaysService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> AddSpecialDay(SpecialDaysAddDto specialDaysAddDto)
        {
            var day= await _context.SpecialDays.FirstOrDefaultAsync(x => x.Date == specialDaysAddDto.Date);
            if (day != null)
            {
                throw new Exception("This day is already added");
            }
            var newDay = _mapper.Map<SpecialDays>(specialDaysAddDto);
            await _context.SpecialDays.AddAsync(newDay);
            await _context.SaveChangesAsync();
            return "Special day added successfully";
        }

        public async Task<List<SpecialDaysGetDto>> GetSpecialDays()
        {
            var days=await _context.SpecialDays.ToListAsync();
            return _mapper.Map<List<SpecialDaysGetDto>>(days);
        }

        public async Task<string> RemovelSpecialDay(DateOnly dateOnly)
        {
            var day = await _context.SpecialDays.FirstOrDefaultAsync(x => x.Date == dateOnly);
            if (day == null)
            {
                throw new Exception("Day dose not exists");
            }
            _context.SpecialDays.Remove(day);
            _context.SaveChanges();
            return $"Teh day has been removed {day.Date}";

        }
    }
}

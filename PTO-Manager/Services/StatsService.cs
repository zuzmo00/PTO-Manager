using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Services
{
    public interface IStatsService
    {
        public Task<List<StatGetDto>> GetStatsForDay(Stats stats);
        public Task<List<weeklyStatGetDto>> GetStatsForWeek(Stats stats);
    }
    public class StatsService : IStatsService
    {
        public readonly AppDbContext _dbContext;
        public readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public StatsService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        /*
         
         
        public async Task<StatsGetDto> GetStatsAsync(Stats stats)
        {
            var users = await _dbContext.Users.Where(u => u.DepartmentId == stats.DepartmentId).ToListAsync();

            var requestBlockIds = new List<Guid>();
            var requests = new List<Request>();


            foreach (var user in users)
            {

                var blocksForThisUser = await _dbContext.RequestBlocks
                    .Where(x => x.UserId == user.Id)
                    .Select(x => x.Id)
                    .ToListAsync();

                requestBlockIds.AddRange(blocksForThisUser);
            }

            foreach (var requestBlockId in requestBlockIds)
            {

                var requestsForThisBlock = await _dbContext.Requests
                    .Where(x => x.Date == stats.Date)
                    .Where(x => x.RequestBlockId == requestBlockId)
                    .ToListAsync();

                requests.AddRange(requestsForThisBlock);
            }


            var tempCounters = new Dictionary<ReservationType, int>();
            foreach (ReservationType type in Enum.GetValues(typeof(ReservationType)))
            {
                tempCounters[type] = 0;
            }

            foreach (var request in requests)
            {
                switch (request.Type)
                {
                    case ReservationType.PTO:
                        tempCounters[ReservationType.PTO]++;
                        break;
                    case ReservationType.SickLeave:
                        tempCounters[ReservationType.SickLeave]++;
                        break;
                    case ReservationType.BusinessTrip:
                        tempCounters[ReservationType.BusinessTrip]++;
                        break;
                    case ReservationType.PlannedLeave:
                        tempCounters[ReservationType.PlannedLeave]++;
                        break;
                    case ReservationType.SpecialWorkDay:
                        tempCounters[ReservationType.SpecialWorkDay]++;
                        break;
                    case ReservationType.SpecialHoliday:
                        tempCounters[ReservationType.SpecialHoliday]++;
                        break;
                }
            }

            var finalTypeStats = new List<RequestTypeStat>();
            foreach (var counter in tempCounters)
            {
                if (counter.Value > 0)
                {
                    finalTypeStats.Add(new RequestTypeStat { Type = counter.Key, count = counter.Value });
                }
            }
            var activeBlockIds = requests.Select(r => r.RequestBlockId).Distinct().ToList();

            var distinctWorkerCount = await _dbContext.RequestBlocks
                .Where(rb => activeBlockIds.Contains(rb.Id))
                .Select(rb => rb.UserId)
                .Distinct()
                .CountAsync();

            return new StatsGetDto
            {
                Count = users.Count,
                Department = (await _dbContext.Department.FirstOrDefaultAsync(d => d.Id == stats.DepartmentId))?.DepartmentName ?? "Unknown",
                Date = stats.Date,
                AllPtoWorkers = distinctWorkerCount,
                RequestTypeStats = finalTypeStats
            };
        }
*/
        public async Task<List<StatGetDto>> GetStatsForDay(Stats stats)
        {
            List<StatGetDto> returnobj = [];

            var temp = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .Include(o => o.User)
                .ThenInclude(l => l.Department)
                .Where(u => u.StartDate <= stats.Date && u.EndDate >= stats.Date  && (u.Status == HolidayStatus.Accepted ||
                            u.Status == HolidayStatus.Pending))
                .ToListAsync();

            var departments = await _dbContext.Department.ToListAsync();
            List<string> colors = ["red", "green", "blue", "yellow", "purple"];
            var index = 0;
            returnobj.AddRange(departments.Select(item => new StatGetDto
            {
                color = colors[index++],
                name = item.DepartmentName,
                value = temp.Count(v => v.User.Department.DepartmentName == item.DepartmentName),
                details = new statDetailGetDto
                {
                    department = item.DepartmentName,
                    date = stats.Date.ToString(),
                    pto = temp.Count(v => v.Type == ReservationType.PTO && v.User.Department.DepartmentName == item.DepartmentName),
                    betegSzab = temp.Count(v => v.Type == ReservationType.SickLeave && v.User.Department.DepartmentName == item.DepartmentName),
                    kikuldetes = temp.Count(v => v.Type == ReservationType.BusinessTrip && v.User.Department.DepartmentName == item.DepartmentName),
                    betervezett = temp.Count(v => v.Type == ReservationType.PlannedLeave && v.User.Department.DepartmentName == item.DepartmentName)
                }
            }));
            
            return returnobj;
        }
        
        
        public async Task<List<weeklyStatGetDto>> GetStatsForWeek(Stats stats)
        {
            List<weeklyStatGetDto> returnobj = [];
            
            var diff = (7 + (stats.Date.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = stats.Date.AddDays(-diff);
            
            var weekEnd = weekStart.AddDays(6);
            
            var temp =  await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .Include(o => o.User)
                .ThenInclude(l => l.Department)
                .Where(u=>u.EndDate >= weekStart && u.StartDate <= weekEnd && (u.Status == HolidayStatus.Accepted || u.Status == HolidayStatus.Pending ))
                .ToListAsync();


            for (var i = weekStart; i <= weekEnd; i = i.AddDays(1))
            {
                returnobj.Add(new weeklyStatGetDto
                {
                    date = i.ToString("yyyy-MM-dd"),
                    pto =  temp.Sum(item =>
                        item.Requests.Count(v => v.Date == i && v.Type == ReservationType.PTO)),
                    
                    betegSzab = temp.Sum(item =>
                        item.Requests.Count(v => v.Date == i && v.Type == ReservationType.SickLeave)),
                        
                    kikuldetes = temp.Sum(item =>
                        item.Requests.Count(v => v.Date == i && v.Type == ReservationType.BusinessTrip)),
                    
                    betervezett = temp.Sum(item =>
                        item.Requests.Count(v => v.Date == i && v.Type == ReservationType.PlannedLeave))
                });
            }
            
            return returnobj;
        }
        
    }
}

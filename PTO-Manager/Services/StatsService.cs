using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities;

namespace PTO_Manager.Services
{
    public interface IStatsService
    {
        public Task<StatsGetDto> GetStatsAsync(Stats stats);
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
    }
}

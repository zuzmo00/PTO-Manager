using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities;
using PTO_Manager.Entities.Enums;
using SzabadsagKezeloWebApp.Services;

namespace PTO_Manager.Services
{
    public interface IRequestService
    {
        public Task<Guid> CreateRequest(RequestAddDto requestAddDto);
        public Task<Guid> AcceptRequest(Guid id);
        public Task<Guid> RejectRequest(Guid id);
        public Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDays();
    }
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAktualisFelhasznaloService _aktualisFelhasznaloService;
        public RequestService(AppDbContext dbContext, IMapper mapper, IAktualisFelhasznaloService aktualisFelhasznaloService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _aktualisFelhasznaloService = aktualisFelhasznaloService;
        }
        public Task<Guid> AcceptRequest(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateRequest(RequestAddDto requestAddDto)
        {
            var request = await _dbContext.Requests.FirstOrDefaultAsync(x => x.Date == requestAddDto.Date);
            if (request != null)
            {
                throw new Exception("Request already exists for this date");
            }
            var newRequest = _mapper.Map<Request>(requestAddDto);
            await _dbContext.Requests.AddAsync(newRequest);
            await _dbContext.SaveChangesAsync();
            return newRequest.Id;
        }

        public async Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDays()
        {
            var requests = await _dbContext.Requests.Where(x => x.UserId.ToString() == _aktualisFelhasznaloService.UserId).ToListAsync(); //évszám kimaradt, hogy melyik évre kell szűrni
            var specialDays = await _dbContext.SpecialDays.ToListAsync();// Itt is
            if (requests == null)
            {
                throw new Exception("No requests found for this user");
            }
            List<ReservedDaysDto> dtos = new List<ReservedDaysDto>();
            foreach (var request in requests)
            {
                ReservedDaysDto dto = new ReservedDaysDto
                {
                    reservedDay = request.Date,
                    reservationType = (ReservationType)(int)request.Tipus
                };
                dtos.Add(dto);
            }
            foreach (var specialDay in specialDays)
            {
                ReservedDaysDto dto = new ReservedDaysDto
                {
                    reservedDay = specialDay.Date,
                    reservationType = specialDay.IsWorkingDay
                        ? ReservationType.SpecialWorkDay
                        : ReservationType.SpecialHoliday
                };
                dtos.Add(dto);
            }
            return dtos;
        }
    
        public Task<Guid> RejectRequest(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

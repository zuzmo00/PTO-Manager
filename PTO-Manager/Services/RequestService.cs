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
        Task<string> CreateRequestAsUser(RequestAddAsUserDto requestAddDto);
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

        public async Task<string> CreateRequestAsUser(RequestAddAsUserDto requestAddDto) //Még a nap nincsen csökkentve, az elérhető napopok, pedig még nincs növelve
        {
            
            var existingRequest = await _dbContext.Requests
                .AnyAsync(x => x.Date >= requestAddDto.Begin_Date
                               && x.Date <= requestAddDto.End_Date
                               && _aktualisFelhasznaloService.UserId == x.UserId.ToString());
            if (existingRequest == true)
            {
                throw new Exception("Request already exists for this date");
            }

            List<Request> requests = [];
            Guid requestId = Guid.NewGuid();

            for (var i = requestAddDto.Begin_Date; i < requestAddDto.End_Date; i = i.AddDays(1))
            {
                requests.Add(new Request
                {
                    UserId = Guid.Parse(_aktualisFelhasznaloService.UserId),
                    Statusz = SzabStatusz.Fuggoben,
                    Date = i,
                    KerelemSzam = requestId,
                    Tipus = SzabadsagTipus.Fizetett,
                });
            }
            
            //SMTP leater need to be added
            
            
            await _dbContext.Requests.AddRangeAsync(requests);
            await _dbContext.SaveChangesAsync();
            return "Successfully created request";
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

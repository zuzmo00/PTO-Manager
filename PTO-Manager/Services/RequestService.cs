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
                               && _aktualisFelhasznaloService.UserId == x.RequestBlock.UserId.ToString());
            if (existingRequest == true)
            {
                throw new Exception("Request already exists for this date");
            }
            
            var RequestBlockNewItem = new RequestBlocks
            {
                UserId = new Guid(_aktualisFelhasznaloService.UserId),
                StartDate = requestAddDto.Begin_Date,
                EndDate = requestAddDto.End_Date,
                Type = ReservationType.PTO,
            };

            List<Request> requests = [];
            for (var i = requestAddDto.Begin_Date; i < requestAddDto.End_Date; i = i.AddDays(1))
            {
                requests.Add(new Request
                {
                    RequestBlockId = RequestBlockNewItem.Id,
                    Date = i,
                    Type = ReservationType.PTO,
                });
            }
            
            //SMTP leater need to be added
            
            
            await _dbContext.Requests.AddRangeAsync(requests);
            await _dbContext.RequestBlocks.AddAsync(RequestBlockNewItem);
            await _dbContext.SaveChangesAsync();
            return "Successfully created request";
        }

        public async Task<List<PendingRequestsGetDto>> GetPendingRequestByDepartment(PendingRequestsInputDto pendingRequestsInputDto)
        {
            var pendingRequestBlocks = await _dbContext.RequestBlocks
                .Include(l=>l.Requests)
                .Include(k => k.User)
                .ThenInclude(l=>l.Department)
                .Where(c => pendingRequestsInputDto.DepartmentIds.Contains(c.User.Department.DepartmentName))
                .ToListAsync();


            var temp = pendingRequestBlocks.Select(c => new PendingRequestsGetDto
            {
                id = c.Id.ToString(),
                name = c.User.Name,
                department = c.User.Department.DepartmentName,
                begin = c.StartDate,
                end = c.EndDate,
            }
            ).ToList();

            return temp;

            /*
             Más logika, másik functionba kell

            var temp = pendingRequestBlocks.SelectMany(v => v.Requests).Select(c => new PendingRequestsGetDto
            {
             id = c.Id.ToString(),
             name = c.RequestBlock.User.Name,
             department = c.RequestBlock.User.Department.DepartmentNev,
             date = c.Date,
            });
             */
        }
        
        
        public async Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDays()
        {
            var requestsBlocks = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .Where(x => x.UserId.ToString() == _aktualisFelhasznaloService.UserId).ToListAsync() ?? []; //évszám kimaradt, hogy melyik évre kell szűrni
            var specialDays = await _dbContext.SpecialDays.ToListAsync();// Itt is
            List<ReservedDaysDto> dtos = [];
       
            if (requestsBlocks.Count != 0) {
                    var temp =  requestsBlocks.SelectMany(c => c.Requests)
                    .Select(k => new ReservedDaysDto
                    {
                        reservedDay = k.Date,
                        reservationType = ReservationType.PTO
                    }).ToList();
                    dtos.AddRange(temp);
            }
            
            var specDays = specialDays.Select(c => new ReservedDaysDto
            {
                reservedDay = c.Date,
                reservationType = c.IsWorkingDay ? ReservationType.SpecialWorkDay : ReservationType.SpecialHoliday
            });
            dtos.AddRange(specDays);
           
            return dtos;
        }
    
        public async Task<string> RejectRequest(RequestDecisionInputDto requestDecisionInputDto)
        {
            var requestBlock = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .FirstOrDefaultAsync(x => x.Id.ToString() == requestDecisionInputDto.RequestBlockId) ?? throw new Exception("RequestBlock not found");

            requestBlock.Status = requestDecisionInputDto.verdict switch
            {
                true => HolidayStatus.Accepted,
                false => HolidayStatus.Pending
            };
            
            _dbContext.RequestBlocks.Update(requestBlock);
            await _dbContext.SaveChangesAsync();
            return requestDecisionInputDto.verdict switch
            {
                true => "Request accepted succesfully",
                false => "Request declined succesfully"
            };
        }
    }
}

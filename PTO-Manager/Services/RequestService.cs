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
        Task<string> CreateRequestAsAdministrator(RequestAddAsAdministratorDto requestAddDto);
        Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDays();
        Task<List<PendingRequestBlockDto>> GetPendingRequestByParams(PendingRequestsInputDto pendingRequestsInputDto);
        Task<List<PendingRequestsGet>> GetPendingRequest();
        Task<string> MakeDecision(RequestDecisionInputDto requestDecisionInputDto);
        Task<string> RevokeWholeRequest(RevokeRequestInputDto revokeRequestInput);
        Task<string> RevokeARequest(RevokeRequestInputDto revokeRequestInput);
        Task<List<PendingRequestBlockDto>> GetAcceptedRequestByParams(AcceptedRequestsInputDto acceptedRequestsInputDto);
        Task<RequestStatsGetDto> GetStatsForRequest(StatsForRequestInputDto inputDto);
        Task<string> RevokeRequest(RevokeRequestInputDto revokeRequestInput);
        Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDaysByUserId(GetRequestsInputDto requestsInputDto);

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
            for (var i = requestAddDto.Begin_Date; i <= requestAddDto.End_Date; i = i.AddDays(1))
            {
                requests.Add(new Request
                {
                    RequestBlockId = RequestBlockNewItem.Id,
                    Date = i,
                    Type = ReservationType.PTO,
                });
            }
            
            //SMTP leater need to be added
            var hetvege_munkanap_e = 
                await _dbContext.Preferences
                .FirstOrDefaultAsync(c=>c.Name == "hetvege_munkanap_e") ?? null;
            var workdaycount = 0;
            if (hetvege_munkanap_e == null || hetvege_munkanap_e.Value == false)
            {
                for (var date = requestAddDto.Begin_Date; date <= requestAddDto.End_Date; date = date.AddDays(1))
                {
                    if (!(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        workdaycount++;
                    }
                }
            }
            else
            {
                workdaycount = (requestAddDto.End_Date.DayNumber - requestAddDto.Begin_Date.DayNumber) + 1;
            }
            
            var userRemaineng = await _dbContext.Remaining
                .FirstOrDefaultAsync(c=> c.UserId.ToString() == _aktualisFelhasznaloService.UserId) ?? throw new Exception("User does not ha remaining days DB entry");

            userRemaineng.RemainingDays -= workdaycount;
            
            
            _dbContext.Remaining.Update(userRemaineng);
            await _dbContext.Requests.AddRangeAsync(requests);
            await _dbContext.RequestBlocks.AddAsync(RequestBlockNewItem);
            await _dbContext.SaveChangesAsync();
            return "Successfully created request";
        }
        
        
        
        public async Task<string> CreateRequestAsAdministrator(RequestAddAsAdministratorDto requestAddDto) 
        {
            
            var existingRequest = await _dbContext.Requests
                .AnyAsync(x => x.Date >= requestAddDto.Begin_Date
                               && x.Date <= requestAddDto.End_Date
                               && requestAddDto.UserId == x.RequestBlock.UserId.ToString());
            if (existingRequest == true)
            {
                throw new Exception("Request already exists for this date");
            }
            
            var RequestBlockNewItem = new RequestBlocks
            {
                UserId = new Guid(requestAddDto.UserId),
                StartDate = requestAddDto.Begin_Date,
                EndDate = requestAddDto.End_Date,
                Status = HolidayStatus.Accepted,
                Type = requestAddDto.RequestType,
            };

            List<Request> requests = [];
            for (var i = requestAddDto.Begin_Date; i <= requestAddDto.End_Date; i = i.AddDays(1))
            {
                requests.Add(new Request
                {
                    RequestBlockId = RequestBlockNewItem.Id,
                    Date = i,
                    Type = requestAddDto.RequestType,
                });
            }
            
            //SMTP leater need to be added
            var hetvege_munkanap_e = 
                await _dbContext.Preferences
                .FirstOrDefaultAsync(c=>c.Name == "hetvege_munkanap_e") ?? null;
            var workdaycount = 0;
            if (hetvege_munkanap_e == null || hetvege_munkanap_e.Value == false)
            {
                for (var date = requestAddDto.Begin_Date; date <= requestAddDto.End_Date; date = date.AddDays(1))
                {
                    if (!(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        workdaycount++;
                    }
                }
            }
            else
            {
                workdaycount = (requestAddDto.End_Date.DayNumber - requestAddDto.Begin_Date.DayNumber) + 1;
            }
            
            var userRemaineng = await _dbContext.Remaining
                .FirstOrDefaultAsync(c=> c.UserId.ToString() == requestAddDto.UserId) ?? throw new Exception("User does not ha remaining days DB entry");
            if (requestAddDto.RequestType == ReservationType.PTO)
            {
                userRemaineng.RemainingDays -= workdaycount;
            }
            
            
            
            _dbContext.Remaining.Update(userRemaineng);
            await _dbContext.Requests.AddRangeAsync(requests);
            await _dbContext.RequestBlocks.AddAsync(RequestBlockNewItem);
            await _dbContext.SaveChangesAsync();
            return "Successfully created request";
        }
        
        
        
        public async Task<List<PendingRequestBlockDto>> GetPendingRequestByParams(PendingRequestsInputDto pendingRequestsInputDto)
        {
            var query = _dbContext.RequestBlocks
                .Include(l => l.Requests)
                .Include(k => k.User)
                .ThenInclude(l => l.Department)
                .Where(c => pendingRequestsInputDto.DepartmentIds.Contains(c.User.Department.DepartmentName) &&
                            c.Status == HolidayStatus.Pending);

            if (!string.IsNullOrEmpty(pendingRequestsInputDto.inputtext))
            {
                query = query.Where(k=> EF.Functions.Like(k.User.Name, $"%{pendingRequestsInputDto.inputtext}%"));
            }

            var result = await query
                .OrderBy(l => l.StartDate)
                .Select(c => new PendingRequestBlockDto
                    {
                        id = c.Id.ToString(),
                        name = c.User.Name,
                        department = c.User.Department.DepartmentName,
                        begin = c.StartDate,
                        end = c.EndDate,
                    }
                ).ToListAsync();
            
            return result;
        }
        
        public async Task<List<PendingRequestBlockDto>> GetAcceptedRequestByParams(AcceptedRequestsInputDto acceptedRequestsInputDto)
        {
            var query = _dbContext.RequestBlocks
                .Include(l => l.Requests)
                .Include(k => k.User)
                .ThenInclude(l => l.Department)
                .Where(c => acceptedRequestsInputDto.DepartmentIds.Contains(c.User.Department.DepartmentName) &&
                            c.Status == HolidayStatus.Accepted);

            if (!string.IsNullOrEmpty(acceptedRequestsInputDto.inputtext))
            {
                query = query.Where(k=> EF.Functions.Like(k.User.Name, $"%{acceptedRequestsInputDto.inputtext}%"));
            }

            var result = await query
                .OrderBy(l => l.StartDate)
                .Select(c => new PendingRequestBlockDto
                    {
                        id = c.Id.ToString(),
                        name = c.User.Name,
                        department = c.User.Department.DepartmentName,
                        begin = c.StartDate,
                        end = c.EndDate,
                    }
                ).ToListAsync();
            
            return result;
        }

        public async Task<RequestStatsGetDto> GetStatsForRequest(StatsForRequestInputDto inputDto)
        {
            var temp = await _dbContext.RequestBlocks
                           .Include(l => l.User)
                           .ThenInclude(k => k.RemainingDay)
                           .FirstOrDefaultAsync(c => c.Id.ToString() == inputDto.requestBlockId) ??
                       throw new Exception("No requestBlock with this id");
                
            var returndto = new RequestStatsGetDto
            {
                startDate = temp.StartDate.ToString("yyyy-MM-dd"),
                endDate = temp.EndDate.ToString("yyyy-MM-dd"),
                AllHoliday = temp.User.RemainingDay.AllHoliday,
                RemainingDays = temp.User.RemainingDay.RemainingDays,
                TimeProportional = (int)Math.Round(((double)temp.User.RemainingDay.AllHoliday /365) * DateTime.Now.DayOfYear)
            };
            
            var hetvege_munkanap_e = _dbContext.Preferences.FirstOrDefault(c=>c.Name == "hetvege_munkanap_e");
            var workdaycount = 0;
            if (hetvege_munkanap_e == null || hetvege_munkanap_e.Value == false)
            {
                for (var date = temp.StartDate; date <= temp.EndDate; date = date.AddDays(1))
                {
                    if (!(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        workdaycount++;
                    }
                }
            }
            else
            {
                workdaycount = (temp.EndDate.DayNumber - temp.StartDate.DayNumber) + 1;
            }
            
            returndto.requiredDayOff = workdaycount;
            returndto.RemainingDays += workdaycount;
            
            return returndto;
        }
        
        
        public async Task<List<PendingRequestsGet>> GetPendingRequest()
        {
            var pendingRequestBlocks = await _dbContext.RequestBlocks
                .Include(l=>l.Requests)
                .Include(k => k.User)
                .ThenInclude(l=>l.Department)
                .Where(c =>  c.User.Id.ToString() == _aktualisFelhasznaloService.UserId && c.Status== HolidayStatus.Pending )
                .ToListAsync();

            var temp = pendingRequestBlocks.Select(c=>new PendingRequestsGet
            {
                PendingRequestBlock = new PendingRequestBlockDto
                {
                 id = c.Id.ToString(),
                 name = c.User.Name,
                 department = c.User.Department.DepartmentName,
                 begin = c.StartDate,
                 end = c.EndDate,
                },
                Requests = c.Requests.Select(k => new RequestsGetDto
                {
                    Date = k.Date,
                    Type = k.Type,
                    Id = k.Id.ToString(),
                })
                .OrderBy(l=>l.Date)
                    .ToList()
            })
            .OrderBy(c=>c.PendingRequestBlock.begin)
                .ToList();

            return temp;
        }
        
        
        
        public async Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDays()
        {
            var requestsBlocks = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .Where(x => x.UserId.ToString() == _aktualisFelhasznaloService.UserId && x.Status == HolidayStatus.Accepted || x.Status == HolidayStatus.Pending ).ToListAsync() ?? []; //évszám kimaradt, hogy melyik évre kell szűrni
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
        
        public async Task<List<ReservedDaysDto>> GetAllRequestsAndSpecialDaysByUserId(GetRequestsInputDto requestsInputDto)
        {
            var requestsBlocks = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .Where(x => x.UserId.ToString() == requestsInputDto.userId && x.Status == HolidayStatus.Accepted || x.Status == HolidayStatus.Pending ).ToListAsync() ?? []; 
            var specialDays = await _dbContext.SpecialDays.ToListAsync();
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
    
        public async Task<string> MakeDecision(RequestDecisionInputDto requestDecisionInputDto)
        {
            var requestBlock = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .FirstOrDefaultAsync(x => x.Id.ToString() == requestDecisionInputDto.RequestBlockId) ?? throw new Exception("RequestBlock not found");

            requestBlock.Status = requestDecisionInputDto.verdict switch
            {
                true => HolidayStatus.Accepted,
                false => HolidayStatus.Declined
            };
            
            _dbContext.RequestBlocks.Update(requestBlock);
            await _dbContext.SaveChangesAsync();
            return requestDecisionInputDto.verdict switch
            {
                true => "Request accepted succesfully",
                false => "Request declined succesfully"
            };
        }
        
        public async Task<string> RevokeRequest(RevokeRequestInputDto revokeRequestInput)
        {
            var requestBlock = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .FirstOrDefaultAsync(x => x.Id.ToString() == revokeRequestInput.RequestBlockId) ?? throw new Exception("RequestBlock not found");

            requestBlock.Status = HolidayStatus.Revoked;
            
            _dbContext.RequestBlocks.Update(requestBlock);
            await _dbContext.SaveChangesAsync();
            
            return "Request revoked succesfully";
        }
        
        
        public async Task<string> RevokeARequest(RevokeRequestInputDto revokeRequestInput)
        {
            
            var request = await _dbContext.Requests.FirstOrDefaultAsync(c=> c.Id.ToString() == revokeRequestInput.RequestBlockId) ?? throw new Exception("Request not found");
            
            var requestBlock = await _dbContext.RequestBlocks
                .Include(k => k.Requests)
                .FirstOrDefaultAsync(c => c.Requests.Any(p => p.Id.ToString() == revokeRequestInput.RequestBlockId)) ?? throw new Exception("RequestBlock not found");



            var isFirst = request.Date == requestBlock?.StartDate;
            var isLast = request.Date == requestBlock?.EndDate;

            if (requestBlock?.Requests.Count == 1) //In this case it is the last item in the list, we delete both DB entry
            {
                _dbContext.Requests.Remove(request);
                _dbContext.RequestBlocks.Remove(requestBlock);
                
            } else if (isFirst) //If it is the first day of the interval
            {
                requestBlock.StartDate = requestBlock.StartDate.AddDays(1);
                
                _dbContext.Requests.Remove(request);
                _dbContext.RequestBlocks.Update(requestBlock);
                
            }else if (isLast)//If it is the last day of the interval
            {
                requestBlock.EndDate = requestBlock.EndDate.AddDays(-1);
                
                _dbContext.Requests.Remove(request);
                _dbContext.RequestBlocks.Update(requestBlock);
            }
            else
            {
               var originalend = requestBlock.EndDate;
               var originalRequest = requestBlock.Requests.ToList();

               var newRequestBlock = new RequestBlocks //This will be the right part of the interval
               {
                   UserId = requestBlock.UserId,
                   Status = requestBlock.Status,
                   Type = requestBlock.Type,
                   EndDate = originalend,
                   StartDate = request.Date.AddDays(1),
                   Requests = originalRequest.Where(p => p.Date > request.Date)
                       .ToList()
                   //When i revoke do i need to store the modification date? 
               };
               
               //And the original will represent the left part of the interval
               
               requestBlock.EndDate = request.Date.AddDays(-1);
               requestBlock.Requests = originalRequest.Where(
                   l=> l.Date < request.Date)
                   .ToList();
               
               _dbContext.Requests.Remove(request);
               _dbContext.RequestBlocks.Add(newRequestBlock);
               _dbContext.RequestBlocks.Update(requestBlock);
            }
            
            await _dbContext.SaveChangesAsync();
            return "RequestBlock modified succesfully";
        }

        public async Task<string> RevokeWholeRequest(RevokeRequestInputDto revokeRequestInput)
        {
            var temp = await _dbContext.RequestBlocks
                .FirstOrDefaultAsync(c=>c.Id.ToString() == revokeRequestInput.RequestBlockId) 
                       ?? throw new Exception("RequestBlock not found");

            _dbContext.RequestBlocks.Remove(temp);
            await _dbContext.SaveChangesAsync();
            return "RequestBlock removed succesfully";
        }
    }
}

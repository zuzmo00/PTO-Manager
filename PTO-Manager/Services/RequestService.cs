using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTO_Manager.Context;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;

namespace PTO_Manager.Services
{
    public interface IRequestService
    {
        public Task<Guid> CreateRequest(RequestAddDto requestAddDto);
        public Task<Guid> AcceptRequest(Guid id);
        public Task<Guid> RejectRequest(Guid id);
        public Task<List<RequiestGetDto>> GetAllRequests();
    }
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public RequestService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public Task<Guid> AcceptRequest(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateRequest(RequestAddDto requestAddDto)
        {
            var request = await _dbContext.Requests.FirstOrDefaultAsync(x=>x.Date==requestAddDto.Datum);
            if (request != null)
            {
                throw new Exception("Request already exists for this date");
            }
            var newRequest = _mapper.Map<Request>(requestAddDto);
            await _dbContext.Requests.AddAsync(newRequest);
            await _dbContext.SaveChangesAsync();
            return newRequest.Id;
        }

        public Task<List<RequiestGetDto>> GetAllRequests()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> RejectRequest(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

namespace PTO_Manager.DTOs;

public class PendingRequestsGet
{
    public PendingRequestBlockDto PendingRequestBlock { get; set; }
    public List<RequestsGetDto> Requests { get; set; }
}
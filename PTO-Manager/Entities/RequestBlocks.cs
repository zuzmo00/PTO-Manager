using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities;

public class RequestBlocks
{
    [Required,Key]
    public Guid Id { get; set; } = Guid.NewGuid();
        
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; }
        
    public HolidayStatus Status { get; set; }=HolidayStatus.Pending;
    
    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
    
    public ReservationType Type { get; set; } 
        
    public Guid? ModifierUserId { get; set; }
    public DateOnly? ModificationTime { get; set; }

    public List<Request> Requests { get; set; } = [];
}
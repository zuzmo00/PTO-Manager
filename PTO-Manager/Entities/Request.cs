using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PTO_Manager.DTOs.Enums;
using PTO_Manager.Entities.Enums;

namespace PTO_Manager.Entities;

public class Request
{
    [Required,Key]
    public Guid Id { get; set; } = Guid.NewGuid();
        
    [ForeignKey("RequestBlocks")]
    public Guid RequestBlockId { get; set; }
    public RequestBlocks RequestBlock { get; set; }
    public DateOnly Date { get; set; }
    public ReservationType Type { get; set; } //Ezt egyesiteni kellen a DOT-ban hasznalatos Typeokkal mert sokkal kifejezobb az és akkor mar egy helyen van
}
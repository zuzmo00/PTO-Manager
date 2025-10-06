using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities;

public class Reszleg
{
    [Required,Key]
    public int Id { get; set; }
    public string ReszlegNev { get; set; }
    
    List<Szemelyek>? Szemelyek { get; set; }
    List<Ugyintezok>? Ugyintezok { get; set; }
}
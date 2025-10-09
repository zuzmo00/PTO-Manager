using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities;

public class Department
{
    [Required,Key]
    public int Id { get; set; }
    public string ReszlegNev { get; set; }
    
    List<User>? Szemelyek { get; set; }
    List<Admin>? Ugyintezok { get; set; }
}
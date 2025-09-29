using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class Ugyintezok
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Torzsszam { get; set; }
        public string Email { get; set; }
    }
}

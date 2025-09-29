using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class Ugyintezok
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public int Torzsszam { get; set; }
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.Entities
{
    public class Szemelyek
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public int ReszlegId { get; set; }
        public int Jelszo { get; set; }
        public string Email { get; set; }
    }
}

using PTO_Manager.Entities.Enums;
using PTO_Manager.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PTO_Manager.DTOs
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Nev { get; set; }
        public int Torzsszam { get; set; }
        public int ReszlegId { get; set; }
        public string Jelszo { get; set; }
        public int FennmaradoNapok { get; set; }
    }
    public class AdminCreateDto
    {
        [ForeignKey("Szemely")]
        public Guid SzemelyId { get; set; }

        [ForeignKey("Reszleg")]
        public int ReszlegId { get; set; }

        public bool Kerhet { get; set; }
        public bool Biralhat { get; set; }
        public bool Visszavonhat { get; set; }
    }
}

namespace PTO_Manager.DTOs
{
    public class PermissionUpdateDto
    {
        public  Guid id { get; set; }
        public int reszlegId { get; set; }
        public bool kerhet { get; set; }
        public bool biralhat { get; set; }
        public bool visszavonhat { get; set; }
    }
}

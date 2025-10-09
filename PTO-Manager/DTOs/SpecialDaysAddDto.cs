namespace PTO_Manager.DTOs
{
    public class SpecialDaysAddDto
    {
        public DateOnly Date { get; set; }
        public bool IsWorkingDay { get; set; }
    }
    public class  SpecialDaysGetDto
    {
        public DateOnly Date { get; set; }
        public bool IsWorkingDay { get; set; }
    }
}

using PTO_Manager.DTOs.Enums;
using System.Text.Json.Serialization;

namespace PTO_Manager.DTOs
{
    public class Stats
    {
        public int DepartmentId { get; set; }
        public DateOnly Date { get; set; }
    }
    public class  StatsGetDto
    {
        public int Count { get; set; }
        public string Department { get; set; }
        public DateOnly Date { get; set; }
        public int AllPtoWorkers { get; set; }
        public List<RequestTypeStat> RequestTypeStats { get; set; } = [];
    }
    public class  RequestTypeStat
    {
        public int count { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReservationType Type { get; set; }
    }
}

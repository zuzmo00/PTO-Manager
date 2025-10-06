namespace PTO_Manager.Entities
{
    public class Preferenciak
    {
        public Guid Id { get; set; }=Guid.NewGuid();
        public string Name { get; set; }
        public bool Value { get; set; }
    }
}

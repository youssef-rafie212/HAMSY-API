namespace Core.Domain.Entities
{
    public class VirtualRegister
    {
        public string ID { get; set; } = string.Empty;
        public PhysicalRegister? PRegister { get; set; }
        public List<int> Uses { get; set; } = [];
        public string Address { get; set; }
    }
}

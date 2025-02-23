namespace Core.Domain.Entities
{
    public class PhysicalRegister
    {
        public string ID { get; set; } = string.Empty;
        public VirtualRegister? VRegister { get; set; }
        public int NextUse { get; set; }
    }
}

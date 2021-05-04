namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    public class VREngineBetfrom
    {
        public VREngineBetfrom(char id, string name)
        {
            Id = id;
            Name = name;
        }

        public char Id { get; }

        public string Name { get; }
    }
}

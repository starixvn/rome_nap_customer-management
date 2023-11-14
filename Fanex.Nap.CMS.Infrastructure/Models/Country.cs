namespace Fanex.Nap.CMS.Infrastructure.Models
{
    public class Country
    {
        public Country()
        {
        }

        public Country(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }

        public string Name { get; }
    }
}

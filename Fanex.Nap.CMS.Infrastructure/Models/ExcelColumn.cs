namespace Fanex.Nap.CMS.Infrastructure.Models
{
    public sealed class ExcelColumn
    {
        public ExcelColumn(string name)
            : this(name, 0)
        {
        }

        public ExcelColumn(string name, int width)
        {
            Name = name;
            Width = width;
        }

        public string Name { get; private set; }

        public int Width { get; private set; }
    }
}

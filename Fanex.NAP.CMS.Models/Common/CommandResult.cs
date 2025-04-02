namespace Fanex.NAP.CMS.Models.Common
{
    public interface ICommandResult
    {
        dynamic Code { get; }

        dynamic Message { get; }

        bool IsSuccessful { get; }
    }

    public class CommandResult : ICommandResult
    {
        public dynamic Code { get; set; }
        public dynamic Message { get; set; }
        public bool IsSuccessful => Code == 0;
    }
}

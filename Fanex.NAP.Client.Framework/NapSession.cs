namespace Fanex.NAP.Client
{
    using System.Collections.Generic;
    using Fanex.NAP.Client.Session;
    using Models;

    public interface INapSession
    {
        string UserName { get; set; }

        int UserId { get; set; }

        int Level { get; set; }

        int OnUserId { get; set; }

        IList<MenuItem> MenuItemList { get; set; }

        void ClearAll();
    }

    public class NapSession : INapSession
    {
        public const string InternalUsername = "InternalUsername";
        public const string InternalUserId = "InternalUserId";
        public const string InternalOnUserId = "InternalOnUserId";
        public const string InternalNetSessionId = "InternalNetSessionId";
        public const string InternalLevel = "InternalLevel";
        public const string InternalMenuItemList = "InternalMenuItemList";

        private readonly ISessionProvider _sessionProvider;

        public NapSession(ISessionProvider sessionProvider = null)
        {
            _sessionProvider = sessionProvider ?? new DefaultSessionProvider();
        }

        public string UserName
        {
            get => _sessionProvider.GetString(InternalUsername) ?? string.Empty;
            set => _sessionProvider.SetString(InternalUsername, value);
        }

        public int UserId
        {
            get => _sessionProvider.GetInt32(InternalUserId) ?? 0;
            set => _sessionProvider.SetInt32(InternalUserId, value);
        }

        public int OnUserId
        {
            get => _sessionProvider.GetInt32(InternalOnUserId) ?? 0;
            set => _sessionProvider.SetInt32(InternalOnUserId, value);
        }

        public int Level
        {
            get => _sessionProvider.GetInt32(InternalLevel) ?? 0;
            set => _sessionProvider.SetInt32(InternalLevel, value);
        }

        public string SessionId
        {
            get => _sessionProvider.GetString(InternalNetSessionId) ?? string.Empty;
            set => _sessionProvider.SetString(InternalNetSessionId, value);
        }

        public IList<MenuItem> MenuItemList
        {
            get => _sessionProvider.Get<List<MenuItem>>(InternalMenuItemList) ?? new List<MenuItem>();
            set => _sessionProvider.Set(InternalMenuItemList, value);
        }

        public void ClearAll()
        {
            _sessionProvider.Clear();
        }
    }

    public interface ISessionProvider
    {
        string GetString(string key);
        void SetString(string key, string value);
        int? GetInt32(string key);
        void SetInt32(string key, int value);
        T Get<T>(string key);
        void Set<T>(string key, T value);
        void Clear();
    }
}
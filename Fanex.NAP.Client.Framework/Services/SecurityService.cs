namespace Fanex.NAP.Client.Services
{
    using System;
    using System.Threading.Tasks;
    using Clients;
    using Models.ParameterObject;

    public interface ISecurityService
    {
        string GetTime();

        Task<string> GetTimeAsync();

        int Login(LoginParameter parameter);

        Task<int> LoginAsync(LoginParameter parameter);

        void Logout(int onlineUserId);

        Task LogoutAsync(int onlineUserId);

        int ReCheckIn(int onlineUserId, int siteId);

        Task<int> ReCheckInAsync(int onlineUserId, int siteId);

        int SSOLogin(SSOLoginParameter parameter);

        Task<int> SSOLoginAsync(SSOLoginParameter parameter);

        bool UpdatePassword(int siteId, int userId, string password);

        Task<bool> UpdatePasswordAsync(int siteId, int userId, string password);
    }

    public class SecurityService : ISecurityService
    {
        private readonly ILoginClient _loginClient;
        private readonly ILogoutClient _logoutClient;
        private readonly ICheckinClient _checkinClient;
        private readonly ISSOClient _ssoClient;
        private readonly IUserInfoClient _userInfoClient;

        public SecurityService(
            ILoginClient loginClient,
            ILogoutClient logoutClient,
            ICheckinClient checkinClient,
            ISSOClient ssoClient,
            IUserInfoClient userInfoClient)
        {
            _loginClient = loginClient;
            _logoutClient = logoutClient;
            _checkinClient = checkinClient;
            _ssoClient = ssoClient;
            _userInfoClient = userInfoClient;
        }

        public int Login(LoginParameter parameter) => LoginAsync(parameter).Result;

        public async Task<int> LoginAsync(LoginParameter parameter)
        {
            return await _loginClient.Login(parameter).ConfigureAwait(false);
        }

        public void Logout(int onlineUserId) => LogoutAsync(onlineUserId).Wait();

        public async Task LogoutAsync(int onlineUserId)
        {
            await _logoutClient.Logout(onlineUserId).ConfigureAwait(false);
        }

        public int ReCheckIn(int onlineUserId, int siteId) => ReCheckInAsync(onlineUserId, siteId).Result;

        public async Task<int> ReCheckInAsync(int onlineUserId, int siteId)
        {
            return await _checkinClient.ReCheckIn(onlineUserId, siteId).ConfigureAwait(false);
        }

        public string GetTime() => GetTimeAsync().Result;

        public async Task<string> GetTimeAsync()
        {
            return await _ssoClient.GetTime().ConfigureAwait(false);
        }

        public int SSOLogin(SSOLoginParameter parameter) => SSOLoginAsync(parameter).Result;

        public async Task<int> SSOLoginAsync(SSOLoginParameter parameter)
        {
            return await _ssoClient.Login(parameter).ConfigureAwait(false);
        }

        public bool UpdatePassword(int siteId, int userId, string password)
            => UpdatePasswordAsync(siteId, userId, password).Result;

        public async Task<bool> UpdatePasswordAsync(int siteId, int userId, string password)
        {
            return await _userInfoClient.UpdatePassword(siteId, userId, password).ConfigureAwait(false);
        }
    }
}
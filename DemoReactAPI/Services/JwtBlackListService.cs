namespace DemoReactAPI.Services
{
    public class JwtBlackListService
    {
        private readonly HashSet<string> _blackListTokens = new();

        public void AddTokenToBlacklist(string token)
        {
            _blackListTokens.Add(token);
        }

        public bool IsTokenBlackListed(string token)
        {
            return _blackListTokens.Contains(token);
        }
    }
}

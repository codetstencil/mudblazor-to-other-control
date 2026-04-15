namespace Application.Services
{
    public class CustomAuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrWhiteSpace(token))
                    return _anonymous;

                return new AuthenticationState(new ClaimsPrincipal(
                    new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
            }
            catch
            {
                return _anonymous;
            }
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await localStorage.SetItemAsync("authToken", token);

            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task MarkUserAsLoggedOut()
        {
            await localStorage.RemoveItemAsync("authToken");
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}

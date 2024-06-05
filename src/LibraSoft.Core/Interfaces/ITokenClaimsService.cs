using System.Security.Claims;
using LibraSoft.Core.Models;
using LibraSoft.Core.Responses.User;

namespace LibraSoft.Core.Interfaces
{
    public interface ITokenClaimsService
    {
        UserTokensResponse GetTokens(User user);
        UserTokensResponse GetNewAccessToken(User user, string refresh_token);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}

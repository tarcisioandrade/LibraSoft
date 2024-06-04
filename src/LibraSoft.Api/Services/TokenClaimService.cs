using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraSoft.Api.Constants;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Responses.User;
using Microsoft.IdentityModel.Tokens;

namespace LibraSoft.Api.Services
{
    public class TokenClaimService : ITokenClaimsService
    {
        private readonly byte[]? key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
        private readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        public UserTokensResponse GetTokens(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken(claims);

            var tokens = new UserTokensResponse { access_token = accessToken, refresh_token = refreshToken };

            return tokens;
        }

        public UserTokensResponse GetNewAccessToken(User user, string refresh_token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = GenerateAccessToken(claims);

            var tokens = new UserTokensResponse { access_token = accessToken, refresh_token = refresh_token };

            return tokens;
        }

        private string GenerateAccessToken(List<Claim> claims)
        {
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);

            return tokenHandler.WriteToken(accessToken);
        }

        private string GenerateRefreshToken(List<Claim> claims)
        {
            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return tokenHandler.WriteToken(refreshToken);
        }
    }
}

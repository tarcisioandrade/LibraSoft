using System.ComponentModel.DataAnnotations;

namespace LibraSoft.Core.Requests.Authenticate
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh Token is required.")]
        public string Refresh_Token { get; set; } = string.Empty;
    }
}

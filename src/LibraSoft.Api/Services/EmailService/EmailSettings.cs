using System.ComponentModel.DataAnnotations;

namespace LibraSoft.Api.Services.EmailService
{
    public class EmailSettings
    {
        [Required(AllowEmptyStrings = false)]
        public string EmailFrom { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Host { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int Port { get; set; }

        [Required(ErrorMessage = "Credentials is required.")]
        public Credentials Credentials { get; set; }
    }
    public class Credentials
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}

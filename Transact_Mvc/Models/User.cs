using Microsoft.AspNetCore.Identity;

namespace Transact_Mvc.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public int Age {  get; set; }
        public double Balance { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

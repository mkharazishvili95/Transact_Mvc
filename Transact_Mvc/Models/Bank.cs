using Microsoft.AspNetCore.Identity;

namespace Transact_Mvc.Models
{
    public class Bank : IdentityUser
    {
        public int Id { get; set; }
        public double Balance {  get; set; }
        public int UserId {  get; set; }
    }
}

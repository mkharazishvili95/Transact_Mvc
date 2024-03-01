using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transact_Mvc.Helpers;
using Transact_Mvc.Models;

namespace Transact_Mvc.Database
{
    public class TransactContext : IdentityDbContext<User>
    {
        public TransactContext(DbContextOptions<TransactContext>options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Bank {  get; set; }
    }
}

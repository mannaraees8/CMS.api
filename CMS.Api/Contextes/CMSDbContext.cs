
using CMS.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.Api.Contextes
{
    public class CMSDbContext:IdentityDbContext
    {
        public CMSDbContext(DbContextOptions options):base(options) { 
        }
        public DbSet<Customer> Customer { get; set; }
    }
}

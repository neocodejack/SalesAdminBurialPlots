using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SalesAdminPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string AgentCode { get; set; }

        [Required]
        public bool IsMasterAgent { get; set; }

        [Required]
        public bool? IsEnabled { get; set; }

        [Required]
        public bool IsSuperAdmin { get; set; }

        //public ICollection<SalesTransaction> SalesTransactions { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Name", Name));
            userIdentity.AddClaim(new Claim("AgentCode", AgentCode));
            userIdentity.AddClaim(new Claim("IsSalesMaster", IsMasterAgent ? "SM" : "A"));
            userIdentity.AddClaim(new Claim("IsSuperAdmin", IsSuperAdmin ? "M" : ""));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MembershipProvider", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<SalesTransaction> SalesTransactions { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class SalesTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OrderId { get; set; }

        public string PorpSellingPrice { get; set; }

        public string Commission { get; set; }

        public string AgentCode { get; set; }
        
    }
}
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

using System;

namespace ASPNET.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //custom properties added to identiy user
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Gender { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual string Language { get; set; }
        public virtual string Country { get; set; }
        public virtual string PostalCode { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationRole class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationRole : IdentityRole
    {
        //custom properties added to identiy role
        [Display(Name = "System Account")]
        public virtual bool IsSytemAccount { get; set; }

        public async Task<IdentityResult> GenerateRoleIdentityAsync(RoleManager<ApplicationRole> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var roleIdentity = await manager.CreateAsync(this);
            return roleIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationRole>().ToTable("AspNetUserRoles")
            //    .Property(p => p.IsSytemAccount).IsRequired();
        }
    }
}
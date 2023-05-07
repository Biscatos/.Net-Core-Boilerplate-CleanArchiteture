using Core.Domain.Entities.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastruture.Identity.DbConfigurations;

namespace Infrastruture.Identity.Context
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");


            builder.ApplyConfiguration(new RolesConfigurations());
            builder.ApplyConfiguration(new UserConfigurations());
            builder.ApplyConfiguration(new UserRoleConfigurations());
            builder.ApplyConfiguration(new ApplicationRoleClaimConfigurations());
            builder.ApplyConfiguration(new ApplicationUserClaimConfigurations());


            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));

        }
    }
}

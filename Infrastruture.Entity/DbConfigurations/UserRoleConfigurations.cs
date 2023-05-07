using Core.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruture.Identity.DbConfigurations
{
    internal class UserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {

            builder.ToTable("UserRoles");

            builder.HasData(new List<IdentityUserRole<string>> {

                 new IdentityUserRole<string>
                 {
                    RoleId =  StandardValues.SuperAdminRoleId,
                    UserId = StandardValues.SuperAdminUserId,
                 }

            });
        }
    }
}

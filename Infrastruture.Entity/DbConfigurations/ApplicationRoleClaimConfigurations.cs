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
    internal class ApplicationRoleClaimConfigurations : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            builder.ToTable("RoleClaims");


            builder.HasData(new List<IdentityRoleClaim<string>> {
#region SUPERADMIN

               new IdentityRoleClaim<string>{
               Id = 1,
               RoleId =  StandardValues.SuperAdminRoleId,
               ClaimType = StandardRoleClaimNames.StudentManager,
               ClaimValue = "CRUD"
             },
             new IdentityRoleClaim<string>{
               Id = 2,
               RoleId =  StandardValues.SuperAdminRoleId,
               ClaimType = StandardRoleClaimNames.ShoolManager,
               ClaimValue = "CRUD"
             },

             new IdentityRoleClaim<string>{
               Id = 3,
               RoleId =  StandardValues.SuperAdminRoleId,
               ClaimType = StandardRoleClaimNames.TeacherManager,
               ClaimValue = "CRUD"
             },
#endregion
            });
        }
    }
}

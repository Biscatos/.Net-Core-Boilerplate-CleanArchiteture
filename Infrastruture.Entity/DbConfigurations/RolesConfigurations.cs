using Core.Application;
using DocumentFormat.OpenXml.Drawing;
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
    public class RolesConfigurations : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("Roles");

            builder.HasData(new List<IdentityRole> {

                new  IdentityRole {
                   Id = StandardValues.SuperAdminRoleId,
                   Name = "SuperAdmin"
                  ,NormalizedName = "SUPERADMIN"
                },
                new  IdentityRole {
                   Id = StandardValues.AdminRoleId,
                   Name = "Admin"
                  ,NormalizedName = "ADMIN"
                },
                new  IdentityRole {
                   Id = StandardValues.TeacherRoleId,
                   Name = "Teacher"
                  ,NormalizedName = "TEACHER"
                },
                new  IdentityRole
                {
                   Id = StandardValues.SecretaryRoleId,
                   Name = "Secretary"
                  ,NormalizedName = "SECRETARY"
                },
            });
        }
    }
}

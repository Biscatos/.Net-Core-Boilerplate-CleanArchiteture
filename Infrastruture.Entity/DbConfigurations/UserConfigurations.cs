using Core.Application;
using Core.Domain.Entities.System;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruture.Identity.DbConfigurations
{
    internal class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
            builder.Property(p => p.UpdatedAt).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAddOrUpdate();

            builder.HasData(new List<ApplicationUser> {new  ApplicationUser{

                    Id = StandardValues.SuperAdminUserId,
                    UserName = "superadmin",
                    NormalizedUserName = "SUPERADMIN",
                    IsActive = true,
                    Email = "julioreis@outlook.com",
                    NormalizedEmail = "JULIOREISDEV@OUTLOOK.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEDJqaBXTrhcS/cIoQ0sReNB+xJ0bN+n1qsOLFv8TnKubdcvwNT8kd+z7z19LsyUYSQ==",
                    FirstName = "Super",
                    LastName = "Admin",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,

            } });
        }
    }
}

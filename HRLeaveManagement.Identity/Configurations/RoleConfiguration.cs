using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public class RoleConfiguration:IEntityTypeConfiguration<IdentityRole>
{

    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        Guid.NewGuid();
        builder.HasData(
            new IdentityRole
            {
                Id = "6107a220-ed7d-451a-a26a-c8fff0f845eb",
                Name="Employee",
                NormalizedName="EMPLOYEE"
            },
            new IdentityRole
            {
                Id = "c73136f5-9102-4048-b4e4-c6f3756adec8",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
         );
    }
}


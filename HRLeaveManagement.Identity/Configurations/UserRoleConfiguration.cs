using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData
            (
            new IdentityUserRole<string>()
            {
                RoleId= "c73136f5-9102-4048-b4e4-c6f3756adec8",
                UserId= "939b160b-f944-4718-bed9-46d005576bc4"
            },
              new IdentityUserRole<string>()
              {
                  RoleId = "6107a220-ed7d-451a-a26a-c8fff0f845eb",
                  UserId = "76de0fb8-c367-472d-8ae2-d0fe2d5f4546"
              }
            );
    }
}


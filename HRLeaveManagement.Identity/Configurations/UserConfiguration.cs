using System;
using HRLeaveManagement.Domain;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        _ = builder.HasData
            (
                new ApplicationUser
                {
                    Id = "939b160b-f944-4718-bed9-46d005576bc4",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "admin@localhost.com",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    Id = "76de0fb8-c367-472d-8ae2-d0fe2d5f4546",
                    Email = "user@localhost.com",
                    NormalizedEmail = "USER@LOCALHOST.COM",
                    FirstName = "System",
                    LastName = "User",
                    UserName = "user@localhost.com",
                    NormalizedUserName = "USER@LOCALHOST.COM",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true
                }
            );
    }
}


using System;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Identity.DbContext;

public class HRLeaveManagementIdentityDbContext:IdentityDbContext<ApplicationUser>
{
	public HRLeaveManagementIdentityDbContext(DbContextOptions<HRLeaveManagementIdentityDbContext>options):base(options)
	{
	}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(HRLeaveManagementIdentityDbContext).Assembly);
    }
}


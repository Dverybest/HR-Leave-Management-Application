﻿using System;
using System.Text;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.DbContext;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HRLeaveManagement.Identity;

public static class IdentityServicesRegistration
{
	public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

		services.AddDbContext<HRLeaveManagementIdentityDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("HrDatabaseConnectionString"))
		);

		services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<HRLeaveManagementIdentityDbContext>()
			.AddDefaultTokenProviders();

		services.AddTransient<IAuthService, AuthService>();
		services.AddTransient<IUserService, UserService>();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(o =>
		{
			o.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ClockSkew = TimeSpan.Zero,
				ValidIssuer = configuration["JwtSettings:Issuer"],
				ValidAudience = configuration["JwtSettings:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
					configuration["JwtSettings:Key"]))
			};
		});

		return services;
	}
}


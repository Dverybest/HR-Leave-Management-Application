using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.DbContext;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
					configuration["JwtSettings:Key"]!))
			};
			o.Events = new JwtBearerEvents()
			{
				OnChallenge = async context =>
					{
						context.HandleResponse();

						context.Response.ContentType = "application/json";
						context.Response.StatusCode = StatusCodes.Status401Unauthorized;

						await context.Response.WriteAsync(
							JsonSerializer.Serialize(new ProblemDetails()
							{
								Title= nameof(UnauthorizedException),
								Detail="Invalid authourization"
							})
						);
					}
			};
		});

		return services;
	}
}

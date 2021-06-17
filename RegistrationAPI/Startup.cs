﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RegistrationAPI.Models;
using RegistrationAPI.Utilities.Services;
using RegistrationAPI.Utilities.Services.Interface;
using Lib.Net.Http.WebPush;

namespace RegistrationAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//inject appsettings
			services.Configure<PushNotificationsOptions>(Configuration.GetSection("PushNotifications"));
			services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
			services.AddSingleton<IPushSubscriptionsService, PushSubscriptionsService>();
			services.AddHttpClient<PushServiceClient>();
			services.AddHostedService<WeatherNotificationsProducer>();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddDbContext<AuthenticationContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
			services.AddDefaultIdentity<ApplicationUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AuthenticationContext>();
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 3;
			});
			services.AddCors();
			//Jwt Authentication
			var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false; //only requests which is of the type HTTP
				x.SaveToken = false; // after a successful authentication, we don't save this token in server
				x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				};
			});
			services.Configure<FormOptions>(o =>
			{
				o.ValueLengthLimit = int.MaxValue;
				o.MultipartBodyLengthLimit = int.MaxValue;
				o.MemoryBufferThreshold = int.MaxValue;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseCors(builder =>
			builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
			.AllowAnyMethod()
			.AllowAnyHeader()
			);
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
				RequestPath = new PathString("/Resources")
			});

			app.UseAuthentication();
			app.UseMvc();
		}
	}
}

﻿using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.PostgreSql;
using LibraSoft.Api.Constants;
using LibraSoft.Api.Database;
using LibraSoft.Api.Events;
using LibraSoft.Api.Filters;
using LibraSoft.Api.Handlers;
using LibraSoft.Api.Services;
using LibraSoft.Api.Services.EmailService;
using LibraSoft.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LibraSoft.Api
{
    public static class BuildExtension
    {
        static string CorsName = "LibraSoftCors";
        public static void AddDatabaseContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>();
        }

        public static void AddCache(this WebApplicationBuilder builder)
        {
            builder.Services.AddDistributedMemoryCache();
        }

        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.TryAddSingleton<IValidateOptions<EmailSettings>, EmailSettingsValidation>();
            builder.Services.AddOptions<EmailSettings>().BindConfiguration("EmailSettings").ValidateOnStart();
        }

        public static void AddFilters(this WebApplicationBuilder builder)
        {
            builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserHandler, UserHandler>();
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
            builder.Services.AddTransient<IBookHandler, BookHandler>();
            builder.Services.AddTransient<IAuthorHandler, AuthorHandler>();
            builder.Services.AddTransient<IRentHandler, RentHandler>();
            builder.Services.AddTransient<IReviewHandler, ReviewHandler>();
            builder.Services.AddTransient<ILikeHandler, LikeHandler>();
            builder.Services.AddTransient<IBagHandler, BagHandler>();
            builder.Services.AddScoped<ITokenClaimsService, TokenClaimService>();
            builder.Services.AddScoped<ICacheService, DistributedCacheService>();
            builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

            // Services to Hungfire
            builder.Services.AddScoped<CheckReturnRentEvent>();
            builder.Services.AddScoped<CheckUserPunishmentStatus>();
            builder.Services.AddScoped<CheckPickingRentEvent>();

            // Others Events
            builder.Services.AddScoped<ChangePasswordAlertEvent>();
        }

        public static void AddDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                x.CustomSchemaIds(n => n.FullName);
            });
        }

        public static void AddSwagger(this WebApplication app)
        {
                app.UseSwagger();
                app.UseSwaggerUI();
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            string[] origins = { "http://localhost:3000", "https://localhost:3000" };
            

            if (builder.Environment.IsDevelopment() is false)
            {
                var clientUrl = builder.Configuration.GetSection("ClientUrl").Value!;
                origins = [clientUrl];
            }

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsName,
                      policy =>
                      {
                          policy.WithOrigins(origins);
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowCredentials();
                      });
            });
        }

        public static void AddAuthBuilderConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("common", policy => policy.RequireClaim(ClaimTypes.Role, "Common"))
                .AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void AddAuthAppConfiguration(this WebApplication app)
        {
            app.UseCors(CorsName);
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static void AddHangfire(this WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));
            builder.Services.AddHangfireServer();
        }

        public static void UseHangFireDashboard(this WebApplication app)
        {
            app.UseHangfireDashboard("/dashboard");
        }

        public static void AddHanfireEvents(this IServiceProvider serviceProvider)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
            RecurringJob.AddOrUpdate<CheckReturnRentEvent>("CheckReturnRentEvent", eventObj => eventObj.ExecuteSync(), Cron.Daily);
            RecurringJob.AddOrUpdate<CheckUserPunishmentStatus>("CheckUserPunishmentStatus", eventObj => eventObj.ExecuteSync(), Cron.Daily);
            RecurringJob.AddOrUpdate<CheckPickingRentEvent>("CheckPickingRentEvent", eventObj => eventObj.ExecuteSync(), "0 0 0,12 * * MON-FRI");
        }

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            Console.WriteLine(">>>>>>>>>>APLICANDO MIGRATIONS<<<<<<<<<<");
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            appDbContext.Database.Migrate();
        }
    }
}

using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using LibraSoft.Api.Constants;
using LibraSoft.Api.Database;
using LibraSoft.Api.Filters;
using LibraSoft.Api.Handlers;
using LibraSoft.Api.Services;
using LibraSoft.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LibraSoft.Api
{
    public static class BuildExtension
    {
        public static void AddDatabaseContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>();
        }

        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
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
            builder.Services.AddScoped<ITokenClaimsService, TokenClaimService>();
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
            const string CORS_POLICY = "CorsPolicy";

            builder.Services.AddCors(
                options => options.AddPolicy(
                    CORS_POLICY,
                    policy =>
                    {
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    }
                        
                ));
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
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}

using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using LibraSoft.Api.Constants;
using LibraSoft.Api.Data;
using LibraSoft.Api.Filters;
using LibraSoft.Api.Handlers;
using LibraSoft.Api.Services;
using LibraSoft.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddRouting(options => options.LowercaseUrls = true);
services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
services.AddDbContext<AppDbContext>();
services.AddTransient<IUserHandler, UserHandler>();
services.AddScoped<ITokenClaimsService, TokenClaimService>();
services.AddAuthorization(options =>
{
    options.AddPolicy("common", policy => policy.RequireClaim(ClaimTypes.Role, "Common"));
    options.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});
services.AddAuthentication(x =>
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

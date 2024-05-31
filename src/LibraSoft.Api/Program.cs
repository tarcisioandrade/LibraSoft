using System.Text.Json.Serialization;
using LibraSoft.Api.Data;
using LibraSoft.Api.Filters;
using LibraSoft.Api.Handlers;
using LibraSoft.Core.Handlers;

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
services.AddScoped<AppDbContext>();
services.AddTransient<IUserHandler, UserHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

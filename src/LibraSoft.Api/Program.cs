using Hangfire;
using LibraSoft.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddDocumentation();
builder.AddCrossOrigin();
builder.AddConfiguration();
builder.AddHangfire();
builder.AddFilters();
builder.AddDatabaseContext();
builder.AddServices();
builder.AddAuthBuilderConfiguration();
builder.AddCache();

var app = builder.Build();

app.UseHangFireDashboard();
app.Services.AddHanfireEvents();

if (app.Environment.IsDevelopment())
{
    app.AddSwagger();
}

app.UseHttpsRedirection();
app.AddAuthAppConfiguration();
app.MapControllers();

app.Run();

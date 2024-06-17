using LibraSoft.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddDocumentation();
builder.AddConfiguration();
builder.AddFilters();
builder.AddDatabaseContext();
builder.AddServices();
builder.AddAuthBuilderConfiguration();
builder.AddCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.AddSwagger();
}

app.UseHttpsRedirection();
app.AddAuthAppConfiguration();
app.MapControllers();

app.Run();

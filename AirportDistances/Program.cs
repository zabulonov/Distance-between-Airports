//ConfigureServices

using AirportDistances.BusinessLogic;
using AirportDistances.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGetCoordinates, GetCoordinates>();
builder.Services.AddSingleton<Distance>();
builder.Services.AddControllers();
builder.Services.AddHttpClient();


//Configure
var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapGet("/", () => "Hello World!");
app.Run();
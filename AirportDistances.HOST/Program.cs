//ConfigureServices

using AirportDistance.Business;
using AirportDistances.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<AirportDistanceService>();
builder.Services.AddSingleton<AirportInfoService>();
builder.Services.AddControllers();
builder.Services.AddHttpClient<IAirportInfoServiceProxy, AirportInfoServiceProxy>(httpClient =>
{
    var baseAddress = builder.Configuration.GetSection("AirportInfoServiceUrl").Value;
    httpClient.BaseAddress = new Uri(baseAddress);
});
builder.Services.AddMemoryCache();

//Configure
var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapGet("/", () => "Hello World!");
app.Run();


// 1. перенести использование кэша в AirportInfoServiceProxy
// 2. заменить свою формулу на использование библиотеки GeoCoordinates 
// 3. перейти от использования InMemory cache к Redis https://hub.docker.com/_/redis
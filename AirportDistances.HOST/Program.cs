//ConfigureServices

using AirportDistance.Business;
using AirportDistances;
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


// Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "RedisDemos_"; // unique to the app
});


//Configure
var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapGet("/", () => "Hello World!");
app.Run();


// 1. перенести использование кэша в AirportInfoServiceProxy +++
// 2. заменить свою формулу на использование библиотеки GeoCoordinates +++
// 3. перейти от использования InMemory cache к Redis https://hub.docker.com/_/redis +++
// 4. https://docs.docker.com/compose/ +++
// 5. https://docs.docker.com/compose/samples-for-compose/ +++
// 6. https://github.com/docker/awesome-compose/tree/master/aspnet-mssql +++
// 7. создать compose файл, с помощью которого будет выполняться билд\запуск нашего приложения совместно с Redis 
// https://docs.docker.com/get-started/ +++


//План
// 1. создать Dockerfile для самого приложения, научиться запускать приложение в докер-контейнере; 127.0.0.1:8080/api/distanse
// 2. создать compose файл для запуска приложения через docker compose 
// 3. ....
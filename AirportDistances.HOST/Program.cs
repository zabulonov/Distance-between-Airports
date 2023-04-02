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

//1. разделить инфраструктурные модели и модели бизнес-логики +++
//2. поправить формулу / заюзать готовую библиотеку GeoCoordinates +++
//3. почитать про mock/stub ++
//4. написать тесты на GetDistance ++
//5. вынести бизнес-логику в отдельную сборку +++
//6. places-dev.cteleport.com вынести в конфиг +++
//7. сделать чтобы SetCoordinates (с изменённым названием) возвращал инфу по одному аэропорту +++
//8*. попробовать прикрутить кэш, чтобы кэшировать инфу по аэропортам
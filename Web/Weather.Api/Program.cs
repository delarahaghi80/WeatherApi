using Application.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.ExternalService;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<OpenWeatherMapSettings>(
    builder.Configuration.GetSection("OpenWeather"));



// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Weather & Air Quality API",
        Version = "v1",
        Description = "API for retrieving weather and air quality data for cities"
    });
});

// Register application services
builder.Services.AddScoped<IWeatherService, WeatherService>();

// Register infrastructure services
builder.Services.AddHttpClient<IWeatherDataProvider, OpenWeatherMapService>();
builder.Services.AddHttpClient<IAirQualityProvider, OpenWeatherMapService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
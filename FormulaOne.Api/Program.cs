
using System.Text.Json.Serialization;
using FormulaOne.Api.Configurations;
using FormulaOne.Api.Services;
using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Service.Email.Interfaces;
using FormulaOne.Service.Repositories;
using FormulaOne.Service.Repositories.Interfaces;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Lo hemos hecho mapeando una clase con la configuración de la base de datos porque es más limpio y mantenible, ya no tenemos que hacer un publish, solo con cambiar el appsettings.json
var dbConfig = new DatabaseConfig();
builder.Configuration.GetSection("DatabaseConfig").Bind(dbConfig);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString, action =>
    {
        action.CommandTimeout(dbConfig.TimeoutTime);
    });
    options.EnableDetailedErrors(dbConfig
        .DetailedError); // Cuándo estamos en desarrollo es útil para ver en detalle los errores de EF, pero en producción no es recomendable
    options.EnableSensitiveDataLogging(dbConfig
        .SensitiveDataLogging); // Muestra los datos sensibles en los logs, es recomendable desactivarlo en producción
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true; //Gets or sets a value that indicates whether JSON should use pretty printing. By default, JSON is serialized without any extra white space.
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // JsonSerializerOptions.IgnoreNullValues is obsolete. To ignore null values when serializing, set DefaultIgnoreCondition to JsonIgnoreCondition.WhenWritingNull.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Inyectamos en el contenedor de DI nuestros repositorios ubicados dentro de UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMerchService, MerchService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();

//Probando Polly
builder.Services.AddSingleton<IFlightService, FlightService>();

// configuración del hangfire client
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(hangfireConnectionString));

// configuración del hangfire server
builder.Services.AddHangfireServer();

//Enabled CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("blazorApp", policy =>
    {
        policy.WithOrigins("https://localhost:7255")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Habilitando CORS
app.UseCors("blazorApp");

//Habilitando el dashboard de Hangfire
app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello from Hangfire!"), "* * * * *");

app.Run();

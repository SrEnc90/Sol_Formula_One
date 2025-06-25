using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(conf =>
{
    conf.SetKebabCaseEndpointNameFormatter();
    conf.SetInMemorySagaRepositoryProvider();

    var asb = typeof(Program).Assembly;

    conf.AddConsumers(asb);
    conf.AddSagaStateMachines(asb);
    conf.AddSagas(asb);
    conf.AddActivities(asb);
    
    conf.UsingRabbitMq((ctx, cfg) =>
    {
        /*cfg.Host(builder.Configuration["RabbitMq:Host"]);*/
        cfg.Host("localhost", "/", h =>
        {
            h.Username("myuser");
            h.Password("mypass");
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();



// para crear un proyecto de net8 pero con controladores:
// dotnet new  webapi -controllers -n FIA.Api -f net8.0, después hay que agregar el proyecto al sln de la solución general

// para referencias desde el FIA.Api al Formula.Entities
// $ dotnet add FIA.Api/FIA.Api.csproj reference  FormulaOne.Entities/FormulaOne.Entities.csproj

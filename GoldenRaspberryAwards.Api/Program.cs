using GoldenRaspberryAwards.Api.Helpers;
using GoldenRaspberryAwards.Repository;
using GoldenRaspberryAwards.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(provider =>
{
    var connection = new SqliteConnection("Data Source=:memory:");
    connection.Open();
    return connection;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerInjections();  
builder.Services.AddRepositoriesInjectios();
builder.Services.AddServicesInjectios();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var movieRepository = scope.ServiceProvider.GetRequiredService<IMovieRepository>();
}

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", $"Intervalo de prêmios - Api v{Assembly.GetEntryAssembly().GetName().Version}");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




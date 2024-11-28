using GoldenRaspberryAwards.Repository;
using GoldenRaspberryAwards.Repository.Interfaces;
using GoldenRaspberryAwards.Services;
using GoldenRaspberryAwards.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


namespace GoldenRaspberryAwards.Api.Helpers
{
    public static class DependencyInjectionExtensions
    {
       
        //Repositiry
        public static void AddRepositoriesInjectios(this IServiceCollection services)
        {
            services.AddTransient<IMovieRepository, MovieRepository>();
        }


        //Services
        public static void AddServicesInjectios(this IServiceCollection services)
        {
            services.AddTransient<IMovieService, MovieService>();
        }

        //Swagger
        public static void AddSwaggerInjections(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName.Replace("+", "_"));
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Golden Raspberry Awards - Api v1",
                    Description = "Intervalo de prêmios."
                });

            });
        }
    }
}

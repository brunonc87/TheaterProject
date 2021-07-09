using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using Theater.Application.Credentials;
using Theater.Application.Movies;
using Theater.Application.Rooms;
using Theater.Application.Sections;
using Theater.Domain.Credentials;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;
using Theater.Infra.Data.Common;
using Theater.Infra.Data.Repositories;

namespace Theater.Api.Dependencies
{
    public static class ServicesDependency
    {
        public static void AddServiceDependency(this IServiceCollection services)
        {
            services.AddDbContext<TheaterContext>(option => option.UseSqlServer(DatabaseConfiguration.GetConnectionString()));

            services.AddTransient<IMoviesRepository, MoviesRepository>();
            services.AddTransient<ICredentialsRepository, CredentialsRepository>();
            services.AddTransient<IRoomsRepository, RoomsRepository>();
            services.AddTransient<ISectionsRepository, SectionsRepository>();

            services.AddTransient<IMoviesService, MoviesService>();
            services.AddTransient<ICredentialsService, CredentialsService>();
            services.AddTransient<IRoomsService, RoomsService>();
            services.AddTransient<ISectionsService, SectionsService>();
        }

       
    }
}

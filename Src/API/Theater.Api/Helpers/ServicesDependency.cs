using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Theater.Api.Filters;
using Theater.Application;
using Theater.Domain.Credentials;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;
using Theater.Infra.Data.Common;
using Theater.Infra.Data.Repositories;

namespace Theater.Api.Helpers
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

            services.AddControllers(options => options.Filters.Add(typeof(CustomValidationFilter)))
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(AppModule).Assembly));

            services.AddAutoMapper(typeof(AppModule).Assembly);

            services.AddMediatR(typeof(AppModule).Assembly);
        }
    }
}

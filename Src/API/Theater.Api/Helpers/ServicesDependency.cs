using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Theater.Api.Filters;
using Theater.Application;
using Theater.Domain.Credentials;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;
using Theater.Infra.Data.Common;
using Theater.Infra.Data.Repositories;
using Theater.Infra.Settings;

namespace Theater.Api.Helpers
{
    public static class ServicesDependency
    {
        public static void AddServiceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TheaterContext>(option => option.UseSqlServer(configuration.GetConnectionString("TheaterDatabase")));

            services.AddTransient<IMoviesRepository, MoviesRepository>();
            services.AddTransient<ICredentialsRepository, CredentialsRepository>();
            services.AddTransient<IRoomsRepository, RoomsRepository>();
            services.AddTransient<ISectionsRepository, SectionsRepository>();

            services.AddControllers(options => options.Filters.Add(typeof(CustomValidationFilter)))
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(AppModule).Assembly));

            services.AddAutoMapper(typeof(AppModule).Assembly);

            services.AddMediatR(typeof(AppModule).Assembly);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Settings.Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
                
        }
    }
}

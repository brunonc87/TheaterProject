using Microsoft.Extensions.DependencyInjection;


namespace Theater.Api.Helpers
{
    public static class ApplicationMapper
    {
        public static void AddApplicationMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Application.Movies.MappingProfile));
            services.AddAutoMapper(typeof(Application.Rooms.MappingProfile));
            services.AddAutoMapper(typeof(Application.Sections.MappingProfile));
            services.AddAutoMapper(typeof(Application.Credentials.MappingProfile));
        }
    }
}

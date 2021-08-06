using AutoMapper;
using Theater.Application.Movies.Commands;
using Theater.Application.Movies.Models;
using Theater.Domain.Movies;

namespace Theater.Application.Movies
{
    public class MoviesMappingProfile : Profile
    {
        public MoviesMappingProfile()
        {
            CreateMap<Movie, MovieModel>().ForMember(mm => mm.Id, map => map.MapFrom(m => m.MovieID));
            CreateMap<MovieAddCommand, Movie>();
            CreateMap<MovieUpdateCommand, Movie>().ForMember(m => m.MovieID, map => map.MapFrom(mm => mm.ID));
        }
    }
}

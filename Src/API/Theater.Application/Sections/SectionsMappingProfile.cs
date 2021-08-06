using AutoMapper;
using Theater.Application.Sections.Commands;
using Theater.Application.Sections.Models;
using Theater.Domain.Sections;

namespace Theater.Application.Sections
{
    public class SectionsMappingProfile : Profile
    {
        public SectionsMappingProfile()
        {
            CreateMap<Section, SectionModel>().ForMember(sm => sm.ID, map => map.MapFrom(s => s.SectionID))
                                              .ForMember(sm => sm.AnimationType, map => map.MapFrom(s => s.AnimationType.GetName()))
                                              .ForMember(sm => sm.MovieTittle, map => map.MapFrom(s => s.Movie.Tittle))
                                              .ForMember(sm => sm.Duration, map => map.MapFrom(s => s.Movie.Duration))
                                              .ForMember(sm => sm.RoomName, map => map.MapFrom(s => s.Room.Name))
                                              .ForMember(sm => sm.NumberOfSeats, map => map.MapFrom(s => s.Room.SeatsNumber));

            CreateMap<SectionAddCommand, Section>().ForMember(s => s.AnimationType, map => map.MapFrom(sc => ParseAnimationType(sc.AnimationType)))
                                                   .AfterMap((sc, s) => { s.Movie = new Domain.Movies.Movie { Tittle = sc.MovieTittle }; })
                                                   .AfterMap((sc, s) => { s.Room = new Domain.Rooms.Room { Name = sc.RoomName}; });
        }


        private AnimationType ParseAnimationType(string animationType)
        {
            switch (animationType.ToUpper())
            {
                case "3D":
                    return Domain.Sections.AnimationType.D3;
                case "2D":
                default:
                    return Domain.Sections.AnimationType.D2;

            }
        }
    }
}

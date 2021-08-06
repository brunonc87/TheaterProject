using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Sections.Commands;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;

namespace Theater.Application.Sections.Handlers
{
    public class AddSectionHandler : IRequestHandler<SectionAddCommand, bool>
    {
        private readonly ISectionsRepository _sectionRepository;
        private readonly IMoviesRepository _movieRepository;
        private readonly IRoomsRepository _roomRepository;
        private readonly IMapper _mapper;

        public AddSectionHandler(ISectionsRepository sectionRepository, IMoviesRepository movieRepository, IRoomsRepository roomRepository, IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _movieRepository = movieRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public Task<bool> Handle(SectionAddCommand request, CancellationToken cancellationToken)
        {
            Movie movieOnDB = _movieRepository.GetByTittle(request.MovieTittle);

            if (movieOnDB == null)
                throw new Exception("Filme não localizado");

            Room roomOnDB = _roomRepository.GetByName(request.RoomName);

            if (roomOnDB == null)
                throw new Exception("Sala não localizada");

            Section section = _mapper.Map<Section>(request);
            section.Movie = movieOnDB;
            section.Room = roomOnDB;

            List<Section> sectionsOnDB = _sectionRepository.GetAll().ToList();

            if (sectionsOnDB.Where(s => s.Room.RoomID == section.Room.RoomID &&
                                 ((s.StartDate < section.StartDate && s.FinishDate > section.StartDate) ||
                                  (s.StartDate < section.FinishDate && s.FinishDate > section.FinishDate))).Any())
                throw new Exception("Não é possivel criar uma seção no horario que já existe outra seção.");

            return Task.FromResult(_sectionRepository.Insert(section));
        }
    }
}

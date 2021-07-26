using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;

namespace Theater.Application.Sections
{
    public class SectionsService : ISectionsService
    {
        private readonly ISectionsRepository _sectionRepository;
        private readonly IMoviesRepository _movieRepository;
        private readonly IRoomsRepository _roomRepository;

        public SectionsService(ISectionsRepository sectionRepository, IMoviesRepository movieRepository, IRoomsRepository roomRepository)
        {
            _sectionRepository = sectionRepository;
            _movieRepository = movieRepository;
            _roomRepository = roomRepository;
        }

        public void AddSection(Section section)
        {
            section.Validate();

            Movie movieOnDB = _movieRepository.GetByTittle(section.Movie.Tittle);

            if (movieOnDB == null)
                throw new Exception("Filme não localizado");

            Room roomOnDB = _roomRepository.GetByName(section.Room.Name);

            if (roomOnDB == null)
                throw new Exception("Sala não localizada");

            section.Movie = movieOnDB;
            section.Room = roomOnDB;

            List<Section> sectionsOnDB = _sectionRepository.GetAll().ToList();

            if (sectionsOnDB.Where(s => s.Room.RoomID == section.Room.RoomID && 
                                 ((s.StartDate < section.StartDate && s.FinishDate > section.StartDate) ||
                                  (s.StartDate < section.FinishDate && s.FinishDate > section.FinishDate))).Any())
                throw new Exception("Não é possivel criar uma seção no horario que já existe outra seção.");

            _sectionRepository.Insert(section);
        }

        public void RemoveSection(int sectionID)
        {
            Section sectionOnDB = _sectionRepository.GetByID(sectionID);

            if (!sectionOnDB.CanBeDeleted())
                throw new Exception("Não é possível remover uma seção se estiver tão proxima de ocorrer.");

            _sectionRepository.Delete(sectionID);
        }

        public IEnumerable<Section> GetSections()
        {
            return _sectionRepository.GetAll();
        }
    }
}

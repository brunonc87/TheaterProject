using MediatR;

namespace Theater.Application.Movies.Commands
{
    public class MovieDeleteCommand : IRequest<bool>
    {
        public MovieDeleteCommand(string tittle)
        {
            MovieTittle = tittle;
        }

        public string MovieTittle { get; set; }
    }
}

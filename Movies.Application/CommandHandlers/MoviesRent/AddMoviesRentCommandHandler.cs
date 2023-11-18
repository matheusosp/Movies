using AutoMapper;
using MediatR;
using Movies.Application.Commands;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Application.Commands.MoviesRent;

namespace Movies.Application.CommandHandlers.MoviesRent
{
    public class AddMoviesRentCommandHandler : IRequestHandler<AddMoviesRentCommand, ICommandResult>
    {
        private readonly IMoviesRentRepository _moviesRentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandResult _result;
        private readonly IMovieRepository _movieRepository;

        public AddMoviesRentCommandHandler(IMoviesRentRepository moviesRentRepository, IMapper mapper,
            IUnitOfWork unitOfWork, ICommandResult commandResult, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _result = commandResult;
            _unitOfWork = unitOfWork;
            _movieRepository = movieRepository;
            _moviesRentRepository = moviesRentRepository;
        }
        public async Task<ICommandResult> Handle(AddMoviesRentCommand request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository
                                            .GetAllByWithTracking(m => request.MoviesIds.Contains(m.Id), cancellationToken);

            var moviesRent = new MovieRent
            {
                Movies = movies,
                CPFClient = request.CPFClient,
                RentDate = DateTime.Now
            };

            await _moviesRentRepository.CreateMovieRent(moviesRent, cancellationToken);

            return await _unitOfWork.SaveChangesAsync(cancellationToken) != 0
                ? _result.Ok()
                : _result.Fail(BusinessErrors.FailToCreateMoviesRent.ToString());
        }
    }
}

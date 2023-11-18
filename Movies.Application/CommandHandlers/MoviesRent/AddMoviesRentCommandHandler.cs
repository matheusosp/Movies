using AutoMapper;
using MediatR;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using System;
using System.Linq;
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
            //Sem filmes repetidos em uma unica locação
            var uniqueMovieIds = request.MoviesIds.Distinct().ToList();

            var movies = await _movieRepository
                .GetAllByWithTracking(m => uniqueMovieIds.Contains(m.Id), cancellationToken);

            var moviesRent = new MovieRent
            {
                Movies = movies,
                CPFClient = request.CPFClient,
                RentDate = DateTime.Now
            };
            
            await _unitOfWork.BeginDatabaseTransactionAsync(cancellationToken);
            await _moviesRentRepository.CreateMovieRent(moviesRent, cancellationToken);

            if (await _unitOfWork.SaveChangesWithTransactionAsync(cancellationToken) == 0)
                return _result.Fail(BusinessErrors.FailToCreateMoviesRent.ToString());

            await _unitOfWork.CommitDatabaseTransactionAsync(cancellationToken);

            return _result.Ok();
        }
    }
}

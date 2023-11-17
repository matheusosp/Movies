using MediatR;
using Movies.Application.Commands;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;

namespace Movies.Application.CommandHandlers
{
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, ICommandResult>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandResult _result;

        public AddMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper, IUnitOfWork unitOfWork,
            ICommandResult commandResult)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _result = commandResult;
        }
        public async Task<ICommandResult> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = _mapper.Map<Movie>(request);

            await _movieRepository.CreateMovie(movie, cancellationToken);

            return await _unitOfWork.SaveChangesAsync(cancellationToken) != 0
                ? _result.Ok()
                : _result.Fail(BusinessErrors.FailToCreateDefi.ToString());
        }
    }
}

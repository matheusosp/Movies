using AutoMapper;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.CommandHandlers.Genders
{
    public class BaseGenderHandler : IBaseGenderHandler
    {
        public BaseGenderHandler(IGenderRepository genderRepository, IMapper mapper,
            IUnitOfWork unitOfWork, ICommandResult commandResult)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            GenderRepository = genderRepository;
            Result = commandResult;
        }

        public IMapper Mapper { get; }
        public IGenderRepository GenderRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public ICommandResult Result { get; }
    }
}

using AutoMapper;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.CommandHandlers.Genders
{
    public interface IBaseGenderHandler
    {
        IMapper Mapper { get; }
        IGenderRepository GenderRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        ICommandResult Result { get; }
    }
}

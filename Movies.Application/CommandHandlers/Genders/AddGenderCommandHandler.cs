using MediatR;
using Movies.Application.Commands;
using Movies.Application.Commands.Gender;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Application.CommandHandlers.Genders
{
    public class AddGenderCommandHandler : IRequestHandler<AddGenderCommand, ICommandResult>
    {
        private readonly IBaseGenderHandler _base;

        public AddGenderCommandHandler(IBaseGenderHandler baseGenderHandler)
        {
            _base = baseGenderHandler;
        }
        public async Task<ICommandResult> Handle(AddGenderCommand request, CancellationToken cancellationToken)
        {
            var gender = _base.Mapper.Map<Gender>(request);

            await _base.GenderRepository.CreateGender(gender, cancellationToken);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) != 0
                ? _base.Result.Ok()
                : _base.Result.Fail(BusinessErrors.FailToCreateGender.ToString());
        }
    }
}

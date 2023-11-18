using FluentValidation;
using MediatR;
using Movies.Domain.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Movies.Application.Validators
{
    //A ideia aqui é diminuir codigo que ficaria no handler de cada command, aqui ja faz para todos os comandos que tiver implementação de validação
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICommandResult _commandResult;
        private readonly ILogger<TRequest> _logger;
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(ILogger<TRequest> logger, ICommandResult commandResult,
            IValidator<TRequest> validator = null)
        {
            _validator = validator;
            _logger = logger;
            _commandResult = commandResult;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling request: {@Request}", request);
            if (_validator == null) return await next();
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (result == null || result.IsValid) return await next();

            var returnType = next.Method.ReturnType; //Task<CommandResult | GenericCommandResult<T>>
            var returnHandlerResult = ReturnHandlerResult(returnType, string.Join('\n', result.Errors));

            return returnHandlerResult;
        }

        private TResponse ReturnHandlerResult(Type returnType, string errors)
        {
            var resultType = returnType.GetGenericArguments()[0]; //CommandResult | GenericCommandResult<T>
            if (resultType == typeof(CommandResult) || resultType == typeof(ICommandResult))
                return (TResponse)_commandResult.Fail(errors);

            //GenericCommandResult
            var genericTypeDefinition = resultType.GetGenericTypeDefinition(); //<T>
            if (genericTypeDefinition != typeof(GenericCommandResult<>))
                return (TResponse)_commandResult.Fail(errors);

            var genericCommandResultType = resultType.GetGenericArguments()[0]; //T
            var commandResult =
                typeof(GenericCommandResult<>).MakeGenericType(genericCommandResultType); //GenericCommandResult<T>
            var genericCommandResultInstance =
                Activator.CreateInstance(commandResult, false, null, errors);
            return (TResponse)genericCommandResultInstance;
        }
    }
}

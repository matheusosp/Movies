using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Interfaces;

namespace Movies.Domain.Generic
{
    public class CommandResult : ICommandResult
    {

        public CommandResult()
        {
        }

        public CommandResult(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public string Error { get; set; }
        public bool Success { get; set; }

        public ICommandResult Ok()
        {
            return new CommandResult(true, string.Empty);
        }

        public ICommandResult OkOrFailure(bool success, string errors)
        {
            return success ? Ok() : Fail(errors);
        }

        public ICommandResult Fail(string errors)
        {
            return new CommandResult(false, errors);
        }
    }
}

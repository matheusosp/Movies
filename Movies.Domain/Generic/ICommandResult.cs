using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Interfaces
{
    public interface ICommandResult
    {
        string Error { get; set; }
        bool Success { get; set; }
        ICommandResult Ok();
        ICommandResult OkOrFailure(bool success, string errors);
        ICommandResult Fail(string errors);
    }
}

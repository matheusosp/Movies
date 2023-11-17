using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Interfaces
{
    public interface IGenericCommandResult<T>
    {
        bool Success { get; }
        string Error { get; }
        T Value { get; }
        IGenericCommandResult<T> Ok(T value);
        IGenericCommandResult<T> OkOrFailure(bool success, string errors, T value = default);
        IGenericCommandResult<T> Fail(string errors, T value = default);
    }
}

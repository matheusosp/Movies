using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Generic
{
    public class GenericCommandResult<T> : CommandResult, IGenericCommandResult<T>
    {
        public T Value { get; }
        public GenericCommandResult() : base()
        {
            
        }
        private GenericCommandResult(bool success, T value, string error) : base(success, error)
        {
            Value = value;
        }
        public IGenericCommandResult<T> Ok(T value)
        {
            return new GenericCommandResult<T>(true, value, string.Empty);
        }

        public IGenericCommandResult<T> OkOrFailure(bool success, string errors, T value = default)
        {
            return success ? Ok(value) : Fail(errors, value);
        }

        public IGenericCommandResult<T> Fail(string errors, T value = default)
        {
            return new GenericCommandResult<T>(false, value, errors);
        }
    }
}

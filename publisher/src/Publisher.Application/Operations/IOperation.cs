using System;
using System.Threading.Tasks;
using Publisher.Domain;

namespace Publisher.Application.Operations
{
    public interface IOperation<T>
    {

        Task<Result> ExecuteAsync(T request);

    }
}

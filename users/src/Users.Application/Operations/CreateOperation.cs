using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts;
using Users.Domain;
using Users.Infrastructure;

namespace Users.Application.Operations
{
    public class CreateOperation : IOperation<User>
    {
        private readonly IUserAggregateStore _store;
        private readonly IReadOnlyUserRepository _repository;

        public CreateOperation(IUserAggregateStore store, IReadOnlyUserRepository repository)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result> ExecuteAsync(User operation, CancellationToken cancellation = default)
        {
            var root = _store.Create();

            if (root.Create(operation.Email, operation.FirstName, 
                operation.LastNames, operation.BirthDay) is ErrorResult error)
            {
                return error;
            }

            if (await _repository.EmailExistAsync(operation.Email, cancellation))
            {
                return DomainError.UserError.EmailAlreadyExist;
            }

            await _store.SaveAsync(root, cancellation);
            return Result.Ok(new User
            {
                Id = root.State.Id,
                Email = operation.Email,
                FirstName = operation.FirstName,
                LastNames = operation.LastNames,
                BirthDay = operation.BirthDay
            });
        }
    }
}

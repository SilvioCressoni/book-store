using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Domain;
using Users.Infrastructure;

namespace Users.Application.Operations
{
    public class UserGetAllOperation : IOperation<UserGetAll>
    {
        private readonly IUserRepository _repository;

        public UserGetAllOperation(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ValueTask<Result> ExecuteAsync(UserGetAll operation, CancellationToken cancellation = default)
        {
            if (operation.Take < 0)
            {
                return new ValueTask<Result>(DomainError.GetError.InvalidTake);
            }

            if (operation.Skip < 0)
            {
                return new ValueTask<Result>(DomainError.GetError.InvalidSkip);
            }

            if (operation.Take == 0)
            {
                operation.Take = 100;
            }

            var collection = new UserCollection(_repository, operation.Skip, operation.Take);
            return new ValueTask<Result>(Result.Ok<IEnumerable<User>>(collection));
        }

        private class UserCollection : IEnumerable<User>
        {
            private readonly IUserRepository _repository;
            private readonly int _skip;
            private readonly int _take;

            public UserCollection(IUserRepository repository, int take, int skip)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _take = take;
                _skip = skip;
            }

            public IEnumerator<User> GetEnumerator()
            {
                foreach (var user in _repository.GetAll(_skip, _take))
                {
                    yield return new User
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastNames = user.LastNames,
                        BirthDay = user.BirthDay,
                        Phones = user.Phones.Select(x => new Phone
                        {
                            Number = x.Number
                        }),
                        Addresses = user.Addresses.Select(x => new Address
                        {
                            Id = x.Id,
                            Line = x.Line,
                            Number = x.Number,
                            PostCode = x.PostCode
                        })
                    };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() 
                => GetEnumerator();
        }
    }
}

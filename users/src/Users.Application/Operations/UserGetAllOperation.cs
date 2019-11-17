using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Contracts.Request;
using Users.Application.Contracts.Response;
using Users.Application.Mapper;
using Users.Domain;
using Users.Infrastructure;

namespace Users.Application.Operations
{
    public class UserGetAllOperation : IOperation<UserGetAll>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper<Domain.Common.User, User> _mapper;

        public UserGetAllOperation(IUserRepository repository, 
            IMapper<Domain.Common.User, User> mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

            var collection = new UserCollection(_repository, operation.Skip, operation.Take, _mapper);
            return new ValueTask<Result>(Result.Ok<IEnumerable<User>>(collection));
        }

        private class UserCollection : IEnumerable<User>
        {
            private readonly IUserRepository _repository;
            private readonly IMapper<Domain.Common.User, User> _mapper;
            private readonly int _skip;
            private readonly int _take;

            public UserCollection(IUserRepository repository, 
                int take, int skip, 
                IMapper<Domain.Common.User, User> mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _take = take;
                _skip = skip;
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public IEnumerator<User> GetEnumerator() 
                => _repository.GetAll(_skip, _take).Select(user => _mapper.Map(user)).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() 
                => GetEnumerator();
        }
    }
}

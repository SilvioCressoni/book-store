using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserGetOperation> _logger;

        public UserGetAllOperation(IUserRepository repository, 
            IMapper<Domain.Common.User, User> mapper, 
            ILogger<UserGetOperation> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ValueTask<Result> ExecuteAsync(UserGetAll operation, CancellationToken cancellation = default)
        {
            var scope = _logger.BeginScope("Get All user. [Take: {0}][Skip: {1}]", 
                operation.Take, operation.Skip);
            try
            {
                if (operation.Take < 0)
                {
                    _logger.LogInformation("Invalid Take: {0}", operation.Take);
                    return new ValueTask<Result>(DomainError.GetError.InvalidTake);
                }

                if (operation.Skip < 0)
                {
                    _logger.LogInformation("Invalid Skip: {0}", operation.Skip);
                    return new ValueTask<Result>(DomainError.GetError.InvalidSkip);
                }

                if (operation.Take == 0)
                {
                    operation.Take = 100;
                }

                var collection = new UserCollection(_repository, operation.Skip, operation.Take, _mapper);
                _logger.LogInformation("GET All user with success");
                return new ValueTask<Result>(Result.Ok<IEnumerable<User>>(collection));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception: ");
                return new ValueTask<Result>(Result.Fail(e));
            }
            finally
            {
                scope.Dispose();
            }
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

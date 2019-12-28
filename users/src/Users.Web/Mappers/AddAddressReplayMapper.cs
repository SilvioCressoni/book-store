using System;
using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;
using AddressApplication = Users.Application.Contracts.Response.Address;

namespace Users.Web.Mappers
{
    public class AddAddressReplayMapper : IMapper<Result, AddAddressReplay>
    {
        private readonly IMapper<AddressApplication, Address> _mapper;

        public AddAddressReplayMapper(IMapper<AddressApplication, Address> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public AddAddressReplay Map(Result source)
        {
            var replay = new AddAddressReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description ?? string.Empty,
                ErrorCode = source.ErrorCode ?? string.Empty
            };

            if (source is OkResult<AddressApplication> okResult)
            {
                replay.Value = _mapper.Map(okResult.Value);
            }

            return replay;
        }
    }
}

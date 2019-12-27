using System;
using System.Collections.Generic;
using Users.Application.Mapper;
using Users.Domain;
using Users.Web.Proto;
using AddressApplication = Users.Application.Contracts.Response.Address;

namespace Users.Web.Mappers
{
    public class GetAddressesReplayMapper : IMapper<Result, GetAddressesReplay>
    {
        private readonly IMapper<AddressApplication, Address> _mapper;

        public GetAddressesReplayMapper(IMapper<AddressApplication, Address> phoneMapper)
        {
            _mapper = phoneMapper ?? throw new ArgumentNullException(nameof(phoneMapper));
        }

        public GetAddressesReplay Map(Result source)
        {
            var replay = new GetAddressesReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };

            if (source is OkResult<IEnumerable<AddressApplication>> okResult)
            {
                foreach (var phone in okResult.Value)
                {
                    replay.Value.Add(_mapper.Map(phone));
                }
            }

            return replay;
        }
    }
}

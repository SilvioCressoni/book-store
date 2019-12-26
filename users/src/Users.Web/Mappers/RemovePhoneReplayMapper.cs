﻿using Users.Application.Mapper;
using Users.Domain;

namespace Users.Web.Mappers
{
    public class RemovePhoneReplayMapper : IMapper<Result, RemovePhoneReplay>
    {
        public RemovePhoneReplay Map(Result source)
        {
            return new RemovePhoneReplay
            {
                IsSuccess = source.IsSuccess,
                Description = source.Description,
                ErrorCode = source.ErrorCode
            };
        }
    }
}

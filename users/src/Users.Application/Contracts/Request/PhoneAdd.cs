﻿using System;

namespace Users.Application.Contracts.Request
{
    public class PhoneAdd
    {
        public Guid UserId { get; set; }
        public string Number { get; set; }
    }
}

﻿using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record UserDTO : IEntity
    {
        public int Id { get; private init; }
        public string Login { get;  init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public DateTime? DateOfBirth { get; init; }

    }
}

using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record UserDTO : IEntity
    {
        public int Id { get; private set; }
        public string Login { get;  set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}

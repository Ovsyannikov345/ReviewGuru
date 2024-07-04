using ReviewGuru.BLL.Services;
using ReviewGuru.BLL.Services.IServices;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record AuthorDTO : IEntity
    {
        public int Id { get; private set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

}

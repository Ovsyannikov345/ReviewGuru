using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record MediaAuthorDTO : IEntity
    {
        public int Id { get; private set; }
        public int? MediaId { get;  set; }
        public int? AuthorId { get; set; }
    }
}

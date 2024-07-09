using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record MediaAuthorDTO
    {
        public int MediaAuthorId { get;  init; }
        public int? MediaId { get;  init; }
        public int? AuthorId { get; init; }
        public MediaDTO? MediaDTO { get; init; }
        public AuthorDTO? AuthorDTO { get; init; }
    }
}

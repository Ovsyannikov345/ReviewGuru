using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record MediaDTO
    {
        public int MediaId { get;  init; }
        public string MediaType { get; init; }
        public string Name { get; init; }
        public List<MediaAuthorDTO>? MediaAuthors { get; set; } = [];
        public List<ReviewDTO> Reviews { get; set; } = [];

    }

}

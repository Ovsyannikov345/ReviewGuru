using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
#pragma warning disable S101 // Types should be named in PascalCase
    public record MediaDTO
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public int MediaId { get; init; }

        public string MediaType { get; init; } = "";

        public string Name { get; init; } = "";

        public DateOnly YearOfCreating { get; set; }

        public virtual ICollection<AuthorDTO> AuthorDTO { get; set; } = [];

        public virtual ICollection<ReviewDTO> ReviewDTO { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<UserDTO> UserDTO { get; set; } = [];
    }
}

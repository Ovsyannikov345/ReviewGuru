using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System.Text.Json.Serialization;

namespace ReviewGuru.BLL.DTOs
{
#pragma warning disable S101 // Types should be named in PascalCase
    public record AuthorDTO
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public int AuthorId { get; init; }

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public ICollection<MediaDTO> MediaDTO { get; set; } = [];
    }
}

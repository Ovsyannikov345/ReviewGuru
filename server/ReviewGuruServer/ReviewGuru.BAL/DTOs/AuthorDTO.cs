using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System.Text.Json.Serialization;

namespace ReviewGuru.BLL.DTOs
{
    public record AuthorDTO
    {
        public int AuthorId { get;  init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public ICollection<MediaDTO> MediaDTO { get; set; } = [];
    }

}

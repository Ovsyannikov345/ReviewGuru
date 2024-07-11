using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record ReviewToCreateDTO
    {
        public int ReviewId { get; init; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int Rating { get; init; }
        public string UserReview { get; init; } = string.Empty;
        public DateTime DateOfCreation { get; private init; } = DateTime.Now;
        public DateTime? DateOfLastModification { get; private set; }
        public DateTime? DateOfDeleting { get; private init; }
        public virtual MediaToCreateDTO MediaToCreateDTO { get; set; } = null!;
    }
}

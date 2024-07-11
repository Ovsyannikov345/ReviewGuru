using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record AuthorToCreateDTO
    {
        [JsonIgnore]
        public int AuthorId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }
}

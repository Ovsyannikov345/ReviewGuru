using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record MediaToCreateYDTO
    {
        [JsonIgnore]
        public int MediaId { get; init; }
        public string MediaType { get; init; }
        public string Name { get; init; }
        public DateTime YearOfCreating { get; set; }
        public virtual ICollection<AuthorToCreateDTO> AuthorsToCreateDTO { get; set; } = [];

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record ReviewToCreateAPIDTO
    {
        [JsonIgnore]
        public int ReviewId { get; init; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int Rating { get; init; }
        public string UserReview { get; init; } = string.Empty;
        public string MediaName { get; init; } 
        public int? YearOfMediaCreation { get; init; }
        [JsonIgnore]
        public DateTime DateOfCreation { get; private init; } = DateTime.Now;
    }
}

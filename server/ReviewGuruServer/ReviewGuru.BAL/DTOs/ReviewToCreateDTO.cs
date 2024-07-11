using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public class ReviewToCreateDTO
    {
        public int ReviewId { get; init; }
        public int UserId { get; set; }
        public int MediaId { get; init; }
        public int Rating { get; init; }
        public string UserReview { get; init; } = string.Empty;
        public DateTime DateOfCreation { get; private init; } = DateTime.Now;
        public DateTime? DateOfLastModification { get; private init; }
        public DateTime? DateOfDeleting { get; private init; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Entities.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public int Rating { get; set; }
        public string UserReview { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}

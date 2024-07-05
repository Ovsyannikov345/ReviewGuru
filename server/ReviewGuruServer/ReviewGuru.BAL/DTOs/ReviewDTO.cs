using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record ReviewDTO : IEntity
    {
        public int Id { get; private set; }
        public int UserId { get;  set; }
        public int MediaId { get; set; }
        public int Rating { get; set; }
        public string UserReview { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public DateTime? DateOfLastModification { get; private set; }
        public DateTime? DateOfDeleting { get; private set; }


    }
}

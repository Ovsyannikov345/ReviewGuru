using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Entities.Models
{
    public class MediaAuthor
    {
        [Required]
        public int MediaAuthorId { get; set; }
        public int MediaId { get; set; }
        public int AuthorId { get; set; }
    }
}

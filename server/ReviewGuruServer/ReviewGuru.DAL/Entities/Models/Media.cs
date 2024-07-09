using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Entities.Models
{
    public class Media
    {
        [Required]
        public int MediaId { get; set; }

        [Required]
        public string MediaType { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        public List<MediaAuthor>? MediaAuthors { get; set; } = [];

        public List<Review> Reviews { get; set; } = [];
    }
}

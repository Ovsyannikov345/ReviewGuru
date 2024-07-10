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
        [Key]
        public int MediaId { get; set; }

        [Required]
        public string MediaType { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public DateOnly YearOfCreating { get; set; }

        public virtual ICollection<Author> Authors { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Entities.Models
{
    public class User
    {
        public int UsertId { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

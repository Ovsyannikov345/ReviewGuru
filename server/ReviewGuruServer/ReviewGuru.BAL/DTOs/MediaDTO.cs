using ReviewGuru.BLL.Services.IServices;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record MediaDTO : IEntity
    {
        public int Id { get; private set; }
        public string MediaType { get; set; }
        public string Name { get; set; }
    }

}

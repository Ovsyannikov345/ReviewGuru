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
        public int Id { get; private init; }
        public string MediaType { get; init; }
        public string Name { get; init; }
    }

}

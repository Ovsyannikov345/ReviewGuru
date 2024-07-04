using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record RefreshTokensDto
    {
        public string RefreshToken { get; set; } = "";
    }
}

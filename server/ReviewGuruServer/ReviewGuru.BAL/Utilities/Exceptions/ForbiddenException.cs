using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Utilities.Exceptions
{
    public class ForbiddenException(string message) : Exception(message)
    {
    }
}

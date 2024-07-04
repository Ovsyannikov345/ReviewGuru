using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class MediaAuthorService(IGenericRepository<MediaAurtorDTO> genericRepository) : GenericService<MediaAurtorDTO>(genericRepository), IMediaAuthorService
    {
    }
}
